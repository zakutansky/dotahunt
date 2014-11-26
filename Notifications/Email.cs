using System.Net.Mail;

namespace Notifications
{
    public class Email
    {
        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="recipients">The recipients.</param>
        public static void SendEmail(string subject, string body, params string[] recipients)
        {
            const string sender = "cgnotificator@outlook.com";
            var client = new SmtpClient("smtp-mail.outlook.com")
            {
                Port = 587,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false
            };

            var credentials = new System.Net.NetworkCredential(sender, "Notif007");
            client.EnableSsl = true;
            client.Credentials = credentials;

            var mail = new MailMessage {From = new MailAddress(sender)};
            foreach (var recipient in recipients)
            {
                mail.To.Add(recipient);
            }
            mail.Subject = subject;
            mail.Body = body;
            client.Send(mail);
        }
    }
}
