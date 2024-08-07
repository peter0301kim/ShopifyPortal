using BrainzParentsPortal.Integration.PortalDb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BrainzParentsPortal.Integration.PortalDb.Helpers;

public class ConversionHelper
{
    public static Member ConvertPortalDbUserForSql(Member portalUser)
    {
        return new Member()
        {
            MemberID = (string.IsNullOrEmpty(portalUser.MemberID)) ? System.Guid.NewGuid().ToString() : portalUser.MemberID,
            Email = portalUser.Email,
            Password = portalUser.Password,
            FirstName = portalUser.FirstName.Contains("'") ? portalUser.FirstName.Replace("'", "''") : portalUser.FirstName,
            LastName = portalUser.LastName.Contains("'") ? portalUser.LastName.Replace("'", "''") : portalUser.LastName,
            Phone = portalUser.Phone,
            Address = portalUser.Address.Contains("'") ? portalUser.Address.Replace("'", "''") : portalUser.Address,
            Address2 = portalUser.Address2.Contains("'") ? portalUser.Address2.Replace("'", "''") : portalUser.Address2,
            City = portalUser.City.Contains("'") ? portalUser.City.Replace("'", "''") : portalUser.City,
            Region = portalUser.Region.Contains("'") ? portalUser.Region.Replace("'", "''") : portalUser.Region,
            PostCode = portalUser.PostCode.Contains("'") ? portalUser.PostCode.Replace("'", "''") : portalUser.PostCode,
            Role = portalUser.Role,
            Tags = portalUser.Tags.Contains("'") ? portalUser.Tags.Replace("'", "''") : portalUser.Tags,
            CustomerID = portalUser.CustomerID,
            BrainzPoint = portalUser.BrainzPoint,
            ImagePathFile = portalUser.ImagePathFile,                 
            RegisteredDate = portalUser.RegisteredDate,
            ModifiedDate = portalUser.ModifiedDate
        };
    }

    public static Organization ConvertPortalDbOrganizationForSql(Organization organization)
    {
        return new Organization()
        {
            OrganizationID = (string.IsNullOrEmpty(organization.OrganizationID)) ? System.Guid.NewGuid().ToString() : organization.OrganizationID,
            OrganizationType = organization.OrganizationType.Contains("'") ? organization.OrganizationType.Replace("'", "''") : organization.OrganizationType,
            OrganizationCode = organization.OrganizationCode,
            OrganizationName = organization.OrganizationName.Contains("'") ? organization.OrganizationName.Replace("'", "''") : organization.OrganizationName,
            Phone = organization.Phone.Contains("'") ? organization.Phone.Replace("'", "''") : organization.Phone,
            Email = organization.Email.Contains("'") ? organization.Email.Replace("'", "''") : organization.Email,
            ContactPerson = organization.ContactPerson.Contains("'") ? organization.ContactPerson.Replace("'", "''") : organization.ContactPerson,
            ParentCommission = organization.ParentCommission,
            BSB = organization.BSB.Contains("'") ? organization.BSB.Replace("'", "''") : organization.BSB,
            AccountNumber = organization.AccountNumber.Contains("'") ? organization.AccountNumber.Replace("'", "''") : organization.AccountNumber,
            Bank = organization.Bank.Contains("'") ? organization.Bank.Replace("'", "''") : organization.Bank,
            PayerRefHelpText = organization.PayerRefHelpText.Contains("'") ? organization.PayerRefHelpText.Replace("'", "''") : organization.PayerRefHelpText,
            Memo = organization.Memo.Contains("'") ? organization.Memo.Replace("'", "''") : organization.Memo,
            ImagePathFile = organization.ImagePathFile,
            CreatedDate = organization.CreatedDate,
            ModifiedDate = organization.ModifiedDate
        };
    }

