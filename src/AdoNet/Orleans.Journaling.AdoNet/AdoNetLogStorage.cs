using Microsoft.Extensions.Logging;
using Orleans.Journaling.AdoNet.Storage;
using Orleans.Runtime;
using Orleans.Serialization.Buffers;
using System.Data;
using System.Runtime.CompilerServices;

namespace Orleans.Journaling;

internal sealed partial class AdoNetLogStorage : IStateMachineStorage
{
    private readonly IRelationalStorage _storage;
    private readonly string _grainId;
    private readonly ILogger<AdoNetLogStorage> _logger;
    private readonly string _tableName;
    private long _currentVersion;
    private int _logSegmentCount;

    public bool IsCompactionRequested => _logSegmentCount > 10;

    public AdoNetLogStorage(
        IRelationalStorage storage,
        GrainId grainId,
        string tableName,
        ILogger<AdoNetLogStorage> logger)
    {
        _storage = storage;
        _grainId = grainId.ToString();
        _tableName = tableName;
        _logger = logger;
        _currentVersion = 0;
        _logSegmentCount = 0;
    }

    public async ValueTask AppendAsync(LogExtentBuilder value, CancellationToken cancellationToken)
    {
        var data = value.ToArray();
        var newVersion = _currentVersion + 1;

        var query = $@"
            INSERT INTO {_tableName} (GrainId, Version, SegmentData, OperationType, CreatedUtc)
            VALUES (@GrainId, @Version, @SegmentData, @OperationType, @CreatedUtc)";

        await _storage.ExecuteAsync(query, command =>
        {
            var grainIdParam = command.CreateParameter();
            grainIdParam.ParameterName = "GrainId";
            grainIdParam.Value = _grainId;
            grainIdParam.DbType = DbType.String;
            command.Parameters.Add(grainIdParam);

            var versionParam = command.CreateParameter();
            versionParam.ParameterName = "Version";
            versionParam.Value = newVersion;
            versionParam.DbType = DbType.Int64;
            command.Parameters.Add(versionParam);

            var segmentDataParam = command.CreateParameter();
            segmentDataParam.ParameterName = "SegmentData";
            segmentDataParam.Value = data;
            segmentDataParam.DbType = DbType.Binary;
            command.Parameters.Add(segmentDataParam);

            var operationTypeParam = command.CreateParameter();
            operationTypeParam.ParameterName = "OperationType";
            operationTypeParam.Value = "Append";
            operationTypeParam.DbType = DbType.String;
            command.Parameters.Add(operationTypeParam);

            var createdUtcParam = command.CreateParameter();
            createdUtcParam.ParameterName = "CreatedUtc";
            createdUtcParam.Value = DateTime.UtcNow;
            createdUtcParam.DbType = DbType.DateTime;
            command.Parameters.Add(createdUtcParam);
        }, cancellationToken: cancellationToken).ConfigureAwait(false);

        _currentVersion = newVersion;
        _logSegmentCount++;

        LogAppend(_logger, value.Length, _grainId, _tableName);
    }

    public async ValueTask DeleteAsync(CancellationToken cancellationToken)
    {
        var query = $@"DELETE FROM {_tableName} WHERE GrainId = @GrainId";

        await _storage.ExecuteAsync(query, command =>
        {
            var grainIdParam = command.CreateParameter();
            grainIdParam.ParameterName = "GrainId";
            grainIdParam.Value = _grainId;
            grainIdParam.DbType = DbType.String;
            command.Parameters.Add(grainIdParam);
        }, cancellationToken: cancellationToken).ConfigureAwait(false);

        _currentVersion = 0;
        _logSegmentCount = 0;

        LogDelete(_logger, _grainId, _tableName);
    }

