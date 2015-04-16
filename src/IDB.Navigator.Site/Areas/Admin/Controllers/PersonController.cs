using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Simple.Data;
using System.Linq;
using IDB.Navigator.Site.Areas.Admin.Models.Person;
using IDB.Navigator.Domain;

namespace IDB.Navigator.Site.Areas.Admin.Controllers
{
    public class PersonController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new CreatePersonModel());
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            return View(new EditPersonModel {
                Id = id
            });
        }

        [HttpGet]
        public ActionResult View(int id)
        {
            return View(new PersonModel
            {
                Id = id
            });
        }
    }
}