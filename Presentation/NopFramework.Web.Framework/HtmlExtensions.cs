using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace NopFramework.Web.Framework
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString Widget(this HtmlHelper helper, string widgetZone, object additionalData = null, string area = null)
        {
            return helper.Action("WidgetsByZone", "Widget", new { widgetZone = widgetZone, additionalData = additionalData, area = area });
        }

        #region 表单字段
        public static MvcHtmlString Hint(this HtmlHelper helper, string value)
        {
            //create tag builder
            var builder = new TagBuilder("div");
            builder.MergeAttribute("title", value);
            builder.MergeAttribute("class", "ico-help");
            var icon = new StringBuilder();
            icon.Append("<i class='fa fa-question-circle'></i>");
            builder.InnerHtml = icon.ToString();
            //render tag
            return MvcHtmlString.Create(builder.ToString());
            //sdf 
        }
        /// <summary>
        /// 自定义lable
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="helper"></param>
        /// <param name="expression"></param>
        /// <param name="displayHint"></param>
        /// <returns></returns>
        public static MvcHtmlString NopFrameowrkLabelFor<TModel, TValue>(this HtmlHelper<TModel> helper,
               Expression<Func<TModel, TValue>> expression, bool displayHint = true)
        {
            var result = new StringBuilder();
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            var hintResource = string.Empty;
            object value;
            result.Append(helper.LabelFor(expression, new { title = hintResource, @class = "control-label" }));
            result.Append(helper.Hint("hello").ToHtmlString());
            var laberWrapper = new TagBuilder("div");
            laberWrapper.Attributes.Add("class", "label-wrapper");
            laberWrapper.InnerHtml = result.ToString();
            return MvcHtmlString.Create(laberWrapper.ToString());
        }
        public static MvcHtmlString NopFrameowrkEditorFor<TModel, TValus>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TValus>> expression, bool? renderFormControlClass = null)
        {
            var result = new StringBuilder();
            object htmlAttributes = null;
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            if ((!renderFormControlClass.HasValue && metadata.ModelType.Name.Equals("String")) ||
                (renderFormControlClass.HasValue && renderFormControlClass.Value))
                htmlAttributes = new { @class = "form-control" };
            result.Append(helper.EditorFor(expression, new { htmlAttributes }));
            return MvcHtmlString.Create(result.ToString());
        }
        public static MvcHtmlString NopFramoworkDropDownList<TModel>(this HtmlHelper<TModel> helper, string name,
            IEnumerable<SelectListItem> itemList, object htmlAttributes = null,bool renderFormControlClass = true)
        {
            var result = new StringBuilder();
            var attrs = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            if(renderFormControlClass)
                attrs = AddFormControlClassToHtmlAttributes(attrs);
            result.Append(helper.DropDownList(name, itemList, attrs));
            return MvcHtmlString.Create(result.ToString());
        }
        public static RouteValueDictionary AddFormControlClassToHtmlAttributes(IDictionary<string, object> htmlAttributes)
        {
            if (htmlAttributes["class"] == null || string.IsNullOrEmpty(htmlAttributes["class"].ToString()))
                htmlAttributes["class"] = "form-control";
            else
                if (!htmlAttributes["class"].ToString().Contains("form-control"))
                htmlAttributes["class"] += " form-control";

            return htmlAttributes as RouteValueDictionary;
        }
        public static string FieldIdFor<T, TResult>(this HtmlHelper<T> html, Expression<Func<T, TResult>> expression)
        {
            var id = html.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression));
            // because "[" and "]" aren't replaced with "_" in GetFullHtmlFieldId
            return id.Replace('[', '_').Replace(']', '_');
        }
        #endregion
    }
}
