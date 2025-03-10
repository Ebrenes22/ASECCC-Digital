using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace ASECCC_Digital.Services
{
    public class EmailService
    {
        public void EnviarCorreo(string destino, string asunto, string cuerpo)
        {
            var smtpServer = ConfigurationManager.AppSettings["SmtpServer"];
            var smtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
            var senderEmail = ConfigurationManager.AppSettings["SenderEmail"];
            var senderPassword = ConfigurationManager.AppSettings["SenderPassword"];
            var enableSSL = bool.Parse(ConfigurationManager.AppSettings["EnableSSL"]);
            using (var client = new SmtpClient(smtpServer, smtpPort))
            {
                client.Credentials = new NetworkCredential(senderEmail, senderPassword);
                client.EnableSsl = enableSSL;
                var message = new MailMessage(senderEmail, destino, asunto, cuerpo)
                {
                    IsBodyHtml = true
                };
                client.Send(message);
            }
        }
    }
}