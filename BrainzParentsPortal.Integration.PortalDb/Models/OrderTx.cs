using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainzParentsPortal.Integration.PortalDb.Models;

public class OrderTx
{
    public string OrderTxID { get; set; } = string.Empty;
    public DateTime OrderTxDate { get; set; }
    public string VendorCode { get; set; } = string.Empty;
    public string VendorOrderID { get; set; } = string.Empty;
    public string OrderName { get; set; } = string.Empty;
    public decimal Total { get; set; } = 0;
    public string BrainzCustomerID { get; set; } = string.Empty;
    public string OrganizationCode { get; set; } = string.Empty;
    public string Comments { get; set; } = string.Empty;
    public int BrainzPoint { get; set; } = 0;
    public string BrainzPointCalc { get; set; } = string.Empty;
    public string ShopifyOrderID { get; set; } = string.Empty;
    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    //public decimal GetTotalPrice()
    //{
    //    decimal totalPrice = 0;
    //    foreach (var item in OrderItems)
    //    {
    //        totalPrice += item.Quantity * item.Amount;
    //    }
    //    return totalPrice;
    //}
}

public class OrderItem
{
    public string OrderItemID { get; set; } = string.Empty;
    public string OrderItemName { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Amount { get; set; }

}
