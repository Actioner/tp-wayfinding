using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using Simple.Data;
using IDB.Navigator.Domain;
using IDB.Navigator.Site.Components.Filters;
using IDB.Navigator.Site.Components.Services;
using IDB.Navigator.Site.Areas.Admin.Models.Node;
using IDB.Navigator.Site.Components.Extensions;
using System.Drawing.Imaging;
using IDB.Navigator.Site.Components.Settings;

namespace IDB.Navigator.Site.Areas.Admin.Controllers.Api
{
    public class NodeController : BaseApiController
    {
        // GET api/Node/buildingid
        public IHttpActionResult Get([FromUri]NodeSearchModel search)
        {
            var db = Database.Open();
            var nodesDb = search.FloorMapId.HasValue ? db.Node.FindAllBy(FloorMapId: search.FloorMapId.Value) : db.Node.All();
            var nodes = nodesDb.ToList<Node>();

            return Ok(MappingEngine.Map<IList<NodeModel>>(nodes));
        }

        // GET api/node/5
        public IHttpActionResult Get(int id)
        {
            var db = Database.Open();
            Node node = db.Node.Get(id);

            if (node == null)
                return NotFound();

            return Ok(MappingEngine.Map<NodeModel>(node));
        }

        // POST api/node
        [ValidationActionFilter]
        public IHttpActionResult Post([FromBody]CreateNodeModel value)
        {
            var node = MappingEngine.Map<Node>(value);
            var db = Database.Open();
            node = db.Node.Insert(node);
            
            return Ok(MappingEngine.Map<NodeModel>(node));
        }

        // PUT api/node/5
        [ValidationActionFilter]
        public IHttpActionResult Put(int id, [FromBody]EditNodeModel value)
        {
            var db = Database.Open();
            Node node = db.Node.Get(id);

            if (node == null)
                return NotFound();

            value.Id = id;
            MappingEngine.Map(value, node);
            db.Node.Update(node);

            return Ok(MappingEngine.Map<NodeModel>(node));
        }

        // DELETE api/node/5
        public IHttpActionResult Delete(int id)
        {
            var db = Database.Open();
            db.Node.DeleteByNodeId(id);
            db.Connection.DeleteByNodeAId(id);
            db.Connection.DeleteByNodeBId(id);

            return Ok(id);
        }
    }
}
