using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace IDBMaps.Models
{

    public class Coordinate 
    {

        private double x;

        public double X
        {
            get { return x; }
            set { x = value; }
        }

        private double y;

        public double Y
        {
            get { return y; }
            set { y = value; }
        }



        private double latitude;

        public double Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

        private double longitude;
        

        public double Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }


        private string displayName;


        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }

        private string officeNumber;

        public string OfficeNumber
        {
            get { return officeNumber; }
            set { officeNumber = value; }
        }


        public Coordinate()
        {

        }

        public Coordinate(double x, double y, double latitude, double longitude)
        {
            this.x= x;
            this.y = y;
            this.latitude = latitude;
            this.longitude = longitude;
        }

        public Coordinate(double latitude, double longitude)
        {
            this.latitude = latitude;
            this.longitude = longitude;
        }

        public void ConvertToCoord(Coordinate NW, Coordinate SE)
        {
            double pdfpointX = (NW.latitude - SE.latitude) / (NW.X - SE.X);
            double pdfpointY = (NW.longitude - SE.longitude) / (NW.Y - SE.Y);

            double dX = this.X - NW.X;
            double dY = this.Y - NW.Y;

            this.longitude = NW.longitude + dY * pdfpointY;
            this.latitude = NW.latitude + dX * pdfpointX; 
        }

        public double distanceTo(double latitude, double longitude)
        {

          double theta = this.longitude - longitude;
          double dist = Math.Sin(deg2rad(this.latitude)) * Math.Sin(deg2rad(latitude)) + Math.Cos(deg2rad(this.latitude)) * Math.Cos(deg2rad(latitude)) * Math.Cos(deg2rad(theta));
          dist = Math.Acos(dist);
          dist = rad2deg(dist);
          dist = dist * 60 * 1.1515;
          return dist;
           // return (float)(Math.Pow(this.latitude - latitude, 2) + Math.Pow(this.longitude - longitude, 2));
        }

        private double deg2rad(double deg) {   
          return (deg * Math.PI / 180.0);
        }

        private double rad2deg(double rad){
         return (rad / Math.PI * 180.0);
        }


    }
}
