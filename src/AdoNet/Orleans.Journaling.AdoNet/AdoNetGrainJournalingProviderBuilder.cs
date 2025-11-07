using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Orleans.Hosting;
using Orleans.Journaling;
using Orleans.Providers;

[assembly: RegisterProvider("AdoNet", "GrainJournaling", "Silo", typeof(AdoNetGrainJournalingProviderBuilder))]

namespace Orleans.Hosting;

internal sealed class AdoNetGrainJournalingProviderBuilder : IProviderBuilder<ISiloBuilder>
{
    public void Configure(ISiloBuilder builder, string? name, IConfigurationSection configurationSection)
    {
        builder.AddAdoNetStateMachineStorage();
        var optionsBuilder = builder.Services.AddOptions<AdoNetStateMachineStorageOptions>();
        optionsBuilder.Configure<IConfiguration>((options, rootConfiguration) =>
        {
            var connectionString = configurationSection["ConnectionString"];
            if (!string.IsNullOrEmpty(connectionString))
            {
                options.ConnectionString = connectionString;
            }
            else
            {
                var connectionName = configurationSection["ConnectionName"];
                if (!string.IsNullOrEmpty(connectionName))
                {
                    connectionString = rootConfiguration.GetConnectionString(connectionName);
                    if (!string.IsNullOrEmpty(connectionString))
                    {
                        options.ConnectionString = connectionString;
                    }
                }
            }

            var invariant = configurationSection["Invariant"];
            if (!string.IsNullOrEmpty(invariant))
            {
                options.Invariant = invariant;
            }

            var tableName = configurationSection["TableName"];
            if (!string.IsNullOrEmpty(tableName))
            {
                options.TableName = tableName;
            }
        });
    }
}
