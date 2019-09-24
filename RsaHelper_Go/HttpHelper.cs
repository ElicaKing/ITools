using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace RsaHelper_Go
{
    public class HttpHelper
    {
        /// <summary>
        /// 获取或设置用于验证服务器证书的回调永远返回true
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="cert"></param>
        /// <param name="chain"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool RemoteCertificateValidate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
        {
            return true;
        }
        public static string SendPOST(string apiUrl, string data, Dictionary<string, string> postHeaders)
        {
            string result = string.Empty;
            int http_StatusCode = 0;
            string http_ResponseMessage = null;
            try
            {
                //注意提交的编码 这边是需要改变的 这边默认的是Default；系统当前编码
                byte[] postData = Encoding.UTF8.GetBytes(data);
                //忽略SSL证书，防止“未能为SSL/TLS安全通道建立信任关系”的报错
                ServicePointManager.ServerCertificateValidationCallback += RemoteCertificateValidate;
                //设置提交的相关参数；
                HttpWebRequest request = WebRequest.Create(apiUrl) as HttpWebRequest;
                //Encoding myEncoding = Encoding.UTF-8;
                request.Method = "POST";
                request.KeepAlive = false;
                //遍历字典
                foreach (KeyValuePair<string, string> header in postHeaders)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
                request.ContentType = "application/json";
                //提交请求数据
                System.IO.Stream outputStream = request.GetRequestStream();
                outputStream.Write(postData, 0, postData.Length);
                outputStream.Close();

                HttpWebResponse response = null;
                Stream responseStream = null;
                StreamReader reader = null;
                string srcString;
                response = request.GetResponse() as HttpWebResponse;
                //StatusCode 为枚举类型，200为正常，其他输出为异常，需要转为int型才会输出状态码
                http_StatusCode = Convert.ToInt32(response.StatusCode);
                http_ResponseMessage = response.StatusCode.ToString();
                if (200 == http_StatusCode)
                {
                    responseStream = response.GetResponseStream();
                    reader = new System.IO.StreamReader(responseStream, Encoding.GetEncoding("UTF-8"));
                    srcString = reader.ReadToEnd();
                    result = srcString;//返回赋值
                    reader.Close();
                }
                else
                {
                    throw new Exception("网络请求异常：HTTP响应码： " + http_StatusCode + ", HTTP响应信息： " + http_ResponseMessage);
                }
            }
            catch (Exception ex)
            {

                throw new Exception("网络请求时发生异常：" + ex.Message);
            }
            return result;
        }

        
    }
}
