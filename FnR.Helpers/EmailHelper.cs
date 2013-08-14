
using System.Net;
using System.Net.Mail;

namespace FnR.Helpers
{
    public class EmailHelper
    {
        public static void SendEmail(string to, string subject, string body)
        {
            var fromAddress = new MailAddress("felixnrover@appnyxservices.com", "Felix and Rover");
            var toAddress = new MailAddress(to, "FnR Client");
            const string fromPassword = "felixnroveremail";

            var smtp = new SmtpClient
            {
                Host = "mail.appnyxservices.com",
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress) { Subject = subject, Body = body, IsBodyHtml = true })
            {
                smtp.Send(message);
            }
        }

        public static void SendEmailWithAttachment(string to, string subject, string body, Attachment attachment)
        {
            var fromAddress = new MailAddress("felixnrover@appnyxservices.com", "Felix and Rover");
            var toAddress = new MailAddress(to, "FnR Client");
            const string fromPassword = "felixnroveremail";

            var smtp = new SmtpClient
            {
                Host = "mail.appnyxservices.com",
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress) { Subject = subject, Body = body, IsBodyHtml = true})
            {
                message.Attachments.Add(attachment);
                smtp.Send(message);
            }
        }
    }
}
