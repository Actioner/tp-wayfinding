using System;

namespace IDB.Navigator.Domain
{
    public class Person
    {
        public int PersonId { get; set; }
        public int MarkerId { get; set; }
        public string AccountName { get; set; }
        public string DisplayName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Department { get; set; }
        public string Division { get; set; }
        public string ImagePath { get; set; }
        public string OfficeNumber { get; set; }
    }
}