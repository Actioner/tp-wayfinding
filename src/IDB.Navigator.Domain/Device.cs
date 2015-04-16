﻿using System;

namespace IDB.Navigator.Domain
{
    public class Device
    {
        public int DeviceId { get; set; }
        public int FloorMapId { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime? LastTick { get; set; }
        public string LastBatteryStatus { get; set; }
    }
}