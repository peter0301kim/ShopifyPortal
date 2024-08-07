using Newtonsoft.Json;

namespace Paytec.Test.SendMail.Console.SettingModels
{
    public class SendEmailSettings
    {
        public string Email_From { get; set; } = string.Empty;
        public List<string> Email_To { get; set; }


    }
}
