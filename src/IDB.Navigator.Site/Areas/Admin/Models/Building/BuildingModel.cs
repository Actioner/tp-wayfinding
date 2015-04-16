using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDB.Navigator.Site.Areas.Admin.Models.Building
{
    public class BuildingModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        public string LastUpdated { get; set; }

        public float NwLatitude { get; set; }
        public float NwLongitude { get; set; }
        public float SeLatitude { get; set; }
        public float SeLongitude { get; set; }
    }
}