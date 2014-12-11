using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;


namespace IDBMaps.Models
{

    public class Office
    {
        private int officeId;

        public int OfficeId
        {
            get { return officeId; }
            set { officeId = value; }
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

        private int typeId;


        public int TypeId
        {
            get { return typeId; }
            set { typeId = value; }
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


        private int floorMapId;

        public int FloorMapId
        {
            get { return floorMapId; }
            set { floorMapId = value; }
        }
        
    }
}