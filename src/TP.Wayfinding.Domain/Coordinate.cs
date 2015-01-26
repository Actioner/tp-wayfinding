using System;

namespace TP.Wayfinding.Domain
{
    public class Coordinate 
    {
        public double X { get; set; }
        public double Y { get; set; }
        public int OfficeId { get; set; }
        public FloorMap FloorMap { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string DisplayName { get; set; }
        public string OfficeNumber { get; set; }
        public string Status { get; set; }
        public string Detail { get; set; }
        public string TypeCode { get; set; }

        public Coordinate(double x, double y, double latitude, double longitude)
        {
            this.X = x;
            this.Y = y;
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        public Coordinate(double latitude, double longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        //public Coordinate(Device device)
        //{
        //    this.Latitude = device.Latitude;
        //    this.Longitude = device.Longitude;
        //    this.FloorMap = device.FloorMap;
        //}

        public void ConvertToCoord(Coordinate NW, Coordinate SE)
        {
            double pdfpointX = (NW.Latitude - SE.Latitude) / (NW.X - SE.X);
            double pdfpointY = (NW.Longitude - SE.Longitude) / (NW.Y - SE.Y);

            double dX = this.X - NW.X;
            double dY = this.Y - NW.Y;

            this.Longitude = NW.Longitude + dY * pdfpointY;
            this.Latitude = NW.Latitude + dX * pdfpointX; 
        }

        public double DistanceTo(double latitude, double longitude)
        {

            double theta = this.Longitude - longitude;
            double dist = Math.Sin(deg2rad(this.Latitude)) * Math.Sin(deg2rad(latitude)) + Math.Cos(deg2rad(this.Latitude)) * Math.Cos(deg2rad(latitude)) * Math.Cos(deg2rad(theta));
            dist = Math.Acos(dist);
            dist = rad2deg(dist);
            dist = dist * 60 * 1.1515;
            return dist;
            // return (float)(Math.Pow(this.latitude - latitude, 2) + Math.Pow(this.longitude - longitude, 2));
        }

        private double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        private double rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }
    }
}
