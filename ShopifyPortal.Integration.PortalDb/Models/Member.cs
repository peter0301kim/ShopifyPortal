using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyPortal.Integration.PortalDb.Models;

public class Member
{
    public string MemberID { get; set; } = string.Empty;    
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Address2 { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string PostCode { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Tags { get; set; } = string.Empty;
    public string CustomerID { get; set; } = string.Empty; //Shopify Customer ID
    public decimal BrainzPoint { get; set; } = 0.00M;
    public string ImagePathFile { get; set; } = string.Empty;
    public DateTime RegisteredDate { get; set; }
    public DateTime ModifiedDate { get; set; }

}
