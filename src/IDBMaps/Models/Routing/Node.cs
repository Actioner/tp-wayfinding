using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace IDBMaps.Models.Routing
{
    [DataContract]
    public class Node
    {


        string identifier;

        [DataMember]
        public string Identifier
        {
            get { return identifier; }
            set { identifier = value; }
        }


        private int floorMapId;
        
        [DataMember]
        public int FloorMapId
        {
            get { return floorMapId; }
            set { floorMapId = value; }
        }

        private int nodeId;

        [DataMember]
        public int NodeId
        {
            get { return nodeId; }
            set { nodeId = value; }
        }

        private double latitude;

        [DataMember]
        public double Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

        private double longitude;

        [DataMember]
        public double Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }

        private bool floorConnector;

        [DataMember]
        public bool FloorConnector
        {
            get { return floorConnector; }
            set { floorConnector = value; }
        }

        public Node()
        {

        }

        public Node(string identifier, double latitude, double longitude)
        {
            this.identifier = identifier;
            this.latitude = latitude;
            this.longitude = longitude;
        }


        public override string ToString()
        {
            return identifier;
        }

        public float distanceTo(double latitudeE, double longitudeE)
        {
            return (float)(Math.Pow(this.latitude - latitudeE, 2) + Math.Pow(this.longitude - longitudeE, 2));
        }
    }
}