using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using SwgDocGen;

namespace swagdocx.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
       

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        public ISwaggerProvider SwaggerGenerator { get; }
        private readonly ILogger<WeatherForecastController> _logger;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, ISwaggerProvider SwaggerGenerator)
        {
            this.SwaggerGenerator = SwaggerGenerator;
            _logger = logger;
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <returns>返回车</returns>
        [HttpGet("/GetSwDoc")]
        public IActionResult GetSwDoc()
        {
            var document = SwaggerGenerator.GetSwagger("v1");
            string memi = string.Empty;
           var stream = new SpireDocHelper().GetSwDoc(document, out memi, $"Templating\\Templates\\SwaggerDoc.cshtml");
           //var stream = new SpireDocHelper().GetSwDoc(document, out memi);
            return File(stream, memi, "sapi简化文档");
        }

    }
}
