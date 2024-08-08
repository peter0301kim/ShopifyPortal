using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peter.Integration.NotificationWebAPI.Models
{
    public class EmailNotification
    {
        public string EmailNotificationID { get; set; } = string.Empty;
        public string EmailFrom { get; set; } = string.Empty;
        public List<string> EmailTo { get; set; } = new List<string>();
        public string Subject { get; set;} = string.Empty;
        public string Contents { get; set;} = string.Empty;
        public string Status { get; set; } = string.Empty; //Ready, Sent
        public DateTime CreatedDate { get; set; } = new DateTime(1900, 01, 01);
        public DateTime? SentDate { get; set; }

    }
}
