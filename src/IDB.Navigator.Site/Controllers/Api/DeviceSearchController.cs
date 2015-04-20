using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using IDB.Navigator.Site.Models;
using Simple.Data;
//using WebAPI.OutputCache;
using System.Configuration;
using IDB.Navigator.Site.Helpers;
using IDB.Navigator.Domain;

namespace IDB.Navigator.Site.Controllers.Api
{
    public class DeviceSearchController : ApiController
    {

        [HttpGet]
        //[CacheOutput(ClientTimeSpan = 30, ServerTimeSpan = 30)]
        public DeviceResponse Get(string DeviceName, string SSID = "", Boolean getBuilding = true)
        {
            var db = Database.Open();
            DeviceResponse response = new DeviceResponse();
             response.Device=   db.Device.WithFloorMap().FindByName(DeviceName);
            //name doesn't exists, get SSID
             if (response.Device == null)
            {
                response.Device = db.Device.WithFloorMap().FindBySSID(SSID);
            }
           

            if (getBuilding)
            {
                //SSID not found, get DEFAULT location
                if (response.Device == null)
                {
                    response.Device = db.Device.WithFloorMap().FindByName("DEFAULT");
                }
                //allways IDB building; ID HARDCODED
                response.Building = db.Building.WithFloorMaps().FindByBuildingId(2);
                foreach (FloorMap map in response.Building.FloorMaps)
                {

                    string mapsFolder = ConfigurationManager.AppSettings["MapsFolderLocal"];

                    map.ImagePath = string.Format(mapsFolder, map.ImagePath);
                    //FIX THIS
                    map.Markers = db.Marker.FindAllBy(FloorMapId: map.FloorMapId).With(db.Marker.MarkerType.As("Type")).ToList<Marker>();

                }
                LogHelper.CreateLog(DeviceName, "Configuration and initial data loaded");
            }
            return response;
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
