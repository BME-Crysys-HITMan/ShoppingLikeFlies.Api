using Microsoft.AspNetCore.Mvc;
using ShoppingLikeFiles.DomainServices.Service;
using System.Drawing;

namespace ShoppingLikeFlies.Api.Controllers
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
        private readonly ICaffService caffService;


        public WeatherForecastController(ILogger<WeatherForecastController> logger, ICaffService caffService)
        {
            _logger = logger;
            this.caffService = caffService;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            X();

            caffService.Ping();

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        private void X()
        {
            Image img = Image.FromFile("testImage.jpg");
        }
    }
}