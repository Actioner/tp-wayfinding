using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using Simple.Data;
using TP.Wayfinding.Domain;
using TP.Wayfinding.Site.Components.Filters;
using TP.Wayfinding.Site.Components.Settings;
using TP.Wayfinding.Site.Models.Person;

namespace TP.Wayfinding.Site.Controllers.Api
{
    public class PersonController : BaseApiController
    {
        // GET api/person
        public IHttpActionResult Get([FromUri]PersonSearchModel search)
        {
            var db = Database.Open();
            var result = new List<PersonListModel>();
            var query = db.Person
                        .All();

            if (search.BuildingId.HasValue)
            {
                query = query
                    .Where(db.Person.Office.FloorMap.Building.BuildingId == search.BuildingId.Value);
            }

            if (!string.IsNullOrWhiteSpace(search.AccountName))
            {
                query = query
                    .Where(db.Person.AccountName.Like("%" + search.AccountName + "%"));
            }

            var persons = query
                .Select(db.Person.AllColumns(),
                            db.Person.Office.DisplayName.As("Office"),
                            db.Person.Office.FloorMap.Description.As("Floor"),
                            db.Person.Office.FloorMap.Building.Name.As("Building"))
                .OrderBy(db.Person.AccountName)
                .Skip(search.CurrentPageIndex * search.PageSize)
                .Take(search.PageSize)
                .ToList();

            foreach (var person in persons)
            {
                var personModel = new PersonListModel
                {
                    Id = person.PersonId,
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    AccountName = person.AccountName,
                    Floor = person.Floor,
                    Office = person.Office,
                    Building = person.Building,
                    ImagePath = string.Format("{0}://{1}{2}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority, GlobalSettings.PeopleFolder.Replace("~", "") + person.ImagePath)
                };

                result.Add(personModel);
            }

            return Ok(new
                {
                    Data = result,
                    Count = query.Count()
                });
        }

        // GET api/person/5
        public IHttpActionResult Get(int id)
        {
            var db = Database.Open();
            Person person = db.Person.Get(id);

            if (person == null)
                return NotFound();

            return Ok(MappingEngine.Map<PersonModel>(person));
        }

        // POST api/person
        [ValidationActionFilter]
        public IHttpActionResult Post([FromBody]CreatePersonModel value)
        {
            var person = MappingEngine.Map<Person>(value);
            var db = Database.Open();

            person = db.Person.Insert(person);     
            return Ok(MappingEngine.Map<PersonModel>(person));
        }

        // PUT api/person/5
        [ValidationActionFilter]
        public IHttpActionResult Put(int id, [FromBody]EditPersonModel value)
        {
            var db = Database.Open();
            Person person = db.Person.Get(id);

            if (person == null)
                return NotFound();

            value.Id = id;
            MappingEngine.Map(value, person);

            Office office = db.Office.Get(person.OfficeId);
            person.OfficeNumber = office.OfficeNumber;


            db.Person.Update(person);
            return Ok(MappingEngine.Map<PersonModel>(person));
        }

        // DELETE api/person/5
        public IHttpActionResult Delete(int id)
        {
            var db = Database.Open();
            db.Person.DeleteByPersonId(id);

            return Ok(id);
        }
    }
}
