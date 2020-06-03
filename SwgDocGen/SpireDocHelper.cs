using System;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Spire.Doc;
using Spire.Doc.Documents;
using SwgDocGen.Properties;

namespace SwgDocGen
{
    /// <summary>
    /// office文档助手
    /// </summary>
    public class SpireDocHelper : IOptions<SpireDocHelper>
    {
        /// <summary>
        /// 生成流
        /// </summary>
        /// <param name="html">内容</param>
        /// <param name="type">类型</param>
        /// <param name="memi">输出类型</param>
        /// <returns></returns>
        public Stream SwaggerHtmlConvers(string html, string type, out string memi)
        {
            var fileName = Guid.NewGuid() + type;
            var path = @"\Files\TempFiles\";
            var addrUrl = path + $"{fileName}";
            FileStream fileStream = null;
            var provider = new FileExtensionContentTypeProvider();
            memi = provider.Mappings[type];
            try
            {
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                var data = Encoding.Default.GetBytes(html);
                var stream = ByteHelper.BytesToStream(data);
                //创建Document实例
                var document = new Document();
                //加载HTML文档
                document.LoadFromStream(stream, FileFormat.Html, XHTMLValidationType.None);
                //保存
                switch (type)
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
                        sw.WriteLine(html); // 写入Hello World
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
                if (fileStream != null)
                    fileStream.Close();
                if (File.Exists(addrUrl))
                    File.Delete(addrUrl); //删掉文件
            }
        }

        /// <summary>
        /// 生成word文档
        /// </summary>
        /// <param name="document">openopi对象</param>
        /// <param name="memi">类型</param>
        /// <param name="tplpath">路径</param>
        /// <returns></returns>
        public Stream GetSwDoc(OpenApiDocument document, out string memi, string tplpath = null)
        {
            var html = string.Empty;
            if (string.IsNullOrEmpty(tplpath))
            {
                var asm = Assembly.Load("SwgDocGen"); //文件所在的项目 
                var sm = asm.GetManifestResourceStream(
                    "SwgDocGen.Templating.Templates.SwaggerDoc.cshtml"); //文件的路径,程序集.路径.文件名 
                if (sm == null)
                    html = T4Helper.GeneritorSwaggerHtml(Resources.SwagDoc, document);
                else
                    using (var sr = new StreamReader(sm))
                    {
                        var content = sr.ReadToEnd();
                        html = T4Helper.GeneritorSwaggerHtml(content, document);
                    }
            }
            else
            {
                html = T4Helper.GeneritorSwaggerHtmlByPath(tplpath, document);
            }
            var stream = new SpireDocHelper().SwaggerHtmlConvers(html, ".docx", out memi);
            return stream;
        }

        public SpireDocHelper Value => this;
    }
}