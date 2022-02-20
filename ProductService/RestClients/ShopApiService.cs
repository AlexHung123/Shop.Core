using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.RestClients
{
    public interface IShopApiService 
    {
        Task<string> Get();
    }
    public class ShopApiService : IShopApiService
    {
        private readonly IShopApiClient shopApiClient;

        public ShopApiService(IShopApiClient shopApiClient)
        {
            this.shopApiClient = shopApiClient;
        }

        public async Task<string> Get()
        {
            return await shopApiClient.Get();
        }
    }
}
