using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyPortal.Integration.PortalDb.Models
{
    public class ResetPassword
    {
        public string MemberEmail { get; set; } = string.Empty;
        public string ResetPasswordID { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
    }
}
