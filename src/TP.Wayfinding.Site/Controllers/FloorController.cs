using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Simple.Data;
using System.Linq;
using TP.Wayfinding.Site.Models.Floor;
using TP.Wayfinding.Domain;

namespace TP.Wayfinding.Site.Controllers
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