using System.Web.Optimization;

namespace IDB.Navigator.Site
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/karmanta/js").Include(
                        "~/Content/karmanta/js/jquery.nicescroll.js",
                        "~/Content/karmanta/js/jquery.scrollTo.js",
                        "~/Content/karmanta/js/scripts.js",
                        "~/Scripts/knockout-{version}.js",
                        "~/Scripts/knockout.validation.js",
                        "~/Scripts/knockout.mapping-latest.js",
                        "~/Content/karmanta/assets/gritter/js/jquery.gritter.js",
                        "~/Scripts/common.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Content/karmanta/js/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/karmanta/css").Include(
                      "~/Content/karmanta/css/bootstrap.css",
                      "~/Content/karmanta/css/bootstrap-theme.css",
                      "~/Content/karmanta/css/elegant-icons-style.css",
                      "~/Content/karmanta/assets/font-awesome/css/font-awesome.css",
                      "~/Content/karmanta/assets/gritter/css/jquery.gritter.css",
                      "~/Content/karmanta/css/style.css",
                      "~/Content/karmanta/css/style-responsive.css"));
        }
    }
}
