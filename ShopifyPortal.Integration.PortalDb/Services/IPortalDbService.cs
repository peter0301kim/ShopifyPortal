using ShopifyPortal.Integration.PortalDb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyPortal.Integration.PortalDb.Services
{
    public interface IPortalDbService
    {
        bool AddPointLevel(PointLevel level);
        List<PointLevel> GetAllPointLevels();
        bool AddVendor(Vendor vendor);
        bool UpdateVendor(Vendor vendor);
        bool UpdateVendorImage(Vendor vendor);
        List<Vendor> GetAllVendors();
        Vendor GetVendorByVendorID(string vendorID);
        Vendor GetVendorByVendorCode(string vendorCode);
        List<Vendor> GetVendorsByVendorName(string vendorName);
        bool DeleteVendor(string vendorID);
        bool DeleteAllVendors();
        bool IsVendorExist(string vendorCode);

        bool AddOrganization(Organization Organization);
        bool UpdateOrganization(Organization Organization);
        bool UpdateOrganizationImage(Organization Organization);
        List<Organization> GetAllOrganizations();
        List<Organization> GetOrganizationsByOrganizationName(string organizationName);
        Organization GetOrganizationByOrganizationCode(string organizationCode);
        Organization GetOrganizationByOrganizationID(string organizationID);
        bool DeleteOrganization(string OrganizationID);
        bool DeleteAllOrganizations();
        bool IsOrganizationExist(string OrganizationName);


        bool AddOrderTx(OrderTx orderTx);
        OrderTx GetOrderTxsByOrderTxID(string orderTxID);
        List<OrderTx> GetOrderTxsByCustomerID(string customerID);
        decimal GetEarnedBrainzPointsByCustomerID(string customerID);
        List<OrderTxAnalysisDataModel> GetOrderTxs_GroupByOrg_Year_Month_TxCount(string year);
        List<OrderTxAnalysisDataModel> GetOrderTxs_GroupByOrg_Year_Month_BrainzPoint(string year);



        bool AddBrainzPointTr(BrainzPointTr brainzPointTr);
        List<BrainzPointTr> GetBrainzPointTrsByMemberID(string memberID);
        BrainzPointTr GetBrainzPointTrByBrainzPointTrID(string brainzPointTrID);
        decimal GetSpentBrainzPointsByMemberID(string memberID);
        bool UpdateBrainzPointReceipt(string BrainzPointTrID, string receiptPathFile);


        bool AddChildOrganization(ChildOrganization childOrganization);

        List<ChildOrganization> GetChildOrganizationByMemberID(string memberID);
        ChildOrganization GetChildOrganizationByChildOrganizationID(string childOrganizationID);
        bool UpdateChildOrganization(ChildOrganization childOrganization);
        bool DeleteChildOrganization(ChildOrganization childOrganization);
    }
}
