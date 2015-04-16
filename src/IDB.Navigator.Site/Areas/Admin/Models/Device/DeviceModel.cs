using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDB.Navigator.Site.Areas.Admin.Models.Device
{
    public class DeviceModel
    {
        public int Id { get; set; }
        public int FloorMapId { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}