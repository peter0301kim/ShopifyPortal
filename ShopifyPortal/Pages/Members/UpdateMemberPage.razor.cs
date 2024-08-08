using global::Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.ComponentModel.DataAnnotations;
using Peter.Integration.Shopify.Models;
using Peter.Integration.Shopify.Services;
using ShopifyPortal.Integration.PortalDb.Models;
using ShopifyPortal.Integration.PortalDb.Services;
using ShopifyPortal.Shared.Helpers;
using Microsoft.Extensions.Configuration;
using ShopifyPortal.Helpers;
using Microsoft.JSInterop;
using ShopifyPortal.Shared.SettingModels;
using Newtonsoft.Json;
using static MudBlazor.Colors;

namespace ShopifyPortal.Pages.Members;

public partial class UpdateMemberPage
{
    [CascadingParameter] private Task<AuthenticationState> AuthenticationState { get; set; }
    [Parameter]        public string MemberID { get; set; }

    private UpdateUserAccountForm model = new UpdateUserAccountForm();

    IReadOnlyList<IBrowserFile> MemberImageFiles { get; set; } = new List<IBrowserFile>();
    int MaxAllowedFiles = 3;
    long MaxFileSize = 1024 * 1024 * 1; //represents 3MB
    public class UpdateUserAccountForm
    {
        public string MemberID { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, ErrorMessage = "Name length can't be more than 100.")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100, ErrorMessage = "Name length can't be more than 100.")]
        public string LastName { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "")]
        public string Organization { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "")]
        [RegularExpression("^\\+?[1-9]\\d{1,14}$", ErrorMessage = "Input valid phone number")]
        public string Phone { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "")]
        public string Address { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "")]
        public string Address2 { get; set; }

        [StringLength(100, ErrorMessage = "")]
        public string City { get; set; }

        [StringLength(100, ErrorMessage = "")]
        public string Region { get; set; }

        [StringLength(100, ErrorMessage = "")]
        public string PostCode { get; set; }
        public string ImagePathFile { get; set; } = string.Empty;

    }
    private MudForm form { get; set; }
    string[] Errors { get; set; } = { };
    bool IsProgress { get; set; } = false;

    private IEnumerable<ChildOrganization> ChildOrganizations = new List<ChildOrganization>();

    private IEnumerable<string> SearchPostcodes = new List<string>();
    
    private string selectedPostCodeLocalRegion { get; set; }
    private string SelectedPostCodeLocalRegion {
        get
        {
            return selectedPostCodeLocalRegion;
        }
        set
        {
            selectedPostCodeLocalRegion = value;
            string[] subs = value.Split('|');
            model.PostCode = subs[0];
            model.City = subs[1];
            model.Region = subs[2];
        }
    }

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationState;
        if (authState.User.Identity != null && !authState.User.Identity.IsAuthenticated)
        {
            NavManager.NavigateTo($"{NavManager.BaseUri}Login/LoginPage", true);
            return;
        }
    }

    protected override async Task OnParametersSetAsync()
    {
        IPortalDbMemberService portalDbMemberService = new PortalDbMemberService(PortalDbConnectionSettings);

        if (!string.IsNullOrEmpty(MemberID))
        {
            var user = portalDbMemberService.GetMemberByMemberID(MemberID);
            model = new UpdateUserAccountForm()
            {
                MemberID = user.MemberID,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                Organization = user.Tags,
                ImagePathFile = !string.IsNullOrEmpty(user.ImagePathFile) ? user.ImagePathFile : "Images/Member/DefaultMember.jpeg",
            };

        }

        IPortalDbService portalDbService = new PortalDbService(PortalDbConnectionSettings);
        ChildOrganizations = portalDbService.GetChildOrganizationByMemberID(MemberID);

        SearchPostcodes = NewZealandPostCodeHelper.ReadAllNewZealandPostCode(BrainzParentsPortalSettings.NewzealandPostcode.PostcodePathFile);

    }

    private async void LoadFiles(InputFileChangeEventArgs e)
    {
        //TODO upload the files to the server

        if (e.FileCount > MaxAllowedFiles)
        {
            Errors.Append($"Error: Attempting to upload {e.FileCount} files, but max allowed file count is {MaxAllowedFiles}");
            return;
        }

        MemberImageFiles = e.GetMultipleFiles(MaxAllowedFiles);

        if (MemberImageFiles.Count > 0)
        {
            var file = MemberImageFiles.FirstOrDefault();
            var firstToken = (model.FirstName.Contains(" ")) ? model.FirstName.Substring(0, model.FirstName.IndexOf(" ")) : model.FirstName;
            if (firstToken.Contains("'")) { firstToken.Replace("'", ""); }

            if (!ImageHelper.VerifyImageFileExtension(Path.GetExtension(file.Name)))
            {
                MemberImageFiles = null;
                await DialogService.ShowMessageBox(
                        "Warning", $"Please select an image file - (.jpg, .jpeg, .png)", yesText: "OK");
                return;
            }

            string newFileName = $"{Path.GetFileNameWithoutExtension(file.Name)}-{firstToken}{Path.GetExtension(file.Name)}";

            string path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Configuration.GetValue<string>("ImageMemberPath")!);
            //string path = Path.Combine(Configuration.GetValue<string>("ImageFileStorage")!, "Images", "School");

            if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }

            var imagePathFile = Path.Combine(path, newFileName);

            await using FileStream fs = new(imagePathFile, FileMode.Create);
            await file.OpenReadStream(MaxFileSize).CopyToAsync(fs);

            Member member = new Member
            {
                MemberID = model.MemberID,
                ImagePathFile = $"{Configuration.GetValue<string>("ImageMemberPath")!}/{newFileName}",
                ModifiedDate = DateTime.UtcNow,
            };

            IPortalDbMemberService portalDbMemberService = new PortalDbMemberService(PortalDbConnectionSettings);
            portalDbMemberService.UpdateMemberImage(member);

            //StateHasChanged();
            NavManager.NavigateTo($"{NavManager.BaseUri}UpdateMember/{MemberID}", true);
        }
    }

    private async Task<IEnumerable<string>> OnSearchPostcode(string value, CancellationToken token)
    {
        // In real life use an asynchronous function for fetching data from an api.
        await Task.Delay(5,token);

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value)) { return SearchPostcodes; }
            
        return SearchPostcodes.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }

    private async Task OnUpdateUser()
    {
        //if (!VerificationHelper.VefifyPhoneNumber(model.Phone))
        //{

        //    return;
        //}


        //Validate email on Shopify
        IShopifyService shopifyService = new ShopifyService(ShopifySettings);
        var userAccount = shopifyService.GetCustomerByEmail(model.Email);
        if (userAccount != null && userAccount.Object.customers.Count > 0)
        {
            RequestModelCustomer requestModelCustomer = new RequestModelCustomer
            {
                customer = new Customer
                {
                    email = model.Email,
                    first_name = model.FirstName,
                    last_name = model.LastName,
                    verified_email = true,
                    tags = model.Organization,
                    phone = model.Phone,

                }
            };
            
            //var response = shopifyService.UpdateCustomer(requestModelCustomer);
            //var isSuccess = response.IsSuccess;

            var isSuccess = true;
            if (isSuccess)
            {
                Member portalUser = new Member
                {
                    MemberID = MemberID,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Phone = model.Phone,
                    Address = model.Address,
                    Address2 = model.Address2,
                    City = model.City,
                    Region = model.Region,
                    PostCode = model.PostCode,
                    Tags = model.Organization,
                    ModifiedDate = DateTime.UtcNow
                };
                IPortalDbMemberService portalDbMemberService = new PortalDbMemberService(PortalDbConnectionSettings);
                portalDbMemberService.UpdateMemberBasic(portalUser);
                portalDbMemberService.UpdateMemberAddress(portalUser);
            }
            else
            {
                //Error message
            }
        }
        await js.InvokeVoidAsync("history.back");
    }

    private async Task OnClickAddSchoolDialog()
    {
        //var options = new DialogOptions { CloseOnEscapeKey = true };
        var parameters = new DialogParameters<SelectSchoolDialog>();
        parameters.Add("SelectedOrganizations", model.Organization);

        var dialog = DialogService.Show<SelectSchoolDialog>("Select organizations", parameters);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            //In a real world scenario we would reload the data from the source here since we "removed" it in the dialog already.
            model.Organization = result.Data.ToString();


        }
    }
    private async Task OnCancel()
    {
        await js.InvokeVoidAsync("history.back");
    }

    private async Task OnAddChildOrganization()
    {
        var parameters = new DialogParameters<UpdateChildOrganizationDialog>();
        parameters.Add("ParamChildOrganizationID", model.Organization);
        DialogOptions options = new DialogOptions() { BackdropClick = false };
        var dialog = DialogService.Show<UpdateChildOrganizationDialog>("Add child and organization", parameters, options);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            //In a real world scenario we would reload the data from the source here since we "removed" it in the dialog already.
            var childOrg = result.Data as ChildOrganization;
            childOrg.MemberID = model.MemberID;

            IPortalDbService portalDbService = new PortalDbService(PortalDbConnectionSettings);
            portalDbService.AddChildOrganization(childOrg);

            ChildOrganizations = portalDbService.GetChildOrganizationByMemberID(MemberID);
        }
    }

    private async void OnUpdateChildOrganization(ChildOrganization childOrganization)
    {
        var parameters = new DialogParameters<UpdateChildOrganizationDialog>();
        parameters.Add("ParamChildOrganizationID", childOrganization.ChildOrganizationID);
        DialogOptions options = new DialogOptions() { BackdropClick = false };
        var dialog = DialogService.Show<UpdateChildOrganizationDialog>("Update child and organization", parameters, options);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            //In a real world scenario we would reload the data from the source here since we "removed" it in the dialog already.
            var returnedChildOrg = result.Data as ChildOrganization;
            returnedChildOrg.MemberID = childOrganization.MemberID;

            IPortalDbService portalDbService = new PortalDbService(PortalDbConnectionSettings);
            portalDbService.UpdateChildOrganization(returnedChildOrg);

            ChildOrganizations = portalDbService.GetChildOrganizationByMemberID(MemberID);

            StateHasChanged();
        }
    }

    private async void OnDeleteChildOrganization(ChildOrganization childOrganization)
    {
        IPortalDbService portalDbService = new PortalDbService(PortalDbConnectionSettings);
        portalDbService.DeleteChildOrganization(childOrganization);

        ChildOrganizations = portalDbService.GetChildOrganizationByMemberID(MemberID);
        StateHasChanged();
    }
}