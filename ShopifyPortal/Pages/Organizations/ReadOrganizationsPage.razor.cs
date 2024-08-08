using Microsoft.AspNetCore.Components;
using MudBlazor;
using ShopifyPortal.Integration.PortalDb.Models;
using ShopifyPortal.Integration.PortalDb.Services;

namespace ShopifyPortal.Pages.Organizations;

public partial class ReadOrganizationsPage
{
    [Inject] private IDialogService DialogService { get; set; }

    private bool dense = false;
    private bool hover = true;
    private bool striped = false;
    private bool bordered = false;

    private string searchOrganizationName = "";
    private string searchString1 = "";
    private Organization selectedItem1 = null;
    private HashSet<Organization> selectedItems = new HashSet<Organization>();
    private IEnumerable<Organization> Organizations = new List<Organization>();
    bool IsProgress { get; set; } = false;
    protected override async Task OnInitializedAsync()
    {
        IPortalDbService portalDbService = new PortalDbService(PortalDbConnectionSettings);
        Organizations = portalDbService.GetAllOrganizations();
    }

    protected override async Task OnParametersSetAsync()
    {

    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {

        }
    }

    private bool FilterFunc1(Organization element) => FilterFunc(element, searchString1);
    private bool FilterFunc(Organization element, string searchString)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.OrganizationCode.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.OrganizationName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        return false;
    }

    private async void OnAddOrganization()
    {
        NavManager.NavigateTo($"{NavManager.BaseUri}AddOrganization", true);

    }

    private async void OnSearchOrganization()
    {
        IPortalDbService portalDbService = new PortalDbService(PortalDbConnectionSettings);
        Organizations = portalDbService.GetOrganizationsByOrganizationName(searchOrganizationName);
    }

    private async void OnUpdateOrganization(Organization organization)
    {
        NavManager.NavigateTo($"{NavManager.BaseUri}UpdateOrganization/{organization.OrganizationID}", true);

    }

    private async Task OnDeleteOrganization(Organization organization)
    {
        bool? result = await DialogService.ShowMessageBox(
            "Warning", $"The organization ({organization.OrganizationName}) is going to be deleted. It can not be undone!", yesText: "Delete", cancelText: "Cancel");

        if (result != null && result == true)
        {
            IPortalDbService portalDbService = new PortalDbService(PortalDbConnectionSettings);
            var bTrue = portalDbService.DeleteOrganization(organization.OrganizationID);

            if (bTrue)
            {
                Organizations = portalDbService.GetAllOrganizations();
            }
        }
        //StateHasChanged();
    }
}