using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace TP.Wayfinding.Site.Components.Settings
{
    public static class GlobalSettings
    {
        public static string MapsFolder
        {
            get
            {
                return ConfigurationManager.AppSettings["Wayfinding.MapsFolder"];
            }
        }
    }
}