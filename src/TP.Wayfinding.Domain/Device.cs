using System;

namespace TP.Wayfinding.Domain
{
    public class Device
    {
        public string MAC { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public FloorMap FloorMap { get; set; }
        public Building Building { get; set; }
    }
}