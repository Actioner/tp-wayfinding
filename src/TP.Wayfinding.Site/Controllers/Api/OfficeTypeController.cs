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
using TP.Wayfinding.Site.Models.OfficeType;
using TP.Wayfinding.Site.Components.Extensions;
using System.Drawing.Imaging;
using TP.Wayfinding.Site.Components.Settings;

namespace TP.Wayfinding.Site.Controllers.Api
{
    public class OfficeTypeController : BaseApiController
    {
        // GET api/OfficeType/id
        public IHttpActionResult Get()
        {
            var db = Database.Open();
            var officeTypes = db.OfficeType.All().OrderByDescription().ToList<OfficeType>();

            return Ok(MappingEngine.Map<IList<OfficeTypeModel>>(officeTypes));
        }

        // GET api/officeType/5
        public IHttpActionResult Get(int id)
        {
            var db = Database.Open();
            OfficeType officeType = db.OfficeType.Get(id);

            if (officeType == null)
                return NotFound();

            return Ok(MappingEngine.Map<OfficeTypeModel>(officeType));
        }

        // POST api/officeType
        [ValidationActionFilter]
        public IHttpActionResult Post([FromBody]CreateOfficeTypeModel value)
        {
            var officeType = MappingEngine.Map<OfficeType>(value);
            var db = Database.Open();
            officeType = db.OfficeType.Insert(officeType);
            
            return Ok(MappingEngine.Map<OfficeTypeModel>(officeType));
        }

        // PUT api/officeType/5
        [ValidationActionFilter]
        public IHttpActionResult Put(int id, [FromBody]EditOfficeTypeModel value)
        {
            var db = Database.Open();
            OfficeType officeType = db.OfficeType.Get(id);

            if (officeType == null)
                return NotFound();

            value.Id = id;
            MappingEngine.Map(value, officeType);
            db.OfficeType.Update(officeType);

            return Ok(MappingEngine.Map<OfficeTypeModel>(officeType));
        }

        // DELETE api/officeType/5
        public IHttpActionResult Delete(int id)
        {
            var db = Database.Open();
            db.OfficeType.DeleteByOfficeTypeId(id);

            return Ok(id);
        }
    }
}
