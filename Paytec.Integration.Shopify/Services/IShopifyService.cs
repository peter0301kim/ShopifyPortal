using Paytec.Integration.Shopify.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Paytec.Integration.Shopify.Services
{
    public interface IShopifyService
    {
        ApiReturnValue<ResponseModelProducts> GetAllProducts();
        ApiReturnValue<ResponseModelCustomers> GetAllCustomers();
        ApiReturnValue<ResponseModelCustomer> GetCustomerById(string customerId);
        ApiReturnValue<ResponseModelCustomers> GetCustomerByEmail(string email);
        ApiReturnValue<ResponseModelCustomer> AddCustomer(RequestModelCustomer customer);
        ApiReturnValue<ResponseModelCustomer> UpdateCustomer(RequestModelCustomer customer);
        ApiReturnValue<ResponseModelCustomerAddresses> GetCustomerAddresses(string customerID);
        ApiReturnValue<ResponseModelOrders> GetOrders();
        ApiReturnValue<ResponseModelOrders> GetOrdersByName(string orderName);
        ApiReturnValue<ResponseModelOrders> GetOrdersByLastModified(DateTime lastModifiedEDT);
        ApiReturnValue<ResponseModelOrder> AddOrder( RequestModelOrder order);
        ApiReturnValue<ResponseModelOrder> Fulfillment(string orderNumber, RequestModelFulfillment fulfillment);
    }
}
