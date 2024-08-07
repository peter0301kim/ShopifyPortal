using Microsoft.AspNetCore.Components.Forms;
using Paytec.Integration.Shopify.Models;
using System.ComponentModel.DataAnnotations;
using BrainzParentsPortal.Integration.PortalDb;
using BrainzParentsPortal.Integration.PortalDb.Services;
using BrainzParentsPortal.Integration.PortalDb.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using BrainzParentsPortal.Shared.Models;
using System;
using Microsoft.AspNetCore.Components.Authorization;
using BrainzParentsPortal.Helpers;

namespace BrainzParentsPortal.Pages.Organizations;

public partial class AddOrganizationPage
{
    public class RegisterOrganizationForm
    {
        public string OrganizationID { get; set; } = string.Empty;

        [Required]
        [StringLength(50, ErrorMessage = "OrganizationType length can't be more than 50.")]
        public string OrganizationType { get; set; } = string.Empty;

        [Required]
        [StringLength(20, ErrorMessage = "OrganizationCode length can't be more than 20.")]
        public string OrganizationCode { get; set; } = string.Empty;

        [Required]
        [StringLength(500, ErrorMessage = "OrganizationName length can't be more than 500.")]
        public string OrganizationName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public decimal ParentCommission { get; set; } = 0;

        [Required]
        [StringLength(20, ErrorMessage = "BSB length can't be more than 20.")]
        public string BSB { get; set; } = string.Empty;

        [Required]
        [StringLength(50, ErrorMessage = "AccountNumber length can't be more than 50.")]
        public string AccountNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(500, ErrorMessage = "Bank length can't be more than 500.")]
        public string Bank { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Payer Ref Help Text can't be more than 500.")]
        public string PayerRefHelpText { get; set; } = string.Empty;
        

        [StringLength(2000, ErrorMessage = "Memo length can't be more than 2000.")]
        public string Memo { get; set; } = string.Empty;

    }
    private MudForm form { get; set; }
    bool success { get; set; }
    string[] Errors { get; set; } = { };
    bool IsProgress { get; set; } = false;
    RegisterOrganizationForm model { get; set; } = new RegisterOrganizationForm();
    IReadOnlyList<IBrowserFile> OrganizationImageFiles { get; set; } = new List<IBrowserFile>();
    int MaxAllowedFiles = 3;
    long MaxFileSize = 1024 * 1024 * 1; //represents 3MB
    string LoadedImagePathFile { get; set; }


    protected override async Task OnInitializedAsync()
    {
    }

    protected override async Task OnParametersSetAsync()
    {
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

        OrganizationImageFiles = e.GetMultipleFiles(MaxAllowedFiles);

        if (OrganizationImageFiles.Count > 0)
        {
            var file = OrganizationImageFiles.FirstOrDefault();

            if (!ImageHelper.VerifyImageFileExtension(Path.GetExtension(file.Name)))
            {
                OrganizationImageFiles = null;
                await DialogService.ShowMessageBox(
                        "Warning", $"Please select an image file - (.jpg, .jpeg, .png)", yesText: "OK");
                return;
            }

        }
    }

    private async Task OnAddOrganization()
    {
        IPortalDbService portalDbService = new PortalDbService(PortalDbConnectionSettings);

        if (portalDbService.IsOrganizationExist(model.OrganizationCode))
        {
            return;
        }


        Organization organization = new Organization
        {
            OrganizationID = Guid.NewGuid().ToString(),
            OrganizationType = model.OrganizationType,
            OrganizationCode = model.OrganizationCode,
            OrganizationName = model.OrganizationName,
            Phone = model.Phone,
            Email = model.Email,
            ContactPerson = model.ContactPerson,
            ParentCommission = model.ParentCommission,
            BSB = model.BSB,
            AccountNumber = model.AccountNumber,
            Bank = model.Bank,
            PayerRefHelpText = model.PayerRefHelpText,
            Memo = model.Memo,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
        };

        if (OrganizationImageFiles != null && OrganizationImageFiles.Count > 0)
        {
            var file = OrganizationImageFiles.FirstOrDefault();
            var firstToken = (model.OrganizationName.Contains(" ")) ? model.OrganizationName.Substring(0, model.OrganizationName.IndexOf(" ")) : model.OrganizationName;

            string newFileName = $"{Path.GetFileNameWithoutExtension(file.Name)}-{firstToken}{Path.GetExtension(file.Name)}";

            string path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Configuration.GetValue<string>("ImageOrganizationPath")!);
            //string path = Path.Combine(Configuration.GetValue<string>("ImageFileStorage")!, "Images", "Organization");

            if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }

            var imagePathFile = Path.Combine(path, newFileName);

            await using FileStream fs = new(imagePathFile, FileMode.Create);
            await file.OpenReadStream(MaxFileSize).CopyToAsync(fs);

            organization.ImagePathFile = $"{Configuration.GetValue<string>("ImageOrganizationPath")!}/{newFileName}";
        }

        var isSuccess = portalDbService.AddOrganization(organization);

    }
}