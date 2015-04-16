using System;
using System.Collections.Generic;

namespace IDB.Navigator.Domain
{
    public class Marker
    {
        public int MarkerId { get; set; }
        public string DisplayName { get; set; }
        public string OfficeNumber { get; set; }
        public int MarkerTypeId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int FloorMapId { get; set; }
        public bool Manual { get; set; }

        public string Detail { get; set; }
        public string Status { get; set; }
        public MarkerType Type { get; set; }
        public FloorMap FloorMap { get; set; }
    }
}