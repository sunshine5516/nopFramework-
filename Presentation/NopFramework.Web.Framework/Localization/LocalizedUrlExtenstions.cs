using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Web.Framework.Localization
{
    public static class LocalizedUrlExtenstions
    {
        private static int _seoCodeLength = 2;

        /// <summary>
        /// 返回一个值，指示nopCommerce是否在虚拟目录中运行
        /// </summary>
        /// <param name="applicationPath">Application path</param>
        /// <returns>Result</returns>
        private static bool IsVirtualDirectory(this string applicationPath)
        {
            if (string.IsNullOrEmpty(applicationPath))
                throw new ArgumentException("Application path is not specified");

            return applicationPath != "/";
        }
        /// <summary>
        /// 从原始URL中删除应用程序路径
        /// </summary>
        /// <param name="rawUrl">Raw URL</param>
        /// <param name="applicationPath">Application path</param>
        /// <returns>Result</returns>
        public static string RemoveApplicationPathFromRawUrl(this string rawUrl, string applicationPath)
        {
            if (string.IsNullOrEmpty(applicationPath))
                throw new ArgumentException("Application path is not specified");

            if (rawUrl.Length == applicationPath.Length)
                return "/";


            var result = rawUrl.Substring(applicationPath.Length);
            //raw url always starts with '/'
            if (!result.StartsWith("/"))
                result = "/" + result;
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="applicationPath"></param>
        /// <param name="isRawPath"></param>
        /// <returns></returns>
        public static bool IsLocalizedUrl(this string url, string applicationPath, bool isRawPath)
        {
            if (string.IsNullOrEmpty(url))
                return false;
            if (isRawPath)
            {
                if (applicationPath.IsVirtualDirectory())
                {
                    url = url.RemoveApplicationPathFromRawUrl(applicationPath);
                }
                int length = url.Length;
                if (length < 1 + _seoCodeLength)
                    return false;

                //url like "/en"
                if (length == 1 + _seoCodeLength)
                    return true;

                //urls like "/en/" or "/en/somethingelse"
                return (length > 1 + _seoCodeLength) && (url[1 + _seoCodeLength] == '/');
            }
            else
            {
                int length = url.Length;
                //too short url
                if (length < 2 + _seoCodeLength)
                    return false;

                //url like "/en"
                if (length == 2 + _seoCodeLength)
                    return true;

                //urls like "/en/" or "/en/somethingelse"
                return (length > 2 + _seoCodeLength) && (url[2 + _seoCodeLength] == '/');
            }
        }
        /// <summary>
        /// 从URL中删除语言SEO代码
        /// </summary>
        /// <param name="url">Raw URL</param>
        /// <param name="applicationPath">Application path</param>
        /// <returns>Result</returns>
        public static string RemoveLanguageSeoCodeFromRawUrl(this string url, string applicationPath)
        {
            if (string.IsNullOrEmpty(url))
                return url;

            string result = null;
            if (applicationPath.IsVirtualDirectory())
            {
                //we're in virtual directory. So remove its path
                url = url.RemoveApplicationPathFromRawUrl(applicationPath);
            }

            int length = url.Length;
            if (length < _seoCodeLength + 1)    //too short url
                result = url;
            else if (length == 1 + _seoCodeLength)  //url like "/en"
                result = url.Substring(0, 1);
            else
                result = url.Substring(_seoCodeLength + 1); //urls like "/en/" or "/en/somethingelse"

            if (applicationPath.IsVirtualDirectory())
                result = applicationPath + result;  //add back applciation path
            return result;
        }
    }
}
