using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Simple.Data;
using System.Linq;
using IDB.Navigator.Site.Areas.Admin.Models.Floor;
using IDB.Navigator.Domain;

namespace IDB.Navigator.Site.Areas.Admin.Controllers
{
    public class NavigationController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}