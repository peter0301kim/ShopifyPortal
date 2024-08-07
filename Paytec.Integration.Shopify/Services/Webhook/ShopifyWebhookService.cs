using Newtonsoft.Json;
using NLog;
using Paytec.Integration.Shopify.Models;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Paytec.Integration.Shopify.Services.Webhook
{
    public class ShopifyWebhookService : IShopifyWebhookService
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();

        public ApiReturnValue<ShopifyWebhookResponseMulti> GetWebhooks(string destUri, string apiKey, string apiPassword)
        {
            Log.Debug($"--- GetWebhooks - Uri:{destUri} ---");

            ApiReturnValue<ShopifyWebhookResponseMulti> apiReturnValue;

            try
            {
                //var client = new RestClient(destUri);
                //client.Authenticator = new HttpBasicAuthenticator(apiKey, apiPassword);

                var options = new RestClientOptions();
                options.Authenticator = new HttpBasicAuthenticator(apiKey, apiPassword);
                var client = new RestClient(options);

                var request = new RestRequest(destUri, Method.Get);

                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                //request.AddHeader("App-Auth-Token", token);

                RestResponse response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    apiReturnValue = new ApiReturnValue<ShopifyWebhookResponseMulti>()
                    {
                        IsSuccess = true,
                        Object = JsonConvert.DeserializeObject<ShopifyWebhookResponseMulti>(response.Content),
                        ReturnMessage = null
                    };
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.BadRequest ||
                        response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        ApiReturnError apiReturnError = JsonConvert.DeserializeObject<ApiReturnError>(response.Content);

                        apiReturnValue = new ApiReturnValue<ShopifyWebhookResponseMulti>()
                        {
                            IsSuccess = false,Object = null,
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

        public ApiReturnValue<ShopifyWebhookResponseMulti> GetWebhookByWebhookId(string destUri, string apiKey, string apiPassword, string webhookId)
        {
            Log.Debug($"--- GetWebhooks - Uri:{destUri} ---");

            destUri = destUri + $"/{webhookId}.json";

            ApiReturnValue<ShopifyWebhookResponseMulti> apiReturnValue;

            try
            {
                //var client = new RestClient(destUri);
                //client.Authenticator = new HttpBasicAuthenticator(apiKey, apiPassword);

                var options = new RestClientOptions();
                options.Authenticator = new HttpBasicAuthenticator(apiKey, apiPassword);
                var client = new RestClient(options);

                var request = new RestRequest(destUri, Method.Get);

                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                //request.AddHeader("App-Auth-Token", token);

                RestResponse response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    apiReturnValue = new ApiReturnValue<ShopifyWebhookResponseMulti>()
                    {
                        IsSuccess = true,
                        Object = JsonConvert.DeserializeObject<ShopifyWebhookResponseMulti>(response.Content),
                        ReturnMessage = null
                    };
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.BadRequest ||
                        response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        ApiReturnError apiReturnError = JsonConvert.DeserializeObject<ApiReturnError>(response.Content);

                        apiReturnValue = new ApiReturnValue<ShopifyWebhookResponseMulti>()
                        {
                            IsSuccess = false,Object = null,
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

        public ApiReturnValue<ShopifyWebhookResponseSingle> CreateWebhook(string destUri, string apiKey, string apiPassword, ShopifyWebhookRequest webhookRequest)
        {
            Log.Debug($"--- CreateWebhook - destUri:{destUri} ---");

            ApiReturnValue<ShopifyWebhookResponseSingle> apiReturnValue;

            try
            {
                //var client = new RestClient(destUri);
                //client.Authenticator = new HttpBasicAuthenticator(apiKey, apiPassword);

                var options = new RestClientOptions();
                options.Authenticator = new HttpBasicAuthenticator(apiKey, apiPassword);
                var client = new RestClient(options);

                var request = new RestRequest(destUri, Method.Post);

                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", JsonConvert.SerializeObject(webhookRequest), ParameterType.RequestBody);

                RestResponse response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.Created)
                {
                    apiReturnValue = new ApiReturnValue<ShopifyWebhookResponseSingle>()
                    {
                        IsSuccess = true,
                        Object = JsonConvert.DeserializeObject<ShopifyWebhookResponseSingle>(response.Content),
                        ReturnMessage = null
                    };
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.BadRequest ||
                        response.StatusCode == HttpStatusCode.Unauthorized ||
                        response.StatusCode == HttpStatusCode.UnprocessableEntity)
                    {
                        ApiReturnError apiReturnError = JsonConvert.DeserializeObject<ApiReturnError>(response.Content);

                        apiReturnValue = new ApiReturnValue<ShopifyWebhookResponseSingle>()
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

        public ApiReturnValue<ShopifyWebhookResponseSingle> UpdateWebhook(string destUri, string apiKey, string apiPassword, string webhookId, ShopifyWebhookRequest webhookRequest)
        {
            Log.Debug($"--- UpdateWebhook - destUri:{destUri} ---");

            destUri = destUri + $"/{webhookId}.json";

            ApiReturnValue<ShopifyWebhookResponseSingle> apiReturnValue;

            try
            {
                //var client = new RestClient(destUri);
                //client.Authenticator = new HttpBasicAuthenticator(apiKey, apiPassword);

                var options = new RestClientOptions();
                options.Authenticator = new HttpBasicAuthenticator(apiKey, apiPassword);
                var client = new RestClient(options);

                var request = new RestRequest(destUri, Method.Put);

                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", JsonConvert.SerializeObject(webhookRequest), ParameterType.RequestBody);

                RestResponse response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    apiReturnValue = new ApiReturnValue<ShopifyWebhookResponseSingle>()
                    {
                        IsSuccess = true,
                        Object = JsonConvert.DeserializeObject<ShopifyWebhookResponseSingle>(response.Content),
                        ReturnMessage = null
                    };
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.BadRequest ||
                        response.StatusCode == HttpStatusCode.Unauthorized ||
                        response.StatusCode == HttpStatusCode.UnprocessableEntity)
                    {
                        WebhookApiReturnError webhookApiReturnError = JsonConvert.DeserializeObject<WebhookApiReturnError>(response.Content);

                        ApiReturnError returnError = new ApiReturnError();

                        apiReturnValue = new ApiReturnValue<ShopifyWebhookResponseSingle>()
                        {
                            IsSuccess = false, Object = null,
                            ReturnMessage = new ReturnMessage() { Code = "Error", Message = string.Join(',', webhookApiReturnError.Errors.Format) }
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

        public ApiReturnValue<string> DeleteWebhook(string destUri, string apiKey, string apiPassword, string webhookId)
        {

            Log.Debug($"--- UpdateWebhook - destUri:{destUri} ---");

            destUri = destUri + $"/{webhookId}.json";

            ApiReturnValue<string> apiReturnValue;

            try
            {
                //var client = new RestClient(destUri);
                //client.Authenticator = new HttpBasicAuthenticator(apiKey, apiPassword);

                var options = new RestClientOptions();
                options.Authenticator = new HttpBasicAuthenticator(apiKey, apiPassword);
                var client = new RestClient(options);

                var request = new RestRequest(destUri, Method.Delete);

                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");

                RestResponse response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    apiReturnValue = new ApiReturnValue<string>()
                    {
                        IsSuccess = true,Object = response.Content,ReturnMessage = null
                    };
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.BadRequest ||
                        response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        ApiReturnError apiReturnError = JsonConvert.DeserializeObject<ApiReturnError>(response.Content);

                        apiReturnValue = new ApiReturnValue<string>()
                        {
                            IsSuccess = false,Object = null,
                            ReturnMessage = new ReturnMessage { Code = apiReturnError.Error.Code, Message = apiReturnError.Error.Message }                     };
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
