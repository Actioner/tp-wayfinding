using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TP.Wayfinding.Site.Models.Person
{
    public class PersonListModel
    {
        public int Id { get; set; }
        public string AccountName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Building { get; set; }
        public string Floor { get; set; }
        public string Office { get; set; }
    }
}