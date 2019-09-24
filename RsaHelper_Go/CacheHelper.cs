//using Microsoft.AspNetCore.Http;
//using System;
//using System.Collections.Generic;
//using System.Net.Http;
//using System.Text;

//namespace RsaHelper_Go
//{
//    public class CacheHelper
//    {
//        // <summary>
//        /// 
//        /// </summary>
//        /// <param name="key"></param>
//        /// <param name="obj"></param>
//        #region 测试C#使用Cache
//        public static void Insert(string key, object obj)
//        {
//            //创建缓存
//            HttpContext
//        }
//        //移除缓存项的文件
//        public static void Remove(string key)
//        {
//            HttpContext.Current.Cache.Remove(key);
//        }
//        /// <summary>
//        /// 创建缓存文件依赖
//        /// </summary>
//        /// <param name="key">缓存key</param>
//        /// <param name="obj">Object对象</param>
//        /// <param name="filename">文件绝对路径</param>
//        public static void Insert(string key, object obj, string filename)
//        {
//            //缓     存依赖项
//            CacheDependency dep = new CacheDependency(filename);
//            //创建缓存
//            HttpContext.Current.Cache.Insert(key, obj, dep);
//        }
//        /// <summary>                                   
//        /// 创建缓存项过期
//        /// </summary>
//        /// <param name="key">缓存key</param>
//        /// <param name="obj">Object 对象</param>
//        /// <param name="expires">过期时间（分钟）</param>
//        public static void Insert(string key, object obj, int expires)
//        {
//            HttpContent.Current.Cache.Insert(key, obj, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, expires, 0));
//        }
//        /// <summary>
//        /// 获取缓存对象
//        /// </summary>
//        /// <param name="key">缓存key</param>
//        /// <returns>object对象</returns>
//        public static object get(string key)
//        {
//            if (string.IsNullOrEmpty(key))
//            {
//                return null;
//            }
//            try
//            {
//                return HttpContext.Current.Cache.Get(key);
//            }
//            catch
//            {
//                return null;
//            }
//        }
//        /// <summary>
//        /// 获取缓存对象
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="key"></param>
//        /// <returns></returns>
//        public static T GetT<T>(string key)
//        {
//            object obj = get(key);
//            return obj == null ? default(T) : (T)obj;
//        }
//        /// <summary>
//        /// 获取缓存数据
//        /// </summary>
//        /// <param name="Cache">键</param>
//        /// <returns></returns>
//        public static object GetCache(string Cache)
//        {
//            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
//            return objCache[Cache];
//        }
//        /// <summary>
//        /// 设置数据缓存
//        /// </summary>
//        /// <param name="Cache">键</param>
//        public static void setCache(string Cache)
//        {
//            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
//            objCache.Insert(Cache, objCache);
//        }
//        /// <summary>
//        /// 移除指定缓存数据
//        /// </summary>
//        /// <param name="Cache"></param>
//        public static void RemoveAll(string Cache)
//        {
//            System.Web.Caching.Cache _cache = HttpRuntime.Cache;
//            _cache.Remove(Cache);
//        }
//        /// <summary>
//        /// 移除全部缓存数据
//        /// </summary>
//        public static void RemoveAll()
//        {
//            System.Web.Caching.Cache _cache = HttpRuntime.Cache;
//            IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
//            while (CacheEnum.MoveNext())
//            {
//                _cache.Remove(CacheEnum.Key.ToString());
//            }
//        }
//        #endregion
//    }
//}
