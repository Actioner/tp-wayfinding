using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using IDBMaps.Models;
using Simple.Data;
using WebAPI.OutputCache;

namespace IDBMaps.Controllers.Api
{
    public class DeviceController : ApiController
    {

        [HttpGet]
        [CacheOutput(ClientTimeSpan = 0, ServerTimeSpan = 1000)]
        public Device Get(string Name)
        {
            var db = Database.Open();
            Device device = db.Device.WithFloorMap().FindByName(Name);
            device.Building = db.Building.WithFloorMaps().FindByBuildingId(device.FloorMap.BuildingId);

            foreach (FloorMap map in device.Building.FloorMaps)
            {
                List<Office> offices = db.Office.FindAllBy(FloorMapId: map.FloorMapId).With(db.Office.OfficeType.As("Type"));
                map.Coordinates = new List<Coordinate>();
                foreach (Office office in offices)
                    map.Coordinates.Add(office.GetCoordinate());                    
            }

            return device;
        }
    }
}
