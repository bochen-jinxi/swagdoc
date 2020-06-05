using System.IO;

namespace SwgDocGen
{
    /// <summary>
    /// 二进制转换助手
    /// </summary>
    public class ByteHelper
    {
        /// <summary>
        /// 流转二进制
        /// </summary>
        /// <param name="stream">流</param>
        /// <returns></returns>
        public static byte[] StreamToBytes(Stream stream)
        {
            var bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始 
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        /// <summary>
        /// 二进制转流
        /// </summary>
        /// <param name="bytes">byte数组</param>
        /// <returns></returns>
        public static Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }
    }
}