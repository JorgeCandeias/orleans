using Orleans.Journaling.AdoNet.Storage;
using Orleans.Runtime;

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
    public const string DEFAULT_ADONET_INVARIANT = AdoNetInvariants.InvariantNameSqlServer;

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
