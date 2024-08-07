﻿using Paytec.Integration.PaytecNotificationWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paytec.Integration.PaytecNotificationWebAPI.Services;

public interface IEmailNotificationWebApiService
{
    Task<ApiReturnValue<List<EmailNotification>>> GetEmailNotificationsByStatus(string status);
    Task<ApiReturnValue<List<EmailNotification>>> GetEmailNotificationsToSend(string status);
    Task<ApiReturnValue<EmailNotification>> AddEmailNotification(string emailFrom, List<string> emailTo, string subject, string contents);
    Task<ApiReturnValue<bool>> UpdateEmailNotificationSendResult(string emailNotificationID, bool isSuccess, string errorMessage);
}

