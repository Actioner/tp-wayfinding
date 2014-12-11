using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using Simple.Data;

namespace IDBMaps.Models
{
    [DataContract]
    public class Building
    {
        private int buildingId;

        [DataMember]
        public int BuildingId
        {
            get { return buildingId; }
            set { buildingId = value; }
        }

        private string name;

        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string location;

        [DataMember]
        public string Location
        {
            get { return location; }
            set { location = value; }
        }

        private string company;

        [DataMember]
        public string Company
        {
            get { return company; }
            set { company = value; }
        }

        private List<FloorMap> floorMaps;

        [DataMember]
        public List<FloorMap> FloorMaps
        {
            get { return floorMaps; }
            set { floorMaps = value; }
        }


        private DateTime? lastUpdated;

        [DataMember]
        public DateTime? LastUpdated
        {
            get { return lastUpdated; }
            set { lastUpdated = value; }
        }


        private float nwLatitude;
        [DataMember]
        public float NWLatitude
        {
            get { return nwLatitude; }
            set { nwLatitude = value; }
        }

        private float nwLongitude;
        [DataMember]
        public float NWLongitude
        {
            get { return nwLongitude; }
            set { nwLongitude = value; }
        }

        private float seLatitude;
        [DataMember]
        public float SELatitude
        {
            get { return seLatitude; }
            set { seLatitude = value; }
        }

        private float seLongitude;
        [DataMember]
        public float SELongitude
        {
            get { return seLongitude; }
            set { seLongitude = value; }
        }



       public static Building GetBuilding(String Company, String Location, String OfficeNumber)
       {
           var db = Database.Open();
           Building building = db.Building.FindByBuildingId(0);
           return building;
       }

    }
}