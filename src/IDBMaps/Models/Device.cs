using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace IDBMaps.Models
{
    [DataContract]
    public class Device
    {
        private string mac;

        [DataMember]
        public string MAC
        {
            get { return mac; }
            set { mac = value; }
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



        private FloorMap floorMap;

         [DataMember]
        public FloorMap FloorMap
        {
            get { return floorMap; }
            set { floorMap = value; }
        }

         private Building building;

         [DataMember]
         public Building Building
         {
             get { return building; }
             set { building = value; }
         }

    }
}