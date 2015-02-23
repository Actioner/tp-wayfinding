using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using Simple.Data;
using Simple.Data.RawSql;
using TP.Wayfinding.Domain;
using TP.Wayfinding.Site.Components.Filters;
using TP.Wayfinding.Site.Components.Services;
using TP.Wayfinding.Site.Models.Node;
using TP.Wayfinding.Site.Components.Extensions;
using System.Drawing.Imaging;
using TP.Wayfinding.Site.Components.Settings;
using TP.Wayfinding.Site.Models.Navigation;
using TP.Wayfinding.Site.Models.Connection;

namespace TP.Wayfinding.Site.Controllers.Api
{
    public class NavigationController : BaseApiController
    {
        // GET api/Node/buildingid
        public IHttpActionResult Get([FromUri]NavigationSearchModel search)
        {
            var nodes = new Dictionary<int, NodeModel>();
            var connections = new Dictionary<int, ConnectionModel>();

            var result = new NavigationModel();

            if (search.FloorMapId == 0)
            {
                return Ok(result);
            }

            Database db = Database.Open(); // Notice statically typed db variable as Database
            var sql = @"select n.NodeId, n.FloorMapId, n.Identifier, n.Latitude, n.Longitude, n.FloorConnector as NodeFloorConnector, 
                        c.ConnectionId, c.NodeAId, c.NodeBId, c.Show, c.FloorConnection as ConnectionFloorConnection
                    from node n
	                    left join Connection c on c.NodeAId = n.NodeId or c.NodeBId = n.nodeId
                    where n.FloorMapId = @floorMapId
                    order by n.nodeid";

            var rows = db.ToRows(sql, new { floorMapId = search.FloorMapId });


            foreach (var row in rows)
            {
                int nodeId = (int) row.NodeId;
                NodeModel nodeModel;
                if (!nodes.TryGetValue(nodeId, out nodeModel))
                {
                    nodeModel = new NodeModel();
                    nodes[nodeId] = nodeModel;
                }
                nodeModel.Id = nodeId;
                nodeModel.FloorMapId = (int)row.FloorMapId;
                nodeModel.Identifier = (string)row.Identifier;
                nodeModel.Latitude = (double)row.Latitude;
                nodeModel.Longitude = (double)row.Longitude;
                nodeModel.FloorConnector = (int)row.NodeFloorConnector > 0;

                if (row.ConnectionId == null)
                    continue;

                int connectionId = (int)row.ConnectionId;
                ConnectionModel connectionModel;
                if (!connections.TryGetValue(connectionId, out connectionModel))
                {
                    connectionModel = new ConnectionModel();
                    connections[connectionId] = connectionModel;
                }

                connectionModel.Id = (int)row.ConnectionId;
                connectionModel.NodeAId = (int)row.NodeAId;
                connectionModel.NodeBId = (int)row.NodeBId;
                connectionModel.Show = ((bool?)row.Show).GetValueOrDefault(false);
                connectionModel.FloorConnection = (bool)row.ConnectionFloorConnection;
            }

            result.Nodes = nodes.Select(gn => gn.Value).ToList();
            result.Connections = connections.Select(gn => gn.Value).ToList();

            return Ok(result);
        }

   
    }
}
