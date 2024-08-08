using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MimeKit;
using MimeKit.Utils;
using MimeKit.Text;

namespace Peter.Test.SendMail.Console.Helpers
{
    public class MailServerInfo
    {
        public string EmailServer_Host;
        public int EmailServer_Port;
        public string EmailServer_Account;
        public string EmailServer_Password;
        public string EmailServer_Encryption;
    }
    public static class SendMailHelper
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();

        // Don't use this function when program start.
        // When use in Settings-Test Email, it works well but program start, it locked and application downed so I changed to MailKit component.
        public static bool SendEmailHtmlReceipt(MailServerInfo mailServerInfo, string emailAddressFrom, List<string> emailAddressTos, StreamReader reader)
        {
            Log.Info("--- Start of SendEmailHtmlReceipt --- ");
            MailMessage message = new MailMessage();
            foreach (string s in emailAddressTos)
            {
                message.To.Add(new MailAddress(s));
            }
            message.From = new MailAddress(emailAddressFrom);
            message.Subject = "Paytec Kiosk - Payment Receipt";
            message.IsBodyHtml = true;
            message.Body = reader.ReadToEnd();

            bool bTrue = true;

            using (SmtpClient smtpClient = new SmtpClient())
            {
                smtpClient.Host = mailServerInfo.EmailServer_Host;
                smtpClient.Port = mailServerInfo.EmailServer_Port;
                smtpClient.EnableSsl = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;

                NetworkCredential networkCredential = new NetworkCredential()
                {
                    UserName = emailAddressFrom,
                    Password = mailServerInfo.EmailServer_Password,
                    //Password = "Noafl2020!"
                };

                smtpClient.Credentials = networkCredential;

                try
                {
                    smtpClient.Send(message);
                    Log.Info("--- End of SendEmailHtmlReceipt --- ");
                    bTrue = true;
                }
                catch (Exception ex)
                {
                    Log.Error(ex.ToString());
                    bTrue = false;
                }
            }
            return bTrue;
        }

        //Send email using MailKit.
        public static bool SendEmailHtml(bool IsUseAuthentication, MailServerInfo mailServerInfo, string emailAddressFrom, List<string> emailAddressTos,
            string mailSubject, string mailBody)
        {
            Log.Info("--- Start of SendEmailHtmlAsync --- ");
            MimeMessage message = new MimeMessage();
            foreach (string s in emailAddressTos)
            {
                message.To.Add(MailboxAddress.Parse(s));
            }
            message.From.Add(MailboxAddress.Parse(emailAddressFrom));
            message.Subject = mailSubject;
            message.Body = new TextPart(TextFormat.Html) { Text = $"{mailBody}" };

            bool bTrue = true;

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    client.Connect(mailServerInfo.EmailServer_Host, mailServerInfo.EmailServer_Port);

                    // Note: only needed if the SMTP server requires authentication

                    if (IsUseAuthentication)
                    {
                        string password = mailServerInfo.EmailServer_Password;
                        client.Authenticate(emailAddressFrom, password);
                    }


                    client.Send(message);

                    client.Disconnect(true);
                }
                catch (Exception ex)
                {
                    Log.Error(ex.ToString());
                    bTrue = false;
                }
            }
            return bTrue;
        }

        //Send email using MailKit.
        public static async Task<bool> SendEmailHtmlAsync(MailServerInfo mailServerInfo, string emailAddressFrom, List<string> emailAddressTos,
            string mailSubject, string mailBody)
        {
            Log.Info("--- Start of SendEmailHtmlAsync --- ");
            MimeMessage message = new MimeMessage();
            foreach (string s in emailAddressTos)
            {
                message.To.Add(MailboxAddress.Parse(s));
            }
            message.From.Add(MailboxAddress.Parse(emailAddressFrom));
            message.Subject = mailSubject;
            message.Body = new TextPart(TextFormat.Html) { Text = $"{mailBody}" };

            bool bTrue = true;

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    client.Connect(mailServerInfo.EmailServer_Host, mailServerInfo.EmailServer_Port);

                    string password = mailServerInfo.EmailServer_Password;
                    // Note: only needed if the SMTP server requires authentication
                    //client.Authenticate(emailAddressFrom, password);

                    await client.SendAsync(message);

                    client.Disconnect(true);
                }
                catch (Exception ex)
                {
                    Log.Error(ex.ToString());
                    bTrue = false;
                }
            }
            return bTrue;
        }

        //Send email pdf using MailKit.
        public static async Task<bool> SendEmailPdfReceiptAsync(MailServerInfo mailServerInfo, string emailAddressFrom, List<string> emailAddressTos,
                                                                string mailSubject, string mailBody, string pdfFilePath)
        {
            Log.Info("--- Start of SendEmailHtmlReceipt --- ");
            MimeMessage message = new MimeMessage();
            foreach (string s in emailAddressTos)
            {
                message.To.Add(MailboxAddress.Parse(s));
            }
            message.From.Add(MailboxAddress.Parse(emailAddressFrom));

            message.Subject = mailSubject; // "Paytec Kiosk - Payment Receipt";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = mailBody;
            bodyBuilder.Attachments.Add(pdfFilePath);

            message.Body = bodyBuilder.ToMessageBody();

            bool bTrue = true;

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    client.Connect(mailServerInfo.EmailServer_Host, mailServerInfo.EmailServer_Port);

                    string password = mailServerInfo.EmailServer_Password;
                    // Note: only needed if the SMTP server requires authentication
                    client.Authenticate(emailAddressFrom, password);

                    await client.SendAsync(message);

                    client.Disconnect(true);
                }
                catch (Exception ex)
                {
                    Log.Error(ex.ToString());
                    bTrue = false;
                }
            }
            return bTrue;
        }
        public static bool SendEmailTest()
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("print1@Peter.com.au", "Noafl2020!"),
                EnableSsl = true
            };

            try
            {
                client.Send("peter@Peter.com.au", "peter0301.kim@gmail.com", "test", "testbody"); return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return false;
            }
        }
    }
}
