using NopFramework.Web.Framework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NopFramework.Web.Framework.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// 成功信息
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="persistForTheNextRequest"></param>
        public virtual void SuccessNotification(string message, bool persistForTheNextRequest = true)
        {
            AddNotification(NotifyType.Success, message, persistForTheNextRequest);
        }
        /// <summary>
        /// 显示通知
        /// </summary>
        /// <param name="type">通知类型</param>
        /// <param name="message">信息</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        protected virtual void AddNotification(NotifyType type, string message, bool persistForTheNextRequest)
        {
            string dataKey = string.Format("NopFramework.notifications.{0}", type);
            if (persistForTheNextRequest)
            {
                if (TempData[dataKey] == null)
                {
                    TempData[dataKey] = new List<string>();

                }
                ((List<string>)TempData[dataKey]).Add(message);
            }
            else
            {
                if (ViewData[dataKey] == null)
                    ViewData[dataKey] = new List<string>();
                ((List<string>)ViewData[dataKey]).Add(message);
            }
            
        }
    }
}
