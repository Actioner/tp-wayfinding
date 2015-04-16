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
using IDB.Navigator.Site.Areas.Admin.Models.MarkerType;
using IDB.Navigator.Site.Components.Extensions;
using System.Drawing.Imaging;
using IDB.Navigator.Site.Components.Settings;

namespace IDB.Navigator.Site.Areas.Admin.Controllers.Api
{
    public class MarkerTypeController : BaseApiController
    {
        // GET api/MarkerType/id
        public IHttpActionResult Get()
        {
            var db = Database.Open();
            var markerTypes = db.MarkerType.All().OrderByDescription().ToList<MarkerType>();

            return Ok(MappingEngine.Map<IList<MarkerTypeModel>>(markerTypes));
        }

        // GET api/markerType/5
        public IHttpActionResult Get(int id)
        {
            var db = Database.Open();
            MarkerType markerType = db.MarkerType.Get(id);

            if (markerType == null)
                return NotFound();

            return Ok(MappingEngine.Map<MarkerTypeModel>(markerType));
        }

        // POST api/markerType
        [ValidationActionFilter]
        public IHttpActionResult Post([FromBody]CreateMarkerTypeModel value)
        {
            var markerType = MappingEngine.Map<MarkerType>(value);
            var db = Database.Open();
            markerType = db.MarkerType.Insert(markerType);
            
            return Ok(MappingEngine.Map<MarkerTypeModel>(markerType));
        }

        // PUT api/markerType/5
        [ValidationActionFilter]
        public IHttpActionResult Put(int id, [FromBody]EditMarkerTypeModel value)
        {
            var db = Database.Open();
            MarkerType markerType = db.MarkerType.Get(id);

            if (markerType == null)
                return NotFound();

            value.Id = id;
            MappingEngine.Map(value, markerType);
            db.MarkerType.Update(markerType);

            return Ok(MappingEngine.Map<MarkerTypeModel>(markerType));
        }

        // DELETE api/markerType/5
        public IHttpActionResult Delete(int id)
        {
            var db = Database.Open();
            db.MarkerType.DeleteByMarkerTypeId(id);

            return Ok(id);
        }
    }
}
