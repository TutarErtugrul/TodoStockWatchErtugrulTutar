using MimeKit;
using MailKit.Net.Smtp;
namespace ProductServiceAPI.Services
{
    public class EmailService : IEmailService
    {
        public void SendEmail(string toEmail, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("StockWatch", "noreply@stockwatch.com"));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = body
            };

            using var client = new SmtpClient();
            client.Connect("smtp.example.com", 587, false);
            client.Authenticate("noreply@stockwatch.com", "password"); 
            client.Send(message);
            client.Disconnect(true);

        }
    }

    public interface IEmailService
    {
        void SendEmail(string toEmail, string subject, string body);
    }
}
