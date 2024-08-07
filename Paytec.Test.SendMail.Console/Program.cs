using Newtonsoft.Json;
using NLog;
using Paytec.Test.SendMail.Console.Helpers;
using Paytec.Test.SendMail.Console.SettingModels;
using System.Reflection;

namespace Paytec.Test.SendMail.Console;

internal class Program
{
    private static Logger Log = LogManager.GetCurrentClassLogger();
    public static string AppBaseDir { get; set; } = string.Empty;
    public static string EmailServerSettingsPathFile { get; set; } = string.Empty;
    public static string SendEmailSettingsPathFile { get; set; } = string.Empty;
    public static string Email_ResetPassword_TemplatePathFile { get; set; } = string.Empty;

    public static SendEmailSettings SendEmailSettings { get; set; }
    public static EmailServerSettings EmailServerSettings { get; set;}

    static void Main(string[] args)
    {
        AppBaseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        EmailServerSettingsPathFile = Path.Combine(AppBaseDir, "Configs", "EmailServerSettings.json");
        SendEmailSettingsPathFile = Path.Combine(AppBaseDir, "Configs", "SendEmailSettings.json");
        Email_ResetPassword_TemplatePathFile = Path.Combine(AppBaseDir, "Templates", "Email_ResetPassword_Template.html");

        if (!LoadApplication()) { return; }


        MailServerInfo mInfo = new MailServerInfo()
        {
            EmailServer_Host = EmailServerSettings.EmailServerHost,
            EmailServer_Port = EmailServerSettings.EmailServerPort,
            EmailServer_Account = EmailServerSettings.EmailServerAccount,
            EmailServer_Password = EmailServerSettings.EmailServerPassword,
            EmailServer_Encryption = EmailServerSettings.EmailServerEncryption,
        };


        string mailSubject = "Brainz Parents Portal - Reset password";

        string emailBody = File.ReadAllText(Email_ResetPassword_TemplatePathFile);
        emailBody = emailBody.Replace("{ResetPasswordURL}", "https://localhost");
        emailBody = emailBody.Replace("{Email}", "user@paytec.com.au");
        emailBody = emailBody.Replace("{ResetPasswordID}", "fdfj2f8jfjdslkfjl");


        Log.Debug($"IsUserAuth:{EmailServerSettings.IsUseAuthentication},Mail_From:{SendEmailSettings.Email_From}, Main_To:{SendEmailSettings.Email_To}");
        var result = SendMailHelper.SendEmailHtml(EmailServerSettings.IsUseAuthentication,
            mInfo, SendEmailSettings.Email_From, SendEmailSettings.Email_To, mailSubject, emailBody);

        if (result)
        {
            Log.Debug("SUCCESS");
        }
        else
        {
            Log.Debug("FAIL");
        }



    }
    static bool LoadApplication()
    {
        bool bTrue = true;

        

        if (File.Exists(EmailServerSettingsPathFile))
        {
            EmailServerSettings
                = JsonConvert.DeserializeObject<EmailServerSettings>(File.ReadAllText(EmailServerSettingsPathFile));
        }
        else
        {
            System.Console.WriteLine("No EmailServerSettingsPathFileSettings file", "ERROR");
            bTrue = false;
            return bTrue;
        }


        if (File.Exists(SendEmailSettingsPathFile))
        {
            SendEmailSettings
                = JsonConvert.DeserializeObject<SendEmailSettings>(File.ReadAllText(SendEmailSettingsPathFile));
        }
        else
        {
            System.Console.WriteLine("No SendEmailSettingsPathFile file", "ERROR");
            bTrue = false;
            return bTrue;
        }

        return bTrue;
    }

}
