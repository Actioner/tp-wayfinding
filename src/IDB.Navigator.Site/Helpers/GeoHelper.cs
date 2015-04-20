using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Web;
using IDB.Navigator.Domain;
using IDB.Navigator.Site.Models;
using Simple.Data;

namespace IDB.Navigator.Site.Helpers
{
    public class GeoHelper
    {

        public static double Distance(IGeo from, IGeo to)
        {

            GeoCoordinate fromG = new GeoCoordinate(from.Latitude, from.Longitude);
            GeoCoordinate toG = new GeoCoordinate(to.Latitude, to.Longitude);

            return fromG.GetDistanceTo(toG);
        }

        public static double Distance(IGeo from, double Latitude, double Longitude)
        {

            GeoCoordinate fromG = new GeoCoordinate(from.Latitude, from.Longitude);
            GeoCoordinate toG = new GeoCoordinate(Latitude, Longitude);

            return fromG.GetDistanceTo(toG);
        }

        public static double bearingTo(IGeo from, IGeo to)
        {
            var dLat = to.Latitude - from.Latitude;
            var dLon = to.Longitude - from.Longitude;
            var dPhi = Math.Log(Math.Tan(to.Latitude / 2 + Math.PI / 4) / Math.Tan(from.Latitude / 2 + Math.PI / 4));
            var q = (Math.Abs(dLat) > 0) ? dLat / dPhi : Math.Cos(from.Latitude);

            if (Math.Abs(dLon) > Math.PI)
            {
                dLon = dLon > 0 ? -(2 * Math.PI - dLon) : (2 * Math.PI + dLon);
            }
            //var d = Math.Sqrt(dLat * dLat + q * q * dLon * dLon) * R; 
            var brng = ToDegree(Math.Atan2(dLon, dPhi));
            return brng;
        }

        public static double ToDegree(double val) { return val * 180 / Math.PI; }

        public static Marker GetFromCoordinates(Building building, int floor, double latitude, double longitude)
        {
            var db = Database.Open();
            Marker result = new Marker();

            FloorMap floorMap = db.FloorMap.Find(db.FloorMap.BuildingId == building.BuildingId && db.FloorMap.Floor == floor);
            List<Marker> marker = db.Marker.FindAllBy(FloorMapId: floorMap.FloorMapId).With(db.Marker.MarkerType.As("Type")).With(db.Marker.FloorMap);
            double minDistance = Double.MaxValue;

            foreach (Marker m in marker)
            {
                if (m.Type != null)
                {
                    if (!m.Type.Static)
                    {
                        double dist = GeoHelper.Distance(m, latitude, longitude);
                        if (dist < minDistance && dist < 10)
                        {
                            minDistance = GeoHelper.Distance(m, latitude, longitude);
                            result = m;
                        }
                    }
                }
            }

            return result;
        }

        public static double GetWeight(Connection conn)
        {
            if (conn.FloorConnection)
                return 1000;

            return Distance(conn.NodeA,conn.NodeB);

        }

        public static Connection Invert(Connection conn)
        {
            Connection con = new Connection();
            con.NodeA = conn.NodeB;
            con.NodeB = conn.NodeA;
            con.Show = conn.Show;
            con.FloorConnection = conn.FloorConnection;
            con.FloorMapId = conn.FloorMapId;

            return con;
        }

        //this method gives you the point in the connection closer to the marker
        public static double DistanceToPoint(Connection conn, IGeo marker)
        {
            double A = marker.Latitude - conn.NodeA.Latitude;
            double B = marker.Longitude - conn.NodeA.Longitude;
            double C = conn.NodeB.Latitude - conn.NodeA.Latitude;
            double D = conn.NodeB.Longitude - conn.NodeA.Longitude;

            double dot = A * C + B * D;
            double len_sq = C * C + D * D;
            double param = dot / len_sq;

            double xx, yy;

            if (param < 0 || (conn.NodeA.Latitude == conn.NodeB.Latitude && conn.NodeA.Longitude == conn.NodeB.Longitude))
            {
                xx = conn.NodeA.Latitude;
                yy = conn.NodeA.Longitude;
            }
            else if (param > 1)
            {
                xx = conn.NodeB.Latitude;
                yy = conn.NodeB.Longitude;

            }
            else
            {
                xx = conn.NodeA.Latitude + param * C;
                yy = conn.NodeA.Longitude + param * D;

            }
            double dx = marker.Latitude - xx;
            double dy = marker.Longitude - yy;

            return Math.Sqrt(dx * dx + dy * dy);
        }

        public static List<Connection> Split(Connection conn, IGeo marker, string identifier)
        {
            List<Connection> result = new List<Connection>();

            double A = marker.Latitude - conn.NodeA.Latitude;
            double B = marker.Longitude - conn.NodeA.Longitude;
            double C = conn.NodeB.Latitude - conn.NodeA.Latitude;
            double D = conn.NodeB.Longitude - conn.NodeA.Longitude;

            double dot = A * C + B * D;
            double len_sq = C * C + D * D;
            double param = dot / len_sq;

            double xx, yy;

            if (param < 0 || (conn.NodeA.Latitude == conn.NodeB.Latitude && conn.NodeA.Longitude == conn.NodeB.Longitude))
            {
                xx = conn.NodeA.Latitude;
                yy = conn.NodeA.Longitude;
            }
            else if (param > 1)
            {
                xx = conn.NodeB.Latitude;
                yy = conn.NodeB.Longitude;

            }
            else
            {
                xx = conn.NodeA.Latitude + param * C;
                yy = conn.NodeA.Longitude + param * D;

            }

            double dx = marker.Latitude - xx;
            double dy = marker.Longitude - yy;

            Connection conn1 = new Connection();
            conn1.NodeA = conn.NodeA;
            conn1.NodeB = new Node(identifier, xx, yy);
            conn1.Show = conn.Show;
            conn1.FloorConnection = false;
            conn1.FloorMapId = conn.FloorMapId;

            Connection conn2 = new Connection();
            conn2.NodeA = new Node(identifier, xx, yy);
            conn2.NodeB = conn.NodeB;
            conn2.Show = conn.Show;
            conn2.FloorConnection = false;
            conn2.FloorMapId = conn.FloorMapId;

            result.Add(conn1);
            result.Add(conn2);

            return result;


        }
    }
}