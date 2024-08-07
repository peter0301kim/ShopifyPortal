namespace BrainzParentsPortal.WebAPI.Models
{
    public class VendorOrderTx
    {
        public string OrderNumber { get; set; } = string.Empty;
        public string BrainzCustomerID { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public string Vendor { get; set; } = string.Empty;
        public string Organization { get; set; } = string.Empty;
    }
}
