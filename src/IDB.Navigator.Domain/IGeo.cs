using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace IDB.Navigator.Domain
{
    public interface IGeo
    {
        double Latitude { get; set; }
        double Longitude { get; set; }
        int FloorMapId { get; set; }
    }
}