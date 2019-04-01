using System;
using System.Threading.Tasks;
using Grains;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Serialization;

namespace Silo
{
    public class Program
    {
        public static Task Main(string[] args)
        {
            Console.Title = nameof(Silo);

            return new HostBuilder()
                .UseOrleans(builder =>
                {
                    builder
                        .UseLocalhostClustering()
                        .ConfigureApplicationParts(manager =>
                        {
                            manager.AddApplicationPart(typeof(SomeGrain).Assembly).WithReferences();
                        })
                        .Configure<SerializationProviderOptions>(options =>
                        {
                            options.SerializationProviders.Add(typeof(BondSerializer));
                        });
                })
                .ConfigureLogging(builder =>
                {
                    builder.AddConsole();
                })
                .ConfigureServices(services =>
                {
                    services.Configure<ConsoleLifetimeOptions>(options =>
                    {
                        options.SuppressStatusMessages = true;
                    });
                })
                .RunConsoleAsync();
        }
    }
}
