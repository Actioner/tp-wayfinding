using System;

namespace IDB.Navigator.Domain
{
    public class MarkerType
    {
        public int MarkerTypeId { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string Icon { get; set; }
        public bool Static { get; set; }
    }
}