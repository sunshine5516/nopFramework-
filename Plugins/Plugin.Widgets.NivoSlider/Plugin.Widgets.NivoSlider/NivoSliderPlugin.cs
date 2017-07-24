using NopFramework.Core;
using NopFramework.Core.Plugins;
using NopFramework.Services.Cms;
using NopFramework.Services.Configuration;
using NopFramework.Services.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace Plugin.Widgets.NivoSlider
{
    /// <summary>
    /// Plugin是插件的核心类，它继承于基类BasePlugin和实现接口IWidgetPlugin，
    /// 作用是插件最终要执行的Controller与Action的路由配置信息、插件安装，卸载以及插件调用的区域名字（GetWidgetZones）等等
    /// </summary>
    public class NivoSliderPlugin : BasePlugin, IWidgetPlugin
    {
        #region 声明实例
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
        private readonly IPictureService _pictureService;
        #endregion
        #region 构造函数
        public NivoSliderPlugin(ISettingService settingService, IWebHelper webHelper, IPictureService _pictureService)
        {
            this._settingService = settingService;
            this._webHelper = webHelper;
            this._pictureService = _pictureService;
        }
        #endregion
        #region 方法
        /// <summary>
        /// 获取该插件的所有区域名称，到时可以通过Html.Widget来调用
        /// </summary>
        /// <returns></returns>
        public IList<string> GetWidgetZones()
        {
            return new List<string> { "home_page_top" };
        }
        /// <summary>
        /// 获取插件的路由配置信息
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "WidgetsNivoSlider";
            routeValues = new RouteValueDictionary
            {
                { "Namespaces", "Plugin.Widgets.NivoSlider.Controllers" },
                { "area", null }
            };
        }
        /// <summary>
        /// 获取要显示的指定区域的路由信息
        /// </summary>
        /// <param name="widgetZone"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        public void GetDisplayWidgetRoute(string widgetZone, out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "PublicInfo";
            controllerName = "WidgetsNivoSlider";
            routeValues = new RouteValueDictionary
            {
                { "Namespaces", "Plugin.Widgets.NivoSlider.Controllers" },
                {"area", null},
                {"widgetZone", widgetZone}
            };
        }
        /// <summary>
        /// 插件安装
        /// </summary>
        public override void Install()
        {
            var sampleImagesPath = CommonHelper.MapPath("~/Plugins/Widgets.NivoSlider/Content/nivoslider/sample-images/");
            var settings = new NivoSliderSettings
            {
                Picture1Id = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "banner1.jpg"), MimeTypes.ImagePJpeg, "banner_1").Id,
                Text1 = "Nokia1020",
                Link1 = _webHelper.GetStoreLocation(false),

                Picture2Id = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "banner2.jpg"), MimeTypes.ImagePJpeg, "banner_2").Id,
                Text2 = "IPAD",
                Link2 = _webHelper.GetStoreLocation(false),
                Picture3Id = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "banner3.jpg"), MimeTypes.ImagePJpeg, "banner_3").Id,
                Text3 = "Iphone",
                //Link3 = _webHelper.GetStoreLocation(false),
                Link3 = "http://www.baidu.com",
                Picture4Id = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "banner4.jpg"), MimeTypes.ImagePJpeg, "banner_4").Id,
                Text4 = "Iphone",
                //Link3 = _webHelper.GetStoreLocation(false),
                Link4 = "http://www.facebook.com",
                Picture5Id = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "banner5.jpg"), MimeTypes.ImagePJpeg, "banner_5").Id,
                Text5 = "Iphone",
                //Link3 = _webHelper.GetStoreLocation(false),
                Link5 = "http://www.google.com",
            };
            _settingService.SaveSetting(settings);
            base.Install();
        }
        public override void Uninstall()
        {
            var settings = _settingService.LoadSetting<NivoSliderSettings>();
            if (settings != null)
            {
                _pictureService.DeletePicture(_pictureService.GetPictureById(settings.Picture1Id));
                _pictureService.DeletePicture(_pictureService.GetPictureById(settings.Picture2Id));
                _pictureService.DeletePicture(_pictureService.GetPictureById(settings.Picture3Id));
                _pictureService.DeletePicture(_pictureService.GetPictureById(settings.Picture4Id));
                _pictureService.DeletePicture(_pictureService.GetPictureById(settings.Picture5Id));
                //foreach(var pictureId in settings.pi)
            }
            //settings
            _settingService.DeleteSetting<NivoSliderSettings>();

            base.Uninstall();
        }
        #endregion


    }
}
