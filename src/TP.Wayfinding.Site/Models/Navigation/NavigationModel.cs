using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TP.Wayfinding.Site.Models.Connection;
using TP.Wayfinding.Site.Models.Node;

namespace TP.Wayfinding.Site.Models.Navigation
{
    public class NavigationModel
    {
        public IList<NodeModel> Nodes { get; set; }
        public IList<ConnectionModel> Connections { get; set; }

        public NavigationModel()
        {
            Nodes = new List<NodeModel>();
            Connections = new List<ConnectionModel>();
        }
    }
}