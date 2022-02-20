using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Core.Consul
{
    public class ConsulOption
    {
        public string Id { get; set; }
        public string ServiceName { get; set; }
        public string[] Tags { get; set; }
        public string ConsulAddress { get; set; }
        public string ServiceAddress { get; set; }
        public bool DisableAgentCheck { get; set; }
    }
}
