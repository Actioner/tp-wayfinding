using System.IO;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using Simple.Data;
using TP.Wayfinding.Site.Components.Services;
using TP.Wayfinding.Site.Models;
using TP.Wayfinding.Site.Models.Account;

namespace TP.Wayfinding.Site.Controllers
{
    [Authorize]
    public abstract class BaseController : Controller
    {
        protected readonly IMappingEngine MappingEngine;

        public BaseController()
        {
            MappingEngine = Mapper.Engine;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var actionDescriptor = filterContext.ActionDescriptor;
            
            ViewBag.ActionName = actionDescriptor.ActionName.ToLowerInvariant();
            ViewBag.ControllerName = actionDescriptor.ControllerDescriptor.ControllerName.ToLowerInvariant();
        }
    
        protected string RenderPartialViewToString(string viewName, object model = null, string prefix = null)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = ControllerContext.RouteData.GetRequiredString("action");
            }

            if (model != null)
            {
                ViewData.Model = model;
            }

            if (prefix != null)
            {
                ViewData.TemplateInfo.HtmlFieldPrefix = prefix;
            }

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.ToString();
            }
        }
    }
}