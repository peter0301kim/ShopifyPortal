using Peter.Integration.Shopify.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Peter.Integration.Shopify.Services.Webhook
{
    public interface IShopifyWebhookService
    {
        ApiReturnValue<ShopifyWebhookResponseMulti> GetWebhooks(string destUri, string apiKey, string apiPassword);
        ApiReturnValue<ShopifyWebhookResponseMulti> GetWebhookByWebhookId(string destUri, string apiKey, string apiPassword, string webhookId);
        ApiReturnValue<ShopifyWebhookResponseSingle> CreateWebhook(string destUri, string apiKey, string apiPassword, ShopifyWebhookRequest webhookRequest);
        ApiReturnValue<ShopifyWebhookResponseSingle> UpdateWebhook(string destUri, string apiKey, string apiPassword, string webhookId, ShopifyWebhookRequest webhookRequest);
        ApiReturnValue<string> DeleteWebhook(string destUri, string apiKey, string apiPassword, string webhookId);
    }
}