    public async IAsyncEnumerable<LogExtent> ReadAsync([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var query = $@"
            SELECT Version, SegmentData, OperationType 
            FROM {_tableName} 
            WHERE GrainId = @GrainId 
            ORDER BY Version ASC";

        var results = await _storage.ReadAsync<(long Version, byte[] SegmentData, string OperationType)>(
            query,
            command =>
            {
                var grainIdParam = command.CreateParameter();
                grainIdParam.ParameterName = "GrainId";
                grainIdParam.Value = _grainId;
                grainIdParam.DbType = DbType.String;
                command.Parameters.Add(grainIdParam);
            },
            async (record, resultSetCount, ct) =>
            {
                var version = record.GetInt64(0);
                var segmentData = (byte[])record.GetValue(1);
                var operationType = record.GetString(2);

                return (version, segmentData, operationType);
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);

        var resultList = results.ToList();
        if (resultList.Count > 0)
        {
            _currentVersion = resultList.Max(r => r.Version);
            _logSegmentCount = resultList.Count;

            long totalBytes = 0;
            foreach (var result in resultList)
            {
                totalBytes += result.SegmentData.Length;
                yield return new LogExtent(result.SegmentData);
            }

            LogRead(_logger, totalBytes, _grainId, _tableName);
        }
    }

    public async ValueTask ReplaceAsync(LogExtentBuilder value, CancellationToken cancellationToken)
    {
        var data = value.ToArray();

        // Note: This operation is not fully atomic due to IRelationalStorage interface limitations.
        // It performs two separate operations: DELETE followed by INSERT.
        // Risk: If DELETE succeeds but INSERT fails (e.g., due to network issues, database shutdown),
        // the grain's state will be lost until recovered from backup or recreation.
        // Mitigation strategies:
        // - For production use, consider implementing database-specific stored procedures with BEGIN TRANSACTION
        // - Most databases will execute these operations quickly, minimizing the failure window
        // - Orleans grain activation lifecycle typically ensures single-threaded access per grain
        // - Consider implementing compensating actions or backup strategies at the application level

        // First, delete all existing segments for this grain
        var deleteQuery = $@"DELETE FROM {_tableName} WHERE GrainId = @GrainId";

        await _storage.ExecuteAsync(deleteQuery, command =>
        {
            var grainIdParam = command.CreateParameter();
            grainIdParam.ParameterName = "GrainId";
            grainIdParam.Value = _grainId;
            grainIdParam.DbType = DbType.String;
            command.Parameters.Add(grainIdParam);
        }, cancellationToken: cancellationToken).ConfigureAwait(false);

        // Insert the new compacted segment
        var insertQuery = $@"
            INSERT INTO {_tableName} (GrainId, Version, SegmentData, OperationType, CreatedUtc)
            VALUES (@GrainId, @Version, @SegmentData, @OperationType, @CreatedUtc)";

        await _storage.ExecuteAsync(insertQuery, command =>
        {
            var grainIdParam = command.CreateParameter();
            grainIdParam.ParameterName = "GrainId";
            grainIdParam.Value = _grainId;
            grainIdParam.DbType = DbType.String;
            command.Parameters.Add(grainIdParam);

            var versionParam = command.CreateParameter();
            versionParam.ParameterName = "Version";
            versionParam.Value = 1L;
            versionParam.DbType = DbType.Int64;
            command.Parameters.Add(versionParam);

            var segmentDataParam = command.CreateParameter();
            segmentDataParam.ParameterName = "SegmentData";
            segmentDataParam.Value = data;
            segmentDataParam.DbType = DbType.Binary;
            command.Parameters.Add(segmentDataParam);

            var operationTypeParam = command.CreateParameter();
            operationTypeParam.ParameterName = "OperationType";
            operationTypeParam.Value = "Replace";
            operationTypeParam.DbType = DbType.String;
            command.Parameters.Add(operationTypeParam);

            var createdUtcParam = command.CreateParameter();
            createdUtcParam.ParameterName = "CreatedUtc";
            createdUtcParam.Value = DateTime.UtcNow;
            createdUtcParam.DbType = DbType.DateTime;
            command.Parameters.Add(createdUtcParam);
        }, cancellationToken: cancellationToken).ConfigureAwait(false);

        _currentVersion = 1;
        _logSegmentCount = 1;

        LogReplace(_logger, _grainId, _tableName, value.Length);
    }

    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Appended {Length} bytes to grain \"{GrainId}\" in table \"{TableName}\"")]
    private static partial void LogAppend(ILogger logger, long length, string grainId, string tableName);

    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Read {Length} bytes from grain \"{GrainId}\" in table \"{TableName}\"")]
    private static partial void LogRead(ILogger logger, long length, string grainId, string tableName);

    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Replaced grain \"{GrainId}\" in table \"{TableName}\", writing {Length} bytes")]
    private static partial void LogReplace(ILogger logger, string grainId, string tableName, long length);

    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Deleted grain \"{GrainId}\" from table \"{TableName}\"")]
    private static partial void LogDelete(ILogger logger, string grainId, string tableName);
}
