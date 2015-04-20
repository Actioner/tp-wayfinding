using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Net.Http.Headers;
using System.Net;
using System.Drawing.Imaging;
using System.Drawing;
using System.Net.Http;
using IDB.Navigator.Site.Models;
using IDB.Navigator.Site.Customs;
using System.Configuration;
using Microsoft.Exchange.WebServices.Data;


namespace IDB.Navigator.Site.Controllers
{
    public class ContentController : Controller
    {
        //
        // GET: /Map/

        public ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ImageResult Picture(string UserName)
        {

            string picturesFolder = ConfigurationManager.AppSettings["PicturesFolder"];
            string picture = "";
            if (!string.IsNullOrEmpty(picturesFolder))
            {
                picture = string.Format("{0}/{1}_DW.gif", picturesFolder, UserName);
            }

            Image avatar = ImageUtils.GetImageFromUrl(picture);
            avatar = ImageUtils.resizeImage(avatar, new Size(80, 100));
            avatar = ImageUtils.RoundCorners(avatar, 40, Color.Transparent);

           // Image avatar = Image.FromFile(String.Format(picturesFolder,UserName));

            MemoryStream memoryStream = new MemoryStream();


            avatar.Save(memoryStream, ImageFormat.Png);
            string contentType = "image/png";



            return new ImageResult(memoryStream,contentType);
        }
       
    }
}
