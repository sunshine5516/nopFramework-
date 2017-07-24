using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Web.Framework.Menu
{
    public static class Extensions
    {
        public static bool ContainsSystemName(this SiteMapNode node, string systemName)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            if (String.IsNullOrWhiteSpace(systemName))
                return false;

            if (systemName.Equals(node.SystemName, StringComparison.InvariantCultureIgnoreCase))
                return true;

            return node.ChildNodes.Any(cn => ContainsSystemName(cn, systemName));
        }
    }
}
