using ShopifyPortal.Helpers;
using ShopifyPortal.Integration.PortalDb.Models;
using ShopifyPortal.Integration.PortalDb.Services;
using ShopifyPortal.Shared.Helpers;
using Microsoft.Extensions.Configuration;
using Peter.Integration.Shopify.Models;
using Peter.Integration.Shopify.Services;
using Peter.Integration.Shopify;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using NLog;
using System.Security.Cryptography.X509Certificates;

namespace ShopifyPortal.Pages.SpendPoints;

public partial class SpendPointsPage
{
    private static Logger Log = LogManager.GetCurrentClassLogger();
    public string MemberID { get; set; } = string.Empty;
    bool IsBusy { get; set; } = false;
    bool IsProgress { get; set; } = false;
    public decimal AvailableBrainzPoint { get; set; }
    private int _brainzPointsToSpend { get; set; }
    public decimal PointToCurrency { get; set; }
    public int BrainzPointsToSpend {
        get
        {
            return _brainzPointsToSpend;
        }
        set
        {
            _brainzPointsToSpend = value;
            PointToCurrency = (decimal)(value / BrainzParentsPortalSettings.TransferFunds.OneDollarToBrainzPoint);
        }
    }

    public List<Organization> Organizations { get; set; } = null;
    public List<ChildOrganization> ChildOrganizations { get; set; } = null;
    public string SelectedChildOrgID { get; set; } 
    public string PayeeReference { get; set; } = "Brainz points transfer";
    public string PayerReference { get; set; } = string.Empty;
    public string PayerReferenceHelpText { get; set; } = string.Empty;
    private Member LoggedInMember { get; set; }
    private bool IsPopUpPayerRefHelp { get; set; } = false;
    protected override async Task OnInitializedAsync()
    {

    }

    protected override async Task OnParametersSetAsync()
    {
        var state = await CustomAuthenticationStateProvider.GetAuthenticationStateAsync();

        foreach (var claim in state.User.Claims)
        {

            if (claim.Type.ToString().Contains("/claims/sid"))
            {
                MemberID = claim.Value;
            }
        }


        if (!string.IsNullOrEmpty(MemberID))
        {
            IPortalDbMemberService portalDbMemberService = new PortalDbMemberService(PortalDbConnectionSettings);
            LoggedInMember = portalDbMemberService.GetMemberByMemberID(MemberID);

            IPortalDbService portalDbService = new PortalDbService(PortalDbConnectionSettings);
            decimal earnedbrainzPoint = portalDbService.GetEarnedBrainzPointsByCustomerID(LoggedInMember.CustomerID);
            decimal spentBrainzPoint = portalDbService.GetSpentBrainzPointsByMemberID(MemberID);

            AvailableBrainzPoint = earnedbrainzPoint - spentBrainzPoint;


            IPortalDbService portalDbUserService = new PortalDbService(PortalDbConnectionSettings);
            Organizations = portalDbUserService.GetAllOrganizations();
            ChildOrganizations = portalDbUserService.GetChildOrganizationByMemberID(MemberID);
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
        }
    }

    private async Task OnPayerReferenceHelp()
    {
        if (IsPopUpPayerRefHelp) { IsPopUpPayerRefHelp = false; }
        else 
        {

            if (string.IsNullOrEmpty(SelectedChildOrgID)) { return; }

            var selectedChildOrg = (from x in ChildOrganizations where x.ChildOrganizationID == SelectedChildOrgID select x).FirstOrDefault();

            var organization = (from org in Organizations where org.OrganizationCode == selectedChildOrg.Organization.OrganizationCode select org).ToList().FirstOrDefault();
            PayerReferenceHelpText = organization!.PayerRefHelpText;
            
            IsPopUpPayerRefHelp = true;
            
            await Task.Delay(3000);

            IsPopUpPayerRefHelp = false;
        }
    }

    private async Task OnSpendBrainzPoint()
    {
        if(IsBusy) { Log.Debug("OnSpendBrainzPoint is in process. Return..."); return; }

        if (AvailableBrainzPoint <= 0)
        {
            await DialogService.ShowMessageBox("Alert", $"Brainz Point is 0", yesText: "OK");
            return;
        }

        if (BrainzPointsToSpend <= 0)
        {
            await DialogService.ShowMessageBox("Alert", $"Please Input points to spend", yesText: "OK");
            return;
        }

        if (BrainzPointsToSpend < BrainzParentsPortalSettings.TransferFunds.TrMininumPoint)
        {
            await DialogService.ShowMessageBox("Alert", $"Transfer minimum is {BrainzParentsPortalSettings.TransferFunds.TrMininumPoint}.", yesText: "OK");
            return;
        }

        if (AvailableBrainzPoint < BrainzPointsToSpend)
        {
            await DialogService.ShowMessageBox("Alert", $"The point you input can't exceed the available brainz point", yesText: "OK");
            return;
        }

        if (string.IsNullOrEmpty(SelectedChildOrgID))
        {
            await DialogService.ShowMessageBox("Alert", $"Please select an organization", yesText: "OK");
            return;
        }

        IsBusy = true;
        IsProgress = true;

        var selectedChildOrg = (from x in ChildOrganizations where x.ChildOrganizationID == SelectedChildOrgID select x).FirstOrDefault();

        var organization = (from org in Organizations where org.OrganizationCode == selectedChildOrg.Organization.OrganizationCode select org).ToList().FirstOrDefault();

        var parameters = new DialogParameters<TxConfirmDialog>()
        {
            {
                x => x.TxConfirmData, new TxConfirmDialog.TxConfirmForm
                {
                     MemberName = $"{LoggedInMember.FirstName} {LoggedInMember.LastName}",
                     PayeeReference = PayeeReference,
                     PayerReference = PayerReference,
                     AvailableBrainzPoints = AvailableBrainzPoint,
                     BrainzPointsToSpend = BrainzPointsToSpend,
                }
            }
        };
        var dialog = await DialogService.ShowAsync<TxConfirmDialog>("Confirm", parameters);
        var result = await dialog.Result;

        //Transfer funds


        string bankTxID = "";


        if (!result.Canceled)
        {
            string BSB = organization!.BSB;
            string bankName = organization.Bank;
            string accNumber = organization.AccountNumber;

            DateTime trDate = DateTime.Now;
            string trComments = $"From:{LoggedInMember.FirstName} {LoggedInMember.LastName}, Date:{trDate}, Amount:{BrainzPointsToSpend}, " +
                $"Org:{organization.OrganizationCode}, BSB:{BSB}, Bank:{bankName}, ACC:{accNumber}, PayeeRef:{PayeeReference}, PayerRef:{PayerReference}," +
                $"BankTxID:{bankTxID}";

            BrainzPointTr brainzPointTr = new BrainzPointTr()
            {
                BrainzPointTrID = Guid.NewGuid().ToString(),
                BrainzPointTrDate = trDate,
                MemberID = LoggedInMember.MemberID,
                Amount = BrainzPointsToSpend,
                OrganizationCode = organization.OrganizationCode,
                PayeeReference = PayeeReference,
                PayerReference = PayerReference,
                TrComments = $"From:{LoggedInMember.FirstName} {LoggedInMember.LastName}, Date:{trDate}, Amount:{BrainzPointsToSpend}, Org:{organization.OrganizationCode}",
                BankTxID = bankTxID,
                Comments = "",
            };

            IPortalDbService portalDbService = new PortalDbService(PortalDbConnectionSettings);
            portalDbService.AddBrainzPointTr(brainzPointTr);

            string memberName = $"{LoggedInMember.FirstName} {LoggedInMember.LastName}";
            string strTrDate = trDate.ToString("dd-MM-yyyy HH:mm:ss");
            string trFundCsvPath = BrainzParentsPortalSettings.TransferFunds.TrFundCsvPath;
            TransferFundFileHelper.WriteCsvFile(trFundCsvPath, brainzPointTr.BrainzPointTrID, memberName, strTrDate, PointToCurrency.ToString(),
                PayeeReference, PayerReference);

            await DialogService.ShowMessageBox("Success", $"Brainz Point transferred successfully !", yesText: "OK");

            NavManager.NavigateTo($"{NavManager.BaseUri}SpendPoints", true);

            //string TrReceiptPath = BrainzParentsPortalSettings.TransferFunds.TrReceiptPath;
            //string TrReceiptTemplatePathFile = BrainzParentsPortalSettings.TransferFunds.TrReceiptTemplatePathFile;

            //var createdHtmlReceiptPathFile = ReceiptHelper.CreateReceipt(TrReceiptPath, TrReceiptTemplatePathFile, memberName, strTrDate, BrainzPointsToSpend.ToString(),
            //    PayeeReference, PayerReference, "");
            //var createdPdfReceiptPathFile = ReceiptHelper.ConvertHtmlToPdf(createdHtmlReceiptPathFile, TrReceiptPath);
            //portalDbService.UpdateBrainzPointReceipt(brainzPointTr.BrainzPointTrID, createdPdfReceiptPathFile);
        }

        IsBusy = false;
        IsProgress = false;
    }
    void OnChildOrgSelectValueChanged( string value)
    {
        SelectedChildOrgID = value;

        var selectedChildOrg = (from x in ChildOrganizations where x.ChildOrganizationID == SelectedChildOrgID select x).FirstOrDefault();

        PayerReference = selectedChildOrg.PayerReference;

    }

}