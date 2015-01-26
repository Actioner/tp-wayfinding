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
using TP.Wayfinding.Site.Models.Building;

namespace TP.Wayfinding.Site.Controllers.Api
{
    public class BuildingController : BaseApiController
    {
        // GET api/building
        public IHttpActionResult Get([FromUri]BuildingSearchModel search)
        {
            var db = Database.Open();
            var query = db.Building
                        .All();

            if (search.OfficeId.HasValue) 
            {
                query = query
                        .Select(db.Building.AllColumns())
                        .Join(db.FloorMap).On(BuildingId: db.Building.BuildingId)
                        .Join(db.Office).On(FloorMapId: db.FloorMap.FloorMapId)
                        .Where(db.Office.OfficeId == search.OfficeId.Value);
            }

            var buildings = query
              .OrderBy(db.Building.LastUpdated)
              .ToList<Building>();
        
            return Ok(MappingEngine.Map<IList<BuildingModel>>(buildings));
        }
        
        // GET api/building/5
        public IHttpActionResult Get(int id)
        {
            var db = Database.Open();
            Building building = db.Building.Get(id);

            if (building == null)
                return NotFound();

            return Ok(MappingEngine.Map<BuildingModel>(building));
        }

        // POST api/building
        [ValidationActionFilter]
        public IHttpActionResult Post([FromBody]CreateBuildingModel value)
        {
            var building = MappingEngine.Map<Building>(value);
            var db = Database.Open();

            building.LastUpdated = DateTime.Now;
            building = db.Building.Insert(building);     
            return Ok(MappingEngine.Map<BuildingModel>(building));
        }

        // PUT api/building/5
        [ValidationActionFilter]
        public IHttpActionResult Put(int id, [FromBody]EditBuildingModel value)
        {
            var db = Database.Open();
            Building building = db.Building.Get(id);

            if (building == null)
                return NotFound();

            value.Id = id;
            MappingEngine.Map(value, building);
            building.LastUpdated = DateTime.Now;

            db.Building.Update(building);
            return Ok(MappingEngine.Map<BuildingModel>(building));
        }

        // DELETE api/building/5
        public IHttpActionResult Delete(int id)
        {
            var db = Database.Open();
            db.Building.DeleteByBuildingId(id);

            return Ok(id);
        }
    }
}
