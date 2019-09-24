using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RsaHelper_Go
{
    //公共类工具
    public class ITool
    {
        /// <summary> 
        /// 获取时间戳 
        /// </summary> 
        /// <returns></returns> 
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
        /// <summary>
        /// 对数据传递的MD5加密
        /// </summary>
        /// <param name="str">加密方式为MD5(servicecode+servicepwd+time)</param>
        /// <returns></returns>
        public static string MD5(string str)
        {
            byte[] b = System.Text.Encoding.Default.GetBytes(str);
            b = new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
            {
                ret += b[i].ToString("x").PadLeft(2, '0');
            }
            return ret;
        }
        /// <summary>
        /// 对json数据进行按照字母排序
        /// 需要去除[]
        /// </summary>
        /// <param name="json">需要排序的json数据</param>
        /// <returns></returns>
        public static string StortJson(string json)
        {
            var dic = JsonConvert.DeserializeObject<SortedDictionary<string, object>>(json);
            SortedDictionary<string, object> keyValues = new SortedDictionary<string, object>(dic);
            keyValues.OrderBy(m => m.Key);//升序 把Key换成Value 就是对Value进行排序
            //keyValues.OrderByDescending(m => m.Key);//降序
            return JsonConvert.SerializeObject(keyValues);
        }
    }
}
