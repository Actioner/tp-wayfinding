using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using Simple.Data;
using TP.Wayfinding.Domain;
using TP.Wayfinding.Site.Components.Filters;
using TP.Wayfinding.Site.Components.Services;
using TP.Wayfinding.Site.Models.Connection;
using TP.Wayfinding.Site.Components.Extensions;
using System.Drawing.Imaging;
using TP.Wayfinding.Site.Components.Settings;

namespace TP.Wayfinding.Site.Controllers.Api
{
    public class ConnectionController : BaseApiController
    {
        // GET api/Connection/buildingid
        public IHttpActionResult Get([FromUri]ConnectionSearchModel search)
        {
            var db = Database.Open();
            var connectionsDb = search.NodeId.HasValue ? 
                db.Connection.FindAll(db.Connection.NodeAId == search.NodeId.Value || db.Connection.NodeBId == search.NodeId.Value) 
                : db.Connection.All();
            var connections = connectionsDb.ToList<Connection>();

            return Ok(MappingEngine.Map<IList<ConnectionModel>>(connections));
        }

        // GET api/connection/5
        public IHttpActionResult Get(int id)
        {
            var db = Database.Open();
            Connection connection = db.Connection.Get(id);

            if (connection == null)
                return NotFound();

            return Ok(MappingEngine.Map<ConnectionModel>(connection));
        }

        // POST api/connection
        [ValidationActionFilter]
        public IHttpActionResult Post([FromBody]CreateConnectionModel value)
        {
            var connection = MappingEngine.Map<Connection>(value);
            var db = Database.Open();
            connection = db.Connection.Insert(connection);
            
            return Ok(MappingEngine.Map<ConnectionModel>(connection));
        }

        // PUT api/connection/5
        [ValidationActionFilter]
        public IHttpActionResult Put(int id, [FromBody]EditConnectionModel value)
        {
            var db = Database.Open();
            Connection connection = db.Connection.Get(id);

            if (connection == null)
                return NotFound();

            value.Id = id;
            MappingEngine.Map(value, connection);
            db.Connection.Update(connection);

            return Ok(MappingEngine.Map<ConnectionModel>(connection));
        }

        // DELETE api/connection/5
        public IHttpActionResult Delete(int id)
        {
            var db = Database.Open();
            db.Connection.DeleteByConnectionId(id);

            return Ok(id);
        }
    }
}
