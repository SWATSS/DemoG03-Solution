using System.Net;
using System.Net.Mail;

namespace DemoG03.PresentationLayer.Utilities
{
    static class EmailSettings
    {
        public static void SendEmail(Email email)
        {
            // Smtp => For Mailing
            var client = new SmtpClient("smtp.gmail.com", 587);// Host, Port
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("ibrahimtaala6@gmail.com", "aecogpkgwkvnubgb"); // Sender
                                                                                                       // How To Get Password => Go Through gmail and manage Ur Google Account
                                                                                                       //                     => Search For App Password
                                                                                                       //                     => Then Add One And Copy  The Password it Shows and Remove Spaces
            client.Send("ibrahimtaala6@gmail.com", email.To, email.Subject, email.Body);


        }
    }
}
