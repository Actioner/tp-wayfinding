using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using IDBMaps.Models;
using Simple.Data;
using WebAPI.OutputCache;
using System.Configuration;

namespace IDBMaps.Controllers.Api
{
    public class DeviceController : ApiController
    {

        [HttpGet]
        [CacheOutput(ClientTimeSpan = 30, ServerTimeSpan = 30)]
        public Device Get(string DeviceName)
        {
            var db = Database.Open();
            Device device = db.Device.WithFloorMap().FindByName(DeviceName);
            device.Building = db.Building.WithFloorMaps().FindByBuildingId(device.FloorMap.BuildingId);

            foreach (FloorMap map in device.Building.FloorMaps)
            {

                string mapsFolder = ConfigurationManager.AppSettings["MapsFolderLocal"];

                map.ImagePath = string.Format(mapsFolder, map.ImagePath);
                List<Office> offices = db.Office.FindAllBy(FloorMapId: map.FloorMapId).With(db.Office.OfficeType.As("Type"));
                map.Coordinates = new List<Coordinate>();
                foreach (Office office in offices)
                    map.Coordinates.Add(office.GetCoordinate());                    
            }

            Log.CreateLog(DeviceName, "Configuration and initial data loaded");

            return device;
        }

        [HttpPost]
        public void Post(string DeviceName)
        {
            var db = Database.Open();
            Device device = db.Device.WithFloorMap().FindByName(DeviceName);
            device.LastTick = DateTime.Now;

            db.Device.UpdateByDeviceId(device);

            //Log.CreateLog(DeviceName, "Ticked - Alive");
        }
    }
}
