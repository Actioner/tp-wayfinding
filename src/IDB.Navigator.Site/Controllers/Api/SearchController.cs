using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using IDB.Navigator.Site.Models;
using System.DirectoryServices;
using Simple.Data;
using System.Text.RegularExpressions;
//using WebAPI.OutputCache;
using IDB.Navigator.Site.Helpers;
using IDB.Navigator.Domain;
using IDB.Navigator.Site.Helpers.DB;


namespace IDB.Navigator.Site.Controllers.Api
{
    public class SearchController : ApiController
    {
        //
        // GET: /Search/
       // [CacheOutput(ClientTimeSpan = 100, ServerTimeSpan = 100)]
        public List<DirectoryUser> Get(string Search, String DeviceName)
        {
            ActiveDirectory ad = new ActiveDirectory();
            List<DirectoryUser> result = ad.SearchUserByName(Search).ToList();

            if (result.Count == 0)
            { 
                //Search in DB
                var db = Database.Open();
                if (Regex.IsMatch(Search, @"^*?\d+$"))
                {
                    Search = Search.Replace(" ","");
                    // for search like NE345
                    if (Search.Length == 5 && Search[0] !='B')
                        Search = Search.Substring(0, 2) + "0" + Search.Substring(2, 3);

                    //for like B512
                    if (Search.Length == 4 && Search[0] == 'B')
                        Search = Search.Substring(0, 1) + "0" + Search.Substring(1, 3);
                }
                List<Marker> marker = db.Marker.All().Where(db.Marker.OfficeNumber.Like("%"+Search+"%"));

                foreach (Marker of in marker)
                {
                    result.Add( new DirectoryUser(){ 
                        AccountName = of.OfficeNumber, 
                        DisplayName = of.OfficeNumber,
                        OfficeNumber = of.OfficeNumber,
                        AvatarUrl = "http://rcerrato/IDBNavigator/Content/picture/?UserName=" + of.OfficeNumber,
                        Company = "Inter-American Development Bank",
                        Location = "HQ",
                        PhoneNumber =""
                    });

                }
            }

            if(Search.Length > 4)
                LogHelper.CreateLog(DeviceName, String.Format("Search {0}", Search));

            return result;
        }

    }
}
