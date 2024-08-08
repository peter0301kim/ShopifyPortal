using ShopifyPortal.Integration.PortalDb.Models;
using ShopifyPortal.Integration.PortalDb.Services;
using ShopifyPortal.Shared.Helpers;
using ShopifyPortal.Shared.Models;
using ShopifyPortal.Shared.SettingModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using NLog;
using NLog.Fluent;
using System.Timers;

namespace ShopifyPortal.Pages.Login;

public partial class Login2FADialog
{
    private static Logger Log = LogManager.GetCurrentClassLogger();
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }
    [Parameter] public string MemberEmail { get; set; }
    private string SecretKey { get; set; }

    //---------- for Timer --------------------------------
    private int TimerSeconds { get; set; } = 60;
    private System.Timers.Timer DialogCloseTimer = null;
   //--------------------------------------------------------
    #region TIMER_FOR_REFRESH
   
   
    public void StartTimer()//modify
    {
        DialogCloseTimer = new System.Timers.Timer();
        DialogCloseTimer.Elapsed += new ElapsedEventHandler(OnTimerExecute);
        DialogCloseTimer.Interval = 600000; //10 mins
        DialogCloseTimer.Enabled = true;
    }

    private async void OnTimerExecute(Object source, ElapsedEventArgs e)
    {
        TimerSeconds -= 1;

        await InvokeAsync(()=>
        {
            StateHasChanged();
        });

        if (TimerSeconds == 0)
        {
            OnCancel();
        }


    }

    #endregion
    public Login2FADialog()
    {

    }

    protected override async Task OnInitializedAsync()
    {
        
    }

    protected override async Task OnParametersSetAsync()
    {
        await SendSecretKey();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {

    }

    private async Task SendSecretKey()
    {
        //StartTimer();
        
        IPortalDbMemberService portalDbMemberService = new PortalDbMemberService(PortalDbConnectionSettings);

        //2FA
        portalDbMemberService.DeleteLogin2FA(MemberEmail);

        Random random = new Random();
        int iSecurityKey = random.Next(10000000, 99999999);

        string securityKey = iSecurityKey.ToString();
        portalDbMemberService.AddLogin2FA(MemberEmail, securityKey);

        MailServerInfo mInfo = new MailServerInfo()
        {
            EmailServer_Host = EmailServerSettings.EmailServerHost,
            EmailServer_Port = EmailServerSettings.EmailServerPort,
            EmailServer_Account = EmailServerSettings.EmailServerAccount,
            EmailServer_Password = EmailServerSettings.EmailServerPassword,
            EmailServer_Encryption = EmailServerSettings.EmailServerEncryption,
        };

        string mailSubject = "Brainz Parents Portal - Verify Security Key";

        string emailBody = File.ReadAllText(BrainzParentsPortalSettings.MultifactorAuthentication.Email_2FA_TemplatePathFile);

        emailBody = emailBody.Replace("{Email}", MemberEmail);
        emailBody = emailBody.Replace("{SecurityKey}", securityKey);
        emailBody = emailBody.Replace("{MFAExpireyMinutes}", BrainzParentsPortalSettings.MultifactorAuthentication.MFAExpireyMinutes.ToString());

        await SendMailHelper.SendEmailHtmlAsync(
            mInfo, EmailServerSettings.EmailServerAccount, new List<string> { MemberEmail }, mailSubject, emailBody);

        await Task.Delay(2000);
    }

    private void OnResendSecretKey()
    {
        SendSecretKey();
    }

    private void OnCancel()
    {
        MudDialog.Cancel();
        
    }

    private async Task OnVerifyKey()
    {
        if (string.IsNullOrEmpty(SecretKey))
        {
            DialogService.ShowMessageBox("Alert", $"Please input secret key", yesText: "Close");
            return;
        }

        IPortalDbMemberService portalDbMemberService = new PortalDbMemberService(PortalDbConnectionSettings);
        //Verify key
        var login2FA = portalDbMemberService.GetLogin2FA(MemberEmail);
        if (SecretKey != login2FA.SecretKey)
        {
            await DialogService.ShowMessageBox("Alert", $"The Secret key doesn't match. Please, try again.", yesText: "Close");
            return ;
        }
        else if ((DateTime.Now - login2FA.CreatedDate).Minutes > BrainzParentsPortalSettings.MultifactorAuthentication.MFAExpireyMinutes)
        {
            await DialogService.ShowMessageBox("Alert", $"The Secret key expired. Please, try again.", yesText: "Close");
            return ;
        }

        MudDialog.Close(DialogResult.Ok("SUCCESS"));
    }
}