using ShopifyPortal.Integration.PortalDb;
using ShopifyPortal.Shared;
using ShopifyPortal.Shared.Helpers;
using Newtonsoft.Json;
using NLog;
using Peter.Integration.Shopify;
using System.Threading;

namespace ShopifyPortal.ShopifyToPortalDb.Console;

internal class Program
{
    private static Logger Log = LogManager.GetCurrentClassLogger();
    private static PortalDbConnectionSettings PortalDbConnectionSettings { get; set; }
    private static ShopifySettings ShopifySettings { get; set; }
    private static ShopifyToPortalDbSettings ShopifyToPortalDbSettings { get; set; }


    static void Main(string[] args)
    {
        if (!LoadApplication()) { return; }

        if (ShopifyToPortalDbSettings.IsCopyShopifyCustomersToPortalDb)
        {
            CopyShopifyCustomersToPortalDbUsers();
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, easternZone);

            ShopifyToPortalDbSettings.CustomersUpdatedDateEDT = easternTime;

            File.WriteAllText(GlobalSettings.Instance.ShopifyToPortalDbSettingsPathFile,
                            JsonConvert.SerializeObject(ShopifyToPortalDbSettings, Formatting.Indented));

        }


        if (ShopifyToPortalDbSettings.IsCopyShopifyOrdersToPortalDb)
        {
            CopyShopifyOrdersToPortalDbOrders();

            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, easternZone);

            ShopifyToPortalDbSettings.OrdersUpdatedDateEDT = easternTime;

            File.WriteAllText(GlobalSettings.Instance.ShopifyToPortalDbSettingsPathFile,
                            JsonConvert.SerializeObject(ShopifyToPortalDbSettings, Formatting.Indented));

        }


    }

    static bool LoadApplication()
    {
        bool bTrue = true;

        if (File.Exists(GlobalSettings.Instance.ShopifySettingsPathFile))
        {
            ShopifySettings
                = JsonConvert.DeserializeObject<ShopifySettings>(File.ReadAllText(GlobalSettings.Instance.ShopifySettingsPathFile));
        }
        else
        {
            System.Console.WriteLine("No ShopifySettingsPathFile file", "ERROR");
            bTrue = false;
            return bTrue;
        }

        if (File.Exists(GlobalSettings.Instance.PortalDbConnectionSettingsPathFile))
        {
            PortalDbConnectionSettings
                = JsonConvert.DeserializeObject<PortalDbConnectionSettings>(File.ReadAllText(GlobalSettings.Instance.PortalDbConnectionSettingsPathFile));
        }
        else
        {
            System.Console.WriteLine("No PortalDbConnectionSettings file", "ERROR");
            bTrue = false;
            return bTrue;
        }

        if (File.Exists(GlobalSettings.Instance.ShopifyToPortalDbSettingsPathFile))
        {
            ShopifyToPortalDbSettings
                = JsonConvert.DeserializeObject<ShopifyToPortalDbSettings>(File.ReadAllText(GlobalSettings.Instance.ShopifyToPortalDbSettingsPathFile));
        }
        else
        {
            System.Console.WriteLine("No ShopifyToPortalDbSettings file", "ERROR");
            bTrue = false;
            return bTrue;
        }

        return bTrue;
    }

    static bool CopyShopifyCustomersToPortalDbUsers()
    {

        return ShopifyToPortalDbHelper.CopyShopifyCustomerToPortalDb(PortalDbConnectionSettings, ShopifySettings);
    }

    static bool CopyShopifyOrdersToPortalDbOrders()
    {

        return ShopifyToPortalDbHelper.CopyShopifyOrdersToPortalDb(PortalDbConnectionSettings, ShopifySettings);
    }

}