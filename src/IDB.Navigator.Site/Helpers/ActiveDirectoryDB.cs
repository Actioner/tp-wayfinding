using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Text;
using System.Text.RegularExpressions;
using Simple.Data;
using System.Configuration;
using IDB.Navigator.Domain;
using IDB.Navigator.Site.Models;


namespace IDB.Navigator.Site.Helpers.DB
{
    public class ActiveDirectory
    {
        private string picturesFolder = ConfigurationManager.AppSettings["PicturesFolderLocal"];
    
       

        public IEnumerable<DirectoryUser> SearchUserByName(String criteria)
        {
            List<DirectoryUser> list = new List<DirectoryUser>();
            List<Person> listPerson = new List<Person>();
            var db  = Database.Open();

            if (Regex.IsMatch(criteria, @"^*?\d+$"))
            {
                // for search like NE345
                if (criteria.Length == 5)
                    criteria = criteria.Substring(0, 2) + "0" + criteria.Substring(2, 3);


                listPerson.AddRange((List<Person>)db.Person.All().Where(db.Person.OfficeNumber.Like("%" + criteria + "%")));
                listPerson.AddRange((List<Person>)db.Person.All().Where(db.Person.AccountName.Like("%" + criteria.Substring(0, 2) + "-" + criteria.Substring(2) + "%")));

                //fieldsSb.Append(string.Format("(|(physicalDeliveryOfficeName=*{0}*)", criteria));
                //fieldsSb.Append(string.Format("(SAMAccountName={0}*))", criteria.Substring(0, 2) + "-" + criteria.Substring(2)));
            }
            else
            {
                if (criteria.Contains(' '))
                {
                    String[] split = criteria.Split(' ');
                    if (split.Length == 2)
                    {
                        string firstName = split[0];
                        string lastName = split[1];
                        if(lastName.Length>3)
                            listPerson.AddRange((List<Person>)db.Person.All().Where(db.Person.DisplayName.Like("%" + lastName + "%")));
                            //fieldsSb.Append(string.Format("|(DisplayName=*{0}*)", lastName));
                    }
                    
                }

                listPerson.AddRange((List<Person>)db.Person.All().Where(db.Person.DisplayName.Like("%" + criteria + "%")));
               // fieldsSb.Append(string.Format("(DisplayName=*{0}*)", criteria));
            }
       
                foreach (Person p in listPerson)
                {

                        DirectoryUser du = new DirectoryUser();
                        du.AccountName = p.AccountName;
                        du.FirstName = p.FirstName;
                        du.LastName = p.LastName;
                        du.PhoneNumber = p.PhoneNumber;
                        du.OfficeNumber = p.OfficeNumber;
                        du.Division = p.Division;
                        du.Department = p.Department;
                        du.DisplayName = p.DisplayName;
                       du.AvatarUrl = string.Format(picturesFolder, p.AccountName);                    

                        if(du.FirstName != String.Empty && du.LastName !=String.Empty)
                            list.Add(du);
                }
            


            return list;
        }


        public DirectoryUser SearchUserByUserName(String criteria)
        {
            List<DirectoryUser> list = new List<DirectoryUser>();
            List<Person> listPerson = new List<Person>();

            var db = Database.Open();

            listPerson.AddRange((List<Person>)db.Person.All().Where(db.Person.AccountNAme.Like("" + criteria + "")));

           // fieldsSb.Append(string.Format("(SAMAccountName={0})", criteria));

            foreach (Person p in listPerson)
            {

                DirectoryUser du = new DirectoryUser();
                du.AccountName = p.AccountName;
                du.FirstName = p.FirstName;
                du.LastName = p.LastName;
                du.PhoneNumber = p.PhoneNumber;
                du.OfficeNumber = p.OfficeNumber;
                du.Division = p.Division;
                du.Department = p.Department;
                du.DisplayName = p.DisplayName;
                du.AvatarUrl = string.Format(picturesFolder, p.AccountName);   

                if (du.FirstName != String.Empty && du.LastName != String.Empty)
                    list.Add(du);
            }
          

            return list.FirstOrDefault();
        }

        public List<DirectoryUser> SearchUserByOfficeNumber(String criteria)
        {
            List<DirectoryUser> list = new List<DirectoryUser>();
            List<Person> listPerson = new List<Person>();

            var db = Database.Open();

            string officenumber = criteria.Replace("-", "");
            string user = criteria.Substring(0, 2) + "-" + criteria.Substring(2);

            listPerson.AddRange((List<Person>)db.Person.All().Where(db.Person.AccountName.Like("%" + criteria + "%")));
            listPerson.AddRange((List<Person>)db.Person.All().Where(db.Person.OfficeNumber.Like("%" + criteria + "%")));

            // fieldsSb.Append(string.Format("(SAMAccountName={0})", criteria));

            foreach (Person p in listPerson)
            {

                DirectoryUser du = new DirectoryUser();
                du.AccountName = p.AccountName;
                du.FirstName = p.FirstName;
                du.LastName = p.LastName;
                du.PhoneNumber = p.PhoneNumber;
                du.OfficeNumber = p.OfficeNumber;
                du.Division = p.Division;
                du.Department = p.Department;
                du.DisplayName = p.DisplayName;
                du.AvatarUrl = string.Format(picturesFolder, p.AccountName);   

                if (du.FirstName != String.Empty && du.LastName != String.Empty)
                    list.Add(du);
            }

           // fieldsSb.Append(string.Format("(|(SAMAccountName=*{0}*)", user));
           // fieldsSb.Append(string.Format("(physicalDeliveryOfficeName={0}))", officenumber));

           

            return list;
        }
    
    


    }
}