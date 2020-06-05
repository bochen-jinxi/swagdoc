using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using SwgDocGen;
using SwgDocGen._01_Abstractions;
using SwgDocGen._02_Implements;

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

        public string[] Summaries =
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public ISwaggerProvider SwaggerGenerator { get; }
        public IGenerator GeneratorWordDoc { get; }


        public WeatherForecastController(ISwaggerProvider SwaggerGenerator, IGenerator  GeneratorWordDoc)
        {
            this.SwaggerGenerator = SwaggerGenerator;
            this.GeneratorWordDoc = GeneratorWordDoc;
            
        }

        /// <summary>
        ///     默认获取word
        /// </summary>
        /// <returns>返回车</returns>
        [HttpGet("/GetSwDoc")]
        public IActionResult GetSwDoc()
        {
            var document = SwaggerGenerator.GetSwagger("v1");
            var stream = GeneratorWordDoc.Generator(document, out var memi);
            return File(stream, memi, "api简化文档.doc");
        }

        /// <summary>
        ///     自定义模板路径获取word
        /// </summary>
        /// <returns>返回车</returns>
        [HttpGet("/GetSwDocByPath")]
        public IActionResult GetSwDocByPath()
        {
            var document = SwaggerGenerator.GetSwagger("v1");
            var stream =  GeneratorWordDoc.Generator(document, out var memi, "Templates\\SwaggerDoc.cshtml");
            return File(stream, memi, "api简化文档.doc");
        }
    }
}