using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace IDBMaps.Models.Routing
{
    [DataContract]
    public class Connection
    {
        private int connectionId;

        [DataMember]
        public int ConnectionId
        {
            get { return connectionId; }
            set { connectionId = value; }
        }

        private Int16 floorMapId;

        [DataMember]
        public Int16 FloorMapId
        {
            get { return floorMapId; }
            set { floorMapId = value; }
        }

        private Int16 nodeAId, nodeBId;

        [DataMember]
        public Int16 NodeBId
        {
            get { return nodeBId; }
            set { nodeBId = value; }
        }

        [DataMember]
        public Int16 NodeAId
        {
            get { return nodeAId; }
            set { nodeAId = value; }
        }

        private Node nodeA, nodeB;

        [DataMember]
        public Node NodeA
        {
            get { return nodeA; }
            set { nodeA = value; }
        }

        [DataMember]
        public Node NodeB
        {
            get { return nodeB; }
            set { nodeB = value; }
        }

        private Boolean show;

        [DataMember]
        public Boolean Show
        {
            get { return show; }
            set { show = value; }
        }

        private Boolean floorConnection;

        [DataMember]
        public Boolean FloorConnection
        {
            get { return floorConnection; }
            set { floorConnection = value; }
        }

        public Connection()
        {

        }

        double weight;

        public double Weight
        {
            get {
                if (FloorConnection)
                    return 1000;

                double theta = nodeA.Longitude - nodeB.Longitude;
                double dist = Math.Sin(deg2rad(nodeA.Latitude)) * Math.Sin(deg2rad(nodeB.Latitude)) + Math.Cos(deg2rad(nodeA.Latitude)) * Math.Cos(deg2rad(nodeB.Latitude)) * Math.Cos(deg2rad(theta));
                dist = Math.Acos(dist);
                dist = rad2deg(dist);
                dist = dist * 60 * 1.1515;
                return dist;
            }
        }

        private double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        private double rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }

        bool selected = false;

        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        public Connection(Node a, Node b, int weight, bool show, bool floorConnection)
        {
            this.nodeA = a;
            this.nodeB = b;
            this.weight = (Math.Pow(a.Latitude - b.Longitude, 2) + Math.Pow(a.Latitude - b.Longitude, 2));
            this.show = show;
            this.floorConnection = floorConnection;
        }

        public Connection Invert()
        {
            return new Connection(this.nodeB, this.nodeA,0, this.show,this.floorConnection);
        }

        public double DistanceToPoint(Coordinate coord)
        {
            double A = coord.Latitude - nodeA.Latitude;
            double B = coord.Longitude - nodeA.Longitude;
            double C = nodeB.Latitude - nodeA.Latitude;
            double D = nodeB.Longitude - nodeA.Longitude;

            double dot = A * C + B * D;
            double len_sq = C * C + D * D;
            double param = dot / len_sq;

            double xx, yy;

            if(param<0 || (nodeA.Latitude == nodeB.Latitude && nodeA.Longitude == nodeB.Longitude))
            {
                xx = nodeA.Latitude;
                yy = nodeA.Longitude;
            }
            else if( param>1)
            {
                xx= nodeB.Latitude;
                yy = nodeB.Longitude;

            }else
            {
                xx= nodeA.Latitude + param*C;
                yy= nodeA.Longitude + param*D;

            }
            double dx = coord.Latitude - xx;
            double dy = coord.Longitude - yy;

            return Math.Sqrt(dx*dx+dy*dy);
        }

        public List<Connection> Split(Coordinate coord, string identifier)
        {
            List<Connection> result = new List<Connection>();

            double A = coord.Latitude - nodeA.Latitude;
            double B = coord.Longitude - nodeA.Longitude;
            double C = nodeB.Latitude - nodeA.Latitude;
            double D = nodeB.Longitude - nodeA.Longitude;

            double dot = A * C + B * D;
            double len_sq = C * C + D * D;
            double param = dot / len_sq;

            double xx, yy;

            if(param<0 || (nodeA.Latitude == nodeB.Latitude && nodeA.Longitude == nodeB.Longitude))
            {
                xx = nodeA.Latitude;
                yy = nodeA.Longitude;
            }
            else if( param>1)
            {
                xx= nodeB.Latitude;
                yy = nodeB.Longitude;

            }else
            {
                xx= nodeA.Latitude + param*C;
                yy= nodeA.Longitude + param*D;

            }

            double dx = coord.Latitude - xx;
            double dy = coord.Longitude - yy;

            result.Add(new Connection(nodeA, new Node(identifier, xx, yy), 0, this.show,false));
            result.Add(new Connection(new Node(identifier, xx, yy), nodeB, 0, this.show,false));

            return result;

            
        }

    }
}