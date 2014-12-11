using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IDBMaps.Models.Routing;
using System.Runtime.Serialization;

namespace IDBMaps.Models
{
    public class FloorConnection
    {
        private List<Connection> route= new List<Connection>();

        [DataMember]
        public List<Connection> Route
        {
            get { return route; }
            set { route = value; }
        }

        private FloorMap floorMap;

        [DataMember]
        public FloorMap FloorMap
        {
            get { return floorMap; }
            set { floorMap = value; }
        }


        
    }
}