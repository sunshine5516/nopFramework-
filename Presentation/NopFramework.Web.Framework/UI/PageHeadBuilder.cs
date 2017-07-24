using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NopFramework.Web.Framework.UI
{
    /// <summary>
    /// 
    /// </summary>
    public partial class PageHeadBuilder : IPageHeadBuilder
    {
        #region 字段
        private readonly Dictionary<ResourceLocation, List<ScriptReferenceMeta>> _scriptParts;
        private readonly Dictionary<ResourceLocation, List<CssReferenceMeta>> _cssParts;
        private string _activeAdminMenuSystemName;
        #endregion
        #region 构造函数
        public PageHeadBuilder()
        {
            this._scriptParts = new Dictionary<ResourceLocation, List<ScriptReferenceMeta>>();
            this._cssParts = new Dictionary<ResourceLocation, List<CssReferenceMeta>>();
        }
        #endregion
        #region 帮助方法
        //protected virtual string GetBundleVirtualPath(string prefix, string extension, string[] parts)
        //{
        //    if (parts == null || parts.Length == 0)
        //        throw new ArgumentException("parts");

        //    //calculate hash
        //    var hash = "";
        //    using (SHA256 sha = new SHA256Managed())
        //    {
        //        // string concatenation
        //        var hashInput = "";
        //        foreach (var part in parts)
        //        {
        //            hashInput += part;
        //            hashInput += ",";
        //        }

        //        byte[] input = sha.ComputeHash(Encoding.Unicode.GetBytes(hashInput));
        //        hash = HttpServerUtility.UrlTokenEncode(input);
        //    }
        //    //ensure only valid chars
        //    hash = SeoExtensions.GetSeName(hash);

        //    var sb = new StringBuilder(prefix);
        //    sb.Append(hash);
        //    //we used "extension" when we had "runAllManagedModulesForAllRequests" set to "true" in web.config
        //    //now we disabled it. hence we should not use "extension"
        //    //sb.Append(extension);
        //    return sb.ToString();
        //}
        #endregion
        #region 方法
        public virtual void AppendCssFileParts(ResourceLocation location, string part, bool excludeFromBundle = false)
        {
            if (!_cssParts.ContainsKey(location))
                _cssParts.Add(location, new List<CssReferenceMeta>());

            if (string.IsNullOrEmpty(part))
                return;
            _cssParts[location].Insert(0, new CssReferenceMeta
            {
                ExcludeFromBundle = excludeFromBundle,
                Part = part
            });
        }

        public virtual void AppendScriptParts(ResourceLocation location, string part, bool excludeFromBundle, bool isAsync)
        {
            if (!_scriptParts.ContainsKey(location))
                _scriptParts.Add(location, new List<ScriptReferenceMeta>());

            if (string.IsNullOrEmpty(part))
                return;

            _scriptParts[location].Insert(0, new ScriptReferenceMeta
            {
                ExcludeFromBundle = excludeFromBundle,
                IsAsync = isAsync,
                Part = part
            });
        }

        public string GenerateCssFiles(UrlHelper urlHelper, ResourceLocation location, bool? bundleFiles = default(bool?))
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 指定应选择的管理菜单项的系统名称（扩展）
        /// </summary>
        /// <param name="systemName">System name</param>
        public virtual void SetActiveMenuItemSystemName(string systemName)
        {
            _activeAdminMenuSystemName = systemName;
        }
        /// <summary>
        /// 获取应选择的管理菜单项的系统名称（扩展）
        /// </summary>
        /// <returns>System name</returns>
        public virtual string GetActiveMenuItemSystemName()
        {
            return _activeAdminMenuSystemName;
        }

        //public virtual string GenerateCssFiles(UrlHelper urlHelper, ResourceLocation location, bool? bundleFiles = null)
        //{
        //    if (!_cssParts.ContainsKey(location) || _cssParts[location] == null)
        //        return "";

        //    if (!_cssParts.Any())
        //        return "";

        //    if (!bundleFiles.HasValue)
        //    {
        //        //use setting if no value is specified
        //        //bundleFiles = _seoSettings.EnableCssBundling && BundleTable.EnableOptimizations;
        //    }
        //    if (bundleFiles.Value)
        //    {
        //        var partsToBundle = _cssParts[location]
        //            .Where(x => !x.ExcludeFromBundle)
        //            .Select(x => x.Part)
        //            .Distinct()
        //            .ToArray();
        //        var partsToDontBundle = _cssParts[location]
        //            .Where(x => x.ExcludeFromBundle)
        //            .Select(x => x.Part)
        //            .Distinct()
        //            .ToArray();


        //        var result = new StringBuilder();

        //        if (partsToBundle.Length > 0)
        //        {
        //            //IMPORTANT: Do not use CSS bundling in virtual categories
        //            string bundleVirtualPath = GetBundleVirtualPath("~/bundles/styles/", ".css", partsToBundle);

        //            //create bundle
        //            lock (s_lock)
        //            {
        //                var bundleFor = BundleTable.Bundles.GetBundleFor(bundleVirtualPath);
        //                if (bundleFor == null)
        //                {
        //                    var bundle = new StyleBundle(bundleVirtualPath);
        //                    //bundle.Transforms.Clear();

        //                    //"As is" ordering
        //                    bundle.Orderer = new AsIsBundleOrderer();
        //                    //disable file extension replacements. renders scripts which were specified by a developer
        //                    bundle.EnableFileExtensionReplacements = false;
        //                    foreach (var ptb in partsToBundle)
        //                    {
        //                        bundle.Include(ptb, GetCssTranform());
        //                    }
        //                    BundleTable.Bundles.Add(bundle);
        //                }
        //            }

        //            //parts to bundle
        //            result.AppendLine(Styles.Render(bundleVirtualPath).ToString());
        //        }

        //        //parts to do not bundle
        //        foreach (var item in partsToDontBundle)
        //        {
        //            result.AppendFormat("<link href=\"{0}\" rel=\"stylesheet\" type=\"{1}\" />", urlHelper.Content(item), MimeTypes.TextCss);
        //            result.Append(Environment.NewLine);
        //        }

        //        return result.ToString();
        //    }
        //    else
        //    {
        //        //bundling is disabled
        //        var result = new StringBuilder();
        //        foreach (var path in _cssParts[location].Select(x => x.Part).Distinct())
        //        {
        //            result.AppendFormat("<link href=\"{0}\" rel=\"stylesheet\" type=\"{1}\" />", urlHelper.Content(path), MimeTypes.TextCss);
        //            result.AppendLine();
        //        }
        //        return result.ToString();
        //    }
        //}

        #endregion


        #region Nested classes

        private class ScriptReferenceMeta
        {
            public bool ExcludeFromBundle { get; set; }

            public bool IsAsync { get; set; }

            public string Part { get; set; }
        }

        private class CssReferenceMeta
        {
            public bool ExcludeFromBundle { get; set; }

            public string Part { get; set; }
        }
        #endregion
    }
}
