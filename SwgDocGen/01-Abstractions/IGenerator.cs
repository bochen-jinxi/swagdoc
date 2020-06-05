using System.IO;
using Microsoft.OpenApi.Models;

namespace SwgDocGen._01_Abstractions
{
    public interface IGenerator
    {
        /// <summary>
        /// 生成文档
        /// </summary>
        /// <param name="document">对象</param>
        /// <param name="outPutType">输出类型</param>
        /// <param name="templatePath">模板路径</param>
        /// <returns></returns>
        Stream Generator(OpenApiDocument document, out string outPutType, string templatePath = null);
    }
}