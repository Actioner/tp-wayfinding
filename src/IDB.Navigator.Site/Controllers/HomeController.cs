using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.IO;
using System.Net.Http.Headers;
using System.Net;
using System.Drawing.Imaging;
using System.Drawing;
using Simple.Data;


namespace IDB.Navigator.Site.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
       
    
    }
}
