using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Core.Plugins
{
    /// <summary>
    /// 插件类扩展
    /// </summary>
   public static class PluginExtensions
    {
        
        private static readonly List<string> SupportedLogoImageExtensions = new List<string>
        {
            "jpg",
            "png",
            "gif"
        };
        public static string GetLogUrl(this PluginDescriptor pluginDescriptor, IWebHelper webHelper)
        //public static string GetLogUrl(this PluginDescriptor pluginDescriptor)
        {
            if (pluginDescriptor == null)
                throw new ArgumentNullException("pluginDescriptor");

            //if (webHelper == null)
            //    throw new ArgumentNullException("webHelper");
            if (pluginDescriptor.OriginalAssemblyFile == null || pluginDescriptor.OriginalAssemblyFile.Directory == null)
                return null;
            var pluginDirectory = pluginDescriptor.OriginalAssemblyFile.Directory;
            var logoExtension=SupportedLogoImageExtensions.FirstOrDefault(ext=>File.Exists(Path.Combine(pluginDirectory.FullName, "logo." + ext)));
            if (string.IsNullOrWhiteSpace(logoExtension))
                return null;
            string logoUrl = string.Format("{0}plugins/{1}/logo.{2}", webHelper.GetStoreLocation(), pluginDirectory.Name, logoExtension);
            return logoUrl;
        }
    }
}
