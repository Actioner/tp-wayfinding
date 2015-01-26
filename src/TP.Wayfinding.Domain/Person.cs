using System;

namespace TP.Wayfinding.Domain
{
    public class Person
    {
        public int PersonId { get; set; }
        public int OfficeId { get; set; }
        public string AccountName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Department { get; set; }
        public string Division { get; set; }
    }
}