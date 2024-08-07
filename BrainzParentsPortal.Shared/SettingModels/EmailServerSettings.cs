using BrainzParentsPortal.Integration.PortalDb;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainzParentsPortal.Shared.SettingModels
{
    public class EmailServerSettings
    {
        public string EmailServerHost { get; set; } = string.Empty;
        public int EmailServerPort { get; set; } = 0;
        public string EmailServerAccount { get; set; } = string.Empty;
        public string EmailServerPassword { get; set; } = string.Empty;
        public string EmailServerEncryption { get; set; } = string.Empty;

        public EmailServerSettings(string settingsPathFile)
        {
            if (File.Exists(settingsPathFile))
            {
                var settings = JsonConvert.DeserializeObject<EmailServerSettings>(File.ReadAllText(settingsPathFile));

                EmailServerHost = settings.EmailServerHost;
                EmailServerPort = settings.EmailServerPort;
                EmailServerAccount = settings.EmailServerAccount;
                EmailServerPassword = settings.EmailServerPassword;
                EmailServerEncryption = settings.EmailServerEncryption;
            }
        }
    }
}
