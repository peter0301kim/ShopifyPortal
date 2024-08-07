using Newtonsoft.Json;
using NLog;
using NLog.Fluent;
using Paytec.Integration.Shopify.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paytec.Integration.Shopify;

internal class Program
{
    private static Logger Log = LogManager.GetCurrentClassLogger();
    private static ShopifySettings ShopifySettings { get; set; }
    static void Main(string[] args)
    {
        if (!LoadApplication()) { return; }

        IShopifyService shopifyService = new ShopifyService(ShopifySettings);
        //var rValue = shopifyService.GetAllCustomers();

        string email = "peter@paytec.com.au";
        GetCustomerByEmail(shopifyService, email);



        GetOrders( shopifyService);
    }

    static void GetOrders(IShopifyService shopifyService)
    {
        var rValue = shopifyService.GetOrders();

        var obj = JsonConvert.SerializeObject(rValue.Object.orders, Formatting.Indented);
        File.WriteAllText(@$".\Orders-{DateTime.Now.ToString("yyyyMMddHHmmss")}.json", obj);
    }

    static void GetCustomerByEmail(IShopifyService shopifyService, string email)
    {
        var rValue = shopifyService.GetCustomerByEmail("peter@paytec.com.au");

        var obj = JsonConvert.SerializeObject(rValue.Object.customers, Formatting.Indented);
        Log.Info(obj);
        //File.WriteAllText(@$".\Customers-{email}.json", obj);

        var customerID = rValue.Object.customers.FirstOrDefault().id;

        var responseAddresses = shopifyService.GetCustomerAddresses(customerID.ToString());
        Log.Info(JsonConvert.SerializeObject(responseAddresses.Object.addresses, Formatting.Indented));
    }

    static bool LoadApplication()
    {
        ShopifySettings = new ShopifySettings
        {
            ApiAccessToken = "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", //Live:, Test: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
            ApiKey = "0dd3516c28104da54d3e75d6a5e97cc3",               //Live:, Test:0dd3516c28104da54d3e75d6a5e97cc3
            ApiSecretKey = "d2d4e23a050c5ead4336cba76f05dbfb",         //Live:, Test:d2d4e23a050c5ead4336cba76f05dbfb
            ApiBaseUrl = "https://paytec-test.myshopify.com", //Live:https://brainz-online.myshopify.com, Test:https://paytec-test.myshopify.com
            ApiPath = "/admin/api/",
            ApiVersion = "2024-01",
            ApiWebhookVersioin = "2024-01",
            DefaultOrderLineItemTitle = "TWG back-to-school stationary", //Live:TWG back-to-school stationary, Test:TWG back-to-school stationary
            DefaultOrderNote = "The order from TWG",
            DefaultProcuctId = "6665684713603", //Live:7051569954974, Test:6665684713603
            DefaultProductName = "TWG back-to-school stationary", //Live:TWG back-to-school stationary, Test:TWG back-to-school stationary
            DefaultCustomerId = "5306329301123", //Live:5867557453982, Test:5306329301123
            DefaultLocationId = "60942745731" // Live:65530724510, Test:60942516355
        };

        return true;
    }
}
