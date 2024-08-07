using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainzParentsPortal.Integration.PortalDb.Models;

public class Vendor
{
    public string VendorID { get; set; } = string.Empty;
    public string VendorCode { get; set; } = string.Empty;
    public string VendorName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string ContactPerson { get; set; } = string.Empty;
    public string Memo { get; set; } = string.Empty;
    public decimal VendorCommission { get; set; } = 0;
    public decimal ParentCommission { get; set; } = 0;
    public string ImagePathFile { get; set; } = string.Empty;
    public DateTime CreatedDate { get; } = DateTime.UtcNow;
    public DateTime ModifiedDate { get; set; }

}
