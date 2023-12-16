using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace TurnikeMailConsole
{
    public class Mail
    {
        public void SendMail(string todayList,string aweekList)
        {
            try
            {

                string body = "<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />\r\n<title>Untitled Document</title>\r\n</head>\r\n<style>\r\ntable {\r\n  font-family: arial, sans-serif;\r\n  border-collapse: collapse;\r\n  width: 100%;\r\n}\r\n\r\ntd, th {\r\n  border: 1px solid #dddddd;\r\n  text-align: left;\r\n  padding: 8px;\r\n}\r\n\r\ntr:nth-child(even) {\r\n  background-color: #dddddd;\r\n}\r\n</style>\r\n</html>\r\n<body>\r\n<h1>BUGUN</h1>\r\n<table>\r\n  <tr>\r\n    <th>Kullanıcı</th>\r\n    <th>Giriş Tarihi</th>\r\n    <th>Çıkış Tarihi</th>\r\n  </tr> {today}\r\n</table>\r\n\r\n\r\n<h1>SON 1 HAFTA</h1>\r\n<table>\r\n  <tr>\r\n    <th>Kullanıcı</th>\r\n    <th>Giriş Tarihi</th>\r\n    <th>Çıkış Tarihi</th>\r\n  </tr> {lastaweek}\r\n</table>\r\n\r\n</body>\r\n</html>\r\n\r\n";
                body = body.Replace("{today}", todayList);
                body = body.Replace("{lastaweek}", aweekList);
                body = body.Replace("þ", "ş");
                body = body.Replace("ý", "ı");
                SmtpClient sc = new SmtpClient();
                sc.Port = 587;
                sc.Host = "smtp.office365.com";
                sc.EnableSsl = true;

                sc.Credentials = new NetworkCredential("no-reply@iksap.com", "eGHHTEJA*2@w6&xy");
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("no-reply@iksap.com", "TURNIKE");
                mail.To.Add("cihan.abay@iksap.com");
                mail.To.Add("yonetim@iksap.com");
                mail.Subject = "TURNIKE Günlük KAYIT - " + DateTime.Now.ToString("yyyy-MM-dd");
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = true;
                mail.Body = body;
                sc.Send(mail);
            }
            catch(Exception ex)
            {
                File.WriteAllText("log.txt", ex.Message.ToString());
                throw new Exception(ex.Message);
            }
        }
        public class Email
        {
            public Email(string json)
            {
                JObject jObject = JObject.Parse(json);
                JToken Settings = jObject["Settings"];
                Host = (string)Settings["Abp.Mailing.Smtp.Host"];
                Port = (int)Settings["Abp.Mailing.Smtp.Port"];
                UserName = (string)Settings["Abp.Mailing.Smtp.UserName"];
                Password = (string)Settings["Abp.Mailing.Smtp.Password"];
                EnableSsl = (bool)Settings["Abp.Mailing.Smtp.EnableSsl"];
                UseDefaultCredentials = (bool)Settings["Abp.Mailing.Smtp.UseDefaultCredentials"];
                DefaultFromAddress = (string)Settings["Abp.Mailing.DefaultFromAddress"];
                DefaultFromDisplayName = (string)Settings["Abp.Mailing.DefaultFromDisplayName"];
            }
            public string Host { get; set; }
            public int Port { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public string Domain { get; set; }
            public bool EnableSsl { get; set; }
            public bool UseDefaultCredentials { get; set; }
            public string DefaultFromAddress { get; set; }
            public string DefaultFromDisplayName { get; set; }
        }
    }
}
