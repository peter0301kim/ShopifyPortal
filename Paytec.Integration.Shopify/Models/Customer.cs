﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Paytec.Integration.Shopify.Models
{
    public class RequestModelCustomer
    {
        public Customer customer { get; set; }
    }
    public class ResponseModelCustomers
    {
        public List<Customer> customers { get; set; }
    }

    public class ResponseModelCustomerAddresses
    {
        public List<Address> addresses { get; set; }
    }

    public class ResponseModelCustomer
    {
        public Customer customer { get; set; }
    }
        
    public class Customer
    {
        public long id { get; set; }
        public string email { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public bool accepts_marketing { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public int orders_count { get; set; }
        public string state { get; set; }
        public string total_spent { get; set; }
        public long? last_order_id { get; set; }
        public string note { get; set; }
        public bool verified_email { get; set; }
        public object multipass_identifier { get; set; }
        public bool tax_exempt { get; set; }
        public string phone { get; set; }
        public string tags { get; set; }
        public string last_order_name { get; set; }
        public string currency { get; set; }
        public List<Address> addresses { get; set; }
        public DateTime? accepts_marketing_updated_at { get; set; }
        public object marketing_opt_in_level { get; set; }
        public List<object> tax_exemptions { get; set; }
        public string admin_graphql_api_id { get; set; }
        public DefaultAddress default_address { get; set; }
     
        public override string ToString()
        {
            var propertyInfos = this.GetType().GetProperties();
            var sb = new StringBuilder();

            foreach (var info in propertyInfos)
            {
                var value = info.GetValue(this, null) ?? "(null)";
                sb.AppendLine(info.Name + ": " + value.ToString());
            }

            return sb.ToString();
        }
    }


    public class Address
    {
        public long id { get; set; }
        public long customer_id { get; set; }
        public object first_name { get; set; }
        public object last_name { get; set; }
        public object company { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string province { get; set; }
        public string country { get; set; }
        public string zip { get; set; }
        public string phone { get; set; }
        public string name { get; set; }
        public string province_code { get; set; }
        public string country_code { get; set; }
        public string country_name { get; set; }
        public bool @default { get; set; }
    }

    public class DefaultAddress
    {
        public long id { get; set; }
        public long customer_id { get; set; }
        public object first_name { get; set; }
        public object last_name { get; set; }
        public object company { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string province { get; set; }
        public string country { get; set; }
        public string zip { get; set; }
        public string phone { get; set; }
        public string name { get; set; }
        public string province_code { get; set; }
        public string country_code { get; set; }
        public string country_name { get; set; }
        public bool @default { get; set; }
    }

    
}
