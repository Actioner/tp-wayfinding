using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDB.Navigator.Site.Areas.Admin.Models.Marker
{
    public class MarkerSearchModel
    {
        public int? BuildingId { get; set; }
        public int? FloorMapId { get; set; }
        public int? MarkerTypeId { get; set; }
        public string DisplayNameTerm { get; set; }
    }
}