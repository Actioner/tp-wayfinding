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
using TP.Wayfinding.Site.Models.Floor;

namespace TP.Wayfinding.Site.Controllers.Api
{
    public class FloorController : BaseApiController
    {
        // GET api/floor/buildingid
        public IHttpActionResult Get([FromUri]FloorSearchModel search)
        {
            var db = Database.Open();
            var floorsDb = search.BuildingId.HasValue ? db.FloorMap.FindAllBy(BuildingId: search.BuildingId.Value) : db.FloorMap.All();
            var floors = floorsDb.OrderByFloor().ToList<FloorMap>();

            return Ok(MappingEngine.Map<IList<FloorListModel>>(floors));
        }

        // GET api/floor/5
        public IHttpActionResult Get(int id)
        {
            var db = Database.Open();
            FloorMap floor = db.FloorMap.Get(id);

            if (floor == null)
                return NotFound();

            return Ok(MappingEngine.Map<FloorModel>(floor));
        }

        // POST api/floor
        [ValidationActionFilter]
        public IHttpActionResult Post([FromBody]CreateFloorModel value)
        {
            var floor = MappingEngine.Map<FloorMap>(value);
            var db = Database.Open();

            floor = db.FloorMap.Insert(floor);     
            return Ok(MappingEngine.Map<FloorModel>(floor));
        }

        // PUT api/floor/5
        [ValidationActionFilter]
        public IHttpActionResult Put(int id, [FromBody]EditFloorModel value)
        {
            var db = Database.Open();
            FloorMap floor = db.FloorMap.Get(id);

            if (floor == null)
                return NotFound();

            value.Id = id;
            MappingEngine.Map(value, floor);

            db.FloorMap.Update(floor);
            return Ok(MappingEngine.Map<FloorModel>(floor));
        }

        // DELETE api/floor/5
        public IHttpActionResult Delete(int id)
        {
            var db = Database.Open();
            var floors = db.FloorMap.DeleteByFloorMapId(id);

            return Ok(id);
        }
    }
}
