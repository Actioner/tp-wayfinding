using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDB.Navigator.Site.Areas.Admin.Models.Person
{
    public class PersonModel
    {
        public int Id { get; set; }
        public int MarkerId { get; set; }
        public string AccountName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Department { get; set; }
        public string Division { get; set; }
        public string DisplayName { get; set; }
        public string ImagePath { get; set; }
    }
}