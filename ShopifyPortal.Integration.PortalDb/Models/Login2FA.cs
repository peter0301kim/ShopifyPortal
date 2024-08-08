using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyPortal.Integration.PortalDb.Models
{
    public class Login2FA
    {
        public string MemberEmail { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty; 
        public DateTime CreatedDate { get; set; }
    }
}
