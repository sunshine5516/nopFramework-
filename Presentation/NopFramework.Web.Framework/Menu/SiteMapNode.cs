using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace NopFramework.Web.Framework.Menu
{
    public class SiteMapNode
    {
        public SiteMapNode()
        {
            RouteValues = new RouteValueDictionary();
            ChildNodes = new List<SiteMapNode>();
        }
        public string SystemName { get; set; }
        public string Title { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        /// <summary>
        /// 路由值.
        /// </summary>
        public RouteValueDictionary RouteValues { get; set; }
        public string Url { get; set; }
        /// <summary>
        /// 子节点
        /// </summary>
        public IList<SiteMapNode> ChildNodes { get; set; }
        /// <summary>
        /// 图标(Font Awesome: http://fontawesome.io/)
        /// </summary>
        public string IconClass { get; set; }

        /// <summary>
        /// 是否可见
        /// </summary>
        public bool Visible { get; set; }
    }
}
