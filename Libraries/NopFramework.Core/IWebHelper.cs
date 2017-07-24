using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NopFramework.Core
{
    /// <summary>
    /// Web帮助类接口，包括获取IP地址等方法
    /// </summary>
    public partial interface IWebHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string GetUrlReferrer();
        /// <summary>
        /// 获取上下文IP地址
        /// </summary>
        /// <returns></returns>
        string GetCurrentIpAddress();
        /// <summary>
        /// 获取当前页面名称
        /// </summary>
        /// <param name="includeQueryString">是否包含查询字符串</param>
        /// <returns></returns>
        string GetThisPageUrl(bool includeQueryString);
        /// <summary>
        /// 获取当前页面名称
        /// </summary>
        /// <param name="includeQueryString">是否包含查询字符串</param>
        /// <param name="useSsl">是否获得SSL保护的页面</param>
        /// <returns></returns>
        string GetThisPageUrl(bool includeQueryString, bool useSsl);
        /// <summary>
        /// 当前连接是否安全
        /// </summary>
        /// <returns></returns>
        bool IsCurrentConnectionSecured();

        /// <summary>
        /// 通过名称获取服务器变量
        /// </summary>
        /// <param name="name"></param>
        /// <returns>服务器变量</returns>
        string ServerVariables(string name);
        /// <summary>
        /// 获取主机位置
        /// </summary>
        /// <param name="useSsl"></param>
        /// <returns></returns>
        string GetStoreHost(bool useSsl);
        /// <summary>
        /// Gets store location
        /// </summary>
        /// <returns>Store location</returns>
        string GetStoreLocation();
        /// <summary>
        /// Gets store location
        /// </summary>
        /// <param name="useSsl">Use SSL</param>
        /// <returns>Store location</returns>
        string GetStoreLocation(bool useSsl);
        /// <summary>
        /// 如果请求的资源是cms引擎不需要处理的典型资源之一，则返回true。
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        bool IsStaticResource(HttpRequest request);
        /// <summary>
        /// 修改查询字符串
        /// </summary>
        /// <param name="url">要修改的url</param>
        /// <param name="queryStringModification">查询字符串修改</param>
        /// <param name="anhcor"></param>
        /// <returns></returns>
        string ModifyQueryString(string url, string queryStringModification, string anhcor);
        /// <summary>
        /// 从url中删除查询字符串
        /// </summary>
        /// <param name="url">要修改的url</param>
        /// <param name="queryString">要删除的查询字符串</param>
        /// <returns>新的url</returns>
        string RemoveQueryString(string url, string queryString);
        /// <summary>
        /// 通过名称获取查询字符串值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">名称</param>
        /// <returns>查询字符串值</returns>
        T QueryString<T>(string name);
        /// <summary>
        /// 重新启动应用程序域
        /// </summary>
        /// <param name="makeRedirect">重启后是否应该重定向</param>
        /// <param name="redirectUrl">重定向网址; 如果要重定向到当前页面URL，则为空字符串</param>
        void RestartAppDomain(bool makeRedirect = false, string redirectUrl = "");
        /// <summary>
        /// 客户端是否被重定向到新位置
        /// </summary>
        bool IsRequestBeingRedirected { get; }

        /// <summary>
        /// 客户端是否使用POST重定向到新位置
        /// </summary>
        bool IsPostBeingDone { get; set; }
    }
}
