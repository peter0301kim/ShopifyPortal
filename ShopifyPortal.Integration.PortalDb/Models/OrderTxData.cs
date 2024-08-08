using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyPortal.Integration.PortalDb.Models
{
    public class OrderTxData : OrderTx
    {
        public string MemberFullName { get; set; } = string.Empty;
    }
}
