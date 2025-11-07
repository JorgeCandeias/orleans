using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orleans.Journaling.AdoNet.Storage;
using Orleans.Runtime;

namespace Orleans.Journaling;

internal sealed class AdoNetStateMachineStorageProvider(
    IOptions<AdoNetStateMachineStorageOptions> options,
    ILoggerFactory loggerFactory) : IStateMachineStorageProvider, ILifecycleParticipant<ISiloLifecycle>
{
    private readonly AdoNetStateMachineStorageOptions _options = options.Value;
    private readonly ILoggerFactory _loggerFactory = loggerFactory;
    private IRelationalStorage? _storage;

    private async Task Initialize(CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_options.ConnectionString))
        {
            throw new OrleansConfigurationException($"Invalid {nameof(AdoNetStateMachineStorageOptions)} values. {nameof(_options.ConnectionString)} is required.");
        }

        if (string.IsNullOrWhiteSpace(_options.Invariant))
        {
            throw new OrleansConfigurationException($"Invalid {nameof(AdoNetStateMachineStorageOptions)} values. {nameof(_options.Invariant)} is required.");
        }

        _storage = RelationalStorage.CreateInstance(_options.Invariant, _options.ConnectionString);
        
        await Task.CompletedTask;
    }

    public IStateMachineStorage Create(IGrainContext grainContext)
    {
        if (_storage is null)
        {
            throw new InvalidOperationException("Storage provider has not been initialized.");
        }

        var logger = _loggerFactory.CreateLogger<AdoNetLogStorage>();
        return new AdoNetLogStorage(_storage, grainContext.GrainId, _options.TableName, logger);
    }

    public void Participate(ISiloLifecycle observer)
    {
        observer.Subscribe(
            nameof(AdoNetStateMachineStorageProvider),
            _options.InitStage,
            onStart: Initialize);
    }
}
