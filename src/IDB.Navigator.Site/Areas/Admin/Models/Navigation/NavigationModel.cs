using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IDB.Navigator.Site.Areas.Admin.Models.Connection;
using IDB.Navigator.Site.Areas.Admin.Models.Node;

namespace IDB.Navigator.Site.Areas.Admin.Models.Navigation
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