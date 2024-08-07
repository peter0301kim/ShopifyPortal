using BrainzParentsPortal.Integration.PortalDb.Services;
using BrainzParentsPortal.Shared.Helpers;
using BrainzParentsPortal.Shared.SettingModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using NLog;
using System;
using System.ComponentModel.DataAnnotations;

namespace BrainzParentsPortal.Pages.Login;

public partial class ResetPasswordPage
{
    private static Logger Log = LogManager.GetCurrentClassLogger();
    [Parameter] public string MemberEmail { get; set; }
    [Parameter] public string ResetPasswordID { get; set; }

    public class ResetPasswordForm
    {
        [Required]
        [StringLength(30, ErrorMessage = "Password must be at least 8 characters long.", MinimumLength = 8)]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        public string Password2 { get; set; }
    }

    private ResetPasswordForm ResetPasswordFormModel = new ResetPasswordForm();

    private bool IsBusy { get; set; } = false;
    private bool IsProgress { get; set; } = false;

    private bool IsShowPassword { get; set; }
    private bool IsShowPassword2 { get; set; }
    private string PasswordInputIcon { get; set; } = Icons.Material.Filled.VisibilityOff;
    private string PasswordInputIcon2 { get; set; } = Icons.Material.Filled.VisibilityOff;
    private InputType PasswordInput { get; set; } = InputType.Password;
    private InputType PasswordInput2 { get; set; } = InputType.Password;
    
    public ResetPasswordPage()
    {

    }

    protected override async Task OnInitializedAsync()
    {

    }
    protected override async Task OnParametersSetAsync()
    {
        var email = MemberEmail;
        var resetPasswordID = ResetPasswordID;


        //IPortalDbService portalDbService = new PortalDbService(PortalDbConnectionSettings);
        //var resetPassword = portalDbMemberService.GetResetPassword(MemberEmail, ResetPasswordID);


    }
    void OnShowPassword()
    {
        if (IsShowPassword)
        {
            IsShowPassword = false;
            PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
            PasswordInput = InputType.Password;
        }
        else
        {
            IsShowPassword = true;
            PasswordInputIcon = Icons.Material.Filled.Visibility;
            PasswordInput = InputType.Text;
        }
    }

    void OnShowPassword2()
    {
        if (IsShowPassword2)
        {
            IsShowPassword2 = false;
            PasswordInputIcon2 = Icons.Material.Filled.VisibilityOff;
            PasswordInput2 = InputType.Password;
        }
        else
        {
            IsShowPassword2 = true;
            PasswordInputIcon2 = Icons.Material.Filled.Visibility;
            PasswordInput2 = InputType.Text;
        }
    }

    private async void OnValidSubmit(EditContext context)
    {
        if (IsBusy) { Log.Warn("OnValidSubmit is in process. Return."); return; }

        IsBusy = true;
        IsProgress = true;
        
        await Task.Delay(1000);

        IPortalDbMemberService portalDbMemberService = new PortalDbMemberService(PortalDbConnectionSettings);

        var resetPassword = portalDbMemberService.GetResetPassword(MemberEmail, ResetPasswordID);

        //Compare time
        if(ResetPasswordID != resetPassword.ResetPasswordID)
        {
            await DialogService.ShowMessageBox("ALERT", "It's not a valid password reset request. The request code doesn't match.", yesText: "CLOSE");
            IsBusy = false; IsProgress = false;
            StateHasChanged();
            return;
        }
        double expireyMinute = BrainzParentsPortalSettings.ResetPassword.RequestExpireyMinutes;

        if ((DateTime.Now - resetPassword.CreatedDate).TotalMinutes > expireyMinute)
        {
            await DialogService.ShowMessageBox("ALERT", $"The password reset request over {expireyMinute} minutes. The request expired.", yesText: "CLOSE");
            IsBusy = false; IsProgress = false;
            StateHasChanged();
            return;
        }


        string encryptedPass = EncryptionHelper.SHA512(ResetPasswordFormModel.Password);
        var success = portalDbMemberService.UpdateMemberPassword(MemberEmail, encryptedPass);

        if (success)
        {
            await DialogService.ShowMessageBox("SUCCESS", "Password has been changed successfully!", yesText: "OK");
            NavManager.NavigateTo($"{NavManager.BaseUri}Login/LoginPage", true);
        }
        else
        {
            await DialogService.ShowMessageBox("ERROR", "Update password error", yesText: "OK");
        }

        IsBusy = false;
        IsProgress = false;
    }



}