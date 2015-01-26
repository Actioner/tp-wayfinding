using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TP.Wayfinding.Site.Models.Office
{
    public class OfficeSearchModel
    {
        public int? BuildingId { get; set; }
        public int? FloorMapId { get; set; }
        public int? OfficeTypeId { get; set; }
        public string DisplayNameTerm { get; set; }
    }
}