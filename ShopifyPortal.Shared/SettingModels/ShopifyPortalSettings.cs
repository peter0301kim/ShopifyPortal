using ShopifyPortal.Integration.PortalDb;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyPortal.Shared.SettingModels;

public class ShopifyPortalSettings
{
    public TransferFundsSettings TransferFunds { get; set; }
    public MultifactorAuthenticationSettings MultifactorAuthentication { get; set; }
    public ResetPasswordSettings ResetPassword { get; set; }
    public NewzealandPostcodeSettings NewzealandPostcode { get; set; }
    public ShopifyPortalSettings(string settingsPathFile)
    {
        if (File.Exists(settingsPathFile))
        {
            var settings = JsonConvert.DeserializeObject<ShopifyPortalSettings>(File.ReadAllText(settingsPathFile));

            TransferFunds = settings.TransferFunds;
            MultifactorAuthentication = settings.MultifactorAuthentication;
            ResetPassword = settings.ResetPassword;
            NewzealandPostcode = settings.NewzealandPostcode;
        }
    }
}

public class TransferFundsSettings
{
    public string TrReceiptPath { get; set; } = string.Empty;
    public string TrReceiptTemplatePathFile { get; set; } = string.Empty;
    public string TrFundCsvPath { get; set; } = string.Empty;
    public int TrMininumPoint { get; set; }
    public double OneDollarToBrainzPoint { get; set; }
}

public class MultifactorAuthenticationSettings
{
    public bool IsUseMFA { get; set; }
    public string Email_2FA_TemplatePathFile { get; set; } = string.Empty;
    public double MFAExpireyMinutes { get; set; }
}

public class ResetPasswordSettings
{
    public string Email_From { get; set; } = string.Empty;
    public string Email_ResetPassword_TemplatePathFile { get; set; } = string.Empty;
    public string ResetPassword_URL { get; set; } = string.Empty;
    public double RequestExpireyMinutes { get; set; }
}


public class NewzealandPostcodeSettings
{
    public string PostcodePathFile { get; set; } = string.Empty;
}