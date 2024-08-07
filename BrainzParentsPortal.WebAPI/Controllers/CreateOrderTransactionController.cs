using BrainzParentsPortal.Integration.PortalDb;
using BrainzParentsPortal.Integration.PortalDb.Models;
using BrainzParentsPortal.Integration.PortalDb.Services;
using BrainzParentsPortal.Shared;
using BrainzParentsPortal.Shared.Models;
using BrainzParentsPortal.WebAPI.Helpers;
using BrainzParentsPortal.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NLog;
using NLog.Targets;
using Paytec.Integration;
using Paytec.Integration.Shopify;
using Paytec.Integration.Shopify.Models;
using Paytec.Integration.Shopify.Services;

namespace BrainzParentsPortal.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CreateOrderTransactionController : ControllerBase
{
    private static Logger Log = LogManager.GetCurrentClassLogger();
    private static PortalDbConnectionSettings PortalDbConnectionSettings { get; set; }
    private static ShopifySettings ShopifySettings { get; set; }

    public CreateOrderTransactionController()
    {
        LoadApplication();
    }
    public ActionResult<ApiReturnValue<string>> CreateOrderTransaction(VendorOrderTx vendorOrderTx)
    {
        Log.Info($"----- CreateOrderTransaction -----");

        ApiReturnValue<string> apiReturnValue = new ApiReturnValue<string>();

        var varifyResult = VerifyVendorOrderTx(vendorOrderTx);
        if (!varifyResult.IsSuccess)
        {
            return new ApiReturnValue<string>
            {
                IsSuccess = false,
                ReturnMessage = new ReturnMessage
                { 
                    Code = varifyResult.ReturnMessage.Code, Message = varifyResult.ReturnMessage.Message
                }
            };
        }

        WriteVendorOrderTxLog(vendorOrderTx);

        try
        {
            IShopifyService shopifyService = new ShopifyService(ShopifySettings);

            var shopifyAddOrder = ConvertVendorOrderTxHelper.ConvertVendorOrderTxToShopifyOrder(vendorOrderTx, ShopifySettings);

            //Add Shopify Order
            var shopifyAddOrderResult = shopifyService.AddOrder(new RequestModelOrder {order = shopifyAddOrder });

            if (!shopifyAddOrderResult.IsSuccess)
            {
                return new ApiReturnValue<string>
                {
                    IsSuccess = false,
                    ReturnMessage = new ReturnMessage { Code = "ERR_ADD_SHOPIFY", Message = "Error - Add Shopify Order" }
                };
            }

            Log.Debug($"SUCCESS - Shopify - Add Order, OrderID : {shopifyAddOrderResult.Object.order.id}");

            //Add Shopify Fulfillment
            string shopifyOrderNumber = shopifyAddOrderResult.Object.order.id.ToString();


            //Fulfillment fulfillment = new Fulfillment()
            //{
            //    location_id = ShopifySettings.DefaultLocationId
            //};

            //var fulfillmentResult = shopifyService.Fulfillment(shopifyOrderNumber, new RequestModelFulfillment { fulfillment = fulfillment });

            //if (!fulfillmentResult.IsSuccess)
            //{
            //    return new ApiReturnValue<string>
            //    {
            //        IsSuccess = false,
            //        ApiReturnMessage = new ApiReturnMessage { Code = "ERR_ADD_FULFILLMENT", Message = "Error - Add Fulfillment Order" } 
            //    };
            //}

            Log.Debug($"SUCCESS - Shopify - Add Fulfillment");

            //Add Order to Portal Db
            var isSuccess = AddOrderToPortalDb(vendorOrderTx, shopifyOrderNumber);
            if (isSuccess)
            {
                apiReturnValue = new ApiReturnValue<string>
                {
                    IsSuccess = true,Object = null,ReturnMessage = null
                };
            }
            else
            {
                apiReturnValue = new ApiReturnValue<string>
                {
                    IsSuccess = false,
                    Object = null,
                    ReturnMessage = new ReturnMessage { Code = "ERR_ADD_ORDER_TO_PORTALDB", Message = "FAIL - Add order to portal db" }
                };

            }
        }
        catch (Exception e)
        {
            apiReturnValue = new ApiReturnValue<string>
            {
                IsSuccess = false,Object = null,ReturnMessage = new ReturnMessage { Code = "ERR_CREATE_ORDER_EXCEPTION", Message = e.ToString()}
            };
        }

        Log.Info($"----- End of CreateOrderTransaction -----");

        return apiReturnValue;
    }

    private bool AddOrderToPortalDb(VendorOrderTx vendorOrderTx, string shopifyOrderNumber)
    {

        IPortalDbService portalDbService = new PortalDbService(PortalDbConnectionSettings);
        var portalOrderTx = ConvertVendorOrderTxHelper.ConvertVendorOrderTxToPortalDbOrderTx(vendorOrderTx);

        string pointCalc = "";
        var organization = portalDbService.GetOrganizationByOrganizationCode(portalOrderTx.OrganizationCode);
        var vendor = portalDbService.GetVendorByVendorCode(portalOrderTx.VendorCode);

        decimal purchasedPrice = portalOrderTx.Total;
        pointCalc = $"Purchased Price [{purchasedPrice}] * ";
        decimal vendorCommission = vendor.VendorCommission;
        pointCalc += $"VendorCommission [{vendor.VendorCode} - {vendorCommission}] * ";

        decimal parentCommission = organization.ParentCommission;

        if (parentCommission != 0)
        {
            pointCalc += $"ParentCommission [Organization : {organization.OrganizationCode} - {parentCommission}]  ";
        }
        else
        {
            parentCommission = vendor.ParentCommission;
            pointCalc += $"ParentCommission [Vendor : {vendor.VendorCode} - {parentCommission}]  ";
        }

        var tmpPoint = purchasedPrice * vendorCommission * parentCommission;

        pointCalc += $" = {tmpPoint} * 100";
        tmpPoint = tmpPoint * 100;
        
        pointCalc += $" = {tmpPoint}";

        int brainzPoint = (int)tmpPoint;
        //var brainzPoint = Math.Truncate(100 * tmpPoint) / 100; //Truncate Two decimal places without rounding

        pointCalc += $" = Brainz Point [{brainzPoint}] ";

        // BrainzPoint
        portalOrderTx.OrderName = ShopifySettings.DefaultOrderNote.Replace("{VENDOR_CODE}", vendorOrderTx.Vendor);
        portalOrderTx.Comments = "";
        portalOrderTx.BrainzPoint = brainzPoint;
        portalOrderTx.BrainzPointCalc = pointCalc;
        portalOrderTx.ShopifyOrderID = shopifyOrderNumber;

        var isSuccess = portalDbService.AddOrderTx(portalOrderTx);

        return isSuccess;

    }
    private void WriteVendorOrderTxLog(VendorOrderTx vendorOrderTx)
    {
        var defaultTarget = (FileTarget)LogManager.Configuration.FindTargetByName("logfile");
        var defaultFileName = defaultTarget.FileName;
        var defaultLayout = defaultTarget.Layout;

        //Change log file name
        var newTarget = (FileTarget)LogManager.Configuration.FindTargetByName("trlogfile");
        defaultTarget.FileName = newTarget.FileName;
        defaultTarget.Layout = newTarget.Layout;

        Log.Info($"Vendor:{vendorOrderTx.Vendor}, Org:{vendorOrderTx.Organization}, " +
            $"OrderNo:{vendorOrderTx.OrderNumber}, BrainzCustomerID:{vendorOrderTx.BrainzCustomerID},  TotalPrice:{vendorOrderTx.TotalPrice} ");

        //Restore log file
        defaultTarget.FileName = defaultFileName;
        defaultTarget.Layout = defaultLayout;
    }

    private BoolReturnValue VerifyVendorOrderTx(VendorOrderTx twgOrder)
    {
        BoolReturnValue apiReturnValue = null;

        string orderNumber = twgOrder.OrderNumber;
        string brainzCustomerId = twgOrder.BrainzCustomerID;
        decimal totalPrice = twgOrder.TotalPrice;

        if (string.IsNullOrEmpty(orderNumber))
        {
            apiReturnValue = new BoolReturnValue
            {
                IsSuccess = false,
                ReturnMessage = new ReturnMessage { Code = "ERR_ORDER_NUMBER_EMPTY", Message = "There is no order number" }
            };
        }
        else if (string.IsNullOrEmpty(brainzCustomerId))
        {
            apiReturnValue = new BoolReturnValue
            {
                IsSuccess = false,
                ReturnMessage = new ReturnMessage { Code = "ERR_CUSTOMER_ID_EMPTY", Message = "There is no customer id" }
            };
        }
        else if (totalPrice <= 0.00m)
        {
            apiReturnValue = new BoolReturnValue
            {
                IsSuccess = false,
                ReturnMessage = new ReturnMessage { Code = "ERR_TOTAL_ZERO", Message = "The price cannot be equal to or less than zero." }
            };
        }
        else
        {
            apiReturnValue = new BoolReturnValue
            {
                IsSuccess = true, ReturnMessage = null
            };
        }

        return apiReturnValue;

    }


    //private ShopifySettings LoadConfigs()
    //{
    //    ShopifySettings shopifySettings = null;
    //    try
    //    {
    //        if (System.IO.File.Exists(GlobalSettings.Instance.ShopifySettingsPathFile))
    //        {
    //            shopifySettings
    //                = JsonConvert.DeserializeObject<ShopifySettings>(System.IO.File.ReadAllText(GlobalSettings.Instance.ShopifySettingsPathFile));
    //        }
    //        else
    //        {
    //            string msg = $"No ShopifySettings file:{GlobalSettings.Instance.ShopifySettingsPathFile}";
    //            Log.Warn(msg);
    //            throw new Exception(msg);
    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        Log.Error(e);
    //        throw new Exception(e.ToString());
    //    }

    //    return shopifySettings;
    //}

    static bool LoadApplication()
    {
        bool bTrue = true;

        ShopifySettings = new ShopifySettings();
        if (System.IO.File.Exists(GlobalSettings.Instance.PortalDbConnectionSettingsPathFile))
        {
            ShopifySettings
                = JsonConvert.DeserializeObject<ShopifySettings>(System.IO.File.ReadAllText(GlobalSettings.Instance.ShopifySettingsPathFile));
        }
        else
        {
            System.Console.WriteLine("No PortalDbConnectionSettings file", "ERROR");
            bTrue = false;
            return bTrue;
        }

        PortalDbConnectionSettings = new PortalDbConnectionSettings();
        if (System.IO.File.Exists(GlobalSettings.Instance.PortalDbConnectionSettingsPathFile))
        {
            PortalDbConnectionSettings
                = JsonConvert.DeserializeObject<PortalDbConnectionSettings>(System.IO.File.ReadAllText(GlobalSettings.Instance.PortalDbConnectionSettingsPathFile));
        }
        else
        {
            System.Console.WriteLine("No PortalDbConnectionSettings file", "ERROR");
            bTrue = false;
            return bTrue;
        }


        return bTrue;
    }
}
