using BrainzParentsPortal.Integration.PortalDb.Helpers;
using BrainzParentsPortal.Integration.PortalDb.Models;
using NLog;
using System;
using System.Data.SqlClient;
using System.Numerics;

namespace BrainzParentsPortal.Integration.PortalDb.Services;

public class PortalDbService : IPortalDbService
{
    private static Logger Log = LogManager.GetCurrentClassLogger();

    private PortalDbConnectionSettings PortalDbConnectionSettings { get; set; }
    public PortalDbService(PortalDbConnectionSettings portalDbConnectionSettings)
    {
        this.PortalDbConnectionSettings = portalDbConnectionSettings;
    }


    public bool AddPointLevel(PointLevel pointLevel)
    {

        bool bTrue = true;
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();

            SqlCommand command = con.CreateCommand();
            SqlTransaction transaction;
            transaction = con.BeginTransaction("AddPointLevelTransaction");
            command.Connection = con;
            command.Transaction = transaction;

            try
            {
                string query
                    = $"INSERT INTO PointLevel (PointLevelID, PointLevelCode, PointLevelValue, ImagePathFile)" +
                    $" VALUES " +
                    $"(" +
                    $"'{pointLevel.PointLevelID}','{pointLevel.PointLevelCode}',{pointLevel.PointLevelValue}, '{pointLevel.ImagePathFile}'" +
                    $")";

                Log.Debug(query);
                command.CommandText = query;
                command.ExecuteNonQuery();

                transaction.Commit();

            }
            catch (Exception e)
            {
                Console.WriteLine("Commit Exception Type: {0}", e.GetType());
                Console.WriteLine("  Message: {0}", e.Message);
                Log.Error(e);
                bTrue = false;
                try
                {
                    transaction.Rollback();
                }
                catch (Exception ex2)
                {
                    // This catch block will handle any errors that may have occurred
                    // on the server that would cause the rollback to fail, such as
                    // a closed connection.
                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Console.WriteLine("  Message: {0}", ex2.Message);
                }
                finally
                {
                    //con.Close();
                    bTrue = false;
                }
            }
            con.Close();
        }

