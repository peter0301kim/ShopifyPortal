
using BrainzParentsPortal.Integration.PortalDb.Services;
using BrainzParentsPortal.Shared.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using BrainzParentsPortal.Pages.Members;
using System.Reflection;
using BrainzParentsPortal.Integration.PortalDb.Models;

namespace BrainzParentsPortal.Pages.Transactions;

public partial class BrainzPointTrsPage
{
    [Inject] private IDialogService DialogService { get; set; }
    [CascadingParameter] private Task<AuthenticationState> AuthenticationState { get; set; }

    private bool dense = false;
    private bool hover = true;
    private bool striped = false;
    private bool bordered = false;
    private string searchString1 = "";
    private DtSpentBrainzPointTr selectedItem1 = null;
    private HashSet<DtSpentBrainzPointTr> selectedItems = new HashSet<DtSpentBrainzPointTr>();
    private IEnumerable<DtSpentBrainzPointTr> DtSpentBrainzPointTrs = new List<DtSpentBrainzPointTr>();

    private bool IsProgress { get; set; } = false;
    private string MemberID { get; set; } = string.Empty;
    private string MemberName { get; set; } = string.Empty;

    public BrainzPointTrsPage()
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
        var spentPoints = portalDbService.GetBrainzPointTrsByMemberID(MemberID);
        var organizations = portalDbService.GetAllOrganizations();

        var dtSpentPoints = from spentPoint in spentPoints
                         join org in organizations on spentPoint.OrganizationCode equals org.OrganizationCode
                         select new DtSpentBrainzPointTr
                         {
                             BrainzPointTrID = spentPoint.BrainzPointTrID,
                             BrainzPointTrDate = spentPoint.BrainzPointTrDate,
                             MemberID = spentPoint.MemberID,
                             Amount = spentPoint.Amount,
                             OrganizationCode = spentPoint.OrganizationCode,
                             PayeeReference = spentPoint.PayeeReference,
                             PayerReference = spentPoint.PayerReference,
                             TrComments = spentPoint.TrComments,
                             BankTxID = spentPoint.BankTxID,
                             Comments = spentPoint.Comments,
                             OrganizationName = org.OrganizationName,
                         };

        DtSpentBrainzPointTrs = dtSpentPoints.ToList();

        IsProgress = false;
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {

        }
    }

    private bool FilterFunc1(DtSpentBrainzPointTr element) => FilterFunc(element, searchString1);
    private bool FilterFunc(DtSpentBrainzPointTr element, string searchString)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.OrganizationName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.PayeeReference.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.PayerReference.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.Comments.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        return false;
    }

    private async void OnShowDetail(DtSpentBrainzPointTr dtSpentBrainzPointTr)
    {
        var parameters = new DialogParameters<BrainzPointTrDialog>();
        parameters.Add("BrainzPointTrID", dtSpentBrainzPointTr.BrainzPointTrID);
        DialogOptions options = new DialogOptions() { BackdropClick = false };
        var dialog = DialogService.Show<BrainzPointTrDialog>("BrainzPoint transfer detail", parameters, options);
        var result = await dialog.Result;
    }
}