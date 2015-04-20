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
using IDB.Navigator.Site.Models;
using Simple.Data;
//using WebApi;
//using WebAPI.OutputCache;
using System.Device.Location;
using IDB.Navigator.Site.Helpers;
using IDB.Navigator.Domain;
using IDB.Navigator.Site.Helpers.DB;
using IDB.Navigator.Site.Helpers.Outlook;



namespace IDB.Navigator.Site.Controllers.Api
{
    public class MapController : ApiController
    {
        const int buildingId = 2;

        [HttpGet]
      //  [CacheOutput(ClientTimeSpan = 0, ServerTimeSpan = 1000)]
        public MapResponse GetOfficeCoordinatesAndRoute(string OfficeNumber, string UserName = "", string FromOfficeNumber = "",  string DeviceName = "", string SSID = "")
        {
            LogHelper.CreateLog(DeviceName, String.Format("Get Office coordinates for Office {0}", OfficeNumber));
            var db = Database.Open();
            MapResponse response = new MapResponse();
            response.Building = db.Building.FindByBuildingId(buildingId);

            List<Marker> markers = db.Marker.FindAllBy(OfficeNumber: OfficeNumber).With(db.Marker.FloorMap.As("FloorMap"));
            response.TargetMarker = markers.Where(o => o.FloorMap.BuildingId == buildingId).FirstOrDefault();
            response.TargetMarker.Detail = "";

            RouteHelper re = new RouteHelper();
            
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
                if (device == null)
                {
                    device = db.Device.WithFloorMap().FindBySSID(SSID);
                }
                //SSID not found, get DEFAULT location
                if (device == null)
                {
                    device = db.Device.WithFloorMap().FindByName("DEFAULT");
                }

                response.FromMarker = new Marker();
                response.FromMarker.Latitude = device.Latitude;
                response.FromMarker.Longitude = device.Longitude;
                response.FromMarker.FloorMap = device.FloorMap;

                if (device != null)
                {
                    response.Route = re.Calculate(response.TargetMarker.FloorMap.FloorMapId,device.FloorMapId, device.Latitude, device.Longitude, response.TargetMarker.Latitude, response.TargetMarker.Longitude);
                }

            }

            if (response.TargetMarker != null)
            {
                response.TargetMarker.DisplayName = response.TargetMarker.OfficeNumber;

                ActiveDirectory ad = new ActiveDirectory();
                DirectoryUser user = null;

                if(UserName != "")
                    user = ad.SearchUserByUserName(UserName);
                else
                    user = ad.SearchUserByOfficeNumber(response.TargetMarker.OfficeNumber).FirstOrDefault();
                
                if (user != null)
                {
                    response.TargetMarker.Detail = user.DisplayName;

                   /* if (user.Company != response.Building.Company || user.Location != response.Building.Location)
                        return new MapResponse();*/
                }

            }

            
            return response;        
        }

        [HttpGet]
        //[CacheOutput(ClientTimeSpan = 0, ServerTimeSpan = 1000)]
        public MapResponse GetFromCoordinates(int Floor, double Latitude, double Longitude, String DeviceName = "")
        {
            LogHelper.CreateLog(DeviceName, String.Format("Get From coordinates {0}:{1} for Floor {2}", Latitude, Longitude, Floor.ToString()));


                var db = Database.Open();
                MapResponse response = new MapResponse();
                response.Building = db.Building.FindByBuildingId(buildingId);
                response.TargetMarker = GeoHelper.GetFromCoordinates(response.Building, Floor, Latitude, Longitude);
           
                ActiveDirectory ad = new ActiveDirectory();
                if (response.TargetMarker.OfficeNumber.Length > 0)
                {
                    response.TargetMarker.DisplayName = response.TargetMarker.OfficeNumber;
                    List<DirectoryUser> users = ad.SearchUserByOfficeNumber(response.TargetMarker.OfficeNumber);

                    if (users != null)
                    {
                        String detail = "";
                        foreach (DirectoryUser user in users)
                        {
                            detail += user.DisplayName + Environment.NewLine;
                        }
                        response.TargetMarker.Detail = detail;
                    }
                }
                else
                {
                    response.TargetMarker.OfficeNumber = response.TargetMarker.DisplayName;
                }
                

                return response;



      
        }


