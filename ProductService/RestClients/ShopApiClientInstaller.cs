using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.RestClients
{
    public static class ShopApiClientInstaller
    {
        public static IServiceCollection AddShopApiRestClient(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IShopApiClient), typeof(ShopApiClient));
            services.AddSingleton(typeof(IShopApiService), typeof(ShopApiService));
            return services;
        }
    }
}
