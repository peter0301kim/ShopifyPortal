using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainzParentsPortal.Integration.PortalDb.Models;

public class Organization
{
    public string OrganizationID { get; set; } = string.Empty;
    public string OrganizationType { get; set; } = string.Empty;
    public string OrganizationCode { get; set; } = string.Empty;
    public string OrganizationName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string ContactPerson { get; set; } = string.Empty;
    public decimal ParentCommission { get; set; } = 0;
    public string BSB { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string Bank { get; set; } = string.Empty;
    public string PayerRefHelpText { get; set; } = string.Empty;    
    public string Memo { get; set; } = string.Empty;
    public string ImagePathFile { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
}
