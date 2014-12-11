using System;

namespace TP.Wayfinding.Domain
{
    public class OfficeType
    {
        public int OfficeTypeId { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string Icon { get; set; }
        public bool Static { get; set; }
    }
}