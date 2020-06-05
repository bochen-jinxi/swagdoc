using System.IO;
using System.Reflection;
using Microsoft.OpenApi.Models;
using SwgDocGen._01_Abstractions;
using SwgDocGen.Properties;

namespace SwgDocGen._02_Implements
{
    /// <summary>
    /// 生成word文档
    /// </summary>
    public class GeneratorWordDoc : IGenerator
    {
        /// <summary>
        /// 生成word文档
        /// </summary>
        /// <param name="document">对象</param>
        /// <param name="outPutType">输出类型</param>
        /// <param name="templatePath">模板路径</param>
        /// <returns></returns>
        public Stream Generator(OpenApiDocument document, out string outPutType, string templatePath = null)
        {
            var content = GetContent(document, templatePath);
            var stream = DocumentHelper.GeneratorStream(content, ".docx", out outPutType);
            return stream;
        }

        /// <summary>
        ///  获得内容
        /// </summary>
        /// <param name="document">对象</param>
        /// <param name="templatePath">自定义模板路径</param>
        /// <returns></returns>
        private string GetContent(OpenApiDocument document, string templatePath)
        {
            return string.IsNullOrEmpty(templatePath) ? ContentByResource(document) : DocumentHelper.GeneratorContentByPath(document, templatePath);
        }
        /// <summary>
        /// 根据资源获取内容
        /// </summary>
        /// <param name="document">对象</param>
        /// <returns></returns>
        private static string ContentByResource(OpenApiDocument document)
        {
            string content;
            var asm = Assembly.Load("SwgDocGen"); //文件所在的项目 
            var sm = asm.GetManifestResourceStream("SwgDocGen._06_Resource.Templates.SwaggerDoc.cshtml"); //文件的路径,程序集.路径.文件名 
            if (sm == null)
                content = DocumentHelper.GeneratorContent(document, Resources.SwagDoc);
            else
                using (var sr = new StreamReader(sm))
                {
                    content = sr.ReadToEnd();
                    content = DocumentHelper.GeneratorContent(document, content);
                }
            return content;
        }
    }
}