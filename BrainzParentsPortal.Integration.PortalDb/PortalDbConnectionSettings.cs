using Newtonsoft.Json;
using Paytec.Security.Encryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainzParentsPortal.Integration.PortalDb
{
    public class PortalDbConnectionSettings
    {
        public string DbServer { get; set; }
        public string DbCatalog { get; set; }
        public bool PersistSecurityInfo { get; set; } = false;
        public bool MultipleActiveResultSets { get; set; }
        public bool IntegratedSecurity { get; set; }
        public string DbUserID { get; set; }
        public string DbEncryptedPassword { get; set; }

        public PortalDbConnectionSettings()
        {
            
        }

        public PortalDbConnectionSettings(string settingsPathFile)
        {
            if (File.Exists(settingsPathFile))
            {
                var settings = JsonConvert.DeserializeObject<PortalDbConnectionSettings>(File.ReadAllText(settingsPathFile));

                DbServer = settings.DbServer;
                DbCatalog = settings.DbCatalog;
                PersistSecurityInfo = settings.PersistSecurityInfo;
                MultipleActiveResultSets = settings.MultipleActiveResultSets;
                IntegratedSecurity = settings.IntegratedSecurity;
                DbUserID = settings.DbUserID;
                DbEncryptedPassword = settings.DbEncryptedPassword;
            }
        }

        [JsonIgnore]
        public string DbPassword
        {
            get
            {
                return string.IsNullOrEmpty(DbEncryptedPassword) ? "" : SymmetricEncryption.DecryptString(DbEncryptedPassword);
            }
            set
            {
                DbEncryptedPassword = string.IsNullOrEmpty(value) ? "" : SymmetricEncryption.EncryptString(value);
            }
        }

        public string GetDbConnectionString()
        {
            string connectionString = $"Data Source={DbServer};" +
                                      $"Initial Catalog={DbCatalog};" +
                                      $"Persist Security Info={PersistSecurityInfo};" +
                                      $"MultipleActiveResultSets={MultipleActiveResultSets};";

            if (IntegratedSecurity)
            { connectionString += "Integrated Security=SSPI;"; }
            else
            {
                connectionString += $"User Id={DbUserID};Password={DbPassword};";
            }

            return connectionString;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (System.Reflection.PropertyInfo property in this.GetType().GetProperties())
            {
                sb.Append(property.Name);
                sb.Append(": ");
                if (property.GetIndexParameters().Length > 0)
                {
                    sb.Append("Indexed Property cannot be used");
                }
                else
                {
                    sb.Append(property.GetValue(this, null));
                }
                sb.Append(",");
            }

            return sb.ToString();
        }
    }
}
