using BrainzParentsPortal.Helpers;
using BrainzParentsPortal.Integration.PortalDb;
using BrainzParentsPortal.Integration.PortalDb.Models;
using BrainzParentsPortal.Integration.PortalDb.Services;
using System.Net;

namespace BrainzParentsPortal.Pages.Analytics;

public partial class SearchOrderTxPage
{
    bool IsProgress { get; set; } = false;

    private string searchString1 = "";
    private OrderTx selectedItem1 = null;
    private HashSet<OrderTx> selectedItems = new HashSet<OrderTx>();
    private IEnumerable<OrderTx> OrderTxs = new List<OrderTx>();

    public SearchOrderTxPage()
    {
    }

    protected override async Task OnInitializedAsync()
    {
    }

    protected override async Task OnParametersSetAsync()
    {
        IsProgress = true;

        var state = await CustomAuthenticationStateProvider.GetAuthenticationStateAsync();
        string memberID = "";
        foreach (var claim in state.User.Claims)
        {

            if (claim.Type.ToString().Contains("/claims/sid"))
            {
                memberID = claim.Value;
            }
        }

        if (!string.IsNullOrEmpty(memberID))
        {
            IPortalDbMemberService portalDbMemberService = new PortalDbMemberService(PortalDbConnectionSettings);
            var member = portalDbMemberService.GetMemberByMemberID(memberID);

            if (member != null)
            {
                IPortalDbService portalDbService = new PortalDbService(PortalDbConnectionSettings);
                OrderTxs = portalDbService.GetOrderTxsByCustomerID(member.CustomerID);
            }
        }

        await Task.Delay(1000);
        IsProgress = false;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
        }
    }
}