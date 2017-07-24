using NopFramework.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Plugin.Widgets.NivoSlider.Models
{
    public class ConfigurationModel: BaseNopFrameworkModel
    {
        [DisplayNameAttribute("图片")]
        [UIHint("Picture")]
        public int Picture1Id { get; set; }
        [DisplayNameAttribute("文字")]
        [AllowHtml]
        public string Text1 { get; set; }
        [DisplayNameAttribute("链接")]
        [AllowHtml]
        public string Link1 { get; set; }
        [DisplayNameAttribute("图片")]
        [UIHint("Picture")]
        public int Picture2Id { get; set; }
        [DisplayNameAttribute("文字")]
        [AllowHtml]
        public string Text2 { get; set; }
        [DisplayNameAttribute("链接")]
        [AllowHtml]
        public string Link2 { get; set; }
        [DisplayNameAttribute("图片")]
        [UIHint("Picture")]
        public int Picture3Id { get; set; }
        [DisplayNameAttribute("文字")]
        [AllowHtml]
        public string Text3 { get; set; }
        [DisplayNameAttribute("链接")]
        [AllowHtml]
        public string Link3 { get; set; }
        [DisplayNameAttribute("图片")]
        [UIHint("Picture")]
        public int Picture4Id { get; set; }
        [DisplayNameAttribute("文字")]
        [AllowHtml]
        public string Text4 { get; set; }
        [DisplayNameAttribute("链接")]
        [AllowHtml]
        public string Link4 { get; set; }
        [DisplayNameAttribute("图片")]
        [UIHint("Picture")]
        public int Picture5Id { get; set; }
        [DisplayNameAttribute("文字")]
        [AllowHtml]
        public string Text5 { get; set; }
        [DisplayNameAttribute("链接")]
        [AllowHtml]
        public string Link5 { get; set; }

    }
}
