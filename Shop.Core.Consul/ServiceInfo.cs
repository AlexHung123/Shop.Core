using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Core.Consul
{
    public class ServiceInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string IP { get; set; }
        public int Port { get; set; }
        public string HealthCheckAddress { get; set; }
    }
}
