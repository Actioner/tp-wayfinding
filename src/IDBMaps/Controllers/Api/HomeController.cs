using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.IO;
using System.Net.Http.Headers;
using System.Net;
using System.Drawing.Imaging;
using System.Drawing;
using Simple.Data;
using IDBMaps.Models.Routing;
using IDBMaps.Models;

namespace IDBMaps.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public void UpdateMarker(int OfficeId, double Latitude, double Longitude)
        {
            var db = Database.Open();

            db.Office.UpdateByOfficeId(OfficeId: OfficeId, Latitude: Latitude, Longitude: Longitude);

        }

        [HttpPost]
        public void DeleteMarker(int OfficeId)
        {
            var db = Database.Open();

            db.Office.DeleteByOfficeId(OfficeId: OfficeId);

        }

        [HttpPost]
        public void AddMarker(string OfficeNumber,int FloorMapId, Double Latitude, Double Longitude)
        {
            var db = Database.Open();

            if (OfficeNumber == "BM")
            {
                db.Office.Insert(OfficeNumber: "", DisplayName: "MEN", Latitude: Latitude, Longitude: Longitude, FloorMapId: FloorMapId, OfficeType: 1);
                return;
            }
            if (OfficeNumber == "BW")
            {
                db.Office.Insert(OfficeNumber: "", DisplayName: "WOMAN", Latitude: Latitude, Longitude: Longitude, FloorMapId: FloorMapId, OfficeType: 2);
                return;
            }

            db.Office.Insert(OfficeNumber: OfficeNumber, Latitude: Latitude, Longitude: Longitude, FloorMapId: FloorMapId , OfficeType:4);    

        }

        [HttpGet]
        public ActionResult GetOffices(int FloorMapId)
        {
            var db = Database.Open();
            FloorMap map = db.FloorMap.Get(FloorMapId);

            List<Office> offices = db.Office.FindAllBy(FloorMapId: map.FloorMapId).With(db.Office.OfficeType.As("Type"));
            map.Coordinates = new List<Coordinate>();
            foreach (Office office in offices)
                map.Coordinates.Add(office.GetCoordinate()); 
            return Json(map, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetGraph(int FloorMapId)
        {
            var db = Database.Open();

            List<Node> Nodes = db.Node.FindAllByFloorMapId(FloorMapId);

            if (Nodes.Count == 0)
                return null;

            //get closer nodes      
            List<Connection> Connections = db.Connection.FindAllByFloorMapId(FloorMapId);//.With(db.Connection.NodeA.As("NodeA")).With(db.Connection.NodeB.As("NodeB"));
            Connections.ForEach(c => { c.NodeA = Nodes.Where(n => n.NodeId == c.NodeAId).FirstOrDefault(); c.NodeB = Nodes.Where(n => n.NodeId == c.NodeBId).FirstOrDefault(); });

            return Json(Connections, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetNodes(int FloorMapId)
        {
            var db = Database.Open();

            List<Node> Nodes = db.Node.FindAllByFloorMapId(FloorMapId);


            return Json(Nodes, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public void AddNode(string NodeId, int FloorMapId, Double Latitude, Double Longitude)
        {
            var db = Database.Open();

            db.Node.Insert(Identifier: NodeId, Latitude: Latitude, Longitude: Longitude, FloorMapId: FloorMapId);

        }

        [HttpPost]
        public void DeleteNode(int NodeId)
        {
            var db = Database.Open();

            db.Node.DeleteByNodeId(NodeId: NodeId);
            db.Connection.DeleteByNodeAId(NodeAId: NodeId);
            db.Connection.DeleteByNodeBId(NodeBId: NodeId);
        }

        [HttpPost]
        public void AddConnection(string NodeAId, string NodeBId, int FloorMapId, Boolean Show)
        {
            var db = Database.Open();

            db.Connection.Insert(NodeAId: NodeAId, NodeBId: NodeBId, FloorMapId: FloorMapId, Show: Show);

        }

        [HttpPost]
        public void DeleteConnection(int ConnectionId)
        {
            var db = Database.Open();

            db.Connection.DeleteByConnectionId(ConnectionId: ConnectionId);

        }


        [HttpPost]
        public void UpdateNode(int NodeId, double Latitude, double Longitude)
        {
            var db = Database.Open();

            db.Node.UpdateByNodeId(NodeId: NodeId, Latitude: Latitude, Longitude: Longitude);

        }
    }
}
