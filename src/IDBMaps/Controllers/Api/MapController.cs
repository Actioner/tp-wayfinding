using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Net.Http.Headers;
using IDBMaps.Models;
using IDBMaps.Models.DB;
using IDBMaps.Models.Mock;
using IDBMaps.Models.Mapping;
using IDBMaps.Models.Routing;
using Simple.Data;
using WebApi;
using WebAPI.OutputCache;


namespace IDBMaps.Controllers.Api
{
    public class MapController : ApiController
    {
        [HttpGet]
      //  [CacheOutput(ClientTimeSpan = 0, ServerTimeSpan = 1000)]
        public MapResponse GetOfficeCoordinates(int BuildingId, string OfficeNumber, string UserName = "", string FromOfficeNumber = "", double? LatitudeStart = null, double? LongitudeStart = null, string DeviceName = "")
        {
            Log.CreateLog(DeviceName, String.Format("Get Office coordinates for Office {0}", OfficeNumber));


            var db = Database.Open();
            MapResponse response = new MapResponse();
            response.Building = db.Building.FindByBuildingId(BuildingId);
           // response.TargetCoordinate = PDFMapManager.GetOfficeCoordinates(response.Building, OfficeNumber);
            response.TargetCoordinate = Office.GetCoordinate(response.Building, OfficeNumber);
            
            RouteEngine re = new RouteEngine();
            
            if (FromOfficeNumber != string.Empty)
            {
                //not ready
               // Coordinate from = PDFMapManager.GetOfficeCoordinates(response.Building, FromOfficeNumber);
               // Coordinate from = Office.GetCoordinate(response.Building, FromOfficeNumber);
               // response.Route = re.Calculate(response.TargetCoordinate.FloorMap.FloorMapId, 0 ,  from.Latitude, from.Longitude, response.TargetCoordinate.Latitude, response.TargetCoordinate.Longitude);
            }
            else if (DeviceName != string.Empty)
            {
               
                Device device = db.Device.WithFloorMap().FindByName(DeviceName);
                response.FromCoordinate = new Coordinate(device);

                if (device != null)
                {
                    response.Route = re.Calculate(response.TargetCoordinate.FloorMap.FloorMapId,device.FloorMapId, device.Latitude, device.Longitude, response.TargetCoordinate.Latitude, response.TargetCoordinate.Longitude);
                }

            }
            else if (LatitudeStart.HasValue && LongitudeStart.HasValue)
            {
                //not ready
               // response.Route = re.Calculate(response.TargetCoordinate.FloorMap.FloorMapId,0, LatitudeStart.Value, LongitudeStart.Value, response.TargetCoordinate.Latitude, response.TargetCoordinate.Longitude);
            }

            if (response.TargetCoordinate != null)
            {
                response.TargetCoordinate.DisplayName = response.TargetCoordinate.OfficeNumber;

                ActiveDirectory ad = new ActiveDirectory();
                DirectoryUser user = null;

                if(UserName != "")
                    user = ad.SearchUserByUserName(UserName);
                else
                    user = ad.SearchUserByOfficeNumber(response.TargetCoordinate.OfficeNumber).FirstOrDefault();
                
                if (user != null)
                {
                    response.TargetCoordinate.Detail = user.DisplayName;

                   /* if (user.Company != response.Building.Company || user.Location != response.Building.Location)
                        return new MapResponse();*/
                }

            }

            
            return response;        
        }

        [HttpGet]
        //[CacheOutput(ClientTimeSpan = 0, ServerTimeSpan = 1000)]
        public MapResponse GetFromCoordinates(int BuildingId, int Floor, double Latitude, double Longitude,String DeviceName)
        {
            Log.CreateLog(DeviceName, String.Format("Get From coordinates {0}:{1} for Floor {2}", Latitude, Longitude, Floor.ToString()));


            var db = Database.Open();
            MapResponse response = new MapResponse();
            response.Building = db.Building.FindByBuildingId(BuildingId);
            response.TargetCoordinate = Office.GetFromCoordinates(response.Building, Floor, Latitude, Longitude);
            response.TargetCoordinate.DisplayName = response.TargetCoordinate.OfficeNumber;

            try
            {
                ActiveDirectory ad = new ActiveDirectory();
                List<DirectoryUser> users = ad.SearchUserByOfficeNumber(response.TargetCoordinate.OfficeNumber);
                
                if (users != null)
                {
                    String detail ="";
                    foreach (DirectoryUser user in users)
                    {
                        detail += user.DisplayName + Environment.NewLine;
                    }
                    response.TargetCoordinate.Detail = detail;
                }
            }
            catch(Exception ex) { }

            
            return response;
        }


        [HttpGet]
        //[CacheOutput(ClientTimeSpan = 0, ServerTimeSpan = 1000)]
        public List<Coordinate> GetOfficeByType(int BuildingId, int Floor, string TypeCode, String DeviceName)
        {
            Log.CreateLog(DeviceName, String.Format("Show Office Type {0} for Floor {1}", TypeCode, Floor.ToString()));

            List<Coordinate> result = new List<Coordinate>();
            try
            {
                var db = Database.Open();
                OfficeType type = db.OfficeType.FindByCode(TypeCode);
                FloorMap floor = db.FloorMap.FindAllBy(BuildingId: BuildingId, Floor: Floor).FirstOrDefault();
                List<Office> offices = db.Office.FindAllBy(OfficeType: type.OfficeTypeId, FloorMapId: floor.FloorMapId).With(db.Office.OfficeType.As("Type")); ;
                List<Office> officesrprivate = db.Office.FindAllBy(OfficeType: 6, FloorMapId: floor.FloorMapId).With(db.Office.OfficeType.As("Type")); ;

                offices.AddRange(officesrprivate);
                //get status
                offices = CalendarManager.GetAppointments(offices);
                
                foreach (Office office in offices)
                {
                    result.Add(office.GetCoordinate());
                }

            }
            catch (Exception ex) { }

           
            return result;
        }

    }
}
