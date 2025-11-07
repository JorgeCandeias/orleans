using Microsoft.Extensions.DependencyInjection;
using Orleans.Configuration.Internal;
using Orleans.Hosting;
using Orleans.Runtime;

namespace Orleans.Journaling;

public static class AdoNetHostingExtensions
{
    public static ISiloBuilder AddAdoNetStateMachineStorage(this ISiloBuilder builder) 
        => builder.AddAdoNetStateMachineStorage(configure: null);

    public static ISiloBuilder AddAdoNetStateMachineStorage(
        this ISiloBuilder builder, 
        Action<AdoNetStateMachineStorageOptions>? configure)
    {
        builder.AddStateMachineStorage();

        var services = builder.Services;

        var options = services.AddOptions<AdoNetStateMachineStorageOptions>();
        if (configure is not null)
        {
            options.Configure(configure);
        }

        if (services.Any(service => service.ServiceType.Equals(typeof(AdoNetStateMachineStorageProvider))))
        {
            return builder;
        }

        services.AddSingleton<AdoNetStateMachineStorageProvider>();
        services.AddFromExisting<IStateMachineStorageProvider, AdoNetStateMachineStorageProvider>();
        services.AddFromExisting<ILifecycleParticipant<ISiloLifecycle>, AdoNetStateMachineStorageProvider>();
        
        return builder;
    }
}
