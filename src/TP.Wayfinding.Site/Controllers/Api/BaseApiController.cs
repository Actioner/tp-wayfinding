using System.IO;
using System.Net;
using System.Web.Http;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Security;
using AutoMapper;
using Simple.Data;
using TP.Wayfinding.Site.Components.Services;
using TP.Wayfinding.Site.Models;
using TP.Wayfinding.Site.Models.Account;

namespace TP.Wayfinding.Site.Controllers.Api
{
    [Authorize]
    public abstract class BaseApiController : ApiController
    {
        protected readonly IMappingEngine MappingEngine;

        public BaseApiController()
        {
            MappingEngine = Mapper.Engine;
        }
    }
}