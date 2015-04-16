using System.Web.Mvc;

namespace IDB.Navigator.Site.Areas.Admin.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        public ActionResult Error()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }
    }
}