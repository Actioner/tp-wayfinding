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
using TP.Wayfinding.Site.Models.Office;
using TP.Wayfinding.Site.Components.Extensions;
using System.Drawing.Imaging;
using TP.Wayfinding.Site.Components.Settings;

namespace TP.Wayfinding.Site.Controllers.Api
{
    public class OfficeController : BaseApiController
    {
        // GET api/Office/buildingid
        public IHttpActionResult Get([FromUri]OfficeSearchModel search)
        {
            var db = Database.Open();
            var officesDb = search.FloorMapId.HasValue ? db.Office.FindAllBy(FloorMapId: search.FloorMapId.Value) : db.Office.All();
            var offices = officesDb.OrderByOfficeNumber().ToList<Office>();

            return Ok(MappingEngine.Map<IList<OfficeModel>>(offices));
        }

        // GET api/office/5
        public IHttpActionResult Get(int id)
        {
            var db = Database.Open();
            Office office = db.Office.Get(id);

            if (office == null)
                return NotFound();

            return Ok(MappingEngine.Map<OfficeModel>(office));
        }

        // POST api/office
        [ValidationActionFilter]
        public IHttpActionResult Post([FromBody]CreateOfficeModel value)
        {
            var office = MappingEngine.Map<Office>(value);
            var db = Database.Open();
            office = db.Office.Insert(office);
            
            return Ok(MappingEngine.Map<OfficeModel>(office));
        }

        // PUT api/office/5
        [ValidationActionFilter]
        public IHttpActionResult Put(int id, [FromBody]EditOfficeModel value)
        {
            var db = Database.Open();
            Office office = db.Office.Get(id);

            if (office == null)
                return NotFound();

            value.Id = id;
            MappingEngine.Map(value, office);
            db.Office.Update(office);

            return Ok(MappingEngine.Map<OfficeModel>(office));
        }

        // DELETE api/office/5
        public IHttpActionResult Delete(int id)
        {
            var db = Database.Open();
            db.Office.DeleteByOfficeId(id);

            return Ok(id);
        }
    }
}
