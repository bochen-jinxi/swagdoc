using Microsoft.AspNetCore.StaticFiles;
using Microsoft.OpenApi.Models;
using RazorEngine;
using RazorEngine.Templating;
using Spire.Doc;
using Spire.Doc.Documents;
using System;
using System.IO;
using Encoding = System.Text.Encoding;

namespace SwgDocGen
{
    /// <summary>
    /// 文档助手
    /// </summary>
    public class DocumentHelper
    {
        /// <summary>
        /// 生成流
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="contentType">内容类型</param>
        /// <param name="outPutType">输出类型</param>
        /// <returns></returns>
        internal static Stream GeneratorStream(string content, string contentType, out string outPutType)
        {
            var fileName = Guid.NewGuid() + contentType;
            var path = @".\Files\TempFiles\";
            var addrUrl =  path + $"{fileName}";
            FileStream fileStream = null;
            var provider = new FileExtensionContentTypeProvider();
            outPutType = provider.Mappings[contentType];
            try
            {
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                var data = Encoding.Default.GetBytes(content);
                var stream = ByteHelper.BytesToStream(data);
                //创建Document实例
                var document = new Document();
                //加载HTML文档
                document.LoadFromStream(stream, FileFormat.Html, XHTMLValidationType.None);
                //保存
                switch (contentType)
                {
                    case ".docx":
                        //Word
                        document.SaveToFile(addrUrl, FileFormat.Docx);
                        break;
                    case ".pdf":
                        //PDF
                        document.SaveToFile(addrUrl, FileFormat.PDF);
                        break;
                    case ".html":
                        //Html
                        var fs = new FileStream(addrUrl, FileMode.Append, FileAccess.Write, FileShare.None); //html直接写入不用spire.doc
                        var sw = new StreamWriter(fs); // 创建写入流
                        sw.WriteLine(content); // 写入Hello World
                        sw.Close(); //关闭文件
                        fs.Close();
                        break;
                    case ".xml":
                        //PDF
                        document.SaveToFile(addrUrl, FileFormat.WordXml);
                        break;
                    case ".svg":
                        //PDF
                        document.SaveToFile(addrUrl, FileFormat.SVG);
                        break;
                }

                document.Close();
                fileStream = File.Open(addrUrl, FileMode.OpenOrCreate);
                var filedata = ByteHelper.StreamToBytes(fileStream);
                var outdata = ByteHelper.BytesToStream(filedata);
                return outdata;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                fileStream?.Close();
                if (File.Exists(addrUrl))
                    File.Delete(addrUrl); //删掉文件
            }
        }

        /// <summary>
        /// 生成内容
        /// </summary>
        /// <param name="document">对象</param>
        /// <param name="content">内容</param>
        /// <returns></returns>
        internal static string GeneratorContent(OpenApiDocument document, string content)
        {
            var result = Engine.Razor.RunCompile(content, "api", typeof(OpenApiDocument), document);
            return result;
        }
        /// <summary>
        /// 根据模板路径生成内容
        /// </summary>
        /// <param name="document">对象</param>
        /// <param name="templatePath">模板路径</param>
        /// <returns></returns>
        internal static string GeneratorContentByPath(OpenApiDocument document, string templatePath)
        {
            var path = Path.Combine(AppContext.BaseDirectory, templatePath);
            var template = File.ReadAllText(path);
            var result = Engine.Razor.RunCompile(template, "api", typeof(OpenApiDocument), document);
            return result;
        }

    }
}