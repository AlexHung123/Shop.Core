using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Models
{
    public class Product
    {
        public Guid Id { get; set; }

        public string ProductName { get; set; }
    }
}
