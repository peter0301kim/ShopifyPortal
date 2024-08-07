using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainzParentsPortal.Integration.PortalDb.Models
{
    public class OrderTxAnalysisDataModel
    {
        public string GroupNyName { get; set; } = string.Empty;
        public int Year { get; set; } = 0;
        public int Month { get; set; } = 0;
        public double Value { get; set; } = 0;
    }
}
