using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using WebservicesSage.Utils;

namespace WebservicesSage.Utils
{
    static class UtilsMail
    {
        public static void SendErrorMail(string body, string subject)
        {
            /*
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("5fluxerror@gmail.com", "inax2f5q");

            MailMessage mm = new MailMessage("donotreply@5fluxerror.com", "5fluxerror@gmail.com", UtilsConfig.User+" ERROR : "+subject, body);
            mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            client.Send(mm);*/
        }
    }
}
