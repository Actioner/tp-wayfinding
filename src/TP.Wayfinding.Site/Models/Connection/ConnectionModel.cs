using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TP.Wayfinding.Site.Models.Connection
{
    public class ConnectionModel
    {
        public int Id { get; set; }

        public int FloorMapId { get; set; }
        public int NodeAId { get; set; }
        public int NodeBId { get; set; }
        public bool Show { get; set; }
        public bool FloorConnection { get; set; }
    }
}