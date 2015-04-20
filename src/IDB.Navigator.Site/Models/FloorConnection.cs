using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IDB.Navigator.Site.Models;
using System.Runtime.Serialization;
using IDB.Navigator.Domain;

namespace IDB.Navigator.Site.Models
{
    public class FloorConnection
    {
        //[DataMember]
        public List<Connection> Route { get; set; }

        //[DataMember]
        public FloorMap FloorMap { get; set; }
        
    }
}