using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Simple.Data;
using System.Linq;
using IDB.Navigator.Site.Areas.Admin.Models.Floor;
using IDB.Navigator.Domain;

namespace IDB.Navigator.Site.Areas.Admin.Controllers
{
    public class FloorController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create(int buildingId)
        {
            return View(new CreateFloorModel {
                BuildingId = buildingId
            });
        }

        [HttpGet]
        public ActionResult View(int id)
        {
            return View(new EditFloorModel {
                Id = id
            });
        }
    }
}