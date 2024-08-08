using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyPortal.Integration.PortalDb.Models
{
    public class PointLevel
    {
        public string PointLevelID { get; set; } = string.Empty;
        public string PointLevelCode { get; set; } = string.Empty;
        public int PointLevelValue { get; set; }
        public string ImagePathFile { get; set; } = string.Empty;
    }
}
