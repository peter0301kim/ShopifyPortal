using BrainzParentsPortal.Integration.PortalDb;
using BrainzParentsPortal.Integration.PortalDb.Helpers;
using BrainzParentsPortal.Integration.PortalDb.Models;
using BrainzParentsPortal.Integration.PortalDb.Services;
using Paytec.Integration;
using Paytec.Integration.Shopify;
using Paytec.Integration.Shopify.Models;
using Paytec.Integration.Shopify.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainzParentsPortal.Shared.Helpers;

public class ShopifyToPortalDbHelper
{

    public static bool CopyShopifyCustomerToPortalDb(PortalDbConnectionSettings portalDbConnectionSettings, ShopifySettings shopifySettings, DateTime? lastUpdatedEdt = null)
    {
        bool bTrue = true;

        IShopifyService shopifyService = new ShopifyService(shopifySettings);


        var rValue = shopifyService.GetAllCustomers();
        if (rValue.IsSuccess)
        {
            IPortalDbMemberService portalDbUserService = new PortalDbMemberService(portalDbConnectionSettings);
            foreach (var customer in rValue.Object.customers)
            {
                if (portalDbUserService.IsUserExist(customer.email))
                {
                    //portalDbUserService.DeletePortalUserByEmail(customer.email);

                }
                else
                {
                    var member = UserConversion.ConvertShopifyCustomerToPortalDbUser(customer);
                    portalDbUserService.AddMember(member);



                    var rValueAddresses = shopifyService.GetCustomerAddresses(customer.id.ToString());
                    if (rValueAddresses.IsSuccess)
                    {
                        var addresses = rValueAddresses.Object.addresses;
                        var defAddress = (from addr in addresses where addr.@default == true select addr).ToList().FirstOrDefault();
                        if (defAddress != null)
                        {
                            member.Country = defAddress.country_name;
                            member.Address = defAddress.address1 ?? "";
                            member.Address2 = defAddress.address2 ?? "";
                            member.City = defAddress.city ?? "";
                            member.Region = defAddress.province ?? "";
                            member.PostCode = defAddress.zip ?? "";

                            portalDbUserService.UpdateMemberAddress(member);
                        }
                    }
                    



                }
            }
        }

        return bTrue;
    }



    public static bool CopyShopifyOrdersToPortalDb(PortalDbConnectionSettings portalDbConnectionSettings, ShopifySettings shopifySettings, DateTime? lastUpdatedEDT = null)
    {
        bool bTrue = true;

        IShopifyService shopifyService = new ShopifyService(shopifySettings);

        ApiReturnValue<ResponseModelOrders> rValue = new ApiReturnValue<ResponseModelOrders>();

        if(lastUpdatedEDT == null)
        {
            rValue = shopifyService.GetOrders();
        }
        else
        {
            rValue = shopifyService.GetOrdersByLastModified(lastUpdatedEDT.Value);
        }

        
        if (rValue.IsSuccess)
        {
            IPortalDbMemberService portalDbUserService = new PortalDbMemberService(portalDbConnectionSettings);
            foreach (var order in rValue.Object.orders)
            {
                //if (portalDbUserService.IsOrderExist(customer.email))
                //{
                //    //portalDbUserService.DeletePortalUserByEmail(customer.email);

                //}
                //else
                //{
                //    var portalUser = UserConversion.ConvertShopifyOrderToPortalDbOrder(order);
                //    portalDbUserService.AddOrder(portalUser);
                //}


            }
        }

        return bTrue;
    }

}
