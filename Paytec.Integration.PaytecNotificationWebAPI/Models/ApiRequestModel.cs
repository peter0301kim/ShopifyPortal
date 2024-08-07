using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paytec.Integration.PaytecNotificationWebAPI.Models
{
    public class GetEmailNotificationsRequestModel
    {
        public string Status { get; set; }
    }

    public class AddEmailNotificationRequestModel
    {
        public string EmailFrom { get; set; } = string.Empty;
        public List<string> EmailTo { get; set; } = new List<string>();
        public string Subject { get; set; } = string.Empty;
        public string Contents { get; set; } = string.Empty;
    }

    public class UpdateEmailNotificationSendResultRequestModel
    {
        public string EmailNotificationID { get; set; } = string.Empty;
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }

}
