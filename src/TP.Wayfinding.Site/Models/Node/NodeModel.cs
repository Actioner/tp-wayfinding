using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TP.Wayfinding.Site.Models.Node
{
    public class NodeModel
    {
        public int Id { get; set; }
        public int FloorMapId { get; set; }
        public string Identifier { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool FloorConnector { get; set; }
    }
}