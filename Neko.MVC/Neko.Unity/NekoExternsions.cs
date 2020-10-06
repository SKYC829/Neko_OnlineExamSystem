using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using Util;
using Util.Common;

namespace Neko.Unity
{
    public static class NekoExternsions
    {
        #region 一些简单的Cookie的扩展方法
        /// <summary>
        /// 添加Cookie
        /// </summary>
        /// <param name="cookies"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddCookie(this IResponseCookies cookies,string key,string value)
        {
            try
            {
                cookies.Append(key, value);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 添加Cookie，并设置过期时间(单位：天)
        /// </summary>
        /// <param name="cookies"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="effectDay"></param>
        public static void AddCookie(this IResponseCookies cookies,string key,string value,int effectDay)
        {
            try
            {
                cookies.Append(key, value, new CookieOptions()
                {
                    Expires = DateTime.Now.AddDays(effectDay)
                });
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 添加Cookie，并设置过期时间
        /// </summary>
        /// <param name="cookies"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="effectDay"></param>
        public static void AddCookie(this IResponseCookies cookies,string key,string value,DateTime effectDay)
        {
            try
            {
                cookies.Append(key, value, new CookieOptions()
                {
                    Expires = effectDay
                });
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 添加Cookie，并设置过期时间
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="cookies"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="effectDay"></param>
        public static void AddCookie<TObject>(this IResponseCookies cookies, string key, TObject value, DateTime effectDay)
        {
            try
            {
                cookies.Append(key, SerializeUtil.ToJson(value), new CookieOptions()
                {
                    Expires = effectDay
                });
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="cookies"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetCookie(this IRequestCookieCollection cookies,string key)
        {
            cookies.TryGetValue(key, out string result);
            return result;
        }

        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="cookies"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TObject GetCookie<TObject>(this IRequestCookieCollection cookies, string key)
        {
            TObject result = default;
            if(cookies.TryGetValue(key,out string json))
            {
                result = SerializeUtil.FromJson<TObject>(json);
            }
            return result;
        }

        /// <summary>
        /// 删除Cookie
        /// </summary>
        /// <param name="cookies"></param>
        /// <param name="key"></param>
        public static void DeleteCookie(this IResponseCookies cookies, string key)
        {
            cookies.Delete(key);
        }
        #endregion
    }
}