        bTrue = true;
        return bTrue;
    }
    public List<PointLevel> GetAllPointLevels()
    {
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        List<PointLevel> pointLevels = new List<PointLevel>();

        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query =
                    "SELECT PointLevelID, PointLevelCode, PointLevelValue, ImagePathFile" +
                    "  FROM PointLevel";

                Log.Debug(query);

                using (SqlCommand command = new SqlCommand(query, con))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            PointLevel table = new PointLevel();
                            table.PointLevelID = (string)reader["PointLevelID"];
                            table.PointLevelCode = (string)reader["PointLevelCode"];
                            table.PointLevelValue = (int)reader["PointLevelValue"];
                            table.ImagePathFile = (reader["ImagePathFile"] != DBNull.Value) ? (string)reader["ImagePathFile"] : "";

                            pointLevels.Add(table);

                        }
                    }
                }
                con.Close();
            }
        }
        catch (Exception e)
        {
            Log.Error(e);
            throw new Exception(e.ToString());
        }

        return pointLevels;
    }
    #region Vendors
    public bool AddVendor(Vendor vendor)
    {

        vendor = ConversionHelper.ConvertPortalDbVendorForSql(vendor);

        bool bTrue = true;
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();

            SqlCommand command = con.CreateCommand();
            SqlTransaction transaction;
            transaction = con.BeginTransaction("AddVendorTransaction");
            command.Connection = con;
            command.Transaction = transaction;

            try
            {
                string insertDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                string query 
                    = $"INSERT INTO Vendor (VendorID, VendorCode, VendorName, Phone, Email,   " +
                                $"ContactPerson, Memo, VendorCommission, ParentCommission, ImagePathFile, " +
                                $"CreatedDate, ModifiedDate)" + 
                    $" VALUES " + 
                    $"("+ 
                    $"'{vendor.VendorID}','{vendor.VendorCode}','{vendor.VendorName}','{vendor.Phone}', '{vendor.Email}', " +
                    $"'{vendor.ContactPerson}','{vendor.Memo}',{vendor.VendorCommission}, " + $"{vendor.ParentCommission}, '{vendor.ImagePathFile}', " +
                    $"'{vendor.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss")}', '{vendor.ModifiedDate.ToString("yyyy - MM - dd HH: mm: ss")}'" +
                    $")";

                Log.Debug(query);
                command.CommandText = query;
                command.ExecuteNonQuery();

                transaction.Commit();

            }
            catch (Exception e)
            {
                Console.WriteLine("Commit Exception Type: {0}", e.GetType());
                Console.WriteLine("  Message: {0}", e.Message);
                Log.Error(e);
                bTrue = false;
                try
                {
                    transaction.Rollback();
                }
                catch (Exception ex2)
                {
                    // This catch block will handle any errors that may have occurred
                    // on the server that would cause the rollback to fail, such as
                    // a closed connection.
                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Console.WriteLine("  Message: {0}", ex2.Message);
                }
                finally
                {
                    //con.Close();
                    bTrue = false;
                }
            }
            con.Close();
        }

        bTrue = true;
        return bTrue;
    }
    public bool UpdateVendor(Vendor vendor)
    {

        vendor = ConversionHelper.ConvertPortalDbVendorForSql(vendor);

        bool bTrue = true;
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();

            SqlCommand command = con.CreateCommand();
            SqlTransaction transaction;
            transaction = con.BeginTransaction("AddVendorTransaction");
            command.Connection = con;
            command.Transaction = transaction;

            try
            {
                string insertDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                string query = 
                    $"UPDATE Vendor SET " +
                        $"VendorName = '{vendor.VendorName}', " +
                        $"Phone = '{vendor.Phone}', " +
                        $"Email = '{vendor.Email}', " +
                        $"ContactPerson = '{vendor.ContactPerson}', " +
                        $"Memo =  '{vendor.Memo}', " +
                        $"VendorCommission = {vendor.VendorCommission}, " +
                        $"ParentCommission = {vendor.ParentCommission}, " +
                        $"ModifiedDate = '{vendor.ModifiedDate.ToString("yyyy-MM-dd HH:mm:ss")}'" +
                    $" WHERE VendorID = '{vendor.VendorID}'";

                Log.Debug(query);
                command.CommandText = query;
                command.ExecuteNonQuery();

                transaction.Commit();

            }
            catch (Exception e)
            {
                Console.WriteLine("Commit Exception Type: {0}", e.GetType());
                Console.WriteLine("  Message: {0}", e.Message);
                Log.Error(e);
                bTrue = false;
                try
                {
                    transaction.Rollback();
                }
                catch (Exception ex2)
                {
                    // This catch block will handle any errors that may have occurred
                    // on the server that would cause the rollback to fail, such as
                    // a closed connection.
                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Console.WriteLine("  Message: {0}", ex2.Message);
                }
                finally
                {
                    //con.Close();
                    bTrue = false;
                }
            }
            con.Close();
        }

        bTrue = true;
        return bTrue;
    }

    public bool UpdateVendorImage(Vendor vendor)
    {
        bool bTrue = true;
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();

            SqlCommand command = con.CreateCommand();
            SqlTransaction transaction;
            transaction = con.BeginTransaction("UpdateVendorImageTransaction");
            command.Connection = con;
            command.Transaction = transaction;

            try
            {
                vendor.ModifiedDate = (vendor.ModifiedDate == null) ? DateTime.UtcNow : vendor.ModifiedDate;

                string query =
                    $"UPDATE Vendor SET " +
                        $"ImagePathFile = '{vendor.ImagePathFile}', " +
                        $"ModifiedDate = '{vendor.ModifiedDate.ToString("yyyy-MM-dd HH:mm:ss")}'" +
                    $" WHERE VendorID = '{vendor.VendorID}'";

                Log.Debug(query);
                command.CommandText = query;
                command.ExecuteNonQuery();

                transaction.Commit();

            }
            catch (Exception e)
            {
                Console.WriteLine("Commit Exception Type: {0}", e.GetType());
                Console.WriteLine("  Message: {0}", e.Message);
                Log.Error(e);
                bTrue = false;
                try
                {
                    transaction.Rollback();
                }
                catch (Exception ex2)
                {
                    // This catch block will handle any errors that may have occurred
                    // on the server that would cause the rollback to fail, such as
                    // a closed connection.
                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Console.WriteLine("  Message: {0}", ex2.Message);
                }
                finally
                {
                    //con.Close();
                    bTrue = false;
                }
            }
            con.Close();
        }

        bTrue = true;
        return bTrue;
    }
    public Vendor GetVendorByVendorID(string vendorID)
    {
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        Vendor Vendor = new Vendor();

        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query = 
                    "SELECT VendorID, VendorCode, VendorName, Phone, Email,  " +
                            "ContactPerson, Memo, VendorCommission, ParentCommission, ImagePathFile " + 
                     "  FROM Vendor" + 
                    $" WHERE VendorID = '{vendorID}'" ;

                Log.Debug(query);

                using (SqlCommand command = new SqlCommand(query, con))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Vendor table = new Vendor();
                            table.VendorID = (string)reader["VendorID"];
                            table.VendorCode = (string)reader["VendorCode"];
                            table.VendorName = (string)reader["VendorName"];
                            table.Phone = (reader["Phone"] != DBNull.Value) ? (string)reader["Phone"] : "";
                            table.Email = (reader["Email"] != DBNull.Value) ? (string)reader["Email"] : "";
                            table.ContactPerson = (reader["ContactPerson"] != DBNull.Value) ? (string)reader["ContactPerson"] : "";
                            table.Memo = (reader["Memo"] != DBNull.Value) ? (string)reader["Memo"] : "";
                            table.VendorCommission = (reader["VendorCommission"] != DBNull.Value) ? (decimal)reader["VendorCommission"] : 0; //(double)reader["VendorCommission"];
                            table.ParentCommission = (reader["ParentCommission"] != DBNull.Value) ? (decimal)reader["ParentCommission"] : 0;
                            table.ImagePathFile = (reader["ImagePathFile"] != DBNull.Value) ? (string)reader["ImagePathFile"] : "";
                            Vendor = table;
                            break;
                        }
                    }
                }
                con.Close();
            }
        }
        catch (Exception e)
        {
            Log.Error(e);
            throw new Exception(e.ToString());
        }

        return Vendor;
    }

    public List<Vendor> GetAllVendors()
    {
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        List<Vendor> Vendors = new List<Vendor>();

        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query =
                    "SELECT VendorID, VendorCode, VendorName, Phone, Email,  " +
                            "ContactPerson, Memo, VendorCommission, ParentCommission, ImagePathFile " +
                    "  FROM Vendor";

                Log.Debug(query);

                using (SqlCommand command = new SqlCommand(query, con))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Vendor table = new Vendor();
                            table.VendorID = (string)reader["VendorID"];
                            table.VendorCode= (string)reader["VendorCode"];
                            table.VendorName = (string)reader["VendorName"];
                            table.Phone = (reader["Phone"] != DBNull.Value) ? (string)reader["Phone"] : "";
                            table.Email = (reader["Email"] != DBNull.Value) ? (string)reader["Email"] : "";
                            table.ContactPerson = (reader["ContactPerson"] != DBNull.Value) ? (string)reader["ContactPerson"] : "";
                            table.Memo = (reader["Memo"] != DBNull.Value) ? (string)reader["Memo"] : "";
                            table.VendorCommission = (reader["VendorCommission"] != DBNull.Value) ? (decimal)reader["VendorCommission"] : 0; //(double)reader["VendorCommission"];
                            table.ParentCommission = (reader["ParentCommission"] != DBNull.Value) ? (decimal)reader["ParentCommission"] : 0;
                            table.ImagePathFile = (reader["ImagePathFile"] != DBNull.Value) ? (string)reader["ImagePathFile"] : "";

                            Vendors.Add(table);

                        }
                    }
                }
                con.Close();
            }
        }
        catch (Exception e)
        {
            Log.Error(e);
            throw new Exception(e.ToString());
        }

        return Vendors;
    }
    public List<Vendor> GetVendorsByVendorName(string vendorName)
    {
        vendorName = vendorName.Contains("'") ? vendorName.Replace("'", "''") : vendorName;

        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        List<Vendor> Vendors = new List<Vendor>();

        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query =

                    "SELECT VendorID, VendorCode, VendorName, Phone, Email, " +
                           "ContactPerson, Memo, VendorCommission, ParentCommission, ImagePathFile " +
                    "  FROM Vendor" + 
                  $"  WHERE VendorName like '%{vendorName}%'";

                Log.Debug(query);

                using (SqlCommand command = new SqlCommand(query, con))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Vendor table = new Vendor();
                            table.VendorID = (string)reader["VendorID"];
                            table.VendorCode = (string)reader["VendorCode"];
                            table.VendorName = (string)reader["VendorName"];
                            table.Phone = (reader["Phone"] != DBNull.Value) ? (string)reader["Phone"] : "";
                            table.Email = (reader["Email"] != DBNull.Value) ? (string)reader["Email"] : "";
                            table.ContactPerson = (reader["ContactPerson"] != DBNull.Value) ? (string)reader["ContactPerson"] : "";
                            table.Memo = (reader["Memo"] != DBNull.Value) ? (string)reader["Memo"] : "";
                            table.VendorCommission = (reader["VendorCommission"] != DBNull.Value) ? (decimal)reader["VendorCommission"] : 0; //(double)reader["VendorCommission"];
                            table.ParentCommission = (reader["ParentCommission"] != DBNull.Value) ? (decimal)reader["ParentCommission"] : 0;
                            table.ImagePathFile = (reader["ImagePathFile"] != DBNull.Value) ? (string)reader["ImagePathFile"] : "";

                            Vendors.Add(table);
                        }
                    }
                }
                con.Close();
            }
        }
        catch (Exception e)
        {
            Log.Error(e);
            throw new Exception(e.ToString());
        }

        return Vendors;
    }

    public Vendor GetVendorByVendorCode(string vendorCode)
    {
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        Vendor vendor = new Vendor();

        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query =
                    "SELECT VendorID, VendorCode, VendorName, Phone, Email, " +
                           "ContactPerson, Memo, VendorCommission, ParentCommission, ImagePathFile " +
                    "  FROM Vendor" +
                  $"  WHERE VendorCode = '{vendorCode}'";

                Log.Debug(query);

                using (SqlCommand command = new SqlCommand(query, con))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Vendor table = new Vendor();
                            table.VendorID = (string)reader["VendorID"];
                            table.VendorCode = (string)reader["VendorCode"];
                            table.VendorName = (string)reader["VendorName"];
                            table.Phone = (reader["Phone"] != DBNull.Value) ? (string)reader["Phone"] : "";
                            table.Email = (reader["Email"] != DBNull.Value) ? (string)reader["Email"] : "";
                            table.ContactPerson = (reader["ContactPerson"] != DBNull.Value) ? (string)reader["ContactPerson"] : "";
                            table.Memo = (reader["Memo"] != DBNull.Value) ? (string)reader["Memo"] : "";
                            table.VendorCommission = (reader["VendorCommission"] != DBNull.Value) ? (decimal)reader["VendorCommission"] : 0; //(double)reader["VendorCommission"];
                            table.ParentCommission = (reader["ParentCommission"] != DBNull.Value) ? (decimal)reader["ParentCommission"] : 0;
                            table.ImagePathFile = (reader["ImagePathFile"] != DBNull.Value) ? (string)reader["ImagePathFile"] : "";

                            vendor = table;
                            break;

                        }
                    }
                }
                con.Close();
            }
        }
        catch (Exception e)
        {
            Log.Error(e);
            throw new Exception(e.ToString());
        }

        return vendor;
    }

    public bool DeleteVendor(string vendorID)
    {
        bool bTrue = true;
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();

            SqlCommand command = con.CreateCommand();
            SqlTransaction transaction;
            transaction = con.BeginTransaction("DeleteVendor");
            command.Connection = con;
            command.Transaction = transaction;

            try
            {
                string query = $"DELETE Vendor WHERE VendorID = '{vendorID}' ";
                Log.Debug(query);
                command.CommandText = query;
                command.ExecuteNonQuery();

                transaction.Commit();
                bTrue = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Commit Exception Type: {0}", e.GetType());
                Console.WriteLine("  Message: {0}", e.Message);
                Log.Error(e);
                bTrue = false;
                try
                {
                    transaction.Rollback();
                }
                catch (Exception ex2)
                {
                    // This catch block will handle any errors that may have occurred
                    // on the server that would cause the rollback to fail, such as
                    // a closed connection.
                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Console.WriteLine("  Message: {0}", ex2.Message);
                    Log.Error(ex2);
                }
                finally
                {
                    bTrue = false;
                    //con.Close();
                }
            }

            con.Close();
        }
        return bTrue;
    }

    public bool DeleteAllVendors()
    {
        bool bTrue = true;
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();

            SqlCommand command = con.CreateCommand();
            SqlTransaction transaction;
            transaction = con.BeginTransaction("DeleteAllVendors");
            command.Connection = con;
            command.Transaction = transaction;

            try
            {
                string query = $"DELETE Vendor; ";
                Log.Debug(query);
                command.CommandText = query;
                command.ExecuteNonQuery();

                transaction.Commit();
                bTrue = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Commit Exception Type: {0}", e.GetType());
                Console.WriteLine("  Message: {0}", e.Message);
                Log.Error(e);
                bTrue = false;
                try
                {
                    transaction.Rollback();
                }
                catch (Exception ex2)
                {
                    // This catch block will handle any errors that may have occurred
                    // on the server that would cause the rollback to fail, such as
                    // a closed connection.
                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Console.WriteLine("  Message: {0}", ex2.Message);
                    Log.Error(ex2);
                }
                finally
                {
                    bTrue = false;
                    //con.Close();
                }
            }

            con.Close();
        }
        return bTrue;
    }



    public bool IsVendorExist(string vendorCode)
    {
        vendorCode = vendorCode.Contains("'") ? vendorCode.Replace("'", "''") : vendorCode;

        bool isExist = false;
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query = $"SELECT VendorCode FROM Vendor WHERE VendorCode = '{vendorCode}'";
                Log.Debug(query);

                using (SqlCommand command = new SqlCommand(query, con))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        isExist = true;
                    }
                    else
                    {
                        isExist = false;
                    }
                }
                con.Close();
            }
        }
        catch (Exception e)
        {
            Log.Error(e);
            isExist = false;
        }

        return isExist;
    }

    #endregion Vendors


    #region Organizations
    public bool AddOrganization(Organization organization)
    {
        organization = ConversionHelper.ConvertPortalDbOrganizationForSql(organization);

        bool bTrue = true;
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();

            SqlCommand command = con.CreateCommand();
            SqlTransaction transaction;
            transaction = con.BeginTransaction("AddOrganizationTransaction");
            command.Connection = con;
            command.Transaction = transaction;

            try
            {
                organization.ModifiedDate = (organization.ModifiedDate == null) ? DateTime.UtcNow : organization.ModifiedDate;

                string query
                    = $"INSERT INTO Organization (" +
                                $"OrganizationID, OrganizationType, OrganizationCode, OrganizationName, Phone,  " +
                                $"Email, ContactPerson, ParentCommission, BSB, AccountNumber, " +
                                $"Bank, PayerRefHelpText, Memo, ImagePathFile, CreatedDate, ModifiedDate)" +
                    $" VALUES " +
                    $"(" +
                    $"'{organization.OrganizationID}','{organization.OrganizationType}','{organization.OrganizationCode}', '{organization.OrganizationName}', '{organization.Phone}',  " +
                    $"'{organization.Email}', '{organization.ContactPerson}', '{organization.ParentCommission}','{organization.BSB}','{organization.AccountNumber}',  " +
                    $"'{organization.Bank}', '{organization.PayerRefHelpText}', '{organization.Memo}', '{organization.ImagePathFile}', '{organization.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss")}', '{organization.ModifiedDate.ToString("yyyy-MM-dd HH:mm:ss")}'" +
                    $")";

                Log.Debug(query);
                command.CommandText = query;
                command.ExecuteNonQuery();

                transaction.Commit();

            }
            catch (Exception e)
            {
                Console.WriteLine("Commit Exception Type: {0}", e.GetType());
                Console.WriteLine("  Message: {0}", e.Message);
                Log.Error(e);
                bTrue = false;
                try
                {
                    transaction.Rollback();
                }
                catch (Exception ex2)
                {
                    // This catch block will handle any errors that may have occurred
                    // on the server that would cause the rollback to fail, such as
                    // a closed connection.
                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Console.WriteLine("  Message: {0}", ex2.Message);
                }
                finally
                {
                    //con.Close();
                    bTrue = false;
                }
            }
            con.Close();
        }

        bTrue = true;
        return bTrue;
    }

    public bool UpdateOrganization(Organization organization)
    {
        organization = ConversionHelper.ConvertPortalDbOrganizationForSql(organization);

        bool bTrue = true;
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();

            SqlCommand command = con.CreateCommand();
            SqlTransaction transaction;
            transaction = con.BeginTransaction("UpdateOrgTransaction");
            command.Connection = con;
            command.Transaction = transaction;

            try
            {
                organization.ModifiedDate = (organization.ModifiedDate == null) ? DateTime.UtcNow : organization.ModifiedDate;

                string query =
                    $"UPDATE Organization SET " +
                        $"OrganizationType = '{organization.OrganizationType}', " +
                        $"OrganizationCode = '{organization.OrganizationCode}', " +
                        $"OrganizationName = '{organization.OrganizationName}', " +
                        $"Phone = '{organization.Phone}', " +
                        $"Email = '{organization.Email}', " +
                        $"ContactPerson = '{organization.ContactPerson}', " +
                        $"ParentCommission = {organization.ParentCommission}, " +
                        $"BSB = '{organization.BSB}', " +
                        $"AccountNumber = '{organization.AccountNumber}', " +
                        $"Bank = '{organization.Bank}', " +
                        $"PayerRefHelpText = '{organization.PayerRefHelpText}', " +
                        $"Memo = '{organization.Memo}', " +
                        $"ModifiedDate = '{organization.ModifiedDate.ToString("yyyy-MM-dd HH:mm:ss")}'" +
                    $" WHERE OrganizationID = '{organization.OrganizationID}'";

                Log.Debug(query);
                command.CommandText = query;
                command.ExecuteNonQuery();

                transaction.Commit();

            }
            catch (Exception e)
            {
                Console.WriteLine("Commit Exception Type: {0}", e.GetType());
                Console.WriteLine("  Message: {0}", e.Message);
                Log.Error(e);
                bTrue = false;
                try
                {
                    transaction.Rollback();
                }
                catch (Exception ex2)
                {
                    // This catch block will handle any errors that may have occurred
                    // on the server that would cause the rollback to fail, such as
                    // a closed connection.
                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Console.WriteLine("  Message: {0}", ex2.Message);
                }
                finally
                {
                    //con.Close();
                    bTrue = false;
                }
            }
            con.Close();
        }

        bTrue = true;
        return bTrue;
    }

    public bool UpdateOrganizationImage(Organization organization)
    {
        bool bTrue = true;
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();

            SqlCommand command = con.CreateCommand();
            SqlTransaction transaction;
            transaction = con.BeginTransaction("UpdateOrgImageTransaction");
            command.Connection = con;
            command.Transaction = transaction;

            try
            {
                organization.ModifiedDate = (organization.ModifiedDate == null) ? DateTime.UtcNow : organization.ModifiedDate;

                string query =
                    $"UPDATE Organization SET " +
                        $"ImagePathFile = '{organization.ImagePathFile}', " +
                        $"ModifiedDate = '{organization.ModifiedDate.ToString("yyyy-MM-dd HH:mm:ss")}'" +
                    $" WHERE OrganizationID = '{organization.OrganizationID}'";

                Log.Debug(query);
                command.CommandText = query;
                command.ExecuteNonQuery();

                transaction.Commit();

            }
            catch (Exception e)
            {
                Console.WriteLine("Commit Exception Type: {0}", e.GetType());
                Console.WriteLine("  Message: {0}", e.Message);
                Log.Error(e);
                bTrue = false;
                try
                {
                    transaction.Rollback();
                }
                catch (Exception ex2)
                {
                    // This catch block will handle any errors that may have occurred
                    // on the server that would cause the rollback to fail, such as
                    // a closed connection.
                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Console.WriteLine("  Message: {0}", ex2.Message);
                }
                finally
                {
                    //con.Close();
                    bTrue = false;
                }
            }
            con.Close();
        }

        bTrue = true;
        return bTrue;
    }
    public Organization GetOrganizationByOrganizationID(string organizationID)
    {
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        Organization organization = new Organization();

        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query
                    = "SELECT OrganizationID, OrganizationType, OrganizationCode, OrganizationName, Phone, " +
                             "Email, ContactPerson, ParentCommission, BSB, AccountNumber,  " +
                             "Bank, PayerRefHelpText, Memo, ImagePathFile " +
                      "  FROM Organization" +
                     $" WHERE OrganizationID = '{organizationID}'";

                Log.Debug(query);

                using (SqlCommand command = new SqlCommand(query, con))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Organization table = new Organization();
                            table.OrganizationID = (string)reader["OrganizationID"];
                            table.OrganizationType = (string)reader["OrganizationType"];
                            table.OrganizationCode = (string)reader["OrganizationCode"];
                            table.OrganizationName = (string)reader["OrganizationName"];
                            table.Phone = (reader["Phone"] != System.DBNull.Value) ? (string)reader["Phone"] : "";
                            table.Email = (reader["Email"] != System.DBNull.Value) ? (string)reader["Email"] : "";
                            table.ContactPerson = (reader["ContactPerson"] != System.DBNull.Value) ? (string)reader["ContactPerson"] : "";
                            table.ParentCommission = (decimal)reader["ParentCommission"];
                            table.BSB = (reader["BSB"] != System.DBNull.Value) ? (string)reader["BSB"] : "";
                            table.AccountNumber = (reader["AccountNumber"] != System.DBNull.Value) ? (string)reader["AccountNumber"] : "";
                            table.Bank = (reader["Bank"] != System.DBNull.Value) ? (string)reader["Bank"] : "";
                            table.PayerRefHelpText = (reader["PayerRefHelpText"] != System.DBNull.Value) ? (string)reader["PayerRefHelpText"] : "";
                            table.Memo = (reader["Memo"] != System.DBNull.Value) ? (string)reader["Memo"] : "";
                            table.ImagePathFile = (reader["ImagePathFile"] != System.DBNull.Value) ? (string)reader["ImagePathFile"] : "";

                            organization = table;
                            break;
                        }
                    }
                }
                con.Close();
            }
        }
        catch (Exception e)
        {
            Log.Error(e);
            throw new Exception(e.ToString());
        }

        return organization;
    }

    public List<Organization> GetAllOrganizations()
    {
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        List<Organization> organizations = new List<Organization>();

        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query =
                    "SELECT OrganizationID, OrganizationType, OrganizationCode, OrganizationName, Phone, " +
                            "Email, ContactPerson, ParentCommission, BSB, AccountNumber, " +
                            "Bank, PayerRefHelpText, Memo, ImagePathFile  " +
                    "  FROM Organization";

                Log.Debug(query);

                using (SqlCommand command = new SqlCommand(query, con))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Organization table = new Organization();
                            table.OrganizationID = (string)reader["OrganizationID"];
                            table.OrganizationType = (string)reader["OrganizationType"];
                            table.OrganizationCode = (string)reader["OrganizationCode"];
                            table.OrganizationName = (string)reader["OrganizationName"];
                            table.Phone = (reader["Phone"] != System.DBNull.Value) ? (string)reader["Phone"] : "";
                            table.Email = (reader["Email"] != System.DBNull.Value) ? (string)reader["Email"] : "";
                            table.ContactPerson = (reader["ContactPerson"] != System.DBNull.Value) ? (string)reader["ContactPerson"] : "";
                            table.ParentCommission = (decimal)reader["ParentCommission"];
                            table.BSB = (reader["BSB"] != System.DBNull.Value) ? (string)reader["BSB"] : "";
                            table.AccountNumber = (reader["AccountNumber"] != System.DBNull.Value) ? (string)reader["AccountNumber"] : "";
                            table.Bank = (reader["Bank"] != System.DBNull.Value) ? (string)reader["Bank"] : "";
                            table.PayerRefHelpText = (reader["PayerRefHelpText"] != System.DBNull.Value) ? (string)reader["PayerRefHelpText"] : "";
                            table.Memo = (reader["Memo"] != System.DBNull.Value) ? (string)reader["Memo"] : "";
                            table.ImagePathFile = (reader["ImagePathFile"] != System.DBNull.Value) ? (string)reader["ImagePathFile"] : "";
                            organizations.Add(table);

                        }
                    }
                }
                con.Close();
            }
        }
        catch (Exception e)
        {
            Log.Error(e);
            throw new Exception(e.ToString());
        }

        return organizations;
    }
    public List<Organization> GetOrganizationsByOrganizationName(string organizationName)
    {
        organizationName = organizationName.Contains("'") ? organizationName.Replace("'", "''") : organizationName;

        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        List<Organization> organizations = new List<Organization>();

        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query
                    = "SELECT OrganizationID, OrganizationType, OrganizationCode, OrganizationName, Phone," +
                             "Email, ContactPersion, ParentCommission, BSB, AccountNumber," +
                             "Bank, PayerRefHelpText, Memo, ImagePathFile  " +
                     "   FROM Organization" +
                    $"  WHERE OrganizationName like '%{organizationName}%'";

                Log.Debug(query);

                using (SqlCommand command = new SqlCommand(query, con))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Organization table = new Organization();
                            table.OrganizationID = (string)reader["OrganizationID"];
                            table.OrganizationType = (string)reader["OrganizationType"];
                            table.OrganizationCode = (string)reader["OrganizationCode"];
                            table.OrganizationName = (string)reader["OrganizationName"];
                            table.Phone = (reader["Phone"] != System.DBNull.Value) ? (string)reader["Phone"] : "";
                            table.Email = (reader["Email"] != System.DBNull.Value) ? (string)reader["Email"] : "";
                            table.ContactPerson = (reader["ContactPerson"] != System.DBNull.Value) ? (string)reader["ContactPerson"] : "";
                            table.ParentCommission = (decimal)reader["ParentCommission"];
                            table.BSB = (reader["BSB"] != System.DBNull.Value) ? (string)reader["BSB"] : "";
                            table.AccountNumber = (reader["AccountNumber"] != System.DBNull.Value) ? (string)reader["AccountNumber"] : "";
                            table.Bank = (reader["Bank"] != System.DBNull.Value) ? (string)reader["Bank"] : "";
                            table.PayerRefHelpText = (reader["PayerRefHelpText"] != System.DBNull.Value) ? (string)reader["PayerRefHelpText"] : "";
                            table.Memo = (reader["Memo"] != System.DBNull.Value) ? (string)reader["Memo"] : "";
                            table.ImagePathFile = (reader["ImagePathFile"] != System.DBNull.Value) ? (string)reader["ImagePathFile"] : "";
                            organizations.Add(table);

                        }
                    }
                }
                con.Close();
            }
        }
        catch (Exception e)
        {
            Log.Error(e);
            throw new Exception(e.ToString());
        }

        return organizations;
    }

    public Organization GetOrganizationByOrganizationCode(string organizationCode)
    {
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        List<Organization> organizations = new List<Organization>();

        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query
                    = "SELECT OrganizationID, OrganizationType, OrganizationCode, OrganizationName, Phone,  " +
                             "Email, ContactPerson, ParentCommission, BSB, AccountNumber, " +
                             "Bank, PayerRefHelpText, Memo, ImagePathFile " +
                     "   FROM Organization" +
                    $"  WHERE OrganizationCode = '{organizationCode}'";

                Log.Debug(query);

                using (SqlCommand command = new SqlCommand(query, con))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Organization table = new Organization();
                            table.OrganizationID = (string)reader["OrganizationID"];
                            table.OrganizationType = (string)reader["OrganizationType"];
                            table.OrganizationCode = (string)reader["OrganizationCode"];
                            table.OrganizationName = (string)reader["OrganizationName"];
                            table.Phone = (reader["Phone"] != System.DBNull.Value) ? (string)reader["Phone"] : "";
                            table.Email = (reader["Email"] != System.DBNull.Value) ? (string)reader["Email"] : "";
                            table.ContactPerson = (reader["ContactPerson"] != System.DBNull.Value) ? (string)reader["ContactPerson"] : "";
                            table.ParentCommission = (decimal)reader["ParentCommission"];
                            table.BSB = (reader["BSB"] != System.DBNull.Value) ? (string)reader["BSB"] : "";
                            table.AccountNumber = (reader["AccountNumber"] != System.DBNull.Value) ? (string)reader["AccountNumber"] : "";
                            table.Bank = (reader["Bank"] != System.DBNull.Value) ? (string)reader["Bank"] : "";
                            table.PayerRefHelpText = (reader["PayerRefHelpText"] != System.DBNull.Value) ? (string)reader["PayerRefHelpText"] : "";
                            table.Memo = (reader["Memo"] != System.DBNull.Value) ? (string)reader["Memo"] : "";
                            table.ImagePathFile = (reader["ImagePathFile"] != System.DBNull.Value) ? (string)reader["ImagePathFile"] : "";

                            organizations.Add(table);
                        }
                    }
                }
                con.Close();
            }
        }
        catch (Exception e)
        {
            Log.Error(e);
            throw new Exception(e.ToString());
        }

        return organizations.FirstOrDefault();
    }

    public bool DeleteOrganization(string organizationID)
    {
        bool bTrue = true;
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();

            SqlCommand command = con.CreateCommand();
            SqlTransaction transaction;
            transaction = con.BeginTransaction("DeleteOrganization");
            command.Connection = con;
            command.Transaction = transaction;

            try
            {
                string query = $"DELETE Organization WHERE OrganizationID = '{organizationID}' ";
                Log.Debug(query);
                command.CommandText = query;
                command.ExecuteNonQuery();

                transaction.Commit();
                bTrue = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Commit Exception Type: {0}", e.GetType());
                Console.WriteLine("  Message: {0}", e.Message);
                Log.Error(e);
                bTrue = false;
                try
                {
                    transaction.Rollback();
                }
                catch (Exception ex2)
                {
                    // This catch block will handle any errors that may have occurred
                    // on the server that would cause the rollback to fail, such as
                    // a closed connection.
                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Console.WriteLine("  Message: {0}", ex2.Message);
                    Log.Error(ex2);
                }
                finally
                {
                    bTrue = false;
                    //con.Close();
                }
            }

            con.Close();
        }
        return bTrue;
    }

    public bool DeleteAllOrganizations()
    {
        bool bTrue = true;
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();

            SqlCommand command = con.CreateCommand();
            SqlTransaction transaction;
            transaction = con.BeginTransaction("DeleteAllOrganizations");
            command.Connection = con;
            command.Transaction = transaction;

            try
            {
                string query = $"DELETE Organization";
                Log.Debug(query);
                command.CommandText = query;
                command.ExecuteNonQuery();

                transaction.Commit();
                bTrue = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Commit Exception Type: {0}", e.GetType());
                Console.WriteLine("  Message: {0}", e.Message);
                Log.Error(e);
                bTrue = false;
                try
                {
                    transaction.Rollback();
                }
                catch (Exception ex2)
                {
                    // This catch block will handle any errors that may have occurred
                    // on the server that would cause the rollback to fail, such as
                    // a closed connection.
                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Console.WriteLine("  Message: {0}", ex2.Message);
                    Log.Error(ex2);
                }
                finally
                {
                    bTrue = false;
                    //con.Close();
                }
            }

            con.Close();
        }
        return bTrue;
    }



    public bool IsOrganizationExist(string organizationCode)
    {
        organizationCode = organizationCode.Contains("'") ? organizationCode.Replace("'", "''") : organizationCode;

        bool isExist = false;
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query = $"SELECT organizationCode FROM Organization WHERE organizationCode = '{organizationCode}'";
                Log.Debug(query);

                using (SqlCommand command = new SqlCommand(query, con))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        isExist = true;
                    }
                    else
                    {
                        isExist = false;
                    }
                }
                con.Close();
            }
        }
        catch (Exception e)
        {
            Log.Error(e);
            isExist = false;
        }

        return isExist;
    }
    #endregion Organization

    public bool AddChildOrganization(ChildOrganization childOrganization)
    {
        bool bTrue = true;
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();

            SqlCommand command = con.CreateCommand();
            SqlTransaction transaction;
            transaction = con.BeginTransaction("AddChildOrganizationTransaction");
            command.Connection = con;
            command.Transaction = transaction;

            try
            {
                string query
                    = $"INSERT INTO ChildOrganization (ChildOrganizationID, OrganizationID, PayerReference, ChildName, MemberID)" +
                    $" VALUES " +
                    $"(" +
                    $"'{childOrganization.ChildOrganizationID}','{childOrganization.Organization.OrganizationID}', '{childOrganization.PayerReference}'," +
                    $" '{childOrganization.ChildName}','{childOrganization.MemberID}'" +
                    $")";

                Log.Debug(query);
                command.CommandText = query;
                command.ExecuteNonQuery();

                transaction.Commit();

            }
            catch (Exception e)
            {
                Console.WriteLine("Commit Exception Type: {0}", e.GetType());
                Console.WriteLine("  Message: {0}", e.Message);
                Log.Error(e);
                bTrue = false;
                try
                {
                    transaction.Rollback();
                }
                catch (Exception ex2)
                {
                    // This catch block will handle any errors that may have occurred
                    // on the server that would cause the rollback to fail, such as
                    // a closed connection.
                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Console.WriteLine("  Message: {0}", ex2.Message);
                }
                finally
                {
                    //con.Close();
                    bTrue = false;
                }
            }
            con.Close();
        }

        bTrue = true;
        return bTrue;
    }

    public List<ChildOrganization> GetChildOrganizationByMemberID(string memberID)
    {
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        List<ChildOrganization> childOrganizations = new List<ChildOrganization>();

        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query =
                    "SELECT co.ChildOrganizationID AS COID, o.OrganizationID AS OrgID, o.OrganizationCode AS OrgCode, o.OrganizationName AS OrgName," +
                          " co.PayerReference, co.ChildName AS ChildName, co.MemberID AS MemberID " +
                    "  FROM ChildOrganization co, Organization o " +
                    " WHERE co.OrganizationID = O.OrganizationID "  +
                   $"   AND co.MemberID = '{memberID}'";

                Log.Debug(query);

                using (SqlCommand command = new SqlCommand(query, con))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ChildOrganization table = new ChildOrganization();
                            table.ChildOrganizationID = (string)reader["COID"];
                            table.Organization = new Organization
                            {
                                OrganizationID = (string)reader["OrgID"],
                                OrganizationCode = (string)reader["OrgCode"],
                                OrganizationName = (string)reader["OrgName"]
                            };
                            table.PayerReference = (reader["PayerReference"] != DBNull.Value) ? (string)reader["PayerReference"] : "";
                            table.ChildName = (reader["ChildName"] != DBNull.Value) ? (string)reader["ChildName"] : ""; 
                            table.MemberID = (string)reader["MemberID"];
                            childOrganizations.Add(table);
                        }
                    }
                }
                con.Close();
            }
        }
        catch (Exception e)
        {
            Log.Error(e);
            throw new Exception(e.ToString());
        }

        return childOrganizations;
    }

    public ChildOrganization GetChildOrganizationByChildOrganizationID(string childOrganizationID)
    {
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        ChildOrganization childOrganization = new ChildOrganization();

        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query =
                    "SELECT co.ChildOrganizationID AS COID, o.OrganizationID AS OrgID, o.OrganizationCode AS OrgCode, o.OrganizationName AS OrgName," +
                          " co.PayerReference, co.ChildName AS ChildName, co.MemberID AS MemberID " +
                    "  FROM ChildOrganization co, Organization o " +
                    " WHERE co.OrganizationID = O.OrganizationID " +
                   $"   AND co.ChildOrganizationID = '{childOrganizationID}'";

                Log.Debug(query);

                using (SqlCommand command = new SqlCommand(query, con))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ChildOrganization table = new ChildOrganization();
                            table.ChildOrganizationID = (string)reader["COID"];
                            table.Organization = new Organization
                            {
                                OrganizationID = (string)reader["OrgID"],
                                OrganizationCode = (string)reader["OrgCode"],
                                OrganizationName = (string)reader["OrgName"]

                            };
                            table.PayerReference = (reader["PayerReference"] != DBNull.Value) ? (string)reader["PayerReference"] : "";
                            table.ChildName = (reader["ChildName"] != DBNull.Value) ? (string)reader["ChildName"] : "";
                            table.MemberID = (string)reader["MemberID"];
                            childOrganization = table;
                            break;
                        }
                    }
                }
                con.Close();
            }
        }
        catch (Exception e)
        {
            Log.Error(e);
            throw new Exception(e.ToString());
        }

        return childOrganization;
    }


    public bool UpdateChildOrganization(ChildOrganization childOrganization)
    {

        childOrganization = ConversionHelper.ConvertPortalDbChildOrganizationForSql(childOrganization);

        bool bTrue = true;
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();

            SqlCommand command = con.CreateCommand();
            SqlTransaction transaction;
            transaction = con.BeginTransaction("UpdateChildOrgTransaction");
            command.Connection = con;
            command.Transaction = transaction;

            try
            {
                string insertDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                string query =
                    $"UPDATE ChildOrganization SET " +

                        $"OrganizationID = '{childOrganization.Organization.OrganizationID}', " +
                        $"PayerReference = '{childOrganization.PayerReference}', " +
                        $"ChildName = '{childOrganization.ChildName}', " +
                        $"MemberID = '{childOrganization.MemberID}' " +
                    $" WHERE ChildOrganizationID = '{childOrganization.ChildOrganizationID}'";

                Log.Debug(query);
                command.CommandText = query;
                command.ExecuteNonQuery();

                transaction.Commit();

            }
            catch (Exception e)
            {
                Console.WriteLine("Commit Exception Type: {0}", e.GetType());
                Console.WriteLine("  Message: {0}", e.Message);
                Log.Error(e);
                bTrue = false;
                try
                {
                    transaction.Rollback();
                }
                catch (Exception ex2)
                {
                    // This catch block will handle any errors that may have occurred
                    // on the server that would cause the rollback to fail, such as
                    // a closed connection.
                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Console.WriteLine("  Message: {0}", ex2.Message);
                }
                finally
                {
                    //con.Close();
                    bTrue = false;
                }
            }
            con.Close();
        }

        bTrue = true;
        return bTrue;
    }

    public bool DeleteChildOrganization(ChildOrganization childOrganization)
    {
        bool bTrue = true;
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();

            SqlCommand command = con.CreateCommand();
            SqlTransaction transaction;
            transaction = con.BeginTransaction("DeleteChildOrg");
            command.Connection = con;
            command.Transaction = transaction;

            try
            {
                string query = $"DELETE ChildOrganization WHERE ChildOrganizationID = '{childOrganization.ChildOrganizationID}'; ";
                Log.Debug(query);
                command.CommandText = query;
                command.ExecuteNonQuery();

                transaction.Commit();
                bTrue = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Commit Exception Type: {0}", e.GetType());
                Console.WriteLine("  Message: {0}", e.Message);
                Log.Error(e);
                bTrue = false;
                try
                {
                    transaction.Rollback();
                }
                catch (Exception ex2)
                {
                    // This catch block will handle any errors that may have occurred
                    // on the server that would cause the rollback to fail, such as
                    // a closed connection.
                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Console.WriteLine("  Message: {0}", ex2.Message);
                    Log.Error(ex2);
                }
                finally
                {
                    bTrue = false;
                    //con.Close();
                }
            }

            con.Close();
        }
        return bTrue;
    }

    #region OrderTx

    public bool AddOrderTx(OrderTx orderTx)
    {
        orderTx = ConversionHelper.ConvertPortalDbOrderTxForSql(orderTx);

        bool bTrue = true;
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();

            SqlCommand command = con.CreateCommand();
            SqlTransaction transaction;
            transaction = con.BeginTransaction("AddOrderTxTransaction");
            command.Connection = con;
            command.Transaction = transaction;

            try
            {
                string insertDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                string queryHeader =
                    "INSERT INTO OrderTx (" +
                            "OrderTxID, OrderTxDate, VendorCode, VendorOrderID, OrderName,  " +
                            "Total, BrainzCustomerID, OrganizationCode, Comments, BrainzPoint," +
                            "BrainzPointCalc, ShopifyOrderID ) " +
                    "VALUES " +
                    "( " +
                            $"'{orderTx.OrderTxID}', '{orderTx.OrderTxDate.ToString("yyyy-MM-dd HH:mm:ss")}', '{orderTx.VendorCode}', '{orderTx.VendorOrderID}', '{orderTx.OrderName}'," +
                            $"{orderTx.Total}, '{orderTx.BrainzCustomerID}','{orderTx.OrganizationCode}', '{orderTx.Comments}', {orderTx.BrainzPoint}," +
                            $"'{orderTx.BrainzPointCalc}', '{orderTx.ShopifyOrderID}' " +
                    ")";

                Log.Debug(queryHeader);
                command.CommandText = queryHeader;
                command.ExecuteNonQuery();

                //Insert items
                foreach (var item in orderTx.OrderItems)
                {
                    string queryItem =
                        "INSERT INTO OrderItem (" +
                                "OrderTxID, OrderItemID, OrderItemName, ProductName, Quantity, " +
                                "UnitPrice, Amount ) "
                        + "VALUES ( "
                        + $"'{orderTx.OrderTxID}', '{item.OrderItemID}', '{item.OrderItemName}','{item.ProductName}', {item.Quantity},"
                        + $"{item.UnitPrice}, {item.Amount}"
                        + ")";

                    Log.Debug(queryItem);
                    command.CommandText = queryItem;
                    command.ExecuteNonQuery();

                }

                transaction.Commit();

            }
            catch (Exception e)
            {
                Console.WriteLine("Commit Exception Type: {0}", e.GetType());
                Console.WriteLine("  Message: {0}", e.Message);
                Log.Error(e);
                bTrue = false;
                try
                {
                    transaction.Rollback();
                }
                catch (Exception ex2)
                {
                    // This catch block will handle any errors that may have occurred
                    // on the server that would cause the rollback to fail, such as
                    // a closed connection.
                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Console.WriteLine("  Message: {0}", ex2.Message);
                }
                finally
                {
                    con.Close();
                    bTrue = false;
                    //throw new Exception(e.Message);

                }
            }
            con.Close();
        }

        bTrue = true;
        return bTrue;
    }

    public List<OrderTx> GetOrderTxsByCustomerID(string customerID)
    {
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        List<OrderTx> orderTxs = new List<OrderTx>();

        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query
                    = "SELECT OrderTxID, OrderTxDate, VendorCode, VendorOrderID, OrderName, " +
                             "Total, BrainzCustomerID, OrganizationCode, Comments, BrainzPoint, " +
                             "BrainzPointCalc, ShopifyOrderID" +
                     "   FROM OrderTx" +
                    $"  WHERE BrainzCustomerID = '{customerID}'" +
                    $"  ORDER BY OrderTxDate desc";

                Log.Debug(query);

                using (SqlCommand command = new SqlCommand(query, con))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            OrderTx table = new OrderTx();
                            table.OrderTxID = (string)reader["OrderTxID"];
                            table.OrderTxDate = (DateTime)reader["OrderTxDate"];
                            table.VendorCode = (string)reader["VendorCode"];
                            table.VendorOrderID = (string)reader["VendorOrderID"];
                            table.OrderName = (string)reader["OrderName"];
                            table.Total = (decimal)reader["Total"];
                            table.BrainzCustomerID = (reader["BrainzCustomerID"] != System.DBNull.Value) ? (string)reader["BrainzCustomerID"] : "";
                            table.OrganizationCode = (string)reader["OrganizationCode"];
                            table.Comments = (reader["Comments"] != System.DBNull.Value) ? (string)reader["Comments"] : "";
                            table.BrainzPoint = (int)reader["BrainzPoint"];
                            table.BrainzPointCalc = (reader["Comments"] != System.DBNull.Value) ? (string)reader["BrainzPointCalc"] : "";
                            table.ShopifyOrderID = (string)reader["ShopifyOrderID"];

                            orderTxs.Add(table);

                        }
                    }
                }
                con.Close();
            }
        }
        catch (Exception e)
        {
            Log.Error(e);
            throw new Exception(e.ToString());
        }

        return orderTxs;
    }

    public OrderTx GetOrderTxsByOrderTxID(string orderTxID)
    {
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        OrderTx orderTx = new OrderTx();

        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query
                    = "SELECT OrderTxID, OrderTxDate, VendorCode, VendorOrderID, OrderName, " +
                             "Total, BrainzCustomerID, OrganizationCode, Comments, BrainzPoint, " +
                             "BrainzPointCalc, ShopifyOrderID" +
                     "   FROM OrderTx" +
                    $"  WHERE OrderTxID = '{orderTxID}'";

                Log.Debug(query);

                using (SqlCommand command = new SqlCommand(query, con))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            OrderTx table = new OrderTx();
                            table.OrderTxID = (string)reader["OrderTxID"];
                            table.OrderTxDate = (DateTime)reader["OrderTxDate"];
                            table.VendorCode = (string)reader["VendorCode"];
                            table.VendorOrderID = (string)reader["VendorOrderID"];
                            table.OrderName = (string)reader["OrderName"];
                            table.Total = (decimal)reader["Total"];
                            table.BrainzCustomerID = (string)reader["BrainzCustomerID"];
                            table.OrganizationCode = (string)reader["OrganizationCode"];
                            table.Comments = (reader["Comments"] != System.DBNull.Value) ? (string)reader["Comments"] : "";
                            table.BrainzPoint = (int)reader["BrainzPoint"];
                            table.BrainzPointCalc = (reader["Comments"] != System.DBNull.Value) ? (string)reader["BrainzPointCalc"] : "";
                            table.ShopifyOrderID = (string)reader["ShopifyOrderID"];

                            orderTx = table;

                        }
                    }
                }
                con.Close();
            }
        }
        catch (Exception e)
        {
            Log.Error(e);
            throw new Exception(e.ToString());
        }

        return orderTx;
    }

    public decimal GetEarnedBrainzPointsByCustomerID(string customerID)
    {
        decimal brainzPoint = 0;
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query = $"SELECT Sum(BrainzPoint) AS BrainzPoint " +
                                $" FROM OrderTx " +
                                $"WHERE BrainzCustomerID = '{customerID}' ";
                Log.Debug(query);

                using (SqlCommand command = new SqlCommand(query, con))
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    if (reader.Read())
                    {
                        string strPoint = reader["BrainzPoint"].ToString();
                        brainzPoint = Convert.ToDecimal(strPoint);
                    }

                }
                con.Close();
            }
        }
        catch (Exception e)
        {
            Log.Error(e);
            brainzPoint = 0;
        }

        return brainzPoint;
    }

    public List<OrderTxAnalysisDataModel> GetOrderTxs_GroupByOrg_Year_Month_TxCount(string year)
    {
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        List<OrderTxAnalysisDataModel> orderTxAnalisysDatas = new List<OrderTxAnalysisDataModel>();

        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query
                    = "SELECT OrganizationCode, Year([OrderTxDate]) AS TxYear,  Month([OrderTxDate]) as TxMonth,  Count(*) as TxCount " +
                     "   FROM OrderTx" +
                    $"  WHERE Year([OrderTxDate]) = '{year}'" +
                    $"  GROUP BY OrganizationCode, Year([OrderTxDate]), Month([OrderTxDate])"+
                    $"  ORDER BY OrganizationCode, Year([OrderTxDate]), Month([OrderTxDate])";

                Log.Debug(query);

                using (SqlCommand command = new SqlCommand(query, con))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            OrderTxAnalysisDataModel table = new OrderTxAnalysisDataModel();
                            table.GroupNyName = (string)reader["OrganizationCode"];
                            table.Year = (int)reader["TxYear"];
                            table.Month = (int)reader["TxMonth"];
                            table.Value = Convert.ToDouble((int)reader["TxCount"]);

                            orderTxAnalisysDatas.Add(table);

                        }
                    }
                }
                con.Close();
            }
        }
        catch (Exception e)
        {
            Log.Error(e);
            throw new Exception(e.ToString());
        }

        return orderTxAnalisysDatas;
    }

    public List<OrderTxAnalysisDataModel> GetOrderTxs_GroupByOrg_Year_Month_BrainzPoint(string year)
    {
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        List<OrderTxAnalysisDataModel> orderTxAnalisysDatas = new List<OrderTxAnalysisDataModel>();

        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query
                    = "SELECT OrganizationCode, Year([OrderTxDate]) AS TxYear,  Month([OrderTxDate]) as TxMonth,  Sum(BrainzPoint) as BrainzPoint " +
                     "   FROM OrderTx" +
                    $"  WHERE Year([OrderTxDate]) = '{year}'" +
                    $"  GROUP BY OrganizationCode, Year([OrderTxDate]), Month([OrderTxDate])" +
                    $"  ORDER BY OrganizationCode, Year([OrderTxDate]), Month([OrderTxDate])";

                Log.Debug(query);

                using (SqlCommand command = new SqlCommand(query, con))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            OrderTxAnalysisDataModel table = new OrderTxAnalysisDataModel();
                            table.GroupNyName = (string)reader["OrganizationCode"];
                            table.Year = (int)reader["TxYear"];
                            table.Month = (int)reader["TxMonth"];
                            table.Value = Convert.ToDouble((int)reader["BrainzPoint"]);

                            orderTxAnalisysDatas.Add(table);

                        }
                    }
                }
                con.Close();
            }
        }
        catch (Exception e)
        {
            Log.Error(e);
            throw new Exception(e.ToString());
        }

        return orderTxAnalisysDatas;
    }



    #endregion OrderTx


    #region BrainzPointTr

    public bool AddBrainzPointTr(BrainzPointTr brainzPointTr)
    {
        brainzPointTr = ConversionHelper.ConvertPortalDbBrainzPointTrForSql(brainzPointTr);

        bool bTrue = true;
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();

            SqlCommand command = con.CreateCommand();
            SqlTransaction transaction;
            transaction = con.BeginTransaction("AddBrainzPointTrTransaction");
            command.Connection = con;
            command.Transaction = transaction;

            try
            {
                string insertDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                string queryHeader =
                    "INSERT INTO BrainzPointTr (" +
                            "BrainzPointTrID, BrainzPointTrDate, MemberID, Amount, OrganizationCode,  " +
                            "PayeeReference, PayerReference, TrComments, BankTxID, Comments ) " +
                    "VALUES " +
                    "( " +
                            $"'{brainzPointTr.BrainzPointTrID}', '{brainzPointTr.BrainzPointTrDate.ToString("yyyy-MM-dd HH:mm:ss")}', '{brainzPointTr.MemberID}', {brainzPointTr.Amount}, '{brainzPointTr.OrganizationCode}'," +
                            $"'{brainzPointTr.PayeeReference}', '{brainzPointTr.PayerReference}','{brainzPointTr.TrComments}', '{brainzPointTr.BankTxID}', '{brainzPointTr.Comments}' " +
                    ")";

                Log.Debug(queryHeader);
                command.CommandText = queryHeader;
                command.ExecuteNonQuery();

                transaction.Commit();

            }
            catch (Exception e)
            {
                Console.WriteLine("Commit Exception Type: {0}", e.GetType());
                Console.WriteLine("  Message: {0}", e.Message);
                Log.Error(e);
                bTrue = false;
                try
                {
                    transaction.Rollback();
                }
                catch (Exception ex2)
                {
                    // This catch block will handle any errors that may have occurred
                    // on the server that would cause the rollback to fail, such as
                    // a closed connection.
                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Console.WriteLine("  Message: {0}", ex2.Message);
                }
                finally
                {
                    con.Close();
                    bTrue = false;
                    //throw new Exception(e.Message);

                }
            }
            con.Close();
        }

        bTrue = true;
        return bTrue;
    }

    public List<BrainzPointTr> GetBrainzPointTrsByMemberID(string memberID)
    {
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        List<BrainzPointTr> brainzPointTrs = new List<BrainzPointTr>();

        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query
                    = "SELECT BrainzPointTrID, BrainzPointTrDate, MemberID, Amount, OrganizationCode,  " +
                             "PayeeReference, PayerReference, TrComments, BankTxID, Comments " +
                     "   FROM BrainzPointTr" +
                    $"  WHERE MemberID = '{memberID}'" +
                    $"  ORDER BY BrainzPointTrDate desc";

                Log.Debug(query);

                using (SqlCommand command = new SqlCommand(query, con))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            BrainzPointTr table = new BrainzPointTr();
                            table.BrainzPointTrID = (string)reader["BrainzPointTrID"];
                            table.BrainzPointTrDate = (DateTime)reader["BrainzPointTrDate"];
                            table.MemberID = (string)reader["MemberID"];
                            table.Amount = (decimal)reader["Amount"];
                            table.OrganizationCode = (string)reader["OrganizationCode"];

                            table.PayeeReference = (string)reader["PayeeReference"];
                            table.PayerReference = (reader["PayerReference"] != System.DBNull.Value) ? (string)reader["PayerReference"] : "";
                            table.TrComments = (reader["TrComments"] != System.DBNull.Value) ? (string)reader["TrComments"] : "";

                            table.TrComments = (reader["TrComments"] != System.DBNull.Value) ? (string)reader["TrComments"] : "";
                            table.BankTxID = (reader["BankTxID"] != System.DBNull.Value) ? (string)reader["BankTxID"] : "";
                            table.Comments = (reader["Comments"] != System.DBNull.Value) ? (string)reader["Comments"] : "";

                            brainzPointTrs.Add(table);

                        }
                    }
                }
                con.Close();
            }
        }
        catch (Exception e)
        {
            Log.Error(e);
            throw new Exception(e.ToString());
        }

        return brainzPointTrs;
    }

    public BrainzPointTr GetBrainzPointTrByBrainzPointTrID(string brainzPointTrID)
    {
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        BrainzPointTr brainzPointTr = new BrainzPointTr();

        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query
                    = "SELECT BrainzPointTrID, BrainzPointTrDate, MemberID, Amount, OrganizationCode,  " +
                             "PayeeReference, PayerReference, TrComments, BankTxID, Comments " +
                     "   FROM BrainzPointTr" +
                    $"  WHERE BrainzPointTrID = '{brainzPointTrID}'";

                Log.Debug(query);

                using (SqlCommand command = new SqlCommand(query, con))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            BrainzPointTr table = new BrainzPointTr();
                            table.BrainzPointTrID = (string)reader["BrainzPointTrID"];
                            table.BrainzPointTrDate = (DateTime)reader["BrainzPointTrDate"];
                            table.MemberID = (string)reader["MemberID"];
                            table.Amount = (decimal)reader["Amount"];
                            table.OrganizationCode = (string)reader["OrganizationCode"];

                            table.PayeeReference = (string)reader["PayeeReference"];
                            table.PayerReference = (reader["PayerReference"] != System.DBNull.Value) ? (string)reader["PayerReference"] : "";
                            table.TrComments = (reader["TrComments"] != System.DBNull.Value) ? (string)reader["TrComments"] : "";

                            table.TrComments = (reader["TrComments"] != System.DBNull.Value) ? (string)reader["TrComments"] : "";
                            table.BankTxID = (reader["BankTxID"] != System.DBNull.Value) ? (string)reader["BankTxID"] : "";
                            table.Comments = (reader["Comments"] != System.DBNull.Value) ? (string)reader["Comments"] : "";

                            brainzPointTr = table;

                        }
                    }
                }
                con.Close();
            }
        }
        catch (Exception e)
        {
            Log.Error(e);
            throw new Exception(e.ToString());
        }

        return brainzPointTr;
    }


    public decimal GetSpentBrainzPointsByMemberID(string memberID)
    {
        decimal brainzPoint = 0;
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query = $"SELECT Sum(Amount) AS SpentBrainzPoint " +
                                $" FROM BrainzPointTr " +
                                $"WHERE MemberID = '{memberID}' ";
                Log.Debug(query);

                using (SqlCommand command = new SqlCommand(query, con))
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    if (reader.Read())
                    {
                        string strPoint = reader["SpentBrainzPoint"].ToString();
                        brainzPoint = Convert.ToDecimal(strPoint);
                    }

                }
                con.Close();
            }
        }
        catch (Exception e)
        {
            Log.Error(e);
            brainzPoint = 0;
        }

        return brainzPoint;
    }

    public bool UpdateBrainzPointReceipt(string brainzPointTrID, string receiptPathFile)
    {
        bool bTrue = true;
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();

            SqlCommand command = con.CreateCommand();
            SqlTransaction transaction;
            transaction = con.BeginTransaction("UpdateBzPtReceiptTransaction");
            command.Connection = con;
            command.Transaction = transaction;

            try
            {
                string query =
                    $"UPDATE BrainzPointTr SET " +
                        $"ReceiptPathFile = '{receiptPathFile}' " +
                    $" WHERE BrainzPointTrID = '{brainzPointTrID}'";

                Log.Debug(query);
                command.CommandText = query;
                command.ExecuteNonQuery();

                transaction.Commit();

            }
            catch (Exception e)
            {
                Console.WriteLine("Commit Exception Type: {0}", e.GetType());
                Console.WriteLine("  Message: {0}", e.Message);
                Log.Error(e);
                bTrue = false;
                try
                {
                    transaction.Rollback();
                }
                catch (Exception ex2)
                {
                    // This catch block will handle any errors that may have occurred
                    // on the server that would cause the rollback to fail, such as
                    // a closed connection.
                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Console.WriteLine("  Message: {0}", ex2.Message);
                }
                finally
                {
                    //con.Close();
                    bTrue = false;
                }
            }
            con.Close();
        }

        bTrue = true;
        return bTrue;
    }

    #endregion BrainzPointTr




}
