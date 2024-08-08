using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyPortal.Shared.Helpers
{
    public class NewZealandPostCodeHelper
    {

        public static List<string> ReadAllNewZealandPostCode(string pathFile)
        {
            var newzealandPostCodes = JsonConvert.DeserializeObject<List<NewzealandPostcode>>(File.ReadAllText(pathFile));

            var searchPostCodes = (from postCode in newzealandPostCodes select $"{postCode.postcode} | {postCode.locality} | {postCode.region}").ToList() ;

            return searchPostCodes;
        }
    }

    public class SearchPostcode
    {
        public string PostCodeRocalityRegion { get; set; }
    }
    public class NewzealandPostcode
    {
        public string postcode { get; set; } = string.Empty;
        public string locality { get; set; } = string.Empty;
        public string region { get; set; } = string.Empty;
        public string territory { get; set; } = string.Empty;
        public string island { get; set; } = string.Empty;


    }
}
