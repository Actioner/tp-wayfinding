using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IDB.Navigator.Domain;

namespace IDB.Navigator.Site.Models
{
    public class DeviceResponse
    {

        public Device Device{ get; set;}
        public Building Building { get; set; }

    }
}