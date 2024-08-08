using Newtonsoft.Json;
using NLog;
using Peter.Integration.NotificationWebAPI.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Peter.Integration.NotificationWebAPI.Services;

public class EmailNotificationWebApiService : IEmailNotificationWebApiService
{
    private static Logger Log = LogManager.GetCurrentClassLogger();

    public NotificationWebApiConnectionSettings NotificationWebApiConnectionSettings { get; set; }
    public EmailNotificationWebApiService(NotificationWebApiConnectionSettings notificationWebApiConnectionSettings)
    {
        this.NotificationWebApiConnectionSettings = notificationWebApiConnectionSettings;
    }


    public async Task<ApiReturnValue<List<EmailNotification>>> GetEmailNotificationsByStatus(string status)
    {
        Log.Debug($"--- GetPaytecEmailNotificationsByStatus - baseUrl:{NotificationWebApiConnectionSettings.Api_BaseUrl}" +
                                     $"/{NotificationWebApiConnectionSettings.Api_EndPoint_GetEmailNotificationsToSend}");


        ApiReturnValue<List<EmailNotification>> apiReturnValue = new ApiReturnValue<List<EmailNotification>>();


        try
        {
            var client = new RestClient(NotificationWebApiConnectionSettings.Api_BaseUrl);
            var request = new RestRequest(NotificationWebApiConnectionSettings.Api_EndPoint_GetEmailNotificationsToSend, Method.Post);

            request.AddHeader("Content-Type", "application/json");

            string authHeader = Convert.ToBase64String(
                Encoding.Default.GetBytes($"{NotificationWebApiConnectionSettings.ApiCredential.UserName}:{NotificationWebApiConnectionSettings.ApiCredential.Password}")
                );
            request.AddHeader("Authorization", $"Basic {authHeader}");

            request.RequestFormat = RestSharp.DataFormat.Json;

            var requestModel = new GetEmailNotificationsRequestModel
            {
                Status = "Ready"
            };

            request.AddJsonBody(requestModel);

            RestResponse response = await client.ExecuteAsync(request);


            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(response.Content.ToString());
            }

            apiReturnValue = JsonConvert.DeserializeObject<ApiReturnValue<List<EmailNotification>>>(response.Content);

        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }

        return apiReturnValue;
    }

    public async Task<ApiReturnValue<List<EmailNotification>>> GetEmailNotificationsToSend(string status)
    {
        Log.Debug($"--- GetPaytecEmailNotificationsByStatus - baseUrl:{NotificationWebApiConnectionSettings.Api_BaseUrl}" +
                                     $"/{NotificationWebApiConnectionSettings.Api_EndPoint_GetEmailNotificationsToSend}");


        ApiReturnValue<List<EmailNotification>> apiReturnValue = new ApiReturnValue<List<EmailNotification>>();


        try
        {
            var client = new RestClient(NotificationWebApiConnectionSettings.Api_BaseUrl);
            var request = new RestRequest(NotificationWebApiConnectionSettings.Api_EndPoint_GetEmailNotificationsToSend, Method.Post);

            request.AddHeader("Content-Type", "application/json");

            string authHeader = Convert.ToBase64String(
                Encoding.Default.GetBytes($"{NotificationWebApiConnectionSettings.ApiCredential.UserName}:{NotificationWebApiConnectionSettings.ApiCredential.Password}")
                );
            request.AddHeader("Authorization", $"Basic {authHeader}");

            request.RequestFormat = RestSharp.DataFormat.Json;

            var requestModel = new GetEmailNotificationsRequestModel
            {
                Status = status
            };

            request.AddJsonBody(requestModel);

            RestResponse response = await client.ExecuteAsync(request);


            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(response.Content.ToString());
            }

            apiReturnValue = JsonConvert.DeserializeObject<ApiReturnValue<List<EmailNotification>>>(response.Content);

        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }

        return apiReturnValue;
    }

    public async Task<ApiReturnValue<EmailNotification>> AddEmailNotification(string emailFrom, List<string> emailTo, string subject, string contents)
    {
        Log.Debug($"--- AddEmailNotification - baseUrl:{NotificationWebApiConnectionSettings.Api_BaseUrl}" +
                                     $"{NotificationWebApiConnectionSettings.Api_EndPoint_AddEmailNotification}");

        ApiReturnValue<EmailNotification> apiReturnValue = new ApiReturnValue<EmailNotification>();

        try
        {
            var client = new RestClient(NotificationWebApiConnectionSettings.Api_BaseUrl);
            var request = new RestRequest(NotificationWebApiConnectionSettings.Api_EndPoint_AddEmailNotification, Method.Post);

            request.AddHeader("Content-Type", "application/json");

            string authHeader = Convert.ToBase64String(
                Encoding.Default.GetBytes($"{NotificationWebApiConnectionSettings.ApiCredential.UserName}:{NotificationWebApiConnectionSettings.ApiCredential.Password}")
                );
            request.AddHeader("Authorization", $"Basic {authHeader}");

            request.RequestFormat = RestSharp.DataFormat.Json;

            var requestModel = new AddEmailNotificationRequestModel
            {
                EmailFrom = emailFrom,
                EmailTo = emailTo,
                Subject = subject,
                Contents = contents
            };

            request.AddJsonBody(requestModel);

            RestResponse response = await client.ExecuteAsync(request);


            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(response.Content.ToString());
            }

            apiReturnValue = JsonConvert.DeserializeObject<ApiReturnValue<EmailNotification>>(response.Content);

        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }

        return apiReturnValue;
    }


    public async Task<ApiReturnValue<bool>> UpdateEmailNotificationSendResult(string emailNotificationID, bool isSuccess, string errorMessage)
    {
        Log.Debug($"--- AddEmailNotification - baseUrl:{NotificationWebApiConnectionSettings.Api_BaseUrl}" +
                                     $"/{NotificationWebApiConnectionSettings.Api_EndPoint_UpdateEmailNotificationSendResult}");

        ApiReturnValue<bool> apiReturnValue = new ApiReturnValue<bool>();

        try
        {
            var client = new RestClient(NotificationWebApiConnectionSettings.Api_BaseUrl);
            var request = new RestRequest(NotificationWebApiConnectionSettings.Api_EndPoint_UpdateEmailNotificationSendResult, Method.Post);

            request.AddHeader("Content-Type", "application/json");

            string authHeader = Convert.ToBase64String(
                Encoding.Default.GetBytes($"{NotificationWebApiConnectionSettings.ApiCredential.UserName}:{NotificationWebApiConnectionSettings.ApiCredential.Password}")
                );
            request.AddHeader("Authorization", $"Basic {authHeader}");

            request.RequestFormat = RestSharp.DataFormat.Json;

            var requestModel = new UpdateEmailNotificationSendResultRequestModel
            {
                EmailNotificationID = emailNotificationID,
                IsSuccess = isSuccess,
                ErrorMessage = errorMessage
            };

            request.AddJsonBody(requestModel);

            RestResponse response = await client.ExecuteAsync(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(response.Content.ToString());
            }

            apiReturnValue = JsonConvert.DeserializeObject<ApiReturnValue<bool>>(response.Content);

        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }

        return apiReturnValue;
    }

}