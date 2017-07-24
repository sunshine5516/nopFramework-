using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Services.Cms
{
    /// <summary>
    /// Widget服务接口
    /// </summary>
    public partial interface IWidgetService
    {
        /// <summary>
        /// 加载活动小部件
        /// </summary>
        /// <returns></returns>
        IList<IWidgetPlugin> LoadActiveWidgets();
        /// <summary>
        /// 加载活动小部件
        /// </summary>
        /// <param name="widgetZone"></param>
        /// <returns></returns>
        IList<IWidgetPlugin> LoadActiveWidgetsByWidgetZone(string widgetZone);
        /// <summary>
        /// 按系统名称加载小部件
        /// </summary>
        /// <param name="systemName"></param>
        /// <returns></returns>
        IWidgetPlugin LoadWidgetBySystemName(string systemName);
        /// <summary>
        /// 加载所有小部件
        /// </summary>
        /// <returns></returns>
        IList<IWidgetPlugin> LoadAllWidgets();
    }
}
