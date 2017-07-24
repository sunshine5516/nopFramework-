using NopFramework.Core.ComponentModel;
using NopFramework.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Compilation;
//Application开始运行前就运行这个类PluginManager的方法Initialize
[assembly: PreApplicationStartMethod(typeof(PluginManager), "Initialize")]
namespace NopFramework.Core.Plugins
{
    /// <summary>
    /// 插件管理类
    /// 在程序运行的时候最开始就要执行PluginFinder的初始化Initialize方法，获取插件目录所有的插件及描述信息及其它初始化操作。
    /// 可以把PluginFinder理解成主要用来管理插件的，就像windows任务管理器一样。
    /// </summary>
    public class PluginManager
    {
        #region 常量
        private const string InstalledPluginsFilePath = "~/App_Data/InstalledPlugins.txt";
        private const string PluginsPath = "~/Plugins";
        private const string ShadowCopyPath = "~/Plugins/bin";
        #endregion
        #region 字段
        private static readonly ReaderWriterLockSlim Locker = new ReaderWriterLockSlim();
        private static DirectoryInfo _shadowCopyFolder;
        private static bool _clearShadowDirectoryOnStartup;
        public static IEnumerable<PluginDescriptor> ReferencedPlugins { get; set; }
        /// <summary>
        ///返回与当前版本不兼容的所有插件的集合
        /// </summary>
        public static IEnumerable<string> IncompatiblePlugins { get; set; }
        #endregion
        #region 方法
        /// <summary>
        /// 初始化
        /// </summary>
        public static void Initialize()
        {
            using (new WriteLockDisposable(Locker))
            {
                var pluginFolder = new DirectoryInfo(CommonHelper.MapPath(PluginsPath));
                _shadowCopyFolder = new DirectoryInfo(CommonHelper.MapPath(ShadowCopyPath));
                var referencedPlugins = new List<PluginDescriptor>();
                var incompatiblePlugins = new List<string>();
                _clearShadowDirectoryOnStartup = !String.IsNullOrEmpty(ConfigurationManager.AppSettings["ClearPluginsShadowDirectoryOnStartup"]) &&
                    Convert.ToBoolean(ConfigurationManager.AppSettings["ClearPluginsShadowDirectoryOnStartup"]);
                try
                {
                    var installedPluginSystemNames = PluginFileParser.ParseInstalledPluginsFile(GetInstalledPluginsFilePath());
                    Debug.WriteLine("Creating shadow copy folder and querying for dlls");
                    Directory.CreateDirectory(pluginFolder.FullName);
                    Directory.CreateDirectory(_shadowCopyFolder.FullName);
                    //获取bin中所有文件的列表
                    var binFiles = _shadowCopyFolder.GetFiles("*",SearchOption.AllDirectories);
                    if (_clearShadowDirectoryOnStartup)
                    {
                        foreach (var f in binFiles)
                        {
                            Debug.WriteLine("Deleting"+f.Name);
                            try
                            {
                                File.Delete(f.FullName);
                            }
                            catch (Exception exc)
                            {
                                Debug.WriteLine("Error deleting file " + f.Name + ". Exception: " + exc);
                            }
                        }
                    }
                    ///加载描述文件
                    foreach (var dfd in GetDescriptionFilesAndDescriptors(pluginFolder))
                    {
                        var descriptionFile = dfd.Key;
                        var pluginDescriptor = dfd.Value;
                        //var isSupport = pluginDescriptor.SupportedVersions.Contains(SunFrameworkVersion.CurrentVersion, StringComparer.InvariantCultureIgnoreCase);
                        ///确保版本正确
                        if (!pluginDescriptor.SupportedVersions.Contains(SunFrameworkVersion.CurrentVersion, StringComparer.InvariantCultureIgnoreCase))
                        {
                            incompatiblePlugins.Add(pluginDescriptor.SystemName);
                            continue;
                        }
                        //some validation
                        if (String.IsNullOrWhiteSpace(pluginDescriptor.SystemName))
                            throw new Exception(string.Format("A plugin '{0}' has no system name. Try assigning the plugin a unique name and recompiling.", descriptionFile.FullName));
                        if (referencedPlugins.Contains(pluginDescriptor))
                            throw new Exception(string.Format("A plugin with '{0}' system name is already defined", pluginDescriptor.SystemName));
                        //set 'Installed' property
                        pluginDescriptor.Installed = installedPluginSystemNames
                            .FirstOrDefault(x => x.Equals(pluginDescriptor.SystemName, StringComparison.InvariantCultureIgnoreCase)) != null;
                        try
                        {
                            if (descriptionFile.Directory == null)
                                throw new Exception(string.Format("Directory cannot be resolved for '{0}' description file", descriptionFile.Name));
                            //获取插件中所有dll的列表 (not in bin!)
                            var pluginFiles = descriptionFile.Directory.GetFiles("*dll", SearchOption.AllDirectories)
                                .Where(x => !binFiles.Select(q => q.Name).Contains(x.FullName))
                                .Where(x => IsPackagePluginFolder(x.Directory))
                                .ToList();
                            //插件其他描述信息
                            var mainPluginFile = pluginFiles
                                .FirstOrDefault(x => x.Name.Equals(pluginDescriptor.PluginFileName, StringComparison.InvariantCultureIgnoreCase));
                            pluginDescriptor.OriginalAssemblyFile = mainPluginFile;
                            //shadow copy main plugin file
                            pluginDescriptor.ReferencedAssembly = PerformFileDeploy(mainPluginFile);
                            //加载所有其他引用的程序集
                            foreach (var plugin in pluginFiles
                                .Where(x => !x.Name.Equals(mainPluginFile.Name, StringComparison.InvariantCultureIgnoreCase))
                                .Where(x => !IsAlreadyLoaded(x)))
                                PerformFileDeploy(plugin);
                            //初始化插件（每个程序集只允许一个插件）
                            foreach (var t in pluginDescriptor.ReferencedAssembly.GetTypes())
                                if (typeof(IPlugin).IsAssignableFrom(t))
                                    if (!t.IsInterface)
                                        if (t.IsClass && !t.IsAbstract)
                                        {
                                            pluginDescriptor.PluginType = t;
                                            break;
                                        }

                            referencedPlugins.Add(pluginDescriptor);
                        }
                        catch (ReflectionTypeLoadException ex)
                        {
                            //add a plugin name. this way we can easily identify a problematic plugin
                            var msg = string.Format("Plugin '{0}'. ", pluginDescriptor.FriendlyName);
                            foreach (var e in ex.LoaderExceptions)
                                msg += e.Message + Environment.NewLine;

                            var fail = new Exception(msg, ex);
                            throw fail;
                        }
                        catch (Exception ex)
                        {
                            //add a plugin name. this way we can easily identify a problematic plugin
                            var msg = string.Format("Plugin '{0}'. {1}", pluginDescriptor.FriendlyName, ex.Message);

                            var fail = new Exception(msg, ex);
                            throw fail;
                        }
                    }
                }
                catch (Exception ex)
                {
                    var msg = string.Empty;
                    for (var e = ex; e != null; e = e.InnerException)
                        msg += e.Message + Environment.NewLine;

                    var fail = new Exception(msg, ex);
                    throw fail;
                }
                ReferencedPlugins = referencedPlugins;
                IncompatiblePlugins = incompatiblePlugins;
            }
        }
        /// <summary>
        /// 标记插件为已安装状态
        /// </summary>
        /// <param name="systemName"></param>
        public static void MarkPluginAsInstalled(string systemName)
        {
            if (String.IsNullOrEmpty(systemName))
                throw new ArgumentNullException("SystemName");
            var filePath = CommonHelper.MapPath(InstalledPluginsFilePath);
            if (!File.Exists(filePath))
                using (File.Create(filePath))
                { }
            var installedPluginSystemNames = PluginFileParser.ParseInstalledPluginsFile(GetInstalledPluginsFilePath());
            bool alreadyMarkedAsInstalled = installedPluginSystemNames.FirstOrDefault
                (x => x.Equals(systemName, StringComparison.InvariantCultureIgnoreCase)) != null;
            if (!alreadyMarkedAsInstalled)
                installedPluginSystemNames.Add(systemName);
            PluginFileParser.SaveInstalledPluginsFile(installedPluginSystemNames,filePath);
        }
        /// <summary>
        /// 标记为未安装状态
        /// </summary>
        /// <param name="systemName"></param>
        public static void MarkPluginAsUninstalled(string systemName)
        {
            if (String.IsNullOrEmpty(systemName))
                throw new ArgumentNullException("SystemName");
            var filePath = CommonHelper.MapPath(InstalledPluginsFilePath);
            if (!File.Exists(filePath))
                using (File.Create(filePath))
                { }
            var installedPluginSystemNames = PluginFileParser.ParseInstalledPluginsFile(GetInstalledPluginsFilePath());
            bool alreadyMarkedAsInstalled = installedPluginSystemNames
                                .FirstOrDefault(x => x.Equals(systemName, StringComparison.InvariantCultureIgnoreCase)) != null;
            if (alreadyMarkedAsInstalled)
                installedPluginSystemNames.Remove(systemName);
            PluginFileParser.SaveInstalledPluginsFile(installedPluginSystemNames, filePath);
        }
        /// <summary>
        /// 删除所有插件
        /// </summary>
        public static void MarkAllPluginsAsUninstalled()
        {
            var filePath = CommonHelper.MapPath(InstalledPluginsFilePath);
            if (File.Exists(filePath))
                File.Delete(filePath);
        }
        #endregion
        #region 辅助方法
        /// <summary>
        /// 获取插件描述文件
        /// </summary>
        /// <param name="pluginFolder">插件目录信息</param>
        /// <returns>原始解析过的插件实体类</returns>
        private static IEnumerable<KeyValuePair<FileInfo, PluginDescriptor>> GetDescriptionFilesAndDescriptors(DirectoryInfo pluginFolder)
        {
            if(pluginFolder==null)
                throw new ArgumentNullException("pluginFolder");
            var result = new List<KeyValuePair<FileInfo, PluginDescriptor>>();
            //Description.txt插件描述文件
            //add display order and path to list
            //把Description.txt文件转化成PluginDescriptor类
            foreach (var descriptionFile in pluginFolder.GetFiles("Description.txt", SearchOption.AllDirectories))
            {
                if (!IsPackagePluginFolder(descriptionFile.Directory))
                    continue;
                var pluginDescriptor = PluginFileParser.ParsePluginDescriptionFile(descriptionFile.FullName);
                result.Add(new KeyValuePair<FileInfo, PluginDescriptor>(descriptionFile, pluginDescriptor));
            }
            result.Sort((firstPair, nextPair)=> firstPair.Value.DisplayOrder.CompareTo(nextPair.Value.DisplayOrder));
            return result;
        }
        private static bool IsPackagePluginFolder(DirectoryInfo folder)
        {
            if (folder == null)
                return false;
            if (folder.Parent == null)
                return false;
            if (!folder.Parent.Name.Equals("Plugins", StringComparison.InvariantCultureIgnoreCase))
                return false;
            return true;
        }
        /// <summary>
        /// 获取InstalledPlugins.txt文件的完整路径
        /// </summary>
        /// <returns></returns>
        private static string GetInstalledPluginsFilePath()
        {
            return CommonHelper.MapPath(InstalledPluginsFilePath);
        }
        /// <summary>
        /// 执行文件部署
        /// </summary>
        /// <param name="plug">Plugin file info</param>
        /// <returns>Assembly</returns>
        private static Assembly PerformFileDeploy(FileInfo plug)
        {
            if (plug.Directory.Parent == null)
                throw new InvalidOperationException("The plugin directory for the " + plug.Name +
                                                    " file exists in a folder outside of the allowed nopCommerce folder hierarchy");

            FileInfo shadowCopiedPlug;

            if (CommonHelper.GetTrustLevel() != AspNetHostingPermissionLevel.Unrestricted)
            {
                //all plugins will need to be copied to ~/Plugins/bin/
                //this is absolutely required because all of this relies on probingPaths being set statically in the web.config

                //were running in med trust, so copy to custom bin folder
                var shadowCopyPlugFolder = Directory.CreateDirectory(_shadowCopyFolder.FullName);
                shadowCopiedPlug = InitializeMediumTrust(plug, shadowCopyPlugFolder);
            }
            else
            {
                var directory = AppDomain.CurrentDomain.DynamicDirectory;
                Debug.WriteLine(plug.FullName + " to " + directory);
                //were running in full trust so copy to standard dynamic folder
                shadowCopiedPlug = InitializeFullTrust(plug, new DirectoryInfo(directory));
            }

            //we can now register the plugin definition
            var shadowCopiedAssembly = Assembly.Load(AssemblyName.GetAssemblyName(shadowCopiedPlug.FullName));

            //add the reference to the build manager
            Debug.WriteLine("Adding to BuildManager: '{0}'", shadowCopiedAssembly.FullName);
            BuildManager.AddReferencedAssembly(shadowCopiedAssembly);

            return shadowCopiedAssembly;
        }
        /// <summary>
        /// 中等信任初始化插件
        /// </summary>
        /// <param name="plug"></param>
        /// <param name="shadowCopyPlugFolder"></param>
        /// <returns></returns>
        private static FileInfo InitializeMediumTrust(FileInfo plug, DirectoryInfo shadowCopyPlugFolder)
        {
            var shouldCopy = true;
            var shadowCopiedPlug = new FileInfo(Path.Combine(shadowCopyPlugFolder.FullName, plug.Name));

            //check if a shadow copied file already exists and if it does, check if it's updated, if not don't copy
            if (shadowCopiedPlug.Exists)
            {
                //it's better to use LastWriteTimeUTC, but not all file systems have this property
                //maybe it is better to compare file hash?
                var areFilesIdentical = shadowCopiedPlug.CreationTimeUtc.Ticks >= plug.CreationTimeUtc.Ticks;
                if (areFilesIdentical)
                {
                    Debug.WriteLine("Not copying; files appear identical: '{0}'", shadowCopiedPlug.Name);
                    shouldCopy = false;
                }
                else
                {
                    //delete an existing file

                    //More info: http://www.nopcommerce.com/boards/t/11511/access-error-nopplugindiscountrulesbillingcountrydll.aspx?p=4#60838
                    Debug.WriteLine("New plugin found; Deleting the old file: '{0}'", shadowCopiedPlug.Name);
                    File.Delete(shadowCopiedPlug.FullName);
                }
            }

            if (shouldCopy)
            {
                try
                {
                    File.Copy(plug.FullName, shadowCopiedPlug.FullName, true);
                }
                catch (IOException)
                {
                    Debug.WriteLine(shadowCopiedPlug.FullName + " is locked, attempting to rename");
                    //this occurs when the files are locked,
                    //for some reason devenv locks plugin files some times and for another crazy reason you are allowed to rename them
                    //which releases the lock, so that it what we are doing here, once it's renamed, we can re-shadow copy
                    try
                    {
                        var oldFile = shadowCopiedPlug.FullName + Guid.NewGuid().ToString("N") + ".old";
                        File.Move(shadowCopiedPlug.FullName, oldFile);
                    }
                    catch (IOException exc)
                    {
                        throw new IOException(shadowCopiedPlug.FullName + " rename failed, cannot initialize plugin", exc);
                    }
                    //ok, we've made it this far, now retry the shadow copy
                    File.Copy(plug.FullName, shadowCopiedPlug.FullName, true);
                }
            }

            return shadowCopiedPlug;
        }
        /// <summary>
        /// 完全信任初始化插件
        /// </summary>
        /// <param name="plug"></param>
        /// <param name="shadowCopyPlugFolder"></param>
        /// <returns></returns>
        private static FileInfo InitializeFullTrust(FileInfo plug, DirectoryInfo shadowCopyPlugFolder)
        {
            var shadowCopiedPlug = new FileInfo(Path.Combine(shadowCopyPlugFolder.FullName, plug.Name));
            try
            {
                File.Copy(plug.FullName, shadowCopiedPlug.FullName, true);
            }
            catch (IOException)
            {
                Debug.WriteLine(shadowCopiedPlug.FullName + " is locked, attempting to rename");
                //this occurs when the files are locked,
                //for some reason devenv locks plugin files some times and for another crazy reason you are allowed to rename them
                //which releases the lock, so that it what we are doing here, once it's renamed, we can re-shadow copy
                try
                {
                    var oldFile = shadowCopiedPlug.FullName + Guid.NewGuid().ToString("N") + ".old";
                    File.Move(shadowCopiedPlug.FullName, oldFile);
                }
                catch (IOException exc)
                {
                    throw new IOException(shadowCopiedPlug.FullName + " rename failed, cannot initialize plugin", exc);
                }
                //ok, we've made it this far, now retry the shadow copy
                File.Copy(plug.FullName, shadowCopiedPlug.FullName, true);
            }
            return shadowCopiedPlug;
        }
        /// <summary>
        /// 程序集是否已加载
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        private static bool IsAlreadyLoaded(FileInfo fileInfo)
        {
            try
            {
                string fileNameWithoutExt = Path.GetFileNameWithoutExtension(fileInfo.FullName);
                if (fileNameWithoutExt == null)
                    throw new Exception(string.Format("Cannot get file extension for {0}", fileInfo.Name));
                foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
                {
                    string assemblyName = a.FullName.Split(new[] { ',' }).FirstOrDefault();
                    if (fileNameWithoutExt.Equals(assemblyName, StringComparison.InvariantCultureIgnoreCase))
                        return true;
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine("Cannot validate whether an assembly is already loaded. " + exc);
            }
            return false;
        }
        #endregion
    }
}