        [HttpGet]
        //[CacheOutput(ClientTimeSpan = 0, ServerTimeSpan = 1000)]
        public List<Marker> GetOfficeByType(int Floor, string TypeCode, String DeviceName="")
        {
            LogHelper.CreateLog(DeviceName, String.Format("Show Office Type {0} for Floor {1}", TypeCode, Floor.ToString()));

            List<Marker> result = new List<Marker>();
            try
            {
                var db = Database.Open();
                MarkerType type = db.MarkerType.FindByCode(TypeCode);
                FloorMap floor = db.FloorMap.FindAllBy(BuildingId: buildingId, Floor: Floor).FirstOrDefault();
                List<Marker> markers = db.Marker.FindAllBy(MarkerTypeId: type.MarkerTypeId, FloorMapId: floor.FloorMapId).With(db.Marker.MarkerType.As("Type")).With(db.Marker.FloorMap);
                if (TypeCode == "CONF")
                {
                    List<Marker> markerrprivate = db.Marker.FindAllBy(MarkerTypeId: 6, FloorMapId: floor.FloorMapId).With(db.Marker.MarkerType.As("Type")); ;
                    markers.AddRange(markerrprivate);
                    //get status
                    markers = CalendarManager.GetAppointments(markers);
                }
                foreach (Marker marker in markers)
                {
                    result.Add(marker);
                }

            }
            catch (Exception ex) { }

           
            return result;
        }


        /*
        // for AR version, not used
        [HttpGet]
        //[CacheOutput(ClientTimeSpan = 0, ServerTimeSpan = 1000)]
        public String GetIndication(int BuildingId, string MarkId, String Target , String DeviceName)
        {
            LogHelper.CreateLog(DeviceName, String.Format("SGetIndication fromt {0} to {1}", MarkId, Target));
            RouteHelper re = new RouteHelper();

            var db = Database.Open();
            ARMark mark = db.ARMark.FindByMarkId(MarkId);
            Marker markCoor = MarkerHelper.GetMarker(mark);

            List<Marker> markers = db.Marker.FindAllBy(OfficeNumber: Target).With(db.Marker.FloorMap.As("FloorMap"));
            Marker target = markers.Where(o => o.FloorMap.BuildingId == BuildingId).FirstOrDefault();
            target.Detail = "";
           

            List<FloorConnection> ways = re.Calculate(target.FloorMap.FloorMapId, mark.FloorMapId, mark.Latitude, mark.Longitude, target.Latitude, target.Longitude);
            // first route
            Connection route = ways[0].Route[1];
            Node from = route.NodeA;
            Node to = route.NodeB;
            
            foreach (Connection c in ways[0].Route)
            {

                if (GeoHelper.Distance(c.NodeA,c.NodeB) > 1)
                {
                    from = c.NodeA;
                    to = c.NodeB;
                    break;
                }
            }

            double bearing = re.DegreeBearing(from.Latitude, from.Longitude, to.Latitude, to.Longitude) - mark.Bearing;

            if (bearing < 0)
                bearing += 360;

            double distancetoTarget = GeoHelper.Distance(markCoor, target);
            double bearingToTarget = GeoHelper.Distance(markCoor, target) - mark.Bearing;

            double dX = Math.Cos((bearingToTarget * Math.PI / 180.0)) * distancetoTarget;
            double dY = Math.Sin((bearingToTarget * Math.PI / 180.0)) * -distancetoTarget;

            return bearing.ToString() + "|" + dX.ToString() + "|" + dY.ToString();
        }
        */
    }
}
