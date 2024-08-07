using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainzParentsPortal.Integration.PortalDb.Models;

public class TransferCommission
{
    public string CustomerID { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string VendorID { get; set; } = string.Empty;
    public string VendorName { get; set; } = string.Empty;
    public string SchoolID { get; set;  } = string.Empty;
    public string SchoolName { get; set; } = string.Empty;
    public decimal TransferAmount { get; set; }

}
