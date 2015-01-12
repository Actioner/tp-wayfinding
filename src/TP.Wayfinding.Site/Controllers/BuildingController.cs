using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Simple.Data;
using System.Linq;
using TP.Wayfinding.Site.Models.Building;
using TP.Wayfinding.Domain;

namespace TP.Wayfinding.Site.Controllers
{
    public class BuildingController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new CreateBuildingModel());
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            return View(new EditBuildingModel {
                Id = id
            });
        }
    }
}