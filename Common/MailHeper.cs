using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Web;
using System.Configuration;

namespace DoAn.Common
{
    public class MailHeper
    {
        public static string passWord = ConfigurationManager.AppSettings["PasswordEmail"];
        public static string email = ConfigurationManager.AppSettings["Email"];
        public static bool sendEmail(string name, string subject, string content, string toEmail)
        {
            bool rs = false;
            try
            {
                MailMessage message = new MailMessage();
                var smtp = new System.Net.Mail.SmtpClient();
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential()
                    {
                        UserName = email,
                        Password = passWord,
                    };
                    
                }
                MailAddress fromAddress = new MailAddress(email, name);
                message.From = fromAddress;
                message.To.Add(toEmail);
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = content;
                smtp.Send(message);
                rs = true;
            }
            catch (Exception)
            {
                rs = false;
            }
            return rs;
        }
    }
}