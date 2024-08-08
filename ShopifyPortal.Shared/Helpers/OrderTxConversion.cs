using ShopifyPortal.Integration.PortalDb.Models;
using ShopifyPortal.Shared.Models;
using Peter.Integration.Shopify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyPortal.Shared.Helpers
{
    public class OrderTxConversion
    {

        public static List<DtOrderTx> ConvertOrderTxsToDtOrderTxs(List<OrderTx> orderTxs)
        {
            var dtOrderTxs = (from orderTx in orderTxs
                             select new DtOrderTx()
                             {
                                 OrderTxID = orderTx.OrderTxID,
                                 OrderTxDate = orderTx.OrderTxDate,
                                 VendorCode = orderTx.VendorCode,
                                 VendorOrderID = orderTx.VendorOrderID,
                                 OrderName = orderTx.OrderName,
                                 Total = orderTx.Total,
                                 BrainzCustomerID = orderTx.BrainzCustomerID,
                                 OrganizationCode = orderTx.OrganizationCode,
                                 Comments = orderTx.Comments,
                                 BrainzPoint = orderTx.BrainzPoint,
                                 BrainzPointCalc = orderTx.BrainzPointCalc,
                                 ShopifyOrderID = orderTx.ShopifyOrderID,
                                 MemberName = "",
                                 VendorName = "",
                                 OrganizationName = "",
                             }).ToList();

            return dtOrderTxs;
        }
        public static DtOrderTx ConvertOrderTxToDtOrderTx(OrderTx orderTx)
        {
            var dtOrderTx = new DtOrderTx()
            {
                OrderTxID = orderTx.OrderTxID,
                OrderTxDate = orderTx.OrderTxDate ,
                VendorCode = orderTx.VendorCode,
                VendorOrderID = orderTx.VendorOrderID ,
                OrderName = orderTx.OrderName,
                Total = orderTx.Total,
                BrainzCustomerID = orderTx.BrainzCustomerID,
                OrganizationCode = orderTx.OrganizationCode,
                Comments = orderTx.Comments,
                BrainzPoint = orderTx.BrainzPoint,
                BrainzPointCalc = orderTx.BrainzPointCalc,
                ShopifyOrderID = orderTx.ShopifyOrderID,
                MemberName = "",
                VendorName =  "" ,
                OrganizationName = "" ,


            };

            return dtOrderTx;
        }
    }
}
