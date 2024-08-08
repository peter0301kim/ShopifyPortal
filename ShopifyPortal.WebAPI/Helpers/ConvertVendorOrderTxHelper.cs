using ShopifyPortal.Integration.PortalDb.Models;
using ShopifyPortal.WebAPI.Models;
using Newtonsoft.Json;
using NLog;
using NLog.Fluent;
using Peter.Integration.Shopify;
using Peter.Integration.Shopify.Models;
using Peter.Integration.Shopify.Services;

namespace ShopifyPortal.WebAPI.Helpers;

public class ConvertVendorOrderTxHelper
{
    private static Logger Log = LogManager.GetCurrentClassLogger();
    public static OrderTx ConvertVendorOrderTxToPortalDbOrderTx(VendorOrderTx vendorOrderTx)
    {
        OrderTx orderTx = new OrderTx
        {
            OrderTxID = System.Guid.NewGuid().ToString(),
            OrderTxDate = System.DateTime.UtcNow,
            VendorCode = vendorOrderTx.Vendor,
            VendorOrderID = vendorOrderTx.OrderNumber,
            OrderName = "",
            Total = vendorOrderTx.TotalPrice,
            BrainzCustomerID = vendorOrderTx.BrainzCustomerID,
            OrganizationCode = vendorOrderTx.Organization,
            Comments = "",
            BrainzPoint = 0,
            BrainzPointCalc = "",
            ShopifyOrderID = "",
        };

        return orderTx;
    }


    public static ShopifyAddOrder ConvertVendorOrderTxToShopifyOrder(VendorOrderTx vendorOrderTx, ShopifySettings shopifySettings)
    {
        IShopifyService shopifyService = new ShopifyService(shopifySettings);

        Customer retrievedCustomer = null;

        if (!string.IsNullOrEmpty(vendorOrderTx.OrderNumber))
        {
            var response = shopifyService.GetCustomerById(vendorOrderTx.BrainzCustomerID);
            if(response.Object != null)
            {
                retrievedCustomer = response.Object.customer;
            }
        }

        if (retrievedCustomer == null)
        {
            Log.Debug($"No customer id - {vendorOrderTx.BrainzCustomerID} - on Shopify. Use default customer id : {shopifySettings.DefaultCustomerId}");
            retrievedCustomer = new Customer { id = long.Parse(shopifySettings.DefaultCustomerId) };
        }

        Log.Info($"OrderNo:{vendorOrderTx.OrderNumber}, Price:{vendorOrderTx.TotalPrice}, BrainzCustomerID:{vendorOrderTx.BrainzCustomerID}, Customer:{retrievedCustomer.email} ");

        string orderNote = shopifySettings.DefaultOrderNote.Replace("{VENDOR_CODE}", vendorOrderTx.Vendor);

        ShopifyAddOrder shopifyAddOrder = new ShopifyAddOrder
        {
            name = vendorOrderTx.OrderNumber,
            customer = new OrderCustomer
            {
                id = retrievedCustomer.id
            },
            line_items = new List<Line_Item>
            {
                new Line_Item
                {
                    title = shopifySettings.DefaultOrderLineItemTitle,
                    name = shopifySettings.DefaultProductName,
                    variant_id = long.Parse(shopifySettings.DefaultProcuctId),
                    quantity = 1,
                    price = vendorOrderTx.TotalPrice,
                    fulfillment_status = "Fulfilled",
                    vendor = vendorOrderTx.Vendor
                    
                }
            },
            tags = vendorOrderTx.Organization,
            note = orderNote
        };

        return shopifyAddOrder;
    }

}
