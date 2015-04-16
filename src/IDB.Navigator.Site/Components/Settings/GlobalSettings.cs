using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace IDB.Navigator.Site.Components.Settings
{
    public static class GlobalSettings
    {
        public static string MapsFolder
        {
            get
            {
                return ConfigurationManager.AppSettings["Navigator.MapsFolder"];
            }
        }
        public static string PeopleFolder
        {
            get
            {
                return ConfigurationManager.AppSettings["Navigator.PeopleFolder"];
            }
        }
        public static int DefaultBuilding
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["Navigator.DefaultBuilding"]);
            }
        }
        public static int DefaultFloorMap
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["Navigator.DefaultFloorMap"]);
            }
        }
    }
}