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
using IDBMaps.Models;
using IDBMaps.Customs;
using System.Configuration;
using Microsoft.Exchange.WebServices.Data;
using IDBMaps.Models.Mapping;


namespace IDBMaps.Controllers
{
    public class MapController : Controller
    {
        //
        // GET: /Map/

        public ActionResult Index()
        {
            return View();
        }

        /*
        [AcceptVerbs(HttpVerbs.Get)]
        public ImageResult ShowByUserName(string UserName, int? CoorX, int? CoorY)
        {
            string location="";
            string avatar = "default.png";

            ActiveDirectory ac = new ActiveDirectory();

            IEnumerable<DirectoryUser> users = ac.SearchUserByCriteria(new DirectorySearchCriteria() { AccountName = UserName });

            DirectoryUser user = users.FirstOrDefault();

            if (user != null)
            {
                FloorMap map = FloorMap.GetMap(user.Company, user.Location, user.OfficeNumber, user.AvatarUrl);

                if (map != null)
                {
                    map.ImageFolder = Server.MapPath(@"~/Images/Maps/");
                    return map.generateImage(CoorX, CoorY);
                }
            }
            return null;
        }

        
        [AcceptVerbs(HttpVerbs.Get)]
        public ImageResult ShowByOffice(string Company, string Location, string OfficeNumber, int? CoorX, int? CoorY)
        {

            string avatar = Server.MapPath(@"~/Images/Marker.png");

            Floor map = Floor.GetMap(Company, Location, OfficeNumber, avatar);

            if (map != null)
            {
                map.ImageFolder = Server.MapPath(@"~/Images/Maps/");
                return map.generateImage(CoorX, CoorY);
            }


            return map.generateImage(CoorX, CoorY);
        }

        */


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

            MemoryStream memoryStream = new MemoryStream();


            avatar.Save(memoryStream, ImageFormat.Png);
            string contentType = "image/png";



            return new ImageResult(memoryStream,contentType);
        }
       
    }
}
