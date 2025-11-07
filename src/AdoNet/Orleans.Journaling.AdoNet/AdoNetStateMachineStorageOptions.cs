using Orleans.Runtime;

#if JOURNALING_ADONET
namespace Orleans.Journaling.AdoNet.Storage
#else
// No default namespace intentionally to cause compile errors if something is not defined
#endif
{
    /// <summary>
    /// ADO.NET invariant name constants
    /// </summary>
    internal static class AdoNetInvariants
    {
        /// <summary>
        /// Microsoft SQL Server invariant name
        /// </summary>
        public const string InvariantNameSqlServer = "System.Data.SqlClient";

        /// <summary>
        /// Oracle Database invariant name
        /// </summary>
        public const string InvariantNameOracleDatabase = "Oracle.DataAccess.Client";

        /// <summary>
        /// MySql invariant name
        /// </summary>
        public const string InvariantNameMySql = "MySql.Data.MySqlClient";

        /// <summary>
        /// PostgreSQL invariant name
        /// </summary>
        public const string InvariantNamePostgreSql = "Npgsql";

        /// <summary>
        /// SQLite invariant name
        /// </summary>
        public const string InvariantNameSqlLite = "Microsoft.Data.Sqlite";
    }
}

namespace Orleans.Journaling;

/// <summary>
/// Options for configuring the ADO.NET state machine storage provider.
/// </summary>
public sealed class AdoNetStateMachineStorageOptions
{
    /// <summary>
    /// Connection string for AdoNet storage.
    /// </summary>
    [Redact]
    public string? ConnectionString { get; set; }

    /// <summary>
    /// The default ADO.NET invariant used for storage if none is given. 
    /// </summary>
    public const string DEFAULT_ADONET_INVARIANT = "System.Data.SqlClient";

    /// <summary>
    /// The invariant name for storage.
    /// </summary>
    public string Invariant { get; set; } = DEFAULT_ADONET_INVARIANT;

    /// <summary>
    /// Stage of silo lifecycle where storage should be initialized. Storage must be initialized prior to use.
    /// </summary>
    public int InitStage { get; set; } = DEFAULT_INIT_STAGE;
    public const int DEFAULT_INIT_STAGE = ServiceLifecycleStage.ApplicationServices;

    /// <summary>
    /// Table name where state machine logs are stored.
    /// </summary>
    public string TableName { get; set; } = DEFAULT_TABLE_NAME;
    public const string DEFAULT_TABLE_NAME = "OrleansJournalingStorage";
}
