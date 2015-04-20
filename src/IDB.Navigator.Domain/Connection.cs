using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace IDB.Navigator.Domain
{
    public class Connection
    {
        public int ConnectionId { get; set; }
        public int FloorMapId { get; set; }
        public int NodeAId { get; set; }
        public int NodeBId { get; set; }
        public bool Show { get; set; }
        public bool FloorConnection { get; set; }
        public Node NodeA { get; set; }
        public Node NodeB { get; set; }
    }
}