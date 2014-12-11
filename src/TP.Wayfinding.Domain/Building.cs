using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace TP.Wayfinding.Domain
{
    public class Building
    {
        public int BuildingId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Company { get; set; }
        public IList<FloorMap> FloorMapsy { get; set; }
        private DateTime? LastUpdated { get; set; }
        public float NWLatitude { get; set; }
        public float NWLongitude { get; set; }
        public float SELatitude { get; set; }
        public float SELongitude { get; set; }
    }
}