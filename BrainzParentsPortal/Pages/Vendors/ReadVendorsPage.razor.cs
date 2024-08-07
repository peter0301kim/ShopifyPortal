using Microsoft.AspNetCore.Components;
using MudBlazor;
using BrainzParentsPortal.Helpers;
using BrainzParentsPortal.Integration.PortalDb.Models;
using BrainzParentsPortal.Integration.PortalDb.Services;
using Microsoft.AspNetCore.Components.Authorization;

namespace BrainzParentsPortal.Pages.Vendors;

public partial class ReadVendorsPage
{
    [Inject] private IDialogService DialogService { get; set; }

    private bool dense = false;
    private bool hover = true;
    private bool striped = false;
    private bool bordered = false;
    private string searchString1 = "";
    private Vendor selectedItem1 = null;
    private HashSet<Vendor> selectedItems = new HashSet<Vendor>();
    private IEnumerable<Vendor> Vendors = new List<Vendor>();

    bool IsProgress { get; set; } = false;
    public int ProgressValue { get; set; } = 0;
    public void StopProgressBar() 
    {
        IsProgress = false;
        ProgressValue = 100;
        StateHasChanged();
    }

    public async void StartProgressBar()
    {
        ProgressValue = 0;
        do
        {
            if (!IsProgress) { return; }

            ProgressValue += 4;
            StateHasChanged();
            await Task.Delay(100);

        } while (ProgressValue < 100);

        StartProgressBar();
    }

    protected override async Task OnInitializedAsync()
    {
        //StartProgressBar();
        IsProgress = true;

        IPortalDbService portalDbService = new PortalDbService(PortalDbConnectionSettings);
        Vendors = portalDbService.GetAllVendors();
        
        IsProgress = false;
        //StopProgressBar();
    }






    private bool FilterFunc1(Vendor element) => FilterFunc(element, searchString1);
    private bool FilterFunc(Vendor element, string searchString)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.VendorCode.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.VendorName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        return false;
    }

    private async void OnAddVendor()
    {
        NavManager.NavigateTo($"{NavManager.BaseUri}AddVendor", true);
    }

    private async void OnUpdateVendor(Vendor vendor)
    {
        NavManager.NavigateTo($"{NavManager.BaseUri}UpdateVendor/{vendor.VendorID}", true);
    }

    private async Task OnDeleteVendor(Vendor vendor)
    {
        bool? result = await DialogService.ShowMessageBox(
            "Warning", $"The vendor ({vendor.VendorName}) is going to be deleted. It can not be undone!", yesText: "Delete", cancelText: "Cancel");

        if (result != null && result == true)
        {
            IPortalDbService portalDbService = new PortalDbService(PortalDbConnectionSettings);
            var bTrue = portalDbService.DeleteVendor(vendor.VendorID);
            if (bTrue)
            {
                Vendors = portalDbService.GetAllVendors();
            }

            //StateHasChanged();            
        }

    }
}