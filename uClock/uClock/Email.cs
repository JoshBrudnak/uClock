using System;
using System.Net;
using System.Net.Mail;


namespace uClock
{
    public class Email
    {
        string user_name;
        string password;
        string subject;
        string body;
        string display;


        public void credential(string user_name1, string password1, string display1 = "UserName")
        {
            user_name = user_name1;
            password = password1;
            if (display1 == "UserName")
            {
                display1 = user_name;
            }
            display = display1;

        }
        public void content(string subject1, string body1)
        {
            subject = subject1;
            body = body1;
        }
        public void send(string to)
        {
            try
            {
                SmtpClient client = new SmtpClient();
                MailMessage mail = new MailMessage();

                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.EnableSsl = true;
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;
                client.Timeout = 2000;
                client.Credentials = new NetworkCredential(user_name, password);

                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                mail.From = new MailAddress(user_name, display);
                mail.To.Add(to);
                mail.Subject = subject;
                mail.Body = body;
                client.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email");
            }
        }

        public void recieve()
        {

        }
    }
}