    public static Vendor ConvertPortalDbVendorForSql(Vendor vendor)
    {
        return new Vendor()
        {
            VendorID = (string.IsNullOrEmpty(vendor.VendorID)) ? System.Guid.NewGuid().ToString() : vendor.VendorID,
            VendorCode = vendor.VendorCode,
            VendorName = vendor.VendorName.Contains("'") ? vendor.VendorName.Replace("'", "''") : vendor.VendorName,
            Phone = vendor.Phone,
            Email = vendor.Email.Contains("'") ? vendor.Email.Replace("'", "''") : vendor.Email,
            ContactPerson = vendor.ContactPerson.Contains("'") ? vendor.ContactPerson.Replace("'", "''") : vendor.ContactPerson,
            Memo = vendor.Memo.Contains("'") ? vendor.Memo.Replace("'", "''") : vendor.Memo,
            VendorCommission = vendor.VendorCommission,
            ParentCommission = vendor.ParentCommission,
            ImagePathFile = vendor.ImagePathFile,
            ModifiedDate = vendor.ModifiedDate
        };
    }

    public static OrderTx ConvertPortalDbOrderTxForSql(OrderTx transaction)
    {
        return new OrderTx()
        {
            OrderTxID = transaction.OrderTxID,
            OrderTxDate = transaction.OrderTxDate,
            VendorCode = transaction.VendorCode,
            OrganizationCode = transaction.OrganizationCode,
            VendorOrderID = transaction.VendorOrderID,
            OrderName = transaction.OrderName.Contains("'") ? transaction.OrderName.Replace("'", "''") : transaction.OrderName,
            Total  = transaction.Total,
            BrainzCustomerID = transaction.BrainzCustomerID,
            Comments = transaction.Comments.Contains("'") ? transaction.Comments.Replace("'", "''") : transaction.Comments,
            BrainzPoint = transaction.BrainzPoint,
            BrainzPointCalc = transaction.BrainzPointCalc,
            ShopifyOrderID = transaction.ShopifyOrderID,
            OrderItems = (from item in transaction.OrderItems
                          select new OrderItem
                          {
                              OrderItemID = item.OrderItemID,
                              OrderItemName = item.OrderItemName.Contains("'") ? item.OrderItemName.Replace("'", "''") : item.OrderItemName,
                              ProductName = item.ProductName.Contains("'") ? item.ProductName.Replace("'", "''") : item.ProductName,
                              Quantity = item.Quantity,
                              UnitPrice = item.UnitPrice,
                              Amount = item.Amount,
                          }).ToList(),
        };
    }


    public static BrainzPointTr ConvertPortalDbBrainzPointTrForSql(BrainzPointTr transaction)
    {
        return new BrainzPointTr()
        {
            BrainzPointTrID = transaction.BrainzPointTrID,
            BrainzPointTrDate = transaction.BrainzPointTrDate,
            MemberID = transaction.MemberID,
            Amount = transaction.Amount,
            OrganizationCode = transaction.OrganizationCode,
            PayeeReference = transaction.PayeeReference.Contains("'") ? transaction.PayeeReference.Replace("'", "''") : transaction.PayeeReference,
            PayerReference = transaction.PayerReference.Contains("'") ? transaction.PayerReference.Replace("'", "''") : transaction.PayerReference,
            TrComments = transaction.TrComments.Contains("'") ? transaction.TrComments.Replace("'", "''") : transaction.TrComments,
            BankTxID = transaction.BankTxID,
            Comments = transaction.Comments.Contains("'") ? transaction.Comments.Replace("'", "''") : transaction.Comments
        };
    }

    public static ChildOrganization ConvertPortalDbChildOrganizationForSql(ChildOrganization childOrganization)
    {
        return new ChildOrganization()
        {
            ChildOrganizationID = childOrganization.ChildOrganizationID,
            ChildName = childOrganization.ChildName.Contains("'") ? childOrganization.ChildName.Replace("'", "''") : childOrganization.ChildName,
            PayerReference = childOrganization.PayerReference.Contains("'") ? childOrganization.PayerReference.Replace("'", "''") : childOrganization.PayerReference,
            MemberID = childOrganization.MemberID,
            Organization = new Organization()
            {
                OrganizationID = childOrganization.Organization.OrganizationID,
                OrganizationCode = childOrganization.Organization.OrganizationCode,
                OrganizationName = childOrganization.Organization.OrganizationName,
            },

        };
    }

}
