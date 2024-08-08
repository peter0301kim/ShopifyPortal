using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using ShopifyPortal.Integration.PortalDb.Models;
using ShopifyPortal.Integration.PortalDb.Services;
using ShopifyPortal.Integration.PortalDb;
using ShopifyPortal.Shared.Helpers;
using ShopifyPortal.Shared.Models;
using ShopifyPortal.Pages.Members;
using System.Reflection;

namespace ShopifyPortal.Pages.Transactions;

public partial class OrderTxsPage
{

    [Inject] private IDialogService DialogService { get; set; }
    [CascadingParameter] private Task<AuthenticationState> AuthenticationState { get; set; }

    private bool dense = false;
    private bool hover = true;
    private bool striped = false;
    private bool bordered = false;
    private string searchString1 = "";
    private DtOrderTx selectedItem1 = null;
    private HashSet<DtOrderTx> selectedItems = new HashSet<DtOrderTx>();
    private IEnumerable<DtOrderTx> DtOrderTxs = new List<DtOrderTx>();

    private bool IsProgress { get; set; } = false;
    private string MemberID { get; set; } = string.Empty;
    private string MemberName { get; set; } = string.Empty;
    public OrderTxsPage()
    {
    }


    protected override async Task OnInitializedAsync()
    {

    }

    protected override async Task OnParametersSetAsync()
    {
        IsProgress = true;
        await Task.Delay(1000);

        var authState = await AuthenticationState;
        if (authState.User.Identity != null && !authState.User.Identity.IsAuthenticated)
        {
            NavManager.NavigateTo($"{NavManager.BaseUri}login", true);

            return;
        }

        var state = await CustomAuthenticationStateProvider.GetAuthenticationStateAsync();
        foreach (var claim in state.User.Claims)
        {
            if (claim.Type.ToString().Contains("/claims/sid"))
            {
                MemberID = claim.Value;
            }
        }

        IPortalDbMemberService portalDbMemberService = new PortalDbMemberService(PortalDbConnectionSettings);
        var member = portalDbMemberService.GetMemberByMemberID(MemberID);
        MemberName = $"{member.FirstName} {member.LastName}";

        IPortalDbService portalDbService = new PortalDbService(PortalDbConnectionSettings);
        var orderTxs = portalDbService.GetOrderTxsByCustomerID(member.CustomerID);

        var vendors = portalDbService.GetAllVendors();
        var organizations = portalDbService.GetAllOrganizations();

        var dtOrderTxs = from order in orderTxs
                         join vendor in vendors on order.VendorCode equals vendor.VendorCode
                         join org in organizations on order.OrganizationCode equals org.OrganizationCode
                         select new DtOrderTx
                         {
                             OrderTxID = order.OrderTxID,
                             OrderTxDate = order.OrderTxDate,
                             VendorCode = order.VendorCode,
                             VendorOrderID = order.VendorOrderID,
                             OrderName = order.OrderName,
                             Total = order.Total,
                             BrainzCustomerID = order.BrainzCustomerID,
                             OrganizationCode = order.OrganizationCode,
                             Comments = order.Comments,
                             BrainzPoint = order.BrainzPoint,
                             BrainzPointCalc = order.BrainzPointCalc,
                             ShopifyOrderID = order.ShopifyOrderID,
                             MemberName = MemberName,
                             VendorName = vendor.VendorName,
                             OrganizationName = org.OrganizationName
                         };

        DtOrderTxs = dtOrderTxs.ToList();

        IsProgress = false;
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {

        }
    }

    private bool FilterFunc1(DtOrderTx element) => FilterFunc(element, searchString1);
    private bool FilterFunc(DtOrderTx element, string searchString)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.VendorName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.OrganizationName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        return false;
    }

    private async void OnShowDetail(DtOrderTx dtOrderTx)
    {
        var parameters = new DialogParameters<OrderTxDialog>();
        parameters.Add("OrderTxID", dtOrderTx.OrderTxID);
        DialogOptions options = new DialogOptions() { BackdropClick = false };
        var dialog = DialogService.Show<OrderTxDialog>("Order transaction detail", parameters, options);
        var result = await dialog.Result;

    }
}