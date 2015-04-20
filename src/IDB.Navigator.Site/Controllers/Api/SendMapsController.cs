using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;

namespace IDB.Navigator.Site.Controllers.Api
{
    public class SendMapsController : ApiController
    {

 
        public void Post(string Email, string DeviceName)
        {
            int i = 0;

            MailMessage message = new MailMessage();

            foreach (string filename in HttpContext.Current.Request.Files)
            {
                HttpPostedFileBase file =
                             new HttpPostedFileWrapper(HttpContext.Current.Request.Files[filename]);
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    String id = Guid.NewGuid().ToString();
                    var path = Path.Combine(
                        HttpContext.Current.Server.MapPath("~/content/uploads"),
                        id+".jpg"
                    );
                    
                    file.SaveAs(path);
                    i++;

                    Attachment att = new Attachment(path);
                    att.ContentDisposition.Inline = true;
                    att.ContentId = id;
                    message.Attachments.Add(att);
                    message.Body += String.Format(@"<img src=""cid:{0}"" />", att.ContentId);
                }
            }

            message.From = new System.Net.Mail.MailAddress("ITSUsr_TST@iadb.org");
            message.To.Add(Email);
            message.Subject = "IDB Wayfinding - Issue - " + DeviceName;
            
            message.IsBodyHtml = true;

            NetworkCredential basicCredential =
            new NetworkCredential("ITSUsr_TST", "Tracking8$");
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtpapp.iadb.org");
            smtp.Credentials = basicCredential;
            smtp.Send(message);

           // File.SaveAs("C:\\test.jpg");

        }
    }
}
