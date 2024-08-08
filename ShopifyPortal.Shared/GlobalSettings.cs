using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyPortal.Shared
{
    public class GlobalSettings
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();

        public string AppBaseDir { get; set; } = string.Empty;
        public string PortalDbConnectionSettingsPathFile { get; set; } = string.Empty;
        public string ShopifySettingsPathFile { get; set; } = string.Empty;
        public string EmailServerSettingsPathFile { get; set; } = string.Empty;
        public string ShopifyToPortalDbSettingsPathFile { get; set; } = string.Empty;
        public string BrainzParentsPortalSettingsPathFile { get; set; } = string.Empty;
        public string NewzealandPostCodePathFile { get; set; } = string.Empty;
        public string NotificationWebApiSettingsPathFile { get; set; } = string.Empty;
        public static GlobalSettings Instance { get; } = new GlobalSettings();
        public GlobalSettings()
        {
            AppBaseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            PortalDbConnectionSettingsPathFile = Path.Combine(AppBaseDir, "Configs", "PortalDbConnectionSettings.json");
            ShopifySettingsPathFile = Path.Combine(AppBaseDir, "Configs", "ShopifySettings.json");
            EmailServerSettingsPathFile = Path.Combine(AppBaseDir, "Configs", "EmailServerSettings.json");
            ShopifyToPortalDbSettingsPathFile = Path.Combine(AppBaseDir, "Configs", "ShopifyToPortalDbSettings.json");
            BrainzParentsPortalSettingsPathFile = Path.Combine(AppBaseDir, "Configs", "BrainzParentsPortalSettings.json");
            NewzealandPostCodePathFile = Path.Combine(AppBaseDir, "Configs", "NewzealandPostCode.json");
            NotificationWebApiSettingsPathFile = Path.Combine(AppBaseDir, "Configs", "NotificationWebApiConnectionSettings.json");
        }
    }
}
