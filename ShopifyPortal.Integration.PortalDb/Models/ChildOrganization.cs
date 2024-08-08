using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyPortal.Integration.PortalDb.Models;

public class ChildOrganization
{
    public string ChildOrganizationID { get; set; } = string.Empty;
    public Organization Organization { get; set; } = new Organization();
    public string PayerReference { get; set; } = string.Empty;
    public string ChildName { get; set; } = string.Empty;    
    public string MemberID { get; set; } = string.Empty;
}
