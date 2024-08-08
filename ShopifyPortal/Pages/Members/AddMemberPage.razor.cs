using System.ComponentModel.DataAnnotations;
using Peter.Integration.Shopify.Models;
using Peter.Integration.Shopify.Services;
using ShopifyPortal.Integration.PortalDb.Models;
using ShopifyPortal.Integration.PortalDb.Services;
using ShopifyPortal.Shared.Helpers;
using MudBlazor;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Configuration;
using System.Numerics;
using ShopifyPortal.Helpers;
using static MudBlazor.CategoryTypes;
using Microsoft.JSInterop;
using ShopifyPortal.Shared.SettingModels;

namespace ShopifyPortal.Pages.Members;

public partial class AddMemberPage
{
    public class AddMemberForm
    {
        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Name length can't be more than 100.")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Name length can't be more than 100.")]
        public string LastName { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Password must be at least 8 characters long.", MinimumLength = 8)]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        public string Password2 { get; set; }

        //[Required]
        //[StringLength(100, ErrorMessage = "")]
        //public string Organization { get; set; }

        [StringLength(100, ErrorMessage = "")]
        [RegularExpression("^\\+?[1-9]\\d{1,14}$", ErrorMessage = "Input valid phone number")]
        public string Phone { get; set; }

        [StringLength(100, ErrorMessage = "")]
        public string Address { get; set; }

        [StringLength(100, ErrorMessage = "")]
        public string Address2 { get; set; }
        
        [StringLength(100, ErrorMessage = "")]
        public string City { get; set; }
        
        [StringLength(100, ErrorMessage = "")]
        public string Region { get; set; }
        
        [StringLength(100, ErrorMessage = "")]
        public string PostCode { get; set; }
    }
    private AddMemberForm AddMemberFormModel = new AddMemberForm();
    private MudForm form { get; set; }
    private bool IsFormSuccess { get; set; }
    private string[] Errors { get; set; } = { };
    private bool IsProgress { get; set; } = false;
    

    IReadOnlyList<IBrowserFile> MemberImageFiles { get; set; } = new List<IBrowserFile>();
    int MaxAllowedFiles = 3;
    long MaxFileSize = 1024 * 1024 * 1; //represents 3MB

    private IList<ChildOrganization> ChildOrganizations = new List<ChildOrganization>();
    
    private bool IsShowPassword { get; set; }
    private bool IsShowPassword2 { get; set; }
    private string PasswordInputIcon { get; set; } = Icons.Material.Filled.VisibilityOff;
    private string PasswordInputIcon2 { get; set; } = Icons.Material.Filled.VisibilityOff;
    private InputType PasswordInput { get; set; } = InputType.Password;
    private InputType PasswordInput2 { get; set; } = InputType.Password;
    protected override async Task OnInitializedAsync()
    {
        
    }

    private async void LoadFiles(InputFileChangeEventArgs e)
    {
        if (e.FileCount > MaxAllowedFiles)
        {
            Errors.Append($"Error: Attempting to upload {e.FileCount} files, but max allowed file count is {MaxAllowedFiles}");
            return;
        }

        MemberImageFiles = e.GetMultipleFiles(MaxAllowedFiles);

        if(MemberImageFiles.Count > 0)
        {
            var file = MemberImageFiles.FirstOrDefault();

            if (!ImageHelper.VerifyImageFileExtension(Path.GetExtension(file.Name)))
            {
                MemberImageFiles = null;
                await DialogService.ShowMessageBox(
                        "Warning", $"Please select an image file - (.jpg, .jpeg, .png)", yesText: "OK");
                return;
            }
        }
    }

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

    void OnShowPassword2()
    {
        if(IsShowPassword2)
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

    private async Task OnAddMember()
    {
        if (!IsFormSuccess)
        {
            var strErrors = Errors.ToString();
            await DialogService.ShowMessageBox("ERROR", string.Join(",",Errors), yesText: "OK");
            return;
        }

        //if (!VerificationHelper.VefifyPhoneNumber(model.Phone))
        //{

        //    return;
        //}

        //Validate email on Shopify

        IsProgress = true;

        IShopifyService shopifyService = new ShopifyService(ShopifySettings);
        var userAccount = shopifyService.GetCustomerByEmail(AddMemberFormModel.Email);
        if (userAccount != null && userAccount.Object.customers.Count > 0)
        {
            //email duplication
            await DialogService.ShowMessageBox("ALERT", $"There is an email on this system already. Please input other email.", yesText: "OK");
            return;
        }
        else {

            string strOrganization = "";
            if (ChildOrganizations.Count > 0)
            {
                foreach (var childOrg in ChildOrganizations)
                {
                    strOrganization += $"[{childOrg.ChildName}:{childOrg.Organization.OrganizationName}],";
                }
            }

            RequestModelCustomer requestModelCustomer = new RequestModelCustomer
            {
                customer = new Customer
                {
                    email = AddMemberFormModel.Email,
                    first_name = AddMemberFormModel.FirstName,
                    last_name = AddMemberFormModel.LastName,
                    verified_email = true,
                    tags = strOrganization,
                    //phone = model.Phone,
                }
            };

            var response = shopifyService.AddCustomer(requestModelCustomer);
            if (response.IsSuccess)
            {
                var customer = response.Object.customer;

                Member member = new Member
                {
                    MemberID = System.Guid.NewGuid().ToString(),
                    Email = AddMemberFormModel.Email,
                    FirstName = AddMemberFormModel.FirstName,
                    LastName = AddMemberFormModel.LastName,
                    Password = EncryptionHelper.SHA512(AddMemberFormModel.Password),
                    Phone = AddMemberFormModel.Phone ?? "",
                    Address = AddMemberFormModel.Address ?? "",
                    Address2 = AddMemberFormModel.Address2 ?? "",
                    City = AddMemberFormModel.City ?? "",
                    Region = AddMemberFormModel.Region ?? "",
                    PostCode = AddMemberFormModel.PostCode ?? "",
                    Role = "User",
                    Tags = strOrganization ?? "",
                    CustomerID = customer.id.ToString(),
                    ModifiedDate = DateTime.UtcNow,
                    RegisteredDate = DateTime.UtcNow,
                };

                if (MemberImageFiles.Count > 0)
                {
                    var file = MemberImageFiles.FirstOrDefault();
                    var firstToken = (AddMemberFormModel.FirstName.Contains(" ")) ? AddMemberFormModel.FirstName.Substring(0, AddMemberFormModel.FirstName.IndexOf(" ")) : AddMemberFormModel.FirstName;
                    if(firstToken.Contains("'")) { firstToken.Replace("'", ""); }

                    string newFileName = $"{Path.GetFileNameWithoutExtension(file.Name)}-{firstToken}{Path.GetExtension(file.Name)}";

                    string path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Configuration.GetValue<string>("ImageMemberPath")!);
                    //string path = Path.Combine(Configuration.GetValue<string>("ImageFileStorage")!, "Images", "School");

                    if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }

                    var imagePathFile = Path.Combine(path, newFileName);

                    await using FileStream fs = new(imagePathFile, FileMode.Create);
                    await file.OpenReadStream(MaxFileSize).CopyToAsync(fs);
                    member.ImagePathFile = $"{Configuration.GetValue<string>("ImageMemberPath")!}/{newFileName}";
                }

                IPortalDbMemberService portalDbMemberService = new PortalDbMemberService(PortalDbConnectionSettings);
                IPortalDbService portalDbService = new PortalDbService(PortalDbConnectionSettings);
                if (portalDbMemberService.AddMember(member))
                {
                    foreach(var childOrg in ChildOrganizations)
                    {
                        childOrg.MemberID = member.MemberID;
                        portalDbService.AddChildOrganization(childOrg);
                        await Task.Delay(500);
                    }

                    //goto prev page
                    await js.InvokeVoidAsync("history.back");
                }
                else
                {
                    await DialogService.ShowMessageBox("ERROR", $"Fail to save a member to Portal db.", yesText: "OK");
                }
            }
            else
            {
                await DialogService.ShowMessageBox("ERROR", $"Fail to save a member to Shopify.", yesText: "OK");
            }
        }

        
    }

    private async Task OnAddChildOrganization()
    {

        var parameters = new DialogParameters<UpdateChildOrganizationDialog>();
        DialogOptions options = new DialogOptions() { BackdropClick = false };
        var dialog = DialogService.Show<UpdateChildOrganizationDialog>("Add child and organization", parameters, options);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            ChildOrganization org = result.Data as ChildOrganization;
            
            ChildOrganizations.Add(org);

        }
    }

    private async Task OnDeleteChildOrganization(ChildOrganization childOrganization)
    {
        ChildOrganizations.Remove(childOrganization);
    }
}