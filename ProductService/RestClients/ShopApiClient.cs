using Microsoft.Extensions.Configuration;
using Polly;
using Polly.Retry;
using RestEase;
using Steeltoe.Common.Discovery;
using Steeltoe.Discovery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProductService.RestClients
{
  
        public class ShopApiClient : IShopApiClient
        {
            private readonly IShopApiClient client;

            private static AsyncRetryPolicy retryPolicy = Policy
                .Handle<HttpRequestException>()
                .WaitAndRetryAsync(retryCount: 3, sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(3));

            public ShopApiClient(IConfiguration configuration, IDiscoveryClient discoveryClient)
            {
                var handler = new DiscoveryHttpClientHandler(discoveryClient);
                var httpClient = new HttpClient(handler, false)
                {
                    BaseAddress = new Uri(configuration.GetValue<string>("ShopApiServiceUri"))
                };
                client = RestClient.For<IShopApiClient>(httpClient);
            }

            public Task<string> Get()
            {
                return retryPolicy.ExecuteAsync(async () => await client.Get());
            }
        }
    

    public interface IShopApiClient
    {
        [Get]
        Task<string> Get();
    }

}
