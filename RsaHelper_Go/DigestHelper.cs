using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace RsaHelper_Go
{
    public class DigestHelper
    {
        //项目ID(公共应用ID),正式环境下贵司将拥有独立的应用ID
        public static string PROJECT_ID = "1111564446";
        // 项目Secret(公共应用Secret),正式环境下贵司将拥有独立的应用Secret
        // 359fed4aaf15efad06b0434e2e7b7b5d
        public static string PROJECT_SECRET = "bf1b84f0ffa6000b1c15d66975367855";
        // 验证令牌并获取用户的登录信息接口调用地址
        public string QUERY_URL = "https://ssoapi.zjzwfw.gov.cn/rest/user/query";

        /// <summary>
        ///  HmacSHA256 加密
        /// </summary>
        /// <param name="secret">projectSecret</param>
        /// <param name="data">请求的JSON参数</param>
        /// <returns></returns>
        public static string GetSignature(string data, string secret)
        {
            byte[] KeyByte = Encoding.UTF8.GetBytes(secret);
            byte[] messageBytes = Encoding.UTF8.GetBytes(data);
            using (var hmacsha256 = new HMACSHA256(KeyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                StringBuilder sb = new StringBuilder();
                foreach (byte item in hashmessage)
                {
                    sb.Append(item.ToString("x2"));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// 验证令牌并获取用户的登录信息
        /// </summary>
        /// <param name="ssotoken"></param>
        public string doQuery(string ssotoken)
        {

            // 请求参数
            JObject jsonObject = new JObject();
            jsonObject.Add("token", ssotoken);
            // 获取请求参数的JSON字符串
            string parm = JsonHelper.getParamsJSON(jsonObject);
            string signature = DigestHelper.GetSignature(parm, PROJECT_SECRET);
            Dictionary<string, string> postHeaders = new Dictionary<string, string>();
            // 项目ID
            postHeaders.Add("x-esso-project-id", PROJECT_ID);
            // 请求参数值
            postHeaders.Add("x-esso-signature", signature);
            postHeaders.Add("Charset", "UTF-8");

            // 以POST方式请求
            string result = HttpHelper.SendPOST(QUERY_URL, parm, postHeaders);
            return result;
        }
    }
}
