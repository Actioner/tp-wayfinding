using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Simple.Data;
using System.Linq;
using TP.Wayfinding.Site.Models.Floor;
using TP.Wayfinding.Domain;

namespace TP.Wayfinding.Site.Controllers
{
    public class NavigationController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}