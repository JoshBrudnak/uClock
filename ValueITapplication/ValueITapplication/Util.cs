
namespace ValueITapplication
{
    using System;
    using System.Net.NetworkInformation;
    using System.Windows;
    using SystemWon;

    /// <summary>
    /// Methods for redundant tasks in Value IT Application
    /// </summary>
    public class Util
    {
        public void BugReport(string title, string desc)
        {
            string ml = "Title: " + title + "@" + "Description: " + desc;

            ml = ml.Replace("@", Environment.NewLine);
            Email mail = new Email();
            mail.credential("ValAppMail@gmail.com", "Valitcon13542!", "Bug Report");
            mail.content(title, ml);
            mail.send("josh@Valueitconsulting.com");
        }

        public string CurrDate()
        {
            string year = DateTime.Now.Year.ToString();
            string month = DateTime.Now.Month.ToString();
            string day = DateTime.Now.Day.ToString();
            string finDate = year + "-" + month + "-" + day;

            return finDate;
        }

        public string ufTime(object sender, RoutedEventArgs e)
        {
            string time = DateTime.Now.TimeOfDay.ToString();
            return time;
        }

        public string ufDate()
        {
            string date = DateTime.Now.Date.ToString();
            return date;
        }

        public string CurrTime()
        {
            string hour = DateTime.Now.Hour.ToString();
            string minute = DateTime.Now.Minute.ToString();
            string second = DateTime.Now.Second.ToString();
            string finTime = hour + ":" + minute + ":" + second;
            return finTime;
        }

        public string DateFormat(string date)
        {
            char z = "/"[0];
            char zq = " "[0];
            string[] words = date.Split(z);
            string[] dt = words[2].Split(zq);
            string finDate = dt[0] + "-" + words[1] + "-" + words[0];
            return finDate;
        }

        public bool CheckConnection()
        {
            try { 
                Ping myPing = new Ping();
                String host = "google.com";
                byte[] buffer = new byte[32];
                int timeout = 2000;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
                return (reply.Status == IPStatus.Success);
            }
            catch (Exception) {
                return false;
            }
        }
    }
}