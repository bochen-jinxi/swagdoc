using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.OpenApi.Models;
using Spire.Doc;
using Spire.Doc.Documents;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SwgDocGen
{
    public class SpireDocHelper
    {
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
                        var fs = new FileStream(addrUrl, FileMode.Append, FileAccess.Write,
                            FileShare.None); //html直接写入不用spire.doc
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

        public Stream GetSwDoc(OpenApiDocument document,out string memi)
        {
            var html = T4Helper.GeneritorSwaggerHtml($"Templating\\Templates\\SwaggerDoc.cshtml", document);
            var stream = new SpireDocHelper().SwaggerHtmlConvers(html, ".docx", out memi);
            return stream;
        }


    }

}
