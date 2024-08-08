using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.ComponentModel.DataAnnotations;
using ShopifyPortal.Helpers;
using ShopifyPortal.Shared.Helpers;
using ShopifyPortal.Shared.Models;
using ShopifyPortal.Integration.PortalDb.Services;
using ShopifyPortal.Shared;
using ShopifyPortal.Integration.PortalDb.Models;
using NLog;
using ShopifyPortal.Pages.Transactions;
using ShopifyPortal.Pages.Members;

namespace ShopifyPortal.Pages.Login;

public partial class LoginPage
{
    [CascadingParameter] public MainLayout Layout { get; set; }
    public class LoginForm
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }

    LoginForm LoginFormModel = new LoginForm();

    private static Logger Log = LogManager.GetCurrentClassLogger();
    bool IsBusy { get; set; } = false;
    bool IsProgress { get; set; } = false;
    bool IsLoginSuccess { get; set; }

    bool IsShowPassword { get; set; }
    InputType PasswordInput { get; set; } = InputType.Password;
    string PasswordInputIcon { get; set; } = Icons.Material.Filled.VisibilityOff;

    void OnShowPassword()
    {
        if(IsShowPassword)
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
    protected override async Task OnInitializedAsync()
    {
#if DEBUG

        //LoginFormModel.Email = "John@myshopify.com"; // DEFAULT_MEMBER_01
        LoginFormModel.Email = "peter@myshopify.com.au"; //ADMIN
        LoginFormModel.Password = "Brainz123!@#";
#endif
    }

    private async Task<bool> MultiFactorAuthenticationProcess(IPortalDbMemberService portalDbMemberService)
    {
        

        var parameters = new DialogParameters<Login2FADialog>();
        parameters.Add("MemberEmail", LoginFormModel.Email);

        DialogOptions options = new DialogOptions() { BackdropClick = false };
        var dialog = DialogService.Show<Login2FADialog>("Anthentication", parameters, options);


        var result = await dialog.Result;

        if (result.Canceled)
        {
            //In a real world scenario we would reload the data from the source here since we "removed" it in the dialog already.
            return false;
        }

        return true;
    }
    private async Task<bool> Authenticate()
    {
        IPortalDbMemberService portalDbMemberService = new PortalDbMemberService(PortalDbConnectionSettings);

        bool bTrue = true;

        var userAccount = portalDbMemberService.GetMemberByEmail(LoginFormModel.Email);
        if (userAccount == null || userAccount.Password != EncryptionHelper.SHA512(LoginFormModel.Password))
        {
            bool? result = await DialogService.ShowMessageBox("Alert", $"Invalid User Name or Password", yesText: "Close");
            bTrue = false;
        }
        else
        {
            if (ShopifyPortalSettings.MultifactorAuthentication.IsUseMFA)
            {
                if (!await MultiFactorAuthenticationProcess(portalDbMemberService))
                {
                    return false;
                }
            }

            await CustomAuthenticationStateProvider.UpdateAuthenticationState(
                new UserSession 
                { 
                    UserName = $"{userAccount.FirstName} {userAccount.LastName}", 
                    UserID = userAccount.MemberID, Role = userAccount.Role 
                });

            //Save user action tracking
            UserAction userAction = new UserAction()
            {
                UserID = userAccount.MemberID,
                Action = ActionType.LogIn.ToString(),
                CreatedDate = DateTime.UtcNow,
            };

            portalDbMemberService.AddUserAction(userAction);
            bTrue = true;
            
        }
        return bTrue;
    }

    private async void OnValidSubmit(EditContext context)
    {
        if (IsBusy) { Log.Warn("OnValidSubmit is in process. Return."); return; }

        IsBusy = true;
        IsProgress = true;
        await Task.Delay(1000);

        if (await Authenticate())
        {
            IsLoginSuccess = true;
            NavManager.NavigateTo($"{NavManager.BaseUri}MyBrainz", true);

        }
        else
        {
            NavManager.NavigateTo($"{NavManager.BaseUri}Login/LoginPage", true);
        }

        IsBusy = false;
        IsProgress = false;
    }

    private void OnRegisterUser()
    {
        NavManager.NavigateTo($"{NavManager.BaseUri}AddMember", true);


    }

    private void OnForgotPassword()
    {
        var parameters = new DialogParameters<ForgotPasswordDialog>();
        //parameters.Add("MemberEmail", LoginFormModel.Email);

        DialogOptions options = new DialogOptions() { BackdropClick = false };
        var dialog = DialogService.Show<ForgotPasswordDialog>("Forgot password", parameters, options);
    }
}