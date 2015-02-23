using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IDBMaps.Models
{
     [DataContract]
    public class Person
    {

         private String officeNumber;

        [DataMember]
         public String OfficeNumber
        {
            get { return officeNumber; }
            set { officeNumber = value; }
        }

           private String imagePath;

        [DataMember]
           public String ImagePath
        {
            get { return imagePath; }
            set { imagePath = value; }
        }

          private String division;

        [DataMember]
          public String Division
        {
            get { return division; }
            set { division = value; }
        }

          private String department;

        [DataMember]
          public String Department
        {
            get { return department; }
            set { department = value; }
        }

          private String phoneNumber;

        [DataMember]
          public String PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }

         private String lastName;

        [DataMember]
         public String LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        private String accountName;

        [DataMember]
        public String AccountName
        {
            get { return accountName; }
            set { accountName = value; }
        }

        private String firstName;

        [DataMember]
        public String FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        private String displayName;

        [DataMember]
        public String DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }
    }
}
