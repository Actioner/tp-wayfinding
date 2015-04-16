using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace IDB.Navigator.Domain
{
    public class Building
    {
        public int BuildingId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        public IList<FloorMap> FloorMaps { get; set; }
        public DateTime? LastUpdated { get; set; }
        public float NwLatitude { get; set; }
        public float NwLongitude { get; set; }
        public float SeLatitude { get; set; }
        public float SeLongitude { get; set; }
    }
}