using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using ShopifyPortal.Integration.PortalDb.Models;
using ShopifyPortal.Integration.PortalDb.Services;
using MudBlazor;
using ShopifyPortal.Helpers;

namespace ShopifyPortal.Pages.Vendors;

public partial class UpdateVendorPage
{
    [Parameter]    public string VendorID { get; set; }

    public class UpdateVendorForm
    {
        [Required]
        [StringLength(50, ErrorMessage = "Name length can't be more than 50.")]
        public string VendorID { get; set; } = string.Empty;

        [Required]
        [StringLength(20, ErrorMessage = "Code length can't be more than 20.")]
        public string VendorCode { get; set; } = string.Empty;
        [Required]
        [StringLength(1000, ErrorMessage = "Name length can't be more than 1000.")]
        public string VendorName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public decimal VendorCommission { get; set; }
        public decimal ParentCommission { get; set; }
        public string ImagePathFile { get; set; } = string.Empty;
        [Required]
        [StringLength(2000, ErrorMessage = "Name length can't be more than 2000.")]
        public string Memo { get; set; } = string.Empty;

    }

    private MudForm form { get; set; }
    bool success { get; set; }
    string[] Errors { get; set; } = { };
    bool IsProgress { get; set; } = false;

    UpdateVendorForm model = new UpdateVendorForm();

    IReadOnlyList<IBrowserFile> VendorImageFiles { get; set; } = new List<IBrowserFile>();
    int MaxAllowedFiles = 3;
    long MaxFileSize = 1024 * 1024 * 1; //represents 3MB

    protected override async Task OnInitializedAsync()
    {
    }

    protected override async Task OnParametersSetAsync()
    {
        if (!string.IsNullOrEmpty(VendorID))
        {
            IPortalDbService portalDbService = new PortalDbService(PortalDbConnectionSettings);

            var vendor = portalDbService.GetVendorByVendorID(VendorID);
            model = new UpdateVendorForm()
            {
                VendorID = vendor.VendorID,
                VendorCode = vendor.VendorCode,
                VendorName = vendor.VendorName,
                Phone = vendor.Phone,
                Email = vendor.Email,
                ContactPerson = vendor.ContactPerson,
                VendorCommission = vendor.VendorCommission,
                ParentCommission = vendor.ParentCommission,
                Memo = vendor.Memo
            };

        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {

    }

    private async void LoadFiles(InputFileChangeEventArgs e)
    {
        //TODO upload the files to the server

        if (e.FileCount > MaxAllowedFiles)
        {
            Errors.Append($"Error: Attempting to upload {e.FileCount} files, but max allowed file count is {MaxAllowedFiles}");
            return;
        }

        VendorImageFiles = e.GetMultipleFiles(MaxAllowedFiles);

        if (VendorImageFiles.Count > 0)
        {
            var file = VendorImageFiles.FirstOrDefault();
            var firstToken = (model.VendorName.Contains(" ")) ? model.VendorName.Substring(0, model.VendorName.IndexOf(" ")) : model.VendorName;

            if (!ImageHelper.VerifyImageFileExtension(Path.GetExtension(file.Name)))
            {
                VendorImageFiles = null;
                await DialogService.ShowMessageBox(
                        "Warning", $"Please select an image file - (.jpg, .jpeg, .png)", yesText: "OK");
                return;
            }

            string newFileName = $"{Path.GetFileNameWithoutExtension(file.Name)}-{firstToken}{Path.GetExtension(file.Name)}";

            string path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Configuration.GetValue<string>("ImageVendorPath")!);
            //string path = Path.Combine(Configuration.GetValue<string>("ImageFileStorage")!, "Images", "School");

            if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }

            var imagePathFile = Path.Combine(path, newFileName);

            await using FileStream fs = new(imagePathFile, FileMode.Create);
            await file.OpenReadStream(MaxFileSize).CopyToAsync(fs);

            Vendor vendor = new Vendor
            {
                VendorID = model.VendorID,
                ImagePathFile = $"{Configuration.GetValue<string>("ImageVendorPath")!}/{newFileName}",
                ModifiedDate = DateTime.UtcNow,
            };

            IPortalDbService portalDbService = new PortalDbService(PortalDbConnectionSettings);
            portalDbService.UpdateVendorImage(vendor);

            //StateHasChanged();
            NavManager.NavigateTo($"{NavManager.BaseUri}/UpdateVendor/{VendorID}", true);
        }
    }

    private async Task OnCancel()
    {
        NavManager.NavigateTo($"{NavManager.BaseUri}ReadVendors", true);
    }
    private async Task OnUpdateVendor()
    {
        Vendor vendor = new Vendor
        {
            VendorID = model.VendorID,
            VendorCode = model.VendorCode,
            VendorName = model.VendorName,
            Phone = model.Phone,
            Email = model.Email,
            ContactPerson = model.ContactPerson,
            VendorCommission = model.VendorCommission,
            ParentCommission = model.ParentCommission,
            Memo = model.Memo,
            ModifiedDate = DateTime.UtcNow,
        };

        IPortalDbService portalDbService = new PortalDbService(PortalDbConnectionSettings);
        portalDbService.UpdateVendor(vendor);

        NavManager.NavigateTo($"{NavManager.BaseUri}ReadVendors", true);
    }
}