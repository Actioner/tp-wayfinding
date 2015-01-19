using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TP.Wayfinding.Site.Models.OfficeType
{
    public class OfficeTypeModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string Icon { get; set; }
        public bool Static { get; set; }
    }
}