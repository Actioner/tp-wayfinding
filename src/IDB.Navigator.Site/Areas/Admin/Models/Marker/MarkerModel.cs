using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDB.Navigator.Site.Areas.Admin.Models.Marker
{
    public class MarkerModel
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string OfficeNumber { get; set; }
        public int MarkerTypeId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int FloorMapId { get; set; }
        public bool Manual { get; set; }
    }
}