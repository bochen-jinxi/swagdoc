using System;
using System.IO;
using Microsoft.OpenApi.Models;
using RazorEngine;
using RazorEngine.Templating;
namespace SwgDocGen
{
    /// <summary>
    /// 内容生成助手
    /// </summary>
    public class T4Helper
    {
        /// <summary>
        /// 根据内容生成文件
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="model">openopi对象</param>
        /// <returns></returns>
        public static string GeneritorSwaggerHtml(string content, OpenApiDocument model)
        {
            var result = Engine.Razor.RunCompile(content, "api", typeof(OpenApiDocument), model);
            return result;
        }
        /// <summary>
        /// 根据模板路径生成文件
        /// </summary>
        /// <param name="templatePath">模板路径</param>
        /// <param name="model">openopi对象</param>
        /// <returns></returns>
        public static string GeneritorSwaggerHtmlByPath(string templatePath, OpenApiDocument model)
        {
            var path = Path.Combine(AppContext.BaseDirectory, templatePath);
            var template = File.ReadAllText(path);
            var result = Engine.Razor.RunCompile(template, "api", typeof(OpenApiDocument), model);
            return result;
        }
    }
}