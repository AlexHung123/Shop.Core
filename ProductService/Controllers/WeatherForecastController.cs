using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductService.RestClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IShopApiService _shopApiService;

        public WeatherForecastController(IShopApiService shopApiService)
        {
            _shopApiService = shopApiService;
        }

        [HttpGet]
        public string Get()
        {
            var rtn = _shopApiService.Get().Result;
            return rtn;

            //return "ok";
        }
    }
}
