using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainzParentsPortal.Integration.PortalDb.Models
{
    public class UserAction
    {
        public string UserID { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set;}

    }

    public enum ActionType
    {
        LogIn,
        LogOut,
        SpentPoints,
    }
}
