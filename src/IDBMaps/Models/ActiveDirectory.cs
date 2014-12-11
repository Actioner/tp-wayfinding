using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Text;
using System.Text.RegularExpressions;


namespace IDBMaps.Models
{
    public class ActiveDirectory
    {

        private string _searcherPath = "CN=Default Global Address List,CN=All Global Address Lists,CN=Address Lists Container,CN=IDB,CN=Microsoft Exchange,CN=Services,CN=Configuration,DC=iadb,DC=org";

        public IEnumerable<DirectoryUser> SearchUserByCriteria(DirectorySearchCriteria criteria)
        {
            List<DirectoryUser> list = new List<DirectoryUser>();

            string queryStr = "(&(objectClass=user)(objectCategory=person){0})";

            StringBuilder fieldsSb = new StringBuilder();

            if (!string.IsNullOrEmpty(criteria.AccountName))
            {
                fieldsSb.Append(string.Format("(SAMAccountName=*{0}*)", criteria.AccountName));
            }

            if (!string.IsNullOrEmpty(criteria.FirstName))
            {
                fieldsSb.Append(string.Format("(givenName={0}*)", criteria.FirstName));
            }

            if (!string.IsNullOrEmpty(criteria.LastName))
            {
                fieldsSb.Append(string.Format("(sn={0}*)", criteria.LastName));
            }

            if (!string.IsNullOrEmpty(criteria.PhoneNumber))
            {
                fieldsSb.Append(string.Format("(telephoneNumber=*{0}*)", criteria.PhoneNumber));
            }

            if (!string.IsNullOrEmpty(criteria.Department))
            {
                fieldsSb.Append(string.Format("(department={0}*)", criteria.Department));
            }

            if (!string.IsNullOrEmpty(criteria.Division))
            {
                fieldsSb.Append(string.Format("(department=*{0})", criteria.Division));
            }

            if (!string.IsNullOrEmpty(criteria.Location))
            {
                fieldsSb.Append(string.Format("(extensionAttribute3=*{0}*)", criteria.Location));
            }

            if (!string.IsNullOrEmpty(criteria.EmployeeId))
            {
                fieldsSb.Append(string.Format("(employeeId=*{0}*)", criteria.EmployeeId));
            }

            if (!string.IsNullOrEmpty(criteria.OfficeNumber))
            {
                fieldsSb.Append(string.Format("(physicalDeliveryOfficeName=*{0}*)", criteria.OfficeNumber));
            }

            DirectoryEntry rootEntry = new DirectoryEntry("GC:");
            //DirectoryEntry rootEntry = new DirectoryEntry("LDAP://HQP-WDC-IADB2/CN=Users;DC=idb;DC=iadb;DC=org");
            //DirectoryEntry rootEntry = new DirectoryEntry("LDAP://HQP-WDC-REG2/CN=Users;DC=idb;DC=org");
            //var searchRoot = rootEntry.Parent;
            var searchRoot = rootEntry.Children.Cast<DirectoryEntry>().First();

            using (DirectorySearcher searcher = new DirectorySearcher(searchRoot, string.Format("(showInAddressBook={0})", _searcherPath)))
            {
                string filter = !string.IsNullOrEmpty(fieldsSb.ToString()) ? "(" + fieldsSb.ToString() + ")" : string.Empty;
                searcher.Filter = string.Format(queryStr, filter);
                searcher.PageSize = 10;

                foreach (SearchResult result in searcher.FindAll())
                {
                    DirectoryUser du = new DirectoryUser(result);

                    list.Add(du);
                }
            }

            rootEntry.Close();
            rootEntry.Dispose();

            return list;
        }

        public IEnumerable<DirectoryUser> SearchUserByName(String criteria)
        {
            List<DirectoryUser> list = new List<DirectoryUser>();

            string queryStr = "(&(objectClass=user)(objectCategory=person)(!useraccountcontrol=514){0})";

            StringBuilder fieldsSb = new StringBuilder();

            if (Regex.IsMatch(criteria, @"^*?\d+$"))
            {
                // for search like NE345
                if (criteria.Length == 5)
                    criteria = criteria.Substring(0, 2) + "0" + criteria.Substring(2, 3);

                fieldsSb.Append(string.Format("(|(physicalDeliveryOfficeName={0})", criteria));
                fieldsSb.Append(string.Format("(SAMAccountName={0}*))", criteria.Substring(0,2)+"-"+criteria.Substring(2)));
            }
            else
                fieldsSb.Append(string.Format("(DisplayName=*{0}*)", criteria));
         //   fieldsSb.Append(string.Format("(SAMAccountName={0}*)", criteria));
          //  fieldsSb.Append(string.Format("(sn={0}*)", criteria));
        //    fieldsSb.Append(string.Format("(physicalDeliveryOfficeName={0}))", criteria));
            DirectoryEntry rootEntry = new DirectoryEntry("GC:");
            //DirectoryEntry rootEntry = new DirectoryEntry("LDAP://HQP-WDC-IADB2/CN=Users;DC=idb;DC=iadb;DC=org");
            //DirectoryEntry rootEntry = new DirectoryEntry("LDAP://HQP-WDC-REG2/CN=Users;DC=idb;DC=org");
            //var searchRoot = rootEntry.Parent;
            var searchRoot = rootEntry.Children.Cast<DirectoryEntry>().First();

            using (DirectorySearcher searcher = new DirectorySearcher(searchRoot, string.Format("(showInAddressBook={0})", _searcherPath)))
            {
                string filter = !string.IsNullOrEmpty(fieldsSb.ToString()) ? "(" + fieldsSb.ToString() + ")" : string.Empty;
                searcher.Filter = string.Format(queryStr, filter);
                searcher.PageSize = 10;

                foreach (SearchResult result in searcher.FindAll())
                {
                    DirectoryUser du = new DirectoryUser(result);

                    list.Add(du);
                }
            }

            rootEntry.Close();
            rootEntry.Dispose();

            return list;
        }


        public DirectoryUser SearchUserByUserName(String criteria)
        {
            List<DirectoryUser> list = new List<DirectoryUser>();

            string queryStr = "(&(objectClass=user)(objectCategory=person)(!useraccountcontrol=514){0})";


            StringBuilder fieldsSb = new StringBuilder();

            fieldsSb.Append(string.Format("(SAMAccountName={0})", criteria));
         
            DirectoryEntry rootEntry = new DirectoryEntry("GC:");
            //DirectoryEntry rootEntry = new DirectoryEntry("LDAP://HQP-WDC-IADB2/CN=Users;DC=idb;DC=iadb;DC=org");
            //DirectoryEntry rootEntry = new DirectoryEntry("LDAP://HQP-WDC-REG2/CN=Users;DC=idb;DC=org");
            //var searchRoot = rootEntry.Parent;
            var searchRoot = rootEntry.Children.Cast<DirectoryEntry>().First();

            using (DirectorySearcher searcher = new DirectorySearcher(searchRoot, string.Format("(showInAddressBook={0})", _searcherPath)))
            {
                string filter = !string.IsNullOrEmpty(fieldsSb.ToString()) ? "(" + fieldsSb.ToString() + ")" : string.Empty;
                searcher.Filter = string.Format(queryStr, filter);
                searcher.PageSize = 10;

                foreach (SearchResult result in searcher.FindAll())
                {
                    DirectoryUser du = new DirectoryUser(result);

                    list.Add(du);
                }
            }

            rootEntry.Close();
            rootEntry.Dispose();

            return list.FirstOrDefault();
        }

        public List<DirectoryUser> SearchUserByOfficeNumber(String criteria)
        {
            List<DirectoryUser> list = new List<DirectoryUser>();

            string queryStr = "(&(objectClass=user)(objectCategory=person)(!useraccountcontrol=514){0})";


            StringBuilder fieldsSb = new StringBuilder();

            string officenumber = criteria.Replace("-", "");
            string user = criteria.Substring(0, 2) + "-" + criteria.Substring(2);

            fieldsSb.Append(string.Format("(|(SAMAccountName=*{0}*)", user));
            fieldsSb.Append(string.Format("(physicalDeliveryOfficeName={0}))", officenumber));

            DirectoryEntry rootEntry = new DirectoryEntry("GC:");
            //DirectoryEntry rootEntry = new DirectoryEntry("LDAP://HQP-WDC-IADB2/CN=Users;DC=idb;DC=iadb;DC=org");
            //DirectoryEntry rootEntry = new DirectoryEntry("LDAP://HQP-WDC-REG2/CN=Users;DC=idb;DC=org");
            //var searchRoot = rootEntry.Parent;
            var searchRoot = rootEntry.Children.Cast<DirectoryEntry>().First();

            using (DirectorySearcher searcher = new DirectorySearcher(searchRoot, string.Format("(showInAddressBook={0})", _searcherPath)))
            {
                string filter = !string.IsNullOrEmpty(fieldsSb.ToString()) ? "(" + fieldsSb.ToString() + ")" : string.Empty;
                searcher.Filter = string.Format(queryStr, filter);
                searcher.PageSize = 10;

                foreach (SearchResult result in searcher.FindAll())
                {
                    DirectoryUser du = new DirectoryUser(result);

                    list.Add(du);
                }
            }

            rootEntry.Close();
            rootEntry.Dispose();

            return list;
        }
    }
}