using ShopifyPortal.Integration.PortalDb.Models;

namespace ShopifyPortal.Shared.Models;

public class DtOrderTx
{
    public string OrderTxID { get; set; } = string.Empty;
    public DateTime OrderTxDate { get; set; }
    public string VendorCode { get; set; } = string.Empty;
    public string VendorOrderID { get; set; } = string.Empty;
    public string OrderName { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public string BrainzCustomerID { get; set; } = string.Empty;
    public string OrganizationCode { get; set; } = string.Empty;
    public string Comments { get; set; } = string.Empty;
    public decimal BrainzPoint { get; set; }
    public string BrainzPointCalc { get; set; } = string.Empty;
    public string ShopifyOrderID { get; set; } = string.Empty;
    public string MemberName { get; set; } = string.Empty;
    public string VendorName { get; set; } = string.Empty;
    public string OrganizationName { get; set; } = string.Empty;

}
