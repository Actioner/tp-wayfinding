using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IDB.Navigator.Domain;
using IDB.Navigator.Site.Models;

namespace IDB.Navigator.Site.Helpers
{
    public class MarkerHelper
    {

        public static Marker GetMarker(ARMark armark)
        {
            Marker marker = new Marker();
            marker.Latitude = armark.Latitude;
            marker.Longitude = armark.Longitude;
            return marker;
        }

    }
}