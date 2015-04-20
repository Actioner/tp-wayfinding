using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IDB.Navigator.Site.Models;
using System.Runtime.Serialization;
using IDB.Navigator.Domain;

namespace IDB.Navigator.Site.Models
{
    //[DataContract]
    public class MapResponse
    {
        //[DataMember]
        public Building Building{ get; set;}

        //[DataMember]
        public Marker TargetMarker { get; set; }

        //[DataMember]
        public Marker FromMarker { get; set; }

        //[DataMember]
        public List<FloorConnection> Route { get; set; }
    }
}