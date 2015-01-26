using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Simple.Data;
using System.Linq;
using TP.Wayfinding.Site.Models.Person;
using TP.Wayfinding.Domain;

namespace TP.Wayfinding.Site.Controllers
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
    }
}