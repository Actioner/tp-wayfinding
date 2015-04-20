using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace IDB.Navigator.Domain
{
    public class Node : IGeo
    {
        public int NodeId { get; set; }
        public int FloorMapId { get; set; }
        public string Identifier { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool FloorConnector { get; set; }

        public Node()
        {
        }

        public Node(string identifier, double latitude, double longitude)
        {
            this.Identifier = identifier;
            this.Latitude = latitude;
            this.Longitude = longitude;
        }
        
        public override string ToString()
        {
            return Identifier;
        }

        public float distanceTo(double latitudeE, double longitudeE)
        {
            return (float)(Math.Pow(this.Latitude - latitudeE, 2) + Math.Pow(this.Longitude - longitudeE, 2));
        }
    }
}