using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using SwgDocGen._01_Abstractions;
using SwgDocGen._02_Implements;

namespace   Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 文档静态扩展
    /// </summary>
    public static class DocExtensions
    {
        /// <summary>
        /// 注册文档服务
        /// </summary>
        /// <param name="services">ServiceCollection</param>
        /// <returns></returns>
        public static IServiceCollection AddDocGenerator(this IServiceCollection services)
        {
            services.AddScoped<IGenerator, GeneratorWordDoc>();
            return services;
        }
    }
}