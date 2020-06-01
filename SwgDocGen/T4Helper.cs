using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.OpenApi.Models;
using RazorEngine;
using RazorEngine.Templating;
using Spire.Doc;
using Spire.Doc.Documents;
using Encoding = System.Text.Encoding;

namespace SwgDocGen
{
    public class T4Helper
    {
        public static string GeneritorSwaggerHtml(string templatePath, OpenApiDocument model)
        {
            var path=Path.Combine(AppContext.BaseDirectory.ToString(), templatePath);
            var template = File.ReadAllText(path);
            var result = Engine.Razor.RunCompile(template, "api", typeof(OpenApiDocument), model);
            return result;
        }
    }
}