using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.DirectoryServices;
using System.Reflection;
using System.Diagnostics;
using System.Configuration;

namespace IDB.Navigator.Site.Models
{
    public enum EmployeeType
    {
        Staff,
        Contractual,
        Contractor,
        Secondee,
        Uncategorized
    }

    public class DirectoryUser
    {

        public string AccountName { get; set; }


        public string DisplayName { get; set; }


        public string FirstName { get; set; }


        public string LastName { get; set; }

        public string AvatarUrl { get; set; }


        public string FullDepartment { get; set; }

        public string Department { get; set; }
        public string Division { get; set; }


        public string PhoneNumber { get; set; }


        public string FaxNumber { get; set; }


        public string MobileNumber { get; set; }


        public string Title { get; set; }

 
        public string OfficeNumber { get; set; }


        public string Extension { get; set; }


        public string Location { get; set; }
        

        public string Manager { get; set; }


        public string MailAddress { get; set; }


        public string EmployeeId { get; set; }



        public string Company { get; set; }


        public string MailDrop { get; set; }

        public string Country { get; set; }


        public string Gender { get; set; }


        public string NamePrefix { get; set; }

        public string DirectoryPath { get; set; }

        public EmployeeType EmployeeType { get; set; }

        public string EmployeeTypeCode 
        {
            get
            {
                // codes taken from table [dbo].[EmplType]
                switch (this.EmployeeType)
                {
                    case EmployeeType.Staff:
                        return "EM";
                    case EmployeeType.Contractual:
                        return "CN";
                    case EmployeeType.Contractor:
                        return "CR";
                    case EmployeeType.Secondee:
                        return "SC";
                    default:
                        return string.Empty;
                }
            }
        }

        public string AccountStatus { get; set; }

        public bool Enabled { get; private set; }

        public DirectoryUser()
        {
        }

        public DirectoryUser(SearchResult directoryEntry)
        {
            const string ACC_DIS_CODE = "514"; // active directory account control disabled codes
            const string ACC_DIS_CODE_NE = "66050";

            this.populateFromActiveDirectoryAttributes(directoryEntry);

            this.DirectoryPath = directoryEntry.Path;

            this.AccountName = this.AccountName.Replace("$", "");

            string picturesFolder = ConfigurationManager.AppSettings["PicturesFolder"];

            if (!string.IsNullOrEmpty(picturesFolder))
            {
                this.AvatarUrl = string.Format("http://rcerrato/IDBNavigator/Content/picture/?UserName={1}", picturesFolder, this.AccountName);
            }
            
            string employeeType = directoryEntry.Properties["extensionAttribute1"].Count > 0 ? directoryEntry.Properties["extensionAttribute1"][0].ToString() : string.Empty;
            EmployeeType et;
            Enum.TryParse(employeeType, out et);
            this.EmployeeType = et;
            
            this.Department = string.IsNullOrEmpty(this.FullDepartment) ? string.Empty : this.FullDepartment.Split('/')[0];

            if (this.FullDepartment.Split('/').Count() > 1)
            {
                this.Division = this.FullDepartment.Split('/')[1];
            }

            if (!string.IsNullOrEmpty(this.Manager))
                this.Manager = this.Manager.Split(',')[0].Split('=')[1];

            this.AccountStatus = directoryEntry.Properties["userAccountControl"].Count > 0 ? directoryEntry.Properties["userAccountControl"][0].ToString() : string.Empty;
            this.Enabled = this.AccountStatus != ACC_DIS_CODE && this.AccountStatus != ACC_DIS_CODE_NE;           
        }

       

        private void populateFromActiveDirectoryAttributes(SearchResult directoryEntry)
        {

            this.AccountName = directoryEntry.Properties["SAMAccountName"].Count > 0 ? directoryEntry.Properties["SAMAccountName"][0].ToString() : string.Empty;
            this.DisplayName = directoryEntry.Properties["DisplayName"].Count > 0 ? directoryEntry.Properties["DisplayName"][0].ToString() : string.Empty;
            this.FirstName = directoryEntry.Properties["givenName"].Count > 0 ? directoryEntry.Properties["givenName"][0].ToString() : string.Empty;
            this.LastName = directoryEntry.Properties["sn"].Count > 0 ? directoryEntry.Properties["sn"][0].ToString() : string.Empty;
            this.PhoneNumber = directoryEntry.Properties["telephoneNumber"].Count > 0 ? directoryEntry.Properties["telephoneNumber"][0].ToString() : string.Empty;
            this.FaxNumber = directoryEntry.Properties["facsimileTelephoneNumber"].Count > 0 ? directoryEntry.Properties["facsimileTelephoneNumber"][0].ToString() : string.Empty;
            this.MobileNumber = directoryEntry.Properties["mobile"].Count > 0 ? directoryEntry.Properties["mobile"][0].ToString() : string.Empty;
            this.Title = directoryEntry.Properties["title"].Count > 0 ? directoryEntry.Properties["title"][0].ToString() : string.Empty;
            this.OfficeNumber = directoryEntry.Properties["physicalDeliveryOfficeName"].Count > 0 ? directoryEntry.Properties["physicalDeliveryOfficeName"][0].ToString() : string.Empty;
            this.Extension = directoryEntry.Properties["extensionAttribute12"].Count > 0 ? directoryEntry.Properties["extensionAttribute12"][0].ToString() : string.Empty;
            this.Location = directoryEntry.Properties["extensionAttribute3"].Count > 0 ? directoryEntry.Properties["extensionAttribute3"][0].ToString() : string.Empty;
            this.EmployeeId = directoryEntry.Properties["employeeID"].Count > 0 ? directoryEntry.Properties["employeeID"][0].ToString() : string.Empty;
            this.Company = directoryEntry.Properties["company"].Count > 0 ? directoryEntry.Properties["company"][0].ToString() : string.Empty;
            this.Country = directoryEntry.Properties["co"].Count > 0 ? directoryEntry.Properties["co"][0].ToString() : string.Empty;
            this.FullDepartment = directoryEntry.Properties["department"].Count > 0 ? directoryEntry.Properties["department"][0].ToString() : string.Empty;


            if (OfficeNumber == String.Empty)
                OfficeNumber = AccountName.Replace("-", "").ToUpper();

            /*MemberInfo[] members = this.GetType().GetMembers();

            foreach (MemberInfo member in members)
            {
                object[] attributes = member.GetCustomAttributes(true);

                foreach (object attribute in attributes)
                {
                    UserAttribute ofnca = attribute as UserAttribute;
                    if (ofnca != null && !String.IsNullOrEmpty(ofnca.ActiveDirectoryName))
                    {
                        object value = directoryEntry.Properties[ofnca.ActiveDirectoryName].Count > 0 ? directoryEntry.Properties[ofnca.ActiveDirectoryName][0].ToString() : string.Empty;
                        PropertyInfo pi = (PropertyInfo)member;
                        if (value.GetType() != typeof(DBNull))
                        {
                            value = Convert.ChangeType(value, pi.PropertyType);
                            pi.SetValue(this, value, null);
                        }
                        break;
                    }
                }
            }*/
            //TODO problms with AD
            if (Company == String.Empty)
            {
                Company = "Inter-American Development Bank";
                this.Location = "HQ";
            }
        }

    }
}