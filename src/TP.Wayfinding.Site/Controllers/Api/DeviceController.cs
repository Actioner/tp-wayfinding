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
using TP.Wayfinding.Site.Models.Device;

namespace TP.Wayfinding.Site.Controllers.Api
{
    public class DeviceController : BaseApiController
    {
        // GET api/device
        public IHttpActionResult Get([FromUri]DeviceSearchModel search)
        {
            var db = Database.Open();
            var devicesDb = search.FloorMapId.HasValue ? db.Device.FindAllBy(FloorMapId: search.FloorMapId.Value) : db.Device.All();
            var devices = devicesDb.ToList<Device>();

            return Ok(MappingEngine.Map<IList<DeviceModel>>(devices));
        }

        // GET api/device/5
        public IHttpActionResult Get(int id)
        {
            var db = Database.Open();
            Device device = db.Device.Get(id);

            if (device == null)
                return NotFound();

            return Ok(MappingEngine.Map<DeviceModel>(device));
        }

        // POST api/device
        [ValidationActionFilter]
        public IHttpActionResult Post([FromBody]CreateDeviceModel value)
        {
            var device = MappingEngine.Map<Device>(value);
            var db = Database.Open();

            device = db.Device.Insert(device);
            return Ok(MappingEngine.Map<DeviceModel>(device));
        }

        // PUT api/device/5
        [ValidationActionFilter]
        public IHttpActionResult Put(int id, [FromBody]EditDeviceModel value)
        {
            var db = Database.Open();
            Device device = db.Device.Get(id);

            if (device == null)
                return NotFound();

            value.Id = id;
            MappingEngine.Map(value, device);

            db.Device.Update(device);
            return Ok(MappingEngine.Map<DeviceModel>(device));
        }

        // DELETE api/device/5
        public IHttpActionResult Delete(int id)
        {
            var db = Database.Open();
            db.Device.DeleteByDeviceId(id);

            return Ok(id);
        }
    }
}
