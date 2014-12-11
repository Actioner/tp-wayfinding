using System;
using System.Collections.Generic;

namespace TP.Wayfinding.Domain
{
    public class FloorMap
    {
        public string ImageFolder { get; set; }
        public int BuildingId { get; set; }
        public int FloorMapId { get; set; }
        public string Description { get; set; }
        public int Floor { get; set; }
        public string ImagePath { get; set; }
        public IList<Coordinate> Coordinates { get; set; }


        //public static FloorMap GetFloorMap(int BuildingId, int Floor)
        //{
        //    var db = Database.Open();
        //    FloorMap floorMap = db.FloorMap.Find(db.FloorMap.BuildingId == BuildingId && db.FloorMap.Floor == Floor);
        //    return floorMap;
        //}   
    }
}