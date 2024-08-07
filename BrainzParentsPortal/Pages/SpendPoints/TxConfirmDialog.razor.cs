using global::Microsoft.AspNetCore.Components;
using MudBlazor;
using BrainzParentsPortal.Integration.PortalDb.Models;
using BrainzParentsPortal.Integration.PortalDb.Services;
using static BrainzParentsPortal.Pages.Members.UpdateMemberPage;
using System.ComponentModel.DataAnnotations;

namespace BrainzParentsPortal.Pages.SpendPoints
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
        MudDialogInstance MudDialog { get; set; }

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