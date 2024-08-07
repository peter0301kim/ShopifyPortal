using System.ComponentModel.DataAnnotations;
using BrainzParentsPortal.Integration.PortalDb.Services;
using BrainzParentsPortal.Integration.PortalDb.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using static BrainzParentsPortal.Pages.Organizations.UpdateOrganizationPage;
using System.Reflection;
using BrainzParentsPortal.Shared.Models;

namespace BrainzParentsPortal.Pages.Transactions;

public partial class OrderTxDialog
{
    [CascadingParameter]    MudDialogInstance MudDialog { get; set; }
    [Parameter] public string OrderTxID { get; set; }
    public DtOrderTx DtOrderTx { get; set; }
    bool IsProgress { get; set; } = false;
    public OrderTxDialog()
    {
    }

    protected override async Task OnInitializedAsync()
    {
    }

    protected override async Task OnParametersSetAsync()
    {
        if (!string.IsNullOrEmpty(OrderTxID))
        {
            IPortalDbMemberService portalDbMemberService = new PortalDbMemberService(PortalDbConnectionSettings);
            IPortalDbService portalDbService = new PortalDbService(PortalDbConnectionSettings);

            var orderTx = portalDbService.GetOrderTxsByOrderTxID(OrderTxID);

            if (orderTx != null)
            {
                var member = portalDbMemberService.GetMemberByCustomerID(orderTx.BrainzCustomerID);
                var org = portalDbService.GetOrganizationByOrganizationCode(orderTx.OrganizationCode);
                var vendor = portalDbService.GetVendorByVendorCode(orderTx.VendorCode);

                DtOrderTx = new DtOrderTx()
                {
                    OrderTxID = orderTx.OrderTxID,
                    OrderTxDate = orderTx.OrderTxDate,
                    VendorCode = orderTx.VendorCode,
                    VendorOrderID = orderTx.VendorOrderID,
                    OrderName = orderTx.OrderName,
                    Total = orderTx.Total,
                    BrainzCustomerID = orderTx.BrainzCustomerID,
                    OrganizationCode = orderTx.OrganizationCode,
                    Comments = orderTx.Comments,
                    BrainzPoint = orderTx.BrainzPoint,
                    BrainzPointCalc = orderTx.BrainzPointCalc,
                    ShopifyOrderID = orderTx.ShopifyOrderID,
                    MemberName = $"{member.FirstName} {member.LastName}",
                    VendorName = vendor.VendorName,
                    OrganizationName = org.OrganizationName,
                };
            }
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {

    }


    private void OnClose()
    {
        MudDialog.Close(DialogResult.Ok(""));
    }
}