using System;
using System.Collections.Generic;

namespace TP.Wayfinding.Domain
{
    public class Office
    {
        public int OfficeId { get; set; }
        public string DisplayName { get; set; }
        public string OfficeNumber { get; set; }
        public int OfficeType { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int FloorMapId { get; set; }
        public bool Manual { get; set; }

        public string Detail { get; set; }
        public string Status { get; set; }
        public OfficeType Type { get; set; }
        public FloorMap FloorMap { get; set; }

        //public  Coordinate GetCoordinate()
        //{
        //    var db = Database.Open();

        //    Coordinate coor = new Coordinate(this.latitude, this.longitude);
        //    coor.FloorMap = db.FloorMap.Get(this.floorMapId);
        //    coor.OfficeNumber = this.OfficeNumber;
        //    coor.DisplayName = this.OfficeNumber;
        //    coor.Status = this.Status;
        //    coor.Detail = this.Detail;
        //    if(this.type != null)
        //    coor.TypeCode = this.Type.Code;
        //    coor.OfficeId = this.OfficeId;
        //    return coor;
        //}

        //public static Coordinate GetCoordinate(Building building, String OfficeNumber)
        //{
        //    var db = Database.Open();
        //    List<Office> offices = db.Office.FindAllBy(OfficeNumber: OfficeNumber).With(db.Office.FloorMap.As("FloorMap"));
        //    Office office = offices.Where(o => o.floorMap.BuildingId == building.BuildingId).FirstOrDefault();


        //    Coordinate coor = new Coordinate(office.latitude, office.longitude);
        //    coor.FloorMap = office.FloorMap;
        //    coor.OfficeNumber = office.OfficeNumber;
        //    coor.Status = office.status;
        //    coor.Detail = office.detail;
        //    return coor;
        //}

        //public static Coordinate GetFromCoordinates(Building building, int floor, double latitude, double longitude)
        //{
        //    var db = Database.Open();
        //    Coordinate result = new Coordinate();

        //    FloorMap floorMap = FloorMap.GetFloorMap(building.BuildingId, floor);
        //    List<Office> offices = db.Office.FindAllByFloorMapId(floorMap.FloorMapId).With(db.Office.OfficeType.As("Type"));
        //    double minDistance = Double.MaxValue;

        //    foreach (Office off in offices)
        //    {
        //        if (!off.Type.Static)
        //        {
        //            double dist = off.distanceTo(latitude, longitude);
        //            if (dist < minDistance && dist<0.015)
        //            {
        //                minDistance = off.distanceTo(latitude, longitude);
        //                result.Latitude = off.latitude;
        //                result.Longitude = off.longitude;
        //                result.FloorMap = floorMap;
        //                result.OfficeNumber = off.OfficeNumber;
        //            }
        //        }

        //    }

        //    return result;
        //}

        public double DistanceTo(double latitude, double longitude)
        {
            double theta = this.Longitude - longitude;
            double dist = Math.Sin(deg2rad(this.Latitude)) * Math.Sin(deg2rad(latitude)) + Math.Cos(deg2rad(this.Latitude)) * Math.Cos(deg2rad(latitude)) * Math.Cos(deg2rad(theta));
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