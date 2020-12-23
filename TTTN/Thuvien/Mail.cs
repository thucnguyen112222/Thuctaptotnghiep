using System;
using System.Net;
using System.Net.Mail;

namespace TTTN
{
    public class Mail
    {
        public static void SendMail(string toEmail, string content,string fromEmailDisplayName, string subject)
        {
            //người nhận
            string fromEmailAddress = toEmail;
          
            //người gửi
            string toEmailAddress = "thucnguyen5511@gmail.com";
            string fromEmailPassword = "2214096811";
            //server
            string smtpHost = "smtp.gmail.com";
            string smtpPost = "587";
            MailMessage message = new MailMessage(new MailAddress(toEmailAddress, fromEmailDisplayName),
                new MailAddress(fromEmailAddress));
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = content;
            var client = new SmtpClient();
            client.Credentials = new NetworkCredential(toEmailAddress, fromEmailPassword);
            client.Host = smtpHost;
            client.EnableSsl = true;
            client.Port = !string.IsNullOrEmpty(smtpPost) ? Convert.ToInt32(smtpPost) : 0;
            client.Send(message);
        }
    }
}