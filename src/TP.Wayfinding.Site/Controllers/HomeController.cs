using System;
using System.Web.Mvc;

namespace TP.Wayfinding.Site.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Graph()
        {
            return View();
        }
        
        public ActionResult UploadTiler()
        {
            return View("Tiler/Upload");
        }

        public ActionResult EditTiler()
        {
            return View("Tiler/Edit");
        }

        public ActionResult TestTiler()
        {
            return View("Tiler/Test");
        }
    }
}