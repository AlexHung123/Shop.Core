using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shop.Core.Consul.Services
{
    public class ConsulRegisterService : IHostedService
    {
        IConsulClient _consulClient;
        ServiceInfo _serviceInfo;

        public ConsulRegisterService(IConfiguration configuration, IConsulClient consulClient)
        {
            _serviceInfo = new ServiceInfo();
            var sc = configuration.GetSection("serviceInfo");

            _serviceInfo.Id = sc["id"];
            _serviceInfo.Name = sc["name"];
            _serviceInfo.IP = sc["ip"];
            _serviceInfo.HealthCheckAddress = sc["healthCheckAddress"];
            _serviceInfo.Port = int.Parse(sc["port"]);

            _consulClient = consulClient;

        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"Start to register service ${_serviceInfo.Id} to consul client ...");
            await _consulClient.Agent.ServiceDeregister(_serviceInfo.Id, cancellationToken);
            await _consulClient.Agent.ServiceRegister(new AgentServiceRegistration
            { 
                ID = _serviceInfo.Id,
                Name = _serviceInfo.Name,
                Address = _serviceInfo.IP,
                Port = _serviceInfo.Port,
                Check = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(0), //Register service when starting service after 0s
                    Interval = TimeSpan.FromSeconds(5), //Interval for health check
                    HTTP = $"http://{_serviceInfo.IP}:{_serviceInfo.Port}/" + _serviceInfo.HealthCheckAddress,
                    Timeout = TimeSpan.FromSeconds(5)
                }
            });

            Console.WriteLine("Register service info to consul client successfully ...");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _consulClient.Agent.ServiceDeregister(_serviceInfo.Id, cancellationToken);
            Console.WriteLine($"Deregister service {_serviceInfo.Id} from consul client successfully ...");
        }
    }
}
