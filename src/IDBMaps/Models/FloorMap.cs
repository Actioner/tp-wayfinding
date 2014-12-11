using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simple.Data;
using System.Runtime.Serialization;
using System.Drawing;
using IDBMaps.Customs;
using System.IO;
using System.Drawing.Imaging;
using IDBMaps.Models.Routing;

namespace IDBMaps.Models
{
    [DataContract]
    public class FloorMap
    {
        private string imageFolder;

        public string ImageFolder
        {
            get { return imageFolder; }
            set { imageFolder = value; }
        }

        private int buildingId;

        [DataMember]
        public int BuildingId
        {
            get { return buildingId; }
            set { buildingId = value; }
        }

        private int floorMapId;

        [DataMember]
        public int FloorMapId
        {
            get { return floorMapId; }
            set { floorMapId = value; }
        }

        private string description;

        [DataMember]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private int floor;

        [DataMember]
        public int Floor
        {
            get { return floor; }
            set { floor = value; }
        }

        private string imagePath;

        [DataMember]
        public string ImagePath
        {
            get { return imagePath; }
            set { imagePath = value; }
        }

        private List<Coordinate> coordinates;

        [DataMember]
        public List<Coordinate> Coordinates
        {
            get { return coordinates; }
            set { coordinates = value; }
        }



        public static FloorMap GetFloorMap(int BuildingId, int Floor)
        {
            var db = Database.Open();
            FloorMap floorMap = db.FloorMap.Find(db.FloorMap.BuildingId == BuildingId && db.FloorMap.Floor == Floor);
            return floorMap;
        }   
    }
}