using System;
using System.Collections.Generic;
using System.Text;

namespace Peter.Integration.Shopify.Models
{
    public class RequestModelFulfillment
    {
        public Fulfillment fulfillment { get; set; }
    }
    public class ResponseModelFulfillment 
    {
        public Fulfillment fulfillment { get; set; }
    }
    public class Fulfillment
    {
        public string id { get; set; }
        public string location_id { get; set; }
        public string order_Id { get; set; }
        public string status { get; set; }
    }
}
