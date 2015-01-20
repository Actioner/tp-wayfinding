using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace TP.Wayfinding.Domain
{
    public class Connection
    {
        public int ConnectionId { get; set; }
        public int FloorMapId { get; set; }
        public int NodeAId { get; set; }
        public int NodeBId { get; set; }
        public bool Show { get; set; }
        public bool FloorConnection { get; set; }

        public Node NodeA { get; set; }
        public Node NodeB { get; set; }

        public Connection()
        {

        }

        private double _weight;
        public double Weight
        {
            get {
                if (FloorConnection)
                    return 1000;

                double theta = NodeA.Longitude - NodeB.Longitude;
                double dist = Math.Sin(deg2rad(NodeA.Latitude)) * Math.Sin(deg2rad(NodeB.Latitude)) + Math.Cos(deg2rad(NodeA.Latitude)) * Math.Cos(deg2rad(NodeB.Latitude)) * Math.Cos(deg2rad(theta));
                dist = Math.Acos(dist);
                dist = rad2deg(dist);
                dist = dist * 60 * 1.1515;
                _weight = dist;

                return _weight;
            }
            private set { _weight = value; }
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

        public bool Selected { get; set; }
        public Connection(Node a, Node b, int weight, bool show, bool floorConnection)
        {
            this.NodeA = a;
            this.NodeB = b;
            this.Weight = (Math.Pow(a.Latitude - b.Longitude, 2) + Math.Pow(a.Latitude - b.Longitude, 2));
            this.Show = show;
            this.FloorConnection = floorConnection;
        }

        public Connection Invert()
        {
            return new Connection(this.NodeB, this.NodeA,0, this.Show,this.FloorConnection);
        }

        public double DistanceToPoint(Coordinate coord)
        {
            double A = coord.Latitude - NodeA.Latitude;
            double B = coord.Longitude - NodeA.Longitude;
            double C = NodeB.Latitude - NodeA.Latitude;
            double D = NodeB.Longitude - NodeA.Longitude;

            double dot = A * C + B * D;
            double len_sq = C * C + D * D;
            double param = dot / len_sq;

            double xx, yy;

            if (param < 0 || (NodeA.Latitude == NodeB.Latitude && NodeA.Longitude == NodeB.Longitude))
            {
                xx = NodeA.Latitude;
                yy = NodeA.Longitude;
            }
            else if( param>1)
            {
                xx = NodeB.Latitude;
                yy = NodeB.Longitude;

            }else
            {
                xx = NodeA.Latitude + param * C;
                yy = NodeA.Longitude + param * D;

            }
            double dx = coord.Latitude - xx;
            double dy = coord.Longitude - yy;

            return Math.Sqrt(dx*dx+dy*dy);
        }

        public List<Connection> Split(Coordinate coord, string identifier)
        {
            List<Connection> result = new List<Connection>();

            double A = coord.Latitude - NodeA.Latitude;
            double B = coord.Longitude - NodeA.Longitude;
            double C = NodeB.Latitude - NodeA.Latitude;
            double D = NodeB.Longitude - NodeA.Longitude;

            double dot = A * C + B * D;
            double len_sq = C * C + D * D;
            double param = dot / len_sq;

            double xx, yy;

            if (param < 0 || (NodeA.Latitude == NodeB.Latitude && NodeA.Longitude == NodeB.Longitude))
            {
                xx = NodeA.Latitude;
                yy = NodeA.Longitude;
            }
            else if( param>1)
            {
                xx= NodeB.Latitude;
                yy = NodeB.Longitude;

            }else
            {
                xx= NodeA.Latitude + param*C;
                yy= NodeA.Longitude + param*D;

            }

            double dx = coord.Latitude - xx;
            double dy = coord.Longitude - yy;

            result.Add(new Connection(NodeA, new Node(identifier, xx, yy), 0, this.Show,false));
            result.Add(new Connection(new Node(identifier, xx, yy), NodeB, 0, this.Show,false));

            return result;
        }
    }
}