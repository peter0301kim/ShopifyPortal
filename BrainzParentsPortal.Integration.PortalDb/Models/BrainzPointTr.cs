using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainzParentsPortal.Integration.PortalDb.Models
{
    public class BrainzPointTr
    {
        public string BrainzPointTrID { get; set; } = string.Empty;
        public DateTime BrainzPointTrDate { get; set; }
        public string MemberID { get; set; } = string.Empty;
        public decimal Amount { get; set; } = 0.00M;
        public string OrganizationCode { get; set; } = string.Empty;
        public string PayeeReference { get; set; } = string.Empty;
        public string PayerReference { get; set; } = string.Empty;
        public string TrComments { get; set; } = string.Empty;
        public string BankTxID { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;

    }
}
