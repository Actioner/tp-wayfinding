using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDB.Navigator.Site.Areas.Admin.Models.MarkerType
{
    public class MarkerTypeModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string Icon { get; set; }
        public bool Static { get; set; }
    }
}