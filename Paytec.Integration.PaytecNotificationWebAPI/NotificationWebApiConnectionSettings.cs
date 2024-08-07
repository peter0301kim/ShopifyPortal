using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paytec.Integration.PaytecNotificationWebAPI;

public class NotificationWebApiConnectionSettings
{
    public string Api_BaseUrl { get; set; } = string.Empty;
    public string Api_EndPoint_GetEmailNotificationsToSend { get; set;} = string.Empty;
    public string Api_EndPoint_AddEmailNotification { get; set; } = string.Empty;
    public string Api_EndPoint_UpdateEmailNotificationSendResult { get; set; } = string.Empty;


    public ApiCredential ApiCredential { get; set; } = new ApiCredential();

}

public class ApiCredential
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LogInUserProfile
{
    public string UserName { get; set; } = string.Empty;
    public string UserFullName { get; set; } = string.Empty;
}
