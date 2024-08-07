using MudBlazor;
using BrainzParentsPortal.Integration.PortalDb.Services;
using BrainzParentsPortal.Integration.PortalDb;
using BrainzParentsPortal.Integration.PortalDb.Models;
using BrainzParentsPortal.Shared.Models;

namespace BrainzParentsPortal.Pages;

public partial class MyBrainzPage
{
    bool IsProgress { get; set; } = false;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string ImagePathFile { get; set; } = string.Empty;
    public DateTime MemberJoinDate { get; set; }
    public decimal BrainzPoint { get; set; }
    public decimal SpentPoint { get; set; }
    public decimal AvailablePoint { get; set; }
    public decimal PointsNeededToReachTheNextMmilestone { get; set; }


    private string searchString1 = "";
    private OrderTx selectedItem1 = null;
    private HashSet<OrderTx> selectedItems = new HashSet<OrderTx>();
    private IEnumerable<OrderTx> OrderTxs = new List<OrderTx>();

    private IEnumerable<BrainzPointTr> BrainzPointTrs = new List<BrainzPointTr>();

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

            IPortalDbService portalDbService = new PortalDbService(PortalDbConnectionSettings);
            decimal earnedBrainzPoint = portalDbService.GetEarnedBrainzPointsByCustomerID(member.CustomerID);
            decimal spentBrainzPoint = portalDbService.GetSpentBrainzPointsByMemberID(memberID);


            FirstName = member.FirstName;
            LastName = member.LastName;
            FullName = $"{member.FirstName} {member.LastName}";
            ImagePathFile = !string.IsNullOrEmpty(member.ImagePathFile) ? member.ImagePathFile : "Images/Member/DefaultMember.jpeg" ;
            MemberJoinDate = member.RegisteredDate;

            BrainzPoint = earnedBrainzPoint;
            SpentPoint = spentBrainzPoint;
            AvailablePoint = earnedBrainzPoint - SpentPoint;
            //PointsNeededToReachTheNextMmilestone =  ,

            OrderTxs = portalDbService.GetOrderTxsByCustomerID(member.CustomerID.ToString());

            BrainzPointTrs = portalDbService.GetBrainzPointTrsByMemberID(memberID);

            var pointLevels = portalDbService.GetAllPointLevels();

            var myPointLevel = GetMyPointLevel(pointLevels, AvailablePoint);

            //Show my pointLevel and Level trophy



        }

        await Task.Delay(1000);
        IsProgress = false;
    }

    private PointLevel GetMyPointLevel(List<PointLevel> pointLevels, decimal currentPoint)
    {
        pointLevels = pointLevels.OrderBy(x => x.PointLevelValue).ToList();

        PointLevel myPointLevel = new PointLevel();


        foreach(PointLevel pointLevel in pointLevels)
        {

            if (currentPoint < pointLevel.PointLevelValue)
            {
                myPointLevel = pointLevel;
                break;
            }
        }

        return myPointLevel;

    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
        }
    }


    private bool FilterFunc1(OrderTx element) => FilterFunc(element, searchString1);
    private bool FilterFunc(OrderTx element, string searchString)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if ($"{element.Comments}".Contains(searchString))
            return true;
        return false;
    }



}