using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IDB.Navigator.Domain;

namespace IDB.Navigator.Site.Models
{
    public class Route
    {

        public double Cost{get;set;}

        public List<Connection> Connections { get; set; }

        string _identifier;

        public Route()
        {

        }

        public Route(string _identifier)
        {
            Cost = int.MaxValue;
            Connections = new List<Connection>();
            this._identifier = _identifier;
        }

        public override string ToString()
        {
            return "Id:" + _identifier + " Cost:" + Cost;
        }

    }
}