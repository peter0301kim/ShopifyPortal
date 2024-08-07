using global::Microsoft.AspNetCore.Components;
using MudBlazor;
using BrainzParentsPortal.Integration.PortalDb.Models;
using BrainzParentsPortal.Integration.PortalDb.Services;

namespace BrainzParentsPortal.Pages.Members;

public partial class SelectSchoolDialog
{
    [CascadingParameter]   MudDialogInstance MudDialog { get; set; }

    [Parameter]    public string SelectedOrganizations { get; set; } = "";

    private HashSet<Organization> selectedItems = new HashSet<Organization>();
    private IEnumerable<Organization> Organizations = new List<Organization>();
    private bool _selectOnRowClick = true;
    private MudTable<Organization> _table;
    private string _selectedItemText = "No row clicked";
    protected override async Task OnInitializedAsync()
    {
        IPortalDbService portalDbService = new PortalDbService(PortalDbConnectionSettings);
        Organizations = portalDbService.GetAllOrganizations();
    }

    private void OnRowClick(TableRowClickEventArgs<Organization> args)
    {
        _selectedItemText = $"{args.Item.OrganizationCode} ({args.Item.OrganizationID})";
    }

    private void OnCancel()
    {
        MudDialog.Cancel();
    }

    private void OnOk()
    {
        string rValue = selectedItems == null ? "" : string.Join(",", selectedItems.OrderBy(x => x.OrganizationCode).Select(x => x.OrganizationCode));
        MudDialog.Close(DialogResult.Ok(rValue));
    }
}