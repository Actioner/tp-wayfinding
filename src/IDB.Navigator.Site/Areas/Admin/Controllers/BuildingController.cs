using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Simple.Data;
using System.Linq;
using IDB.Navigator.Site.Areas.Admin.Models.Building;
using IDB.Navigator.Domain;

namespace IDB.Navigator.Site.Areas.Admin.Controllers
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