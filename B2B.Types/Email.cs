using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Net.Mail;
using System.Collections.Generic;

/// <summary>
/// Descripción breve de Email
/// </summary>
public static class Email
{
    public static bool Send(string addressesFrom, string subject, string messageBody)
    {
        try
        {
            string emailInfo = ConfigurationManager.AppSettings["emailInfo"];

            string smtp = ConfigurationManager.AppSettings["smtp"];
            string emailLogin = ConfigurationManager.AppSettings["emailLogin"];
            string emailPass = ConfigurationManager.AppSettings["emailPass"];
            MailMessage message = new MailMessage();
            message.From = new MailAddress(addressesFrom.ToLowerInvariant());
            message.To.Add(new MailAddress(emailInfo.ToLowerInvariant()));

            message.Subject = subject;
            message.Body = messageBody;
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient(smtp);
            if (!string.IsNullOrEmpty(emailLogin.Trim()) && !string.IsNullOrEmpty(emailPass.Trim()))
            {
                client.Credentials = new System.Net.NetworkCredential(emailLogin, emailPass);
            }
            client.EnableSsl = ConfigurationManager.AppSettings["ssl"].ToLower() == "true" ? true : false;
            if (client.EnableSsl)
            {
                int port = 0;
                if (Int32.TryParse(ConfigurationManager.AppSettings["sslPort"], out port)) client.Port = port;
            }
            client.Send(message);
            return true;
        }
        catch { return false; }
    }

    public static bool Send(string addressesFrom, string addressTo, string subject, string messageBody)
    {
        try
        {
            string emailInfo = ConfigurationManager.AppSettings["emailInfo"];

            string smtp = ConfigurationManager.AppSettings["smtp"];
            string emailLogin = ConfigurationManager.AppSettings["emailLogin"];
            string emailPass = ConfigurationManager.AppSettings["emailPass"];
            MailMessage message = new MailMessage();
            message.From = new MailAddress(addressesFrom.ToLowerInvariant());
            message.To.Add(new MailAddress(addressTo.ToLowerInvariant()));

            message.Subject = subject;
            message.Body = messageBody;
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient(smtp);
            if (!string.IsNullOrEmpty(emailLogin.Trim()) && !string.IsNullOrEmpty(emailPass.Trim()))
            {
                client.Credentials = new System.Net.NetworkCredential(emailLogin, emailPass);
            }
            client.EnableSsl = ConfigurationManager.AppSettings["ssl"].ToLower() == "true" ? true : false;
            if (client.EnableSsl)
            {
                int port = 0;
                if (Int32.TryParse(ConfigurationManager.AppSettings["sslPort"], out port)) client.Port = port;
            }
            client.Send(message);
            return true;
        }
        catch { return false; }
    }

    public static bool Send(List<string> addressesTo, List<string> addressesCc, List<string> addressesBcc, string subject, string messageBody, string att)
    {
        try
        {
            string emailInfo = ConfigurationManager.AppSettings["emailInfo"];

            string smtp = ConfigurationManager.AppSettings["smtp"];
            string emailLogin = ConfigurationManager.AppSettings["emailLogin"];
            string emailPass = ConfigurationManager.AppSettings["emailPass"];
            MailMessage message = new MailMessage();
            message.From = new MailAddress(emailInfo);

            if (addressesTo != null) addressesTo.ForEach(p => { if (!string.IsNullOrEmpty(p)) message.To.Add(p); });
            if (addressesCc != null) addressesCc.ForEach(p => { if (!string.IsNullOrEmpty(p)) message.CC.Add(p); });
            if (addressesBcc != null) addressesBcc.ForEach(p => { if (!string.IsNullOrEmpty(p)) message.Bcc.Add(p); });


            message.Subject = subject;
            message.Body = messageBody;
            message.IsBodyHtml = true;

            if (!string.IsNullOrEmpty(att))
            {
                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(att);
                message.Attachments.Add(attachment);
            }

            SmtpClient client = new SmtpClient(smtp);
            client.EnableSsl = ConfigurationManager.AppSettings["ssl"].ToLower() == "true" ? true : false;
            if (client.EnableSsl)
            {
                int port = 0;
                if (Int32.TryParse(ConfigurationManager.AppSettings["sslPort"], out port)) client.Port = port;
            }
            if (!string.IsNullOrEmpty(emailLogin.Trim()) && !string.IsNullOrEmpty(emailPass.Trim()))
            {
                client.Credentials = new System.Net.NetworkCredential(emailLogin, emailPass);
            }
            client.Send(message);
            return true;
        }
        catch { return false; }
    }
}
