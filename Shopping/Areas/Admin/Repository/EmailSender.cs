using System.Net.Mail;
using System.Net;

namespace Shopping.Areas.Admin.Repository
{
    public class EmailSender: IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true, //bật bảo mật
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("renducluyentai666@gmail.com", "llqaqwpjcyfrrbpr")
            };

            return client.SendMailAsync(
                new MailMessage(from: "renducluyentai666@gmail.com",
                                to: email,
                                subject,
                                message
                                ));
        }
    }
}
