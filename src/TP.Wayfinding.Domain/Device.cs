using System;

namespace TP.Wayfinding.Domain
{
    public class Device
    {
        public int DeviceId { get; set; }
        public int FloorMapId { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}