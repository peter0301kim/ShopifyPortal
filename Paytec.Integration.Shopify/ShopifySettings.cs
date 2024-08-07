using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Paytec.Integration.Shopify
{
    public class ShopifySettings
    {
        public string ApiAccessToken { get; set; }
        public string ApiKey { get; set; }
        public string ApiSecretKey { get; set; }
        public string ApiBaseUrl { get; set; }
        public string ApiPath { get; set; }
        public string ApiVersion { get; set; }
        public string ApiWebhookVersioin { get; set; }
        public string DefaultOrderLineItemTitle { get; set; }
        public string DefaultOrderNote { get; set; }
        public string DefaultProcuctId { get; set; }        
        public string DefaultProductName { get; set; }
        public string DefaultCustomerId { get; set; }
        public string DefaultLocationId { get; set; }

        public ShopifySettings()
        {
                
        }
        public ShopifySettings(string settingsPathFile)
        {
            if (File.Exists(settingsPathFile))
            {
                var settings = JsonConvert.DeserializeObject<ShopifySettings>(File.ReadAllText(settingsPathFile));
                this.ApiAccessToken = settings.ApiAccessToken;
                this.ApiKey = settings.ApiKey;
                this.ApiSecretKey = settings.ApiSecretKey;
                this.ApiBaseUrl = settings.ApiBaseUrl;
                this.ApiPath = settings.ApiPath;
                this.ApiVersion = settings.ApiVersion;
                this.ApiWebhookVersioin = settings.ApiWebhookVersioin;
                this.DefaultOrderLineItemTitle = settings.DefaultOrderLineItemTitle;
                this.DefaultOrderNote = settings.DefaultOrderNote;
                this.DefaultProcuctId = settings.DefaultProcuctId;
                this.DefaultProductName = settings.DefaultProductName;
                this.DefaultCustomerId = settings.DefaultCustomerId;
                this.DefaultLocationId = settings.DefaultLocationId;
            }
        }

    }
}
