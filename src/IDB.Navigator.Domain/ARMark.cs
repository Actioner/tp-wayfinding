using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace IDB.Navigator.Domain
{
    public class ARMark: IGeo
    {
        public int MarkId { get; set; }
        public String VuforiaId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int FloorMapId { get; set; }
        public double Bearing { get; set; }

       
    }
}