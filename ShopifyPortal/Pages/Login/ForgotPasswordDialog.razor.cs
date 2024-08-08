using Microsoft.AspNetCore.Components;
using MudBlazor;
using NLog;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using ShopifyPortal.Shared;
using ShopifyPortal.Shared.Helpers;
using ShopifyPortal.Shared.SettingModels;
using ShopifyPortal.Integration.PortalDb.Services;
using Peter.Integration.NotificationWebAPI;
using Peter.Integration.NotificationWebAPI.Services;

namespace ShopifyPortal.Pages.Login;

public partial class ForgotPasswordDialog
{
    private static Logger Log = LogManager.GetCurrentClassLogger();
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }

    public class ForgotPasswordForm
    {
        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Invalid email address.")]
        //[EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
    ForgotPasswordForm ForgotPasswordFormModel = new ForgotPasswordForm();
    private MudForm form { get; set; }
    private bool IsFormSuccess { get; set; }
    private string[] Errors { get; set; } = { };
    private bool IsProgress { get; set; } = false;

    public ForgotPasswordDialog()
    {

    }

    protected override async Task OnInitializedAsync()
    {

    }

    protected override async Task OnParametersSetAsync()
    {
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {

    }

    private void OnCancel()
    {
        MudDialog.Cancel();

    }

    private async Task OnSendResetPasswordEmail()
    {
        if (Errors.Count() > 0)
        {
            var strErrors = Errors.ToString();
            await DialogService.ShowMessageBox("ERROR", string.Join(",", Errors), yesText: "OK");
            return;
        }

        IPortalDbMemberService portalDbMemberService = new PortalDbMemberService(PortalDbConnectionSettings);

        var member = portalDbMemberService.GetMemberByEmail(ForgotPasswordFormModel.Email);
        if (member == null )
        {
            await DialogService.ShowMessageBox("Alert", $"{ForgotPasswordFormModel.Email} is not registered.", yesText: "Close");
            ForgotPasswordFormModel.Email = "";
            return;
        }

        portalDbMemberService.DeleteResetPassword(ForgotPasswordFormModel.Email);

        string resetPasswordID = System.Guid.NewGuid().ToString();

        portalDbMemberService.AddResetPassword(ForgotPasswordFormModel.Email, resetPasswordID);

        string mailSubject = "Brainz Parents Portal - Reset password";

        string emailBody = File.ReadAllText(BrainzParentsPortalSettings.ResetPassword.Email_ResetPassword_TemplatePathFile);
        emailBody = emailBody.Replace("{ResetPasswordURL}", BrainzParentsPortalSettings.ResetPassword.ResetPassword_URL);
        emailBody = emailBody.Replace("{Email}", ForgotPasswordFormModel.Email);
        emailBody = emailBody.Replace("{ResetPasswordID}", resetPasswordID);

        //var result = await AddPayecEmailNotification(EmailServerSettings.EmailServerAccount, new List<string> { ForgotPasswordFormModel.Email }, mailSubject, emailBody);

        MailServerInfo mInfo = new MailServerInfo()
        {
            EmailServer_Host = EmailServerSettings.EmailServerHost,
            EmailServer_Port = EmailServerSettings.EmailServerPort,
            EmailServer_Account = EmailServerSettings.EmailServerAccount,
            EmailServer_Password = EmailServerSettings.EmailServerPassword,
            EmailServer_Encryption = EmailServerSettings.EmailServerEncryption,
        };

        var result = await SendMailHelper.SendEmailHtmlAsync(
            mInfo, EmailServerSettings.EmailServerAccount, new List<string> { ForgotPasswordFormModel.Email }, mailSubject, emailBody);

        if (result)
        {
            await DialogService.ShowMessageBox("SUCCESS", $"Reset password email has posted successfully. Please check your email.", yesText: "Close");
            MudDialog.Close(DialogResult.Ok("SUCCESS"));
        }
        else
        {
            await DialogService.ShowMessageBox("FAIL", $"Send reset password email fail. Could you please try again later.", yesText: "Close");
        }        

    }

    private async Task<bool> AddPayecEmailNotification(string emailFrom, List<string> emailTo, string subject, string contents)
    {

        bool isSuccess = true;        
        
        var emailNotificationSettings = LoadNotificationApiSettings();
        IEmailNotificationWebApiService emailNotificationWebApiService = new EmailNotificationWebApiService(emailNotificationSettings);
        var apiReturnValue = await emailNotificationWebApiService.AddEmailNotification(
            emailFrom, emailTo,
            subject,
            contents
            );

        Log.Debug(JsonConvert.SerializeObject(apiReturnValue, Formatting.Indented));

        isSuccess = apiReturnValue.IsSuccess;

        return isSuccess;

    }
    private NotificationWebApiConnectionSettings LoadNotificationApiSettings()
    {

        var notificationWebApiSettingsPathFile = GlobalSettings.Instance.NotificationWebApiSettingsPathFile;

        return JsonConvert.DeserializeObject<NotificationWebApiConnectionSettings>(System.IO.File.ReadAllText(notificationWebApiSettingsPathFile));
    }
}