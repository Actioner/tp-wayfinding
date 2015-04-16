using System.IO;
using System.Net;
using System.Web.Http;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Security;
using AutoMapper;
using Simple.Data;
using IDB.Navigator.Site.Components.Services;
using IDB.Navigator.Site.Areas.Admin.Models;
using IDB.Navigator.Site.Areas.Admin.Models.Account;

namespace IDB.Navigator.Site.Areas.Admin.Controllers.Api
{
    [Authorize(Users= "IDB\\jfrodriguez,IDB\\rcerrato")]
    public abstract class BaseApiController : ApiController
    {
        protected readonly IMappingEngine MappingEngine;

        public BaseApiController()
        {
            MappingEngine = Mapper.Engine;
        }
    }
}