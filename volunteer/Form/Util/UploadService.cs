using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Net;
using System.Diagnostics;
using System.Threading;
using System.Net.Http;

namespace Form.Util
{
    /// <summary>
    /// 提供上传功能的服务类
    /// </summary>
    public class UploadService
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="url">上传路径</param>
        /// <param name="drawName">参数</param>
        /// <param name="token">身份验证令牌</param>
        /// <param name="callback">传输过程中用于显示进度的回调函数，返回值表示是否继续传输</param>
        /// <param name="endCallback">上传结束后的回调函数</param>
        /// <remarks>
        /// 上传文件的逻辑如下：
        /// 1. 使用 HttpClient 创建一个客户端
        /// 2. 创建一个 MultipartFormDataContent 对象，并添加需要上传的参数
        /// 3. 创建一个 StreamContent 对象，用于读取文件内容
        /// 4. 将文件内容添加到 MultipartFormDataContent 对象中，并设置上传进度回调函数
        /// 5. 添加身份验证令牌到请求头部
        /// 6. 使用客户端发送 POST 请求，将文件上传到指定的 URL
        /// 7. 获取响应内容并判断是否上传成功
        /// 8. 调用上传结束回调函数，传递上传是否成功和响应内容
        /// 9. 如果发生异常，调用上传结束回调函数，传递上传是否成功和异常信息
        /// </remarks>
        public async static void Upload(string filePath,string url,Func<long,long,bool> callback,Action<bool,string> endCallback)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var content = new MultipartFormDataContent();
                    var fileContent = new StreamContent(File.OpenRead(filePath));
                    content.Add(new ProgressableStreamContent(
                        fileContent,
                        4096,
                        callback)
                    , "file", Path.GetFileName(filePath));
                    var response = await client.PostAsync(url, content);
                    var result = await response.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(result))
                    {
                        result = "服务器未返回有效信息！";
                        endCallback(false, result);
                    }
                    else
                    {
                        result = "Upload successfully!";
                        endCallback(true, result);
                    }
                }
            }
            catch (Exception ex)
            {
                endCallback(false, ex.Message);
            }
        }
    }
}
