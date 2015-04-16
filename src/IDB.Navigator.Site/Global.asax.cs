using System;
using System.Configuration;
using System.Reflection;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using FluentValidation.Attributes;
using FluentValidation.Mvc;
using FluentValidation.WebApi;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Newtonsoft.Json.Serialization;
using IDB.Navigator.Site.Components.AutoMapper;

namespace IDB.Navigator.Site
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
            FluentValidation.Mvc.FluentValidationModelValidatorProvider.Configure(c => c.AddImplicitRequiredValidator = false);
            FluentValidation.WebApi.FluentValidationModelValidatorProvider.Configure(GlobalConfiguration.Configuration);

            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            AutoMappingsCreator.CreateModelMappings();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            // Useful for debugging
            var ex = Server.GetLastError();
            var serverEx = ex as ReflectionTypeLoadException;
            if (serverEx != null && serverEx.InnerException != null)
            {
                ex = serverEx.InnerException;
            }
            else
            {
                ex = ex.GetBaseException();
            }
            if (!ExceptionPolicy.HandleException(ex, "Exception Policy"))
            {
                // Redirecciono manualmente cuando HandleException indica que no hay que hacer un Rethrow para no perder la sesión
                Response.Redirect(
                    ((CustomErrorsSection)ConfigurationManager.GetSection("system.web/customErrors")).DefaultRedirect, false);
                Server.ClearError();
            }
        }
    }
}
