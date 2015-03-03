using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace TP.Wayfinding.Site.Components.Extensions
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString WayfindingValidationMessageFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            var labelBuilder = new TagBuilder("label");
            labelBuilder.AddCssClass("error");
            labelBuilder.Attributes.Add("data-valmsg-replace", "true");

            var modelMetadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            labelBuilder.Attributes.Add("data-valmsg-for", modelMetadata.PropertyName);

            if (htmlAttributes != null)
            {
                var containerHtmlAttributesDictionary = new RouteValueDictionary(htmlAttributes)
                    .ToDictionary(attr => attr.Key.Replace('_', '-'), attr => attr.Value);
                labelBuilder.MergeAttributes(containerHtmlAttributesDictionary);
            }



            return MvcHtmlString.Create(labelBuilder.ToString(TagRenderMode.Normal));
        }
    }
}