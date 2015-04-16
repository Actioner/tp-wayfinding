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
using IDB.Navigator.Site.Areas.Admin.Models.Marker;
using IDB.Navigator.Site.Components.Extensions;
using System.Drawing.Imaging;
using IDB.Navigator.Site.Components.Settings;

namespace IDB.Navigator.Site.Areas.Admin.Controllers.Api
{
    public class MarkerController : BaseApiController
    {
        // GET api/Marker/buildingid
        public IHttpActionResult Get([FromUri]MarkerSearchModel search)
        {
            var db = Database.Open();
            var query = db.Marker.All();

            if (search.BuildingId.HasValue)
            {
                query = query
                     .Where(db.Marker.FloorMap.Building.BuildingId == search.BuildingId.Value);
            }

            if (search.FloorMapId.HasValue) {
                query = query
                     .Where(db.Marker.FloorMapId == search.FloorMapId.Value);
            }

            if (search.MarkerTypeId.HasValue)
            {
                query = query
                     .Where(db.Marker.MarkerType == search.MarkerTypeId.Value);
            }

            if (!string.IsNullOrEmpty(search.DisplayNameTerm))
            {
                query = query
                     .Where(db.Marker.DisplayName.Like("%" + search.DisplayNameTerm + "%"));
            }

            var markers = query
                .OrderByOfficeNumber()
                .ToList<Marker>();

            return Ok(MappingEngine.Map<IList<MarkerModel>>(markers));
        }

        // GET api/marker/5
        public IHttpActionResult Get(int id)
        {
            var db = Database.Open();
            Marker marker = db.Marker.Get(id);

            if (marker == null)
                return NotFound();

            return Ok(MappingEngine.Map<MarkerModel>(marker));
        }

        // POST api/marker
        [ValidationActionFilter]
        public IHttpActionResult Post([FromBody]CreateMarkerModel value)
        {
            var marker = MappingEngine.Map<Marker>(value);
            var db = Database.Open();
            marker = db.Marker.Insert(marker);
            
            return Ok(MappingEngine.Map<MarkerModel>(marker));
        }

        // PUT api/marker/5
        [ValidationActionFilter]
        public IHttpActionResult Put(int id, [FromBody]EditMarkerModel value)
        {
            var db = Database.Open();
            Marker marker = db.Marker.Get(id);

            if (marker == null)
                return NotFound();

            value.Id = id;
            MappingEngine.Map(value, marker);
            db.Marker.Update(marker);

            return Ok(MappingEngine.Map<MarkerModel>(marker));
        }

        // DELETE api/marker/5
        public IHttpActionResult Delete(int id)
        {
            var db = Database.Open();
            db.Marker.DeleteByMarkerId(id);

            return Ok(id);
        }
    }
}
