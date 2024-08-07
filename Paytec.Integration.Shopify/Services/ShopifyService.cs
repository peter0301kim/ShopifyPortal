using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using Paytec.Integration.Shopify.Models;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Paytec.Integration.Shopify.Services
{
    public class ShopifyService : IShopifyService
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();
        private ShopifySettings ShopifySettings { get; set; }
        public ShopifyService(ShopifySettings shopifySettings)
        {
            this.ShopifySettings = shopifySettings;
        }

        public ApiReturnValue<ResponseModelProducts> GetAllProducts()
        {
            string destUri = ShopifySettings.ApiBaseUrl + ShopifySettings.ApiPath + ShopifySettings.ApiVersion
                + "/products.json";

            Log.Debug($"--- GetAllProducts - Uri:{destUri} ---");

            ApiReturnValue<ResponseModelProducts> apiReturnValue;

            try
            {
                //var client = new RestClient(destUri);
                //client.Authenticator = new HttpBasicAuthenticator(ShopifySettings.ApiKey, ShopifySettings.ApiAccessToken);

                var options = new RestClientOptions();
                options.Authenticator = new HttpBasicAuthenticator(ShopifySettings.ApiKey, ShopifySettings.ApiAccessToken);
                var client = new RestClient(options);

                var request = new RestRequest(destUri, Method.Get);

                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                //request.AddHeader("X-Shopify-Access-Token", ShopifySettings.ApiAccessToken);

                RestResponse response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    apiReturnValue = new ApiReturnValue<ResponseModelProducts>()
                    {
                        IsSuccess = true,
                        Object = JsonConvert.DeserializeObject<ResponseModelProducts>(response.Content),
                        ReturnMessage = null
                    };
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.BadRequest ||
                        response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        ApiReturnError apiReturnError = JsonConvert.DeserializeObject<ApiReturnError>(response.Content);

                        apiReturnValue = new ApiReturnValue<ResponseModelProducts>()
                        {
                            IsSuccess = false,Object = null,ReturnMessage = new ReturnMessage { Code = apiReturnError.Error.Code, Message = apiReturnError.Error.Message }
                        };
                    }
                    else
                    {
                        throw new Exception(response.Content.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return apiReturnValue;
        }

        public ApiReturnValue<ResponseModelCustomers> GetAllCustomers()
        {
            string destUri = ShopifySettings.ApiBaseUrl + ShopifySettings.ApiPath + ShopifySettings.ApiVersion 
                + "/customers.json";

            Log.Debug($"--- GetAllCustomers - Uri:{destUri} ---");

            ApiReturnValue<ResponseModelCustomers> apiReturnValue;

            try
            {
                //var client = new RestClient(destUri);
                //client.Authenticator = new HttpBasicAuthenticator(ShopifySettings.ApiKey, ShopifySettings.ApiAccessToken);

                var options = new RestClientOptions();
                options.Authenticator = new HttpBasicAuthenticator(ShopifySettings.ApiKey, ShopifySettings.ApiAccessToken);
                var client = new RestClient(options);

                var request = new RestRequest(destUri, Method.Get);

                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                //request.AddHeader("X-Shopify-Access-Token", ShopifySettings.ApiAccessToken);

                RestResponse response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    apiReturnValue = new ApiReturnValue<ResponseModelCustomers>()
                    {
                        IsSuccess = true,Object = JsonConvert.DeserializeObject<ResponseModelCustomers>(response.Content),ReturnMessage = null
                    };
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.BadRequest ||
                        response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        ApiReturnError apiReturnError = JsonConvert.DeserializeObject<ApiReturnError>(response.Content);

                        apiReturnValue = new ApiReturnValue<ResponseModelCustomers>()
                        {
                            IsSuccess = false, Object = null,
                            ReturnMessage = new ReturnMessage { Code = apiReturnError.Error.Code, Message = apiReturnError.Error.Message }
                        };
                    }
                    else
                    {
                        throw new Exception(response.Content.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return apiReturnValue;
        }

        public ApiReturnValue<ResponseModelCustomer> GetCustomerById(string customerId)
        {
            string destUri = ShopifySettings.ApiBaseUrl + ShopifySettings.ApiPath + ShopifySettings.ApiVersion 
            + $"/customers/{customerId}.json";

            Log.Debug($"--- GetCustomerById - Uri:{destUri}, customerId:{customerId} ---");

            ApiReturnValue<ResponseModelCustomer> apiReturnValue;

            try
            {
                //var client = new RestClient(destUri);
                //client.Authenticator = new HttpBasicAuthenticator(ShopifySettings.ApiKey, ShopifySettings.ApiAccessToken);

                var options = new RestClientOptions();
                options.Authenticator = new HttpBasicAuthenticator(ShopifySettings.ApiKey, ShopifySettings.ApiAccessToken);
                var client = new RestClient(options);

                var request = new RestRequest(destUri, Method.Get);

                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                //request.AddHeader("X-Shopify-Access-Token", ShopifySettings.ApiAccessToken);

                RestResponse response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    apiReturnValue = new ApiReturnValue<ResponseModelCustomer>()
                    {
                        IsSuccess = true,
                        Object = JsonConvert.DeserializeObject<ResponseModelCustomer>(response.Content),
                        ReturnMessage = null
                    };
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.BadRequest ||
                        response.StatusCode == HttpStatusCode.Unauthorized )
                    {
                        ApiReturnError apiReturnError = JsonConvert.DeserializeObject<ApiReturnError>(response.Content);

                        apiReturnValue = new ApiReturnValue<ResponseModelCustomer>()
                        {
                            IsSuccess = false,
                            Object = null,
                            ReturnMessage = new ReturnMessage { Code = apiReturnError.Error.Code, Message = apiReturnError.Error.Message }
                        };
                    } 
                    else if(response.StatusCode == HttpStatusCode.NotFound) //Data count is 0
                    {
                        apiReturnValue = new ApiReturnValue<ResponseModelCustomer>()
                        {
                            IsSuccess = false,
                            Object = null,
                            ReturnMessage = new ReturnMessage { Code = "", Message = response.Content } 
                        };
                    }
                    else
                    {
                        throw new Exception(response.Content.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return apiReturnValue;
        }


        public ApiReturnValue<ResponseModelCustomers> GetCustomerByEmail(string email)
        {
            string destUri = ShopifySettings.ApiBaseUrl + ShopifySettings.ApiPath + ShopifySettings.ApiVersion
                + "/customers/search.json";

            Log.Debug($"--- GetCustomerByEmail - Uri:{destUri} ---");

            ApiReturnValue<ResponseModelCustomers> apiReturnValue;

            try
            {
                //var client = new RestClient(destUri);
                //client.Authenticator = new HttpBasicAuthenticator(ShopifySettings.ApiKey, ShopifySettings.ApiAccessToken);

                var options = new RestClientOptions();
                options.Authenticator = new HttpBasicAuthenticator(ShopifySettings.ApiKey, ShopifySettings.ApiAccessToken);
                var client = new RestClient(options);

                var request = new RestRequest(destUri, Method.Get);

                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                //request.AddHeader("X-Shopify-Access-Token", ShopifySettings.ApiAccessToken);
                
                request.AddParameter("query", $"email:{email}");

                RestResponse response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    apiReturnValue = new ApiReturnValue<ResponseModelCustomers>()
                    {
                        IsSuccess = true,
                        Object = JsonConvert.DeserializeObject<ResponseModelCustomers>(response.Content),
                        ReturnMessage = null
                    };
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.BadRequest ||
                        response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        ApiReturnError apiReturnError = null;
                        if (response.Content.Contains("errors"))
                        {
                            apiReturnError = new ApiReturnError { Error = new ReturnError { Code = "", Message = response.Content } };
                        }
                        else
                        {
                            apiReturnError = JsonConvert.DeserializeObject<ApiReturnError>(response.Content);
                        }

                        apiReturnValue = new ApiReturnValue<ResponseModelCustomers>()
                        {
                            IsSuccess = false,Object = null,
                            ReturnMessage = new ReturnMessage { Code = apiReturnError.Error.Code, Message = apiReturnError.Error.Message }
                        };
                    }
                    else if (response.StatusCode == HttpStatusCode.NotFound) //Data count is 0
                    {
                        apiReturnValue = new ApiReturnValue<ResponseModelCustomers>()
                        {
                            IsSuccess = false,Object = null,
                            ReturnMessage = new ReturnMessage { Code = "", Message = response.Content }
                        };
                    }
                    else
                    {
                        throw new Exception(response.Content.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return apiReturnValue;
        }

        public ApiReturnValue<ResponseModelCustomer> AddCustomer( RequestModelCustomer customer)
        {
            string destUri = ShopifySettings.ApiBaseUrl + ShopifySettings.ApiPath + ShopifySettings.ApiVersion 
                + "/customers.json";
            Log.Debug($"--- AddCustomer - Uri:{destUri} ---");

            ApiReturnValue<ResponseModelCustomer> apiReturnValue;

            try
            {
                //var client = new RestClient(destUri);
                //client.Authenticator = new HttpBasicAuthenticator(ShopifySettings.ApiKey, ShopifySettings.ApiAccessToken);

                var options = new RestClientOptions();
                options.Authenticator = new HttpBasicAuthenticator(ShopifySettings.ApiKey, ShopifySettings.ApiAccessToken);
                var client = new RestClient(options);

                var request = new RestRequest(destUri, Method.Post);

                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                //request.AddHeader("X-Shopify-Access-Token", ShopifySettings.ApiAccessToken);

                request.AddJsonBody(customer);

                RestResponse response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.Created)
                {
                    apiReturnValue = new ApiReturnValue<ResponseModelCustomer>()
                    {
                        IsSuccess = true,
                        Object = JsonConvert.DeserializeObject<ResponseModelCustomer>(response.Content),
                        ReturnMessage = null
                    };
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.BadRequest ||
                        response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        ApiReturnError apiReturnError = JsonConvert.DeserializeObject<ApiReturnError>(response.Content);

                        apiReturnValue = new ApiReturnValue<ResponseModelCustomer>()
                        {
                            IsSuccess = false, Object = null,
                            ReturnMessage = new ReturnMessage { Code = apiReturnError.Error.Code, Message = apiReturnError.Error.Message }
                        };
                    }
                    else if (response.StatusCode == HttpStatusCode.NotFound) //Data count is 0
                    {
                        apiReturnValue = new ApiReturnValue<ResponseModelCustomer>()
                        {
                            IsSuccess = false,Object = null,
                            ReturnMessage = new ReturnMessage { Code = "", Message = response.Content } 
                        };
                    }
                    else
                    {
                        if (response.Content.Contains("email"))
                        {
                            dynamic d = JObject.Parse(response.Content);//"{\"errors\":{\"email\":[\"contains an invalid domain name\"]}}"
                            string errMsg = d.errors.email[0];

                            apiReturnValue = new ApiReturnValue<ResponseModelCustomer>()
                            {
                                IsSuccess = false,
                                Object = null,
                                ReturnMessage = new ReturnMessage { Code = $"{response.StatusCode}", Message = $"Email {errMsg}" } 
                            };

                        }
                        else if (response.Content.Contains("phone")) 
                        {
                            dynamic d = JObject.Parse(response.Content);//"{\"errors\":{\"phone\":[\"contains an invalid domain name\"]}}"
                            string errMsg = d.errors.phone[0];

                            apiReturnValue = new ApiReturnValue<ResponseModelCustomer>()
                            {
                                IsSuccess = false,
                                Object = null,
                                ReturnMessage = new ReturnMessage { Code = $"{response.StatusCode}", Message = $"Email {errMsg}" } 
                            };
                        }
                        else
                        {
                            throw new Exception(response.Content.ToString());
                        }                        
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return apiReturnValue;
        }

        public ApiReturnValue<ResponseModelCustomer> UpdateCustomer( RequestModelCustomer rCustomer)
        {
            // Put,     ../admin/api/2023-07/customers/{customerID}.json
            string destUri = ShopifySettings.ApiBaseUrl + ShopifySettings.ApiPath + ShopifySettings.ApiVersion
                            + $"/customers/{rCustomer.customer.id}.json";

            Log.Debug($"--- UpdateCustomer - Uri:{destUri} ---");

            ApiReturnValue<ResponseModelCustomer> apiReturnValue;

            try
            {
                //var client = new RestClient(destUri);
                //client.Authenticator = new HttpBasicAuthenticator(ShopifySettings.ApiKey, ShopifySettings.ApiAccessToken);

                var options = new RestClientOptions();
                options.Authenticator = new HttpBasicAuthenticator(ShopifySettings.ApiKey, ShopifySettings.ApiAccessToken);
                var client = new RestClient(options);

                var request = new RestRequest(destUri, Method.Put);

                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                //request.AddHeader("X-Shopify-Access-Token", ShopifySettings.ApiAccessToken);

                request.AddJsonBody( 
                    new
                    {
                        customer = new
                        {
                            id = rCustomer.customer.id,
                            first_name = rCustomer.customer.first_name,
                            last_name = rCustomer.customer.last_name,
                            tags = rCustomer.customer.tags,
                            note = rCustomer.customer.note
                        }
                    });

                RestResponse response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    apiReturnValue = new ApiReturnValue<ResponseModelCustomer>()
                    {
                        IsSuccess = true,
                        Object = JsonConvert.DeserializeObject<ResponseModelCustomer>(response.Content),
                        ReturnMessage = null
                    };
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.BadRequest ||
                        response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        ApiReturnError apiReturnError = JsonConvert.DeserializeObject<ApiReturnError>(response.Content);

                        apiReturnValue = new ApiReturnValue<ResponseModelCustomer>()
                        {
                            IsSuccess = false,Object = null,
                            ReturnMessage = new ReturnMessage { Code = apiReturnError.Error.Code, Message = apiReturnError.Error.Message }
                        };
                    }
                    else if (response.StatusCode == HttpStatusCode.NotFound) //Data count is 0
                    {
                        apiReturnValue = new ApiReturnValue<ResponseModelCustomer>()
                        {
                            IsSuccess = false,Object = null,
                            ReturnMessage = new ReturnMessage {  Code = "", Message = response.Content } 
                        };
                    }
                    else
                    {
                        throw new Exception(response.Content.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return apiReturnValue;
        }



        public ApiReturnValue<ResponseModelCustomerAddresses> GetCustomerAddresses(string customerID)
        {
            ///admin/api/2023-10/customers/207119551/addresses.json
            string destUri = ShopifySettings.ApiBaseUrl + ShopifySettings.ApiPath + ShopifySettings.ApiVersion
                + $"/customers/{customerID}/addresses.json";

            Log.Debug($"--- GetCustomerAddresses - Uri:{destUri} ---");

            ApiReturnValue<ResponseModelCustomerAddresses> apiReturnValue;

            try
            {
                //var client = new RestClient(destUri);
                //client.Authenticator = new HttpBasicAuthenticator(ShopifySettings.ApiKey, ShopifySettings.ApiAccessToken);

                var options = new RestClientOptions();
                options.Authenticator = new HttpBasicAuthenticator(ShopifySettings.ApiKey, ShopifySettings.ApiAccessToken);
                var client = new RestClient(options);

                var request = new RestRequest(destUri, Method.Get);

                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                //request.AddParameter("limit", "1");
                //request.AddHeader("X-Shopify-Access-Token", ShopifySettings.ApiAccessToken);

                RestResponse response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    apiReturnValue = new ApiReturnValue<ResponseModelCustomerAddresses>()
                    {
                        IsSuccess = true,
                        Object = JsonConvert.DeserializeObject<ResponseModelCustomerAddresses>(response.Content),
                        ReturnMessage = null
                    };
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.BadRequest ||
                        response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        ApiReturnError apiReturnError = JsonConvert.DeserializeObject<ApiReturnError>(response.Content);

                        apiReturnValue = new ApiReturnValue<ResponseModelCustomerAddresses>()
                        {
                            IsSuccess = false,
                            Object = null,
                            ReturnMessage = new ReturnMessage { Code = apiReturnError.Error.Code, Message = apiReturnError.Error.Message }
                        };
                    }
                    else
                    {
                        throw new Exception(response.Content.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return apiReturnValue;
        }


        public ApiReturnValue<ResponseModelOrders> GetOrders()
        {
            string destUri = ShopifySettings.ApiBaseUrl + ShopifySettings.ApiPath + ShopifySettings.ApiVersion
                + "/orders.json";

            //need to support pagination.

            Log.Debug($"--- GetOrders - Uri:{destUri} ---");

            ApiReturnValue<ResponseModelOrders> apiReturnValue;

            try
            {
                //var client = new RestClient(destUri);
                //client.Authenticator = new HttpBasicAuthenticator(ShopifySettings.ApiKey, ShopifySettings.ApiAccessToken);

                var options = new RestClientOptions();
                options.Authenticator = new HttpBasicAuthenticator(ShopifySettings.ApiKey, ShopifySettings.ApiAccessToken);
                var client = new RestClient(options);

                var request = new RestRequest(destUri, Method.Get);

                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                //request.AddHeader("X-Shopify-Access-Token", ShopifySettings.ApiAccessToken);

                request.AddParameter("status", "any");
                request.AddParameter("limit", "100");

                RestResponse response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    apiReturnValue = new ApiReturnValue<ResponseModelOrders>()
                    {
                        IsSuccess = true,
                        Object = JsonConvert.DeserializeObject<ResponseModelOrders>(response.Content),
                        ReturnMessage = null
                    };
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.BadRequest ||
                        response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        ApiReturnError apiReturnError = JsonConvert.DeserializeObject<ApiReturnError>(response.Content);

                        apiReturnValue = new ApiReturnValue<ResponseModelOrders>()
                        {
                            IsSuccess = false,Object = null,
                            ReturnMessage = new ReturnMessage { Code = apiReturnError.Error.Code, Message = apiReturnError.Error.Message }
                        };
                    }
                    else if (response.StatusCode == HttpStatusCode.NotFound) //Data count is 0
                    {
                        apiReturnValue = new ApiReturnValue<ResponseModelOrders>()
                        {
                            IsSuccess = false,
                            Object = null,
                            ReturnMessage = new ReturnMessage { Code = "", Message = response.Content } 
                        };
                    }
                    else
                    {
                        throw new Exception(response.Content.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return apiReturnValue;
        }

        public ApiReturnValue<ResponseModelOrders> GetOrdersByName(string orderName)
        {
            string destUri = ShopifySettings.ApiBaseUrl + ShopifySettings.ApiPath + ShopifySettings.ApiVersion
                + "/orders.json";
            //need to support pagination.

            Log.Debug($"--- AddOrder - Uri:{destUri} ---");

            ApiReturnValue<ResponseModelOrders> apiReturnValue;

            try
            {
                //var client = new RestClient(destUri);
                //client.Authenticator = new HttpBasicAuthenticator(ShopifySettings.ApiKey, ShopifySettings.ApiAccessToken);

                var options = new RestClientOptions();
                options.Authenticator = new HttpBasicAuthenticator(ShopifySettings.ApiKey, ShopifySettings.ApiAccessToken);
                var client = new RestClient(options);

                var request = new RestRequest(destUri, Method.Get);

                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                //request.AddHeader("X-Shopify-Access-Token", ShopifySettings.ApiAccessToken);

                request.AddParameter("status", "any");
                request.AddParameter("fields", "id,name");
                request.AddParameter("name", orderName);

                RestResponse response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    apiReturnValue = new ApiReturnValue<ResponseModelOrders>()
                    {
                        IsSuccess = true,
                        Object = JsonConvert.DeserializeObject<ResponseModelOrders>(response.Content),
                        ReturnMessage = null
                    };
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.BadRequest ||
                        response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        ApiReturnError apiReturnError = JsonConvert.DeserializeObject<ApiReturnError>(response.Content);

                        apiReturnValue = new ApiReturnValue<ResponseModelOrders>()
                        {
                            IsSuccess = false,Object = null,
                            ReturnMessage = new ReturnMessage { Code = apiReturnError.Error.Code, Message = apiReturnError.Error.Message }
                        };
                    }
                    else if (response.StatusCode == HttpStatusCode.NotFound) //Data count is 0
                    {
                        apiReturnValue = new ApiReturnValue<ResponseModelOrders>()
                        {
                            IsSuccess = false,Object = null,
                            ReturnMessage = new ReturnMessage { Code = "", Message = response.Content } 
                        };
                    }
                    else
                    {
                        throw new Exception(response.Content.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return apiReturnValue;
        }

        public ApiReturnValue<ResponseModelOrders> GetOrdersByLastModified(DateTime lastModifiedEDT)
        {
            string destUri = ShopifySettings.ApiBaseUrl + ShopifySettings.ApiPath + ShopifySettings.ApiVersion
                + "/orders.json";
            //need to support pagination.

            Log.Debug($"--- AddOrder - Uri:{destUri} ---");

            ApiReturnValue<ResponseModelOrders> apiReturnValue;

            try
            {
                //var client = new RestClient(destUri);
                //client.Authenticator = new HttpBasicAuthenticator(ShopifySettings.ApiKey, ShopifySettings.ApiAccessToken);

                var options = new RestClientOptions();
                options.Authenticator = new HttpBasicAuthenticator(ShopifySettings.ApiKey, ShopifySettings.ApiAccessToken);
                var client = new RestClient(options);

                var request = new RestRequest(destUri, Method.Get);

                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                //request.AddHeader("X-Shopify-Access-Token", ShopifySettings.ApiAccessToken);

                request.AddParameter("updated_at_min", lastModifiedEDT.ToString("O")); //DateTime format : ISO8601

                RestResponse response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    apiReturnValue = new ApiReturnValue<ResponseModelOrders>()
                    {
                        IsSuccess = true,
                        Object = JsonConvert.DeserializeObject<ResponseModelOrders>(response.Content),
                        ReturnMessage = null
                    };
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.BadRequest ||
                        response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        ApiReturnError apiReturnError = JsonConvert.DeserializeObject<ApiReturnError>(response.Content);

                        apiReturnValue = new ApiReturnValue<ResponseModelOrders>()
                        {
                            IsSuccess = false,Object = null,
                            ReturnMessage = new ReturnMessage { Code = apiReturnError.Error.Code, Message = apiReturnError.Error.Message }
                        };
                    }
                    else if (response.StatusCode == HttpStatusCode.NotFound) //Data count is 0
                    {
                        apiReturnValue = new ApiReturnValue<ResponseModelOrders>()
                        {
                            IsSuccess = false,Object = null,ReturnMessage = new ReturnMessage { Code = "", Message = response.Content }
                        };
                    }
                    else
                    {
                        throw new Exception(response.Content.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return apiReturnValue;
        }


        public ApiReturnValue<ResponseModelOrder> AddOrder(RequestModelOrder order)
        {
            string destUri = ShopifySettings.ApiBaseUrl + ShopifySettings.ApiPath + ShopifySettings.ApiVersion
                + "/orders.json";

            Log.Debug($"--- AddOrder - Uri:{destUri} ---");

            ApiReturnValue<ResponseModelOrder> apiReturnValue;

            try
            {
                //var client = new RestClient(destUri);
                //client.Authenticator = new HttpBasicAuthenticator(ShopifySettings.ApiKey, ShopifySettings.ApiAccessToken);

                var options = new RestClientOptions();
                options.Authenticator = new HttpBasicAuthenticator(ShopifySettings.ApiKey, ShopifySettings.ApiAccessToken);
                var client = new RestClient(options);

                var request = new RestRequest(destUri, Method.Post);

                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                //request.AddHeader("X-Shopify-Access-Token", ShopifySettings.ApiAccessToken);

                //var jsonOrder = JsonConvert.SerializeObject(order);

                request.AddJsonBody(order);

                RestResponse response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.Created)
                {
                    apiReturnValue = new ApiReturnValue<ResponseModelOrder>()
                    {
                        IsSuccess = true,
                        Object = JsonConvert.DeserializeObject<ResponseModelOrder>(response.Content),
                        ReturnMessage = null
                    };
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.BadRequest ||
                        response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        ApiReturnError apiReturnError = JsonConvert.DeserializeObject<ApiReturnError>(response.Content);

                        apiReturnValue = new ApiReturnValue<ResponseModelOrder>()
                        {
                            IsSuccess = false,Object = null,
                            ReturnMessage = new ReturnMessage { Code = apiReturnError.Error.Code, Message = apiReturnError.Error.Message }
                        };
                    }
                    else if (response.StatusCode == HttpStatusCode.NotFound) //Data count is 0
                    {
                        apiReturnValue = new ApiReturnValue<ResponseModelOrder>()
                        {
                            IsSuccess = false,Object = null,ReturnMessage = new ReturnMessage { Code = "", Message = response.Content } 
                        };
                    }
                    else
                    {
                        throw new Exception(response.Content.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return apiReturnValue;
        }


        public ApiReturnValue<ResponseModelOrder> Fulfillment(string orderNumber, RequestModelFulfillment fulfillment)
        {
            string destUri = ShopifySettings.ApiBaseUrl + ShopifySettings.ApiPath + ShopifySettings.ApiVersion
                + $"/orders/{orderNumber}/fulfillments.json";

            Log.Debug($"--- Fulfillment - Uri:{destUri} ---");

            ApiReturnValue<ResponseModelOrder> apiReturnValue;

            try
            {
                //var client = new RestClient(destUri);
                //client.Authenticator = new HttpBasicAuthenticator(ShopifySettings.ApiKey, ShopifySettings.ApiAccessToken);

                var options = new RestClientOptions();
                options.Authenticator = new HttpBasicAuthenticator(ShopifySettings.ApiKey, ShopifySettings.ApiAccessToken);
                var client = new RestClient(options);

                var request = new RestRequest(destUri, Method.Post);

                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                //request.AddHeader("X-Shopify-Access-Token", ShopifySettings.ApiAccessToken);

                request.AddJsonBody(fulfillment);

                RestResponse response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.Created)
                {
                    apiReturnValue = new ApiReturnValue<ResponseModelOrder>()
                    {
                        IsSuccess = true,
                        Object = JsonConvert.DeserializeObject<ResponseModelOrder>(response.Content),
                        ReturnMessage = null
                    };
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.BadRequest ||
                        response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        ApiReturnError apiReturnError = JsonConvert.DeserializeObject<ApiReturnError>(response.Content);

                        apiReturnValue = new ApiReturnValue<ResponseModelOrder>()
                        {
                            IsSuccess = false,
                            Object = null,
                            ReturnMessage = new ReturnMessage { Code = apiReturnError.Error.Code, Message = apiReturnError.Error.Message } 
                        };
                    }
                    else if (response.StatusCode == HttpStatusCode.NotFound) //Data count is 0
                    {
                        apiReturnValue = new ApiReturnValue<ResponseModelOrder>()
                        {
                            IsSuccess = false,
                            Object = null,
                            ReturnMessage = new ReturnMessage { Code = "", Message = response.Content } 
                        };
                    }
                    else
                    {
                        throw new Exception(response.Content.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return apiReturnValue;
        }
    }
}
