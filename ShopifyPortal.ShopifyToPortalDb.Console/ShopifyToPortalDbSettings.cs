using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyPortal.ShopifyToPortalDb.Console;

public class ShopifyToPortalDbSettings
{
    //Shopify uses EDT.
    //Eastern Daylight Time (EDT) is 4 hours behind Coordinated Universal Time (UTC).
    //This time zone is a Daylight Saving Time time zone and is used in: North America,

    public bool IsCopyShopifyCustomersToPortalDb { get; set; }
    public bool IsCopyShopifyOrdersToPortalDb { get; set; }
    public DateTime? CustomersUpdatedDateEDT { get; set; }
    public DateTime? OrdersUpdatedDateEDT { get; set; }
}
