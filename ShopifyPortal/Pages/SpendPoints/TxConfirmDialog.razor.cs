using global::Microsoft.AspNetCore.Components;
using MudBlazor;
using ShopifyPortal.Integration.PortalDb.Models;
using ShopifyPortal.Integration.PortalDb.Services;
using static ShopifyPortal.Pages.Members.UpdateMemberPage;
using System.ComponentModel.DataAnnotations;

namespace ShopifyPortal.Pages.SpendPoints
{
    public partial class TxConfirmDialog
    {

        public class TxConfirmForm
        {
            public string MemberName { get; set; } = string.Empty;

            public string PayeeReference { get; set; } = string.Empty;

            public string PayerReference { get; set; } = string.Empty;

            public decimal AvailableBrainzPoints { get; set; } 

            public decimal BrainzPointsToSpend { get; set; } 

        }

        [CascadingParameter]
        IMudDialogInstance MudDialog { get; set; }

        [Parameter]
        public TxConfirmForm TxConfirmData { get; set; }



        protected override async Task OnInitializedAsync()
        {




        }

        private void OnCancel()
        {
            MudDialog.Cancel();
        }

        private void OnOk()
        {
            string rValue = "Ok";
            MudDialog.Close(DialogResult.Ok(rValue));
        }
    }
}