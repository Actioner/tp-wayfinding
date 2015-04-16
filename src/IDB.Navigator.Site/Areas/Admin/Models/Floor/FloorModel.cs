using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDB.Navigator.Site.Areas.Admin.Models.Floor
{
    public class FloorModel
    {
        public int Id { get; set; }
        public int BuildingId { get; set; }
        public int Floor { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string ImagePath { get; set; }
        public float NeLatitude { get; set; }
        public float NeLongitude { get; set; }
        public float SwLatitude { get; set; }
        public float SwLongitude { get; set; }
    }
}