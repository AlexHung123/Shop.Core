using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace Shop.Core.Consul
{
    public static class ConsulExtension
    {
        public static IServiceCollection AddConsul(this IServiceCollection services, IConfiguration configuration)
        {
            var option = new ConsulOption();
            configuration.GetSection(nameof(ConsulOption)).Bind(option);
            //add it to the dependency injection service container
            services.Configure<ConsulOption>(configuration.GetSection(nameof(ConsulOption)));

            services.AddSingleton<IConsulClient, ConsulClient>(p=> new ConsulClient(consulConfig=> 
            {
                var address = option.ConsulAddress;
                consulConfig.Address = new Uri(address);

            }));

            return services;
        }

        public static IApplicationBuilder UseConsul(this IApplicationBuilder app, IHostApplicationLifetime lifetime)
        {
            //Retrieve consul client & Config from DI
            var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
            var consulConfig = app.ApplicationServices.GetRequiredService<IOptions<ConsulOption>>();

            //var loggingFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
            //var logger = loggingFactory.CreateLogger<IApplicationBuilder>();

            //Get IP from server
            var address = consulConfig.Value.ServiceAddress;

            if (string.IsNullOrEmpty(address))
            {
                var features = app.Properties["server.Features"] as FeatureCollection;
                var addresses = features.Get<IServerAddressesFeature>();
                address = addresses.Addresses.First();

                Console.WriteLine($"Could not find service address in config. Using '{address}'");
            }

            //Register service with consul
            var uri = new Uri(address);
            var serviceName = consulConfig.Value.ServiceName;
            var registration = new AgentServiceRegistration
            {
                ID = $"{serviceName.ToLowerInvariant()}-{consulConfig.Value.Id ?? Guid.NewGuid().ToString()}",
                Name = serviceName,
                Address = uri.Host,
                Port = uri.Port,
                Tags = consulConfig.Value.Tags
            };


            //add health check for 10s
            if (!consulConfig.Value.DisableAgentCheck)
            {
                registration.Check = new AgentServiceCheck
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),
                    Interval = TimeSpan.FromSeconds(10),
                    HTTP = new Uri(uri, "health").OriginalString
                };
            }

            //logger.LogInformation($"Registering {registration.ID} with Consul");
            Console.WriteLine($"Registering {registration.ID} with Consul");
            consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            consulClient.Agent.ServiceRegister(registration).Wait();

            lifetime.ApplicationStopping.Register(()=> 
            {
                //logger.LogInformation($"Deregistering {registration.ID} from Consul");
                Console.WriteLine($"Registering {registration.ID} with Consul");
                consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            });

            return app;

        }
    }
}
