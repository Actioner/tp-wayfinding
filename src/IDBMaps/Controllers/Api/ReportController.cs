using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IDBMaps.Controllers.Api
{
    public class ReportController : ApiController
    {

        [HttpGet]
        public void Get(string Description, string DeviceName)
        {
            NetworkCredential basicCredential =
             new NetworkCredential("ITSUsr_TST", "Tracking8$"); 
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            message.To.Add("rcerrato@iadb.org");
            message.Subject = "IDB Wayfinding - Issue - "+ DeviceName;
            message.From = new System.Net.Mail.MailAddress("ITSUsr_TST@iadb.org");
            message.Body = Description;
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtpapp.iadb.org");
            smtp.Credentials = basicCredential;
            smtp.Send(message);
        }

    }
}
