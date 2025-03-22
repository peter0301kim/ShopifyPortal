using ShopifyPortal.Integration.PortalDb.Models;
using ShopifyPortal.Integration.PortalDb.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ShopifyPortal.Pages.Members;

public partial class UpdateChildOrganizationDialog
{
    [CascadingParameter] IMudDialogInstance MudDialog { get; set; }
    [Parameter] public string ParamChildOrganizationID { get; set; } = string.Empty;
    private string ChildName { get; set; }
    private List<Organization> Organizations { get; set; } = null;
    private string PayerReference { get; set; }
    private string SelectedOrganizationCode { get; set; } = string.Empty;

    public UpdateChildOrganizationDialog()
    {

    }
    protected override async Task OnInitializedAsync()
    {
        IPortalDbService portalDbService = new PortalDbService(PortalDbConnectionSettings);
        Organizations = portalDbService.GetAllOrganizations();


        if(!string.IsNullOrEmpty(ParamChildOrganizationID))
        {
            ChildOrganization childOrg = portalDbService.GetChildOrganizationByChildOrganizationID(ParamChildOrganizationID);
            PayerReference = childOrg.PayerReference;
            ChildName = childOrg.ChildName;
            SelectedOrganizationCode = childOrg.Organization.OrganizationCode;
        }
    }
    private void OnCancel()
    {
        MudDialog.Cancel();
    }

    private void OnOk()
    {

        if (string.IsNullOrEmpty(SelectedOrganizationCode))
        {
            DialogService.ShowMessageBox("Alert", $"Please select an organization", yesText: "OK");
            return;
        }

        IPortalDbService portalDbService = new PortalDbService(PortalDbConnectionSettings);
        var organization = portalDbService.GetOrganizationByOrganizationCode(SelectedOrganizationCode);

        ChildOrganization childOrganization = null;
        if (string.IsNullOrEmpty(ParamChildOrganizationID))
        {//Add


            childOrganization = new ChildOrganization
            {
                ChildOrganizationID = System.Guid.NewGuid().ToString(),
                Organization = new Organization
                {
                    OrganizationID = organization.OrganizationID,
                    OrganizationName = organization.OrganizationName,
                },
                PayerReference = PayerReference,
                ChildName = ChildName,
                MemberID = string.Empty
            };

        }
        else
        {//Update
            childOrganization = new ChildOrganization
            {
                ChildOrganizationID = ParamChildOrganizationID,                
                Organization = new Organization
                {
                    OrganizationID = organization.OrganizationID,
                    OrganizationName = organization.OrganizationName,
                },
                PayerReference = PayerReference,
                ChildName = ChildName,
                MemberID = string.Empty
            };
        }

        MudDialog.Close(DialogResult.Ok(childOrganization));
    }
}