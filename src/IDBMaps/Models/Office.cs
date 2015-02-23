using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Drawing;
using Simple.Data;

namespace IDBMaps.Models
{
    [DataContract]
    public class Office
    {
        private int officeId;

        [DataMember]
        public int OfficeId
        {
            get { return officeId; }
            set { officeId = value; }
        }

        private string displayName;

        [DataMember]
        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }

        private string officeNumber;

        [DataMember]
        public string OfficeNumber
        {
            get { return officeNumber; }
            set { officeNumber = value; }
        }

        private int typeId;

        [DataMember]
        public int TypeId
        {
            get { return typeId; }
            set { typeId = value; }
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

        private int floorMapId;

        [DataMember]
        public int FloorMapId
        {
            get { return floorMapId; }
            set { floorMapId = value; }
        }

        private OfficeType type;

        [DataMember]
        public OfficeType Type
        {
            get { return type; }
            set { type = value; }
        }

        private string status = "";

        [DataMember]
        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        private string detail= "";

        [DataMember]
        public string Detail
        {
            get { return detail; }
            set { detail = value; }
        }

        private FloorMap floorMap;

        [DataMember]
        public FloorMap FloorMap
        {
            get { return floorMap; }
            set { floorMap = value; }
        }

        public  Coordinate GetCoordinate()
        {
            var db = Database.Open();

            Coordinate coor = new Coordinate(this.latitude, this.longitude);
            coor.FloorMap = db.FloorMap.Get(this.floorMapId);
            coor.OfficeNumber = this.OfficeNumber;
            coor.DisplayName = this.OfficeNumber;
            coor.Status = this.Status;
            coor.Detail = this.Detail;
            if(this.type != null)
            coor.TypeCode = this.Type.Code;
            coor.OfficeId = this.OfficeId;
            return coor;
        }

        public static Coordinate GetCoordinate(Building building, String OfficeNumber)
        {
            var db = Database.Open();
            List<Office> offices = db.Office.FindAllBy(OfficeNumber: OfficeNumber).With(db.Office.FloorMap.As("FloorMap"));
            Office office = offices.Where(o => o.floorMap.BuildingId == building.BuildingId).FirstOrDefault();


            Coordinate coor = new Coordinate(office.latitude, office.longitude);
            coor.FloorMap = office.FloorMap;
            coor.OfficeNumber = office.OfficeNumber;
            coor.Status = office.status;
            coor.Detail = office.detail;
            return coor;
        }

        public static Coordinate GetFromCoordinates(Building building, int floor, double latitude, double longitude)
        {
            var db = Database.Open();
            Coordinate result = new Coordinate();

            FloorMap floorMap = FloorMap.GetFloorMap(building.BuildingId, floor);
            List<Office> offices = db.Office.FindAllByFloorMapId(floorMap.FloorMapId).With(db.Office.OfficeType.As("Type"));
            double minDistance = Double.MaxValue;

            foreach (Office off in offices)
            {
                if (!off.Type.Static.Value)
                {
                    double dist = off.distanceTo(latitude, longitude);
                    if (dist < minDistance && dist<0.015)
                    {
                        minDistance = off.distanceTo(latitude, longitude);
                        result.Latitude = off.latitude;
                        result.Longitude = off.longitude;
                        result.FloorMap = floorMap;
                        result.OfficeNumber = off.OfficeNumber;
                    }
                }

            }

            return result;
        }

        public double distanceTo(double latitude, double longitude)
        {
            double theta = this.longitude - longitude;
            double dist = Math.Sin(deg2rad(this.latitude)) * Math.Sin(deg2rad(latitude)) + Math.Cos(deg2rad(this.latitude)) * Math.Cos(deg2rad(latitude)) * Math.Cos(deg2rad(theta));
            dist = Math.Acos(dist);
            dist = rad2deg(dist);
            dist = dist * 60 * 1.1515;
            return dist;
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