using System;
using System.Collections.Generic;
using System.Text;

namespace Peter.Integration.Shopify.Models
{

    public class ShopifyWebhookResponseMulti
    {
        public List<ShopifyWebhook> webhooks { get; set; }
    }

    public class ShopifyWebhookResponseSingle
    {
        public ShopifyWebhook webhook { get; set; }
    }

    public class ShopifyWebhook
    {
        public long id { get; set; }
        public string address { get; set; }
        public string topic { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string format { get; set; }
        public List<object> fields { get; set; }
        public List<object> metafield_namespaces { get; set; }
        public string api_version { get; set; }
        public List<object> private_metafield_namespaces { get; set; }
    }

    public class ShopifyWebhookRequest
    {
        //Create
        /*
        {
          "webhook": {
            "topic": "orders/create",
            "address": "https://whatever.hostname.com/",
            "format": "json"
          }
        }
        */

        //Update
        /*
        {
            "webhook": {
            "id": 4759306,
            "address": "https://somewhere-else.com/"
            }
        }    
        */

        public ShopifyWebhook webhook { get; set; }

    }

}
