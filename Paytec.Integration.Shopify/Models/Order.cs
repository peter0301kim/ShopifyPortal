using System;
using System.Collections.Generic;
using System.Text;

namespace Paytec.Integration.Shopify.Models
{
    public class RequestModelOrder
    {
        public ShopifyAddOrder order { get; set; }
    }
    public class ResponseModelOrders
    {
        public List<ShopifyOrder> orders { get; set; }
    }

    public class ResponseModelOrder
    {
        public ShopifyOrder order { get; set; }
    }
    public class ShopifyOrder
    {
        public long id { get; set; }
        public string name { get; set; }
        public Customer customer { get; set; }
        public List<Line_Item> line_items { get; set; }
        public string note { get; set; }

    }

    public class ShopifyAddOrder
    {
        public long id { get; set; }
        public string name { get; set; }
        public OrderCustomer customer { get; set; }
        public List<Line_Item> line_items { get; set; }
        public string tags { get; set; }
        public string note { get; set; }
    }

    public class OrderCustomer
    {
        public long id { get; set; }
    }

    public class Line_Item
    { 
        public string title { get; set; }
        public string name { get; set; }
        public long? variant_id { get; set; }
        public int quantity { get; set; }
        public decimal price { get; set; }
        public string fulfillment_status { get; set; }
        public string vendor { get; set; }

    }
}
