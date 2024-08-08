using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ShopifyPortal.Shared.Helpers;

public class VerificationHelper
{

    public static bool VefifyPhoneNumber(string phone)
    {
        //Phone - E.164 format
        string pattern = "^\\+?[1-9]\\d{1,14}$";
        Regex rg = new Regex(pattern);
        
        return rg.IsMatch(phone);
    }

}
