using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simple.Data;
using System.Runtime.Serialization;

using System.IO;


namespace IDBMaps.Models
{

    public class FloorMap
    {
        private string imageFolder;

        public string ImageFolder
        {
            get { return imageFolder; }
            set { imageFolder = value; }
        }

        private int buildingId;


        public int BuildingId
        {
            get { return buildingId; }
            set { buildingId = value; }
        }

        private int floorMapId;


        public int FloorMapId
        {
            get { return floorMapId; }
            set { floorMapId = value; }
        }

        private string description;

 
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private int floor;


        public int Floor
        {
            get { return floor; }
            set { floor = value; }
        }

        private string imagePath;


        public string ImagePath
        {
            get { return imagePath; }
            set { imagePath = value; }
        }


    }
}