using ShopifyPortal.Integration.PortalDb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyPortal.Integration.PortalDb.Services
{
    public interface IPortalDbMemberService
    {
        bool AddMember(Member member);
        bool UpdateMemberBasic(Member member);
        bool UpdateMemberAddress(Member member);
        bool UpdateMemberImage(Member member);
        bool UpdateMemberPassword(string email, string password);
        decimal AddMemberBrainzPoint(string customerID, decimal brainzPoint);
        List<Member> GetAllMembers();
        Member GetMemberByMemberID(string userID);
        Member GetMemberByEmail(string email);
        Member GetMemberByCustomerID(string customerID);
        bool DeleteMemberByEmail(string email);
        bool DeleteAllMembers();
        bool IsUserExist(string email);
        bool AddUserAction(UserAction userAction);

        bool AddLogin2FA(string email, string secretKey);
        Login2FA GetLogin2FA(string email);
        bool DeleteLogin2FA(string email);


        bool AddResetPassword(string email, string secretKey);
        ResetPassword GetResetPassword(string email, string resetPasswordID);
        bool DeleteResetPassword(string email);

    }
}
