using System;
using System.Configuration;
using System.Reflection;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using FluentValidation.Attributes;
using FluentValidation.Mvc;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace TP.Wayfinding.Site
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
            FluentValidationModelValidatorProvider.Configure(c => c.AddImplicitRequiredValidator = false);
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
