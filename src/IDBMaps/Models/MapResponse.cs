using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IDBMaps.Models.Routing;
using System.Runtime.Serialization;

namespace IDBMaps.Models
{
    [DataContract]
    public class MapResponse
    {

        private Building building;

        [DataMember]
        public Building Building
        {
            get { return building; }
            set { building = value; }
        }

        private Coordinate targeCoordinate;

        [DataMember]
        public Coordinate TargetCoordinate
        {
            get { return targeCoordinate; }
            set { targeCoordinate = value; }
        }

        private Coordinate fromCoordinate;

        [DataMember]
        public Coordinate FromCoordinate
        {
            get { return fromCoordinate; }
            set { fromCoordinate = value; }
        }

        private List<FloorConnection> route;

        [DataMember]
        public List<FloorConnection> Route
        {
            get { return route; }
            set { route = value; }
        }
    }
}