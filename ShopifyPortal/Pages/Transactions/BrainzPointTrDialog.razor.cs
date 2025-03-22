using ShopifyPortal.Integration.PortalDb.Models;
using ShopifyPortal.Integration.PortalDb.Services;
using ShopifyPortal.Shared.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ShopifyPortal.Pages.Transactions;

public partial class BrainzPointTrDialog
{
    [CascadingParameter] IMudDialogInstance MudDialog { get; set; }
    [Parameter] public string BrainzPointTrID { get; set; }
    public DtSpentBrainzPointTr DtSpentBrainzPointTr { get; set; }
    public Organization Organization { get; set; }
    bool IsProgress { get; set; } = false;

    public BrainzPointTrDialog()
    {
    }


    protected override async Task OnInitializedAsync()
    {
    }

    protected override async Task OnParametersSetAsync()
    {
        if (!string.IsNullOrEmpty(BrainzPointTrID))
        {
            IPortalDbMemberService portalDbMemberService = new PortalDbMemberService(PortalDbConnectionSettings);
            IPortalDbService portalDbService = new PortalDbService(PortalDbConnectionSettings);


            var brainzPointTr = portalDbService.GetBrainzPointTrByBrainzPointTrID(BrainzPointTrID);

            var member = portalDbMemberService.GetMemberByMemberID(brainzPointTr.MemberID);
            Organization = portalDbService.GetOrganizationByOrganizationCode(brainzPointTr.OrganizationCode);
            DtSpentBrainzPointTr = new DtSpentBrainzPointTr
            {
                BrainzPointTrID = brainzPointTr.BrainzPointTrID,
                BrainzPointTrDate = brainzPointTr.BrainzPointTrDate,
                MemberID = brainzPointTr.MemberID,
                Amount = brainzPointTr.Amount,
                OrganizationCode = brainzPointTr.OrganizationCode,
                PayeeReference = brainzPointTr.PayeeReference,
                PayerReference = brainzPointTr.PayerReference,
                TrComments = brainzPointTr.TrComments,
                BankTxID = brainzPointTr.BankTxID,
                Comments = brainzPointTr.Comments,
                MemberName = $"{member.FirstName} {member.LastName}",
                OrganizationName = Organization.OrganizationName
            };
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