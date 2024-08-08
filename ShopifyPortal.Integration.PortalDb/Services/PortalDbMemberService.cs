using ShopifyPortal.Integration.PortalDb.Helpers;
using ShopifyPortal.Integration.PortalDb.Models;
using NLog;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;

namespace ShopifyPortal.Integration.PortalDb.Services;

public class PortalDbMemberService : IPortalDbMemberService
{
    private static Logger Log = LogManager.GetCurrentClassLogger();

    private PortalDbConnectionSettings PortalDbConnectionSettings { get; set; }
    public PortalDbMemberService(PortalDbConnectionSettings portalDbConnectionSettings)
    {
        this.PortalDbConnectionSettings = portalDbConnectionSettings;
    }

    public bool AddMember(Member member)
    {
        member = ConversionHelper.ConvertPortalDbUserForSql(member);

        bool bTrue = true;
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();

            SqlCommand command = con.CreateCommand();
            SqlTransaction transaction;
            transaction = con.BeginTransaction("AddMemberTransaction");
            command.Connection = con;
            command.Transaction = transaction;

            try
            {
                string insertDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                string query = $"INSERT INTO Member (MemberID, Email, Password, FirstName, LastName,"+
                     $" Role, Phone, Tags, CustomerID, BrainzPoint, " +
                     $" Address, Address2, City, Region, PostCode, " +
                     $" ImagePathFile, RegisteredDate, ModifiedDate) " +
                     $" VALUES " +
                     $"(" +
                     $"'{member.MemberID}','{member.Email}','{member.Password}', '{member.FirstName}', '{member.LastName}',"+
                     $"'{member.Role}','{member.Phone}','{member.Tags}','{member.CustomerID}',{member.BrainzPoint}," +
                     $"'{member.Address}','{member.Address2}','{member.City}','{member.Region}','{member.PostCode}'," +
                     $"'{member.ImagePathFile}','{member.RegisteredDate.ToString("yyyy-MM-dd HH:mm:ss")}', '{member.ModifiedDate.ToString("yyyy-MM-dd HH:mm:ss")}'"+
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

    public bool UpdateMemberBasic(Member member)
    {
        member = ConversionHelper.ConvertPortalDbUserForSql(member);

        bool bTrue = true;
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();

            SqlCommand command = con.CreateCommand();
            SqlTransaction transaction;
            transaction = con.BeginTransaction("AddMemberTransaction");
            command.Connection = con;
            command.Transaction = transaction;

            try
            {
                string insertDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                string query = $"UPDATE Member SET "; 
                if (!string.IsNullOrEmpty(member.FirstName)) { query += $" FirstName = '{member.FirstName}',"; }
                if (!string.IsNullOrEmpty(member.LastName)) { query += $" LastName = '{member.LastName}',"; }
                if (!string.IsNullOrEmpty(member.Phone)) { query += $" Phone = '{member.Phone}',"; }
                if (!string.IsNullOrEmpty(member.Tags)) { query += $" Tags = '{member.Tags}',"; }

                query += $" ModifiedDate = '{member.ModifiedDate.ToString("yyyy-MM-dd HH:mm:ss")}'" +
                         $" WHERE MemberID = '{member.MemberID}'";

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

    public bool UpdateMemberAddress(Member member)
    {
        member = ConversionHelper.ConvertPortalDbUserForSql(member);

        bool bTrue = true;
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();

            SqlCommand command = con.CreateCommand();
            SqlTransaction transaction;
            transaction = con.BeginTransaction("AddMemberTransaction");
            command.Connection = con;
            command.Transaction = transaction;

            try
            {
                string insertDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                string query = $"UPDATE Member SET ";
                if (!string.IsNullOrEmpty(member.Address)) { query += $" Address = '{member.Address}',"; }
                if (!string.IsNullOrEmpty(member.Address2)) { query += $" Address2 = '{member.Address2}',"; }
                if (!string.IsNullOrEmpty(member.City)) { query += $" City = '{member.City}',"; }
                if (!string.IsNullOrEmpty(member.Region)) { query += $" Region = '{member.Region}',"; }
                if (!string.IsNullOrEmpty(member.PostCode)) { query += $" PostCode = '{member.PostCode}',"; }

                query += $" ModifiedDate = '{member.ModifiedDate.ToString("yyyy-MM-dd HH:mm:ss")}'" +
                         $" WHERE MemberID = '{member.MemberID}'";

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

    public bool UpdateMemberImage(Member member)
    {
        bool bTrue = true;
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();

            SqlCommand command = con.CreateCommand();
            SqlTransaction transaction;
            transaction = con.BeginTransaction("UpdateMemberImageTransaction");
            command.Connection = con;
            command.Transaction = transaction;

            try
            {
                member.ModifiedDate = (member.ModifiedDate == null) ? DateTime.UtcNow : member.ModifiedDate;

                string query =
                    $"UPDATE Member SET " +
                        $"ImagePathFile = '{member.ImagePathFile}', " +
                        $"ModifiedDate = '{member.ModifiedDate.ToString("yyyy-MM-dd HH:mm:ss")}'" +
                    $" WHERE MemberID = '{member.MemberID}'";

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

    public bool UpdateMemberPassword(string email, string password)
    {
        bool bTrue = true;
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();

            SqlCommand command = con.CreateCommand();
            SqlTransaction transaction;
            transaction = con.BeginTransaction("UpdatePasswordTransaction");
            command.Connection = con;
            command.Transaction = transaction;

            try
            {
                string updateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                string query = $"UPDATE Member SET  Password = '{password}', ModifiedDate = '{updateDate}'" +
                               $" WHERE Email = '{email}'";

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

    public decimal AddMemberBrainzPoint(string customerID, decimal brainzPoint)
    {
        var member = GetMemberByCustomerID(customerID);
        if(member == null) 
        {
            Log.Warn($"No member found corresponding customerID : {customerID}");
            return 0; 
        }

        var newBrainzPoint = member.BrainzPoint + brainzPoint;

        double addedPoint = 0;
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();

            SqlCommand command = con.CreateCommand();
            SqlTransaction transaction;
            transaction = con.BeginTransaction("AddMemberPointTransaction");
            command.Connection = con;
            command.Transaction = transaction;

            try
            {
                string modifiedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                string query = $"UPDATE Member SET " +
                                        $" BrainzPoint = {newBrainzPoint}, " +
                                        $" ModifiedDate = '{modifiedDate}'" +
                                $" WHERE MemberID = '{member.MemberID}'";

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
                addedPoint = 0;
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
                    addedPoint = 0;
                }
            }
            con.Close();
        }


        member = GetMemberByMemberID(member.MemberID);
        if (member == null)
        {
            Log.Warn($"Get Added Member.BrainzPoint. No member found : {member.MemberID}");
            return 0;
        }

        return member.BrainzPoint;
    }

    public Member GetMemberByMemberID(string memberID)
    {
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        Member member = new Member();

        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query = "SELECT MemberID, Email, Password, FirstName, LastName,  " +
                                     " Role, Phone, Tags, CustomerID, BrainzPoint, " +
                                     " Address, Address2, City, Region, PostCode, " +
                                     " ImagePathFile, RegisteredDate, ModifiedDate " +
                               "  FROM Member" +
                              $" WHERE MemberID = '{memberID}'";

                Log.Debug(query);

                using (SqlCommand command = new SqlCommand(query, con))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Member table = new Member();
                            table.MemberID = (string)reader["MemberID"];
                            table.Email = (string)reader["Email"];
                            table.Password = (string)reader["Password"];
                            table.FirstName = (string)reader["FirstName"];
                            table.LastName = (string)reader["LastName"];
                            table.Role = (string)reader["Role"];
                            table.Phone = (reader["Phone"] != System.DBNull.Value) ? (string)reader["Phone"] : "";
                            table.Tags = (reader["Tags"] != System.DBNull.Value) ? (string)reader["Tags"] : "";
                            table.CustomerID = (reader["CustomerID"] != System.DBNull.Value) ? (string)reader["CustomerID"] : "";
                            table.BrainzPoint = (reader["BrainzPoint"] != System.DBNull.Value) ? (decimal)reader["BrainzPoint"] : 0;

                            table.Address = (reader["Address"] != System.DBNull.Value) ? (string)reader["Address"] : "";
                            table.Address2 = (reader["Address2"] != System.DBNull.Value) ? (string)reader["Address2"] : "";
                            table.City = (reader["City"] != System.DBNull.Value) ? (string)reader["City"] : "";
                            table.Region = (reader["Region"] != System.DBNull.Value) ? (string)reader["Region"] : "";
                            table.PostCode = (reader["PostCode"] != System.DBNull.Value) ? (string)reader["PostCode"] : "";

                            table.ImagePathFile = (reader["ImagePathFile"] != System.DBNull.Value) ? (string)reader["ImagePathFile"] : "";
                            table.RegisteredDate = (DateTime)reader["RegisteredDate"];
                            table.ModifiedDate = (DateTime)reader["ModifiedDate"];

                            member = table;
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

        return member;
    }

    public Member GetMemberByEmail(string email)
    {
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        Member member = null;

        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query = "SELECT MemberID, Email, Password, FirstName, LastName, " +
                                      "Role, Phone, Tags, CustomerID, BrainzPoint, " +
                                      "Address, Address2, City, Region, PostCode, " +
                                      "ImagePathFile, RegisteredDate, ModifiedDate " +
                              "  FROM Member" +
                             $" WHERE Email = '{email}'";

                Log.Debug(query);

                using (SqlCommand command = new SqlCommand(query, con))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        member = new Member();
                        while (reader.Read())
                        {
                            Member table = new Member();
                            table.MemberID = (string)reader["MemberID"];
                            table.Email = (string)reader["Email"];
                            table.Password = (string)reader["Password"];
                            table.FirstName = (string)reader["FirstName"];
                            table.LastName = (string)reader["LastName"];
                            table.Role = (string)reader["Role"];
                            table.Phone = (reader["Phone"] != System.DBNull.Value) ? (string)reader["Phone"] : "";
                            table.Tags = (reader["Tags"] != System.DBNull.Value) ? (string)reader["Tags"] : "";
                            table.CustomerID = (reader["CustomerID"] != System.DBNull.Value) ? (string)reader["CustomerID"] : "";
                            table.BrainzPoint = (reader["BrainzPoint"] != System.DBNull.Value) ? (decimal)reader["BrainzPoint"] : 0;

                            table.Address = (reader["Address"] != System.DBNull.Value) ? (string)reader["Address"] : "";
                            table.Address2 = (reader["Address2"] != System.DBNull.Value) ? (string)reader["Address2"] : "";
                            table.City = (reader["City"] != System.DBNull.Value) ? (string)reader["City"] : "";
                            table.Region = (reader["Region"] != System.DBNull.Value) ? (string)reader["Region"] : "";
                            table.PostCode = (reader["PostCode"] != System.DBNull.Value) ? (string)reader["PostCode"] : "";

                            table.ImagePathFile = (reader["ImagePathFile"] != System.DBNull.Value) ? (string)reader["ImagePathFile"] : "";
                            table.RegisteredDate = (DateTime)reader["RegisteredDate"];
                            table.ModifiedDate = (DateTime)reader["ModifiedDate"];

                            member = table;
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

        return member;
    }

    public Member GetMemberByCustomerID(string customerID)
    {
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        Member member = new Member();

        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query = "SELECT MemberID, Email, Password, FirstName, LastName, " +
                                      "Role, Phone, Tags, CustomerID, BrainzPoint, " +
                                      "Address, Address2, City, Region, PostCode, " +
                                      "ImagePathFile, RegisteredDate, ModifiedDate " +
                              "  FROM Member" +
                             $" WHERE CustomerID = '{customerID}'";

                Log.Debug(query);

                using (SqlCommand command = new SqlCommand(query, con))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Member table = new Member();
                            table.MemberID = (string)reader["MemberID"];
                            table.Email = (string)reader["Email"];
                            table.Password = (string)reader["Password"];
                            table.FirstName = (string)reader["FirstName"];
                            table.LastName = (string)reader["LastName"];
                            table.Role = (string)reader["Role"];
                            table.Phone = (reader["Phone"] != System.DBNull.Value) ? (string)reader["Phone"] : "";
                            table.Tags = (reader["Tags"] != System.DBNull.Value) ? (string)reader["Tags"] : "";
                            table.CustomerID = (reader["CustomerID"] != System.DBNull.Value) ? (string)reader["CustomerID"] : "";
                            table.BrainzPoint = (reader["BrainzPoint"] != System.DBNull.Value) ? (decimal)reader["BrainzPoint"] : 0;

                            table.Address = (reader["Address"] != System.DBNull.Value) ? (string)reader["Address"] : "";
                            table.Address2 = (reader["Address2"] != System.DBNull.Value) ? (string)reader["Address2"] : "";
                            table.City = (reader["City"] != System.DBNull.Value) ? (string)reader["City"] : "";
                            table.Region = (reader["Region"] != System.DBNull.Value) ? (string)reader["Region"] : "";
                            table.PostCode = (reader["PostCode"] != System.DBNull.Value) ? (string)reader["PostCode"] : "";


                            table.ImagePathFile = (reader["ImagePathFile"] != System.DBNull.Value) ? (string)reader["ImagePathFile"] : "";
                            table.RegisteredDate = (DateTime)reader["RegisteredDate"];
                            table.ModifiedDate = (DateTime)reader["ModifiedDate"];

                            member = table;
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

        return member;
    }

    public List<Member> GetAllMembers()
    {
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        List<Member> members = new List<Member>();

        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query = "SELECT MemberID, Email, Password, FirstName, LastName,  " +
                                      "Role, Phone, Tags, CustomerID, BrainzPoint, " +
                                      "Address, Address2, City, Region, PostCode, " +
                                      "ImagePathFile, RegisteredDate, ModifiedDate " +
                               "  FROM Member";

                Log.Debug(query);

                using (SqlCommand command = new SqlCommand(query, con))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Member table = new Member();
                            table.MemberID = (string)reader["MemberID"];
                            table.Email = (string)reader["Email"];
                            table.Password = (string)reader["Password"];
                            table.FirstName = (string)reader["FirstName"];
                            table.LastName = (string)reader["LastName"];
                            table.Role = (string)reader["Role"];
                            table.Phone = (reader["Phone"] != System.DBNull.Value) ? (string)reader["Phone"] : "";
                            table.Tags = (reader["Tags"] != System.DBNull.Value) ? (string)reader["Tags"] : "";
                            table.CustomerID = (reader["CustomerID"] != System.DBNull.Value) ? (string)reader["CustomerID"] : "";
                            table.BrainzPoint = (reader["BrainzPoint"] != System.DBNull.Value) ? (decimal)reader["BrainzPoint"] : 0;

                            table.Address = (reader["Address"] != System.DBNull.Value) ? (string)reader["Address"] : "";
                            table.Address2 = (reader["Address2"] != System.DBNull.Value) ? (string)reader["Address2"] : "";
                            table.City = (reader["City"] != System.DBNull.Value) ? (string)reader["City"] : "";
                            table.Region = (reader["Region"] != System.DBNull.Value) ? (string)reader["Region"] : "";
                            table.PostCode = (reader["PostCode"] != System.DBNull.Value) ? (string)reader["PostCode"] : "";

                            table.ImagePathFile = (reader["ImagePathFile"] != System.DBNull.Value) ? (string)reader["ImagePathFile"] : "";
                            table.RegisteredDate = (DateTime)reader["RegisteredDate"];
                            table.ModifiedDate = (DateTime)reader["ModifiedDate"];
                            members.Add(table);

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

        return members;
    }
    public List<Member> GetMemberByUserName(string name)
    {
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        List<Member> members = new List<Member>();

        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query = "SELECT MemberID, Email, Password, FirstName, LastName, " +
                                      "Role, Phone, Tags, CustomerID, BrainzPoint, " +
                                      "Address, Address2, City, Region, PostCode, " +
                                      "ImagePathFile, RegisteredDate, ModifiedDate, Concat(FirstName, ' ', LastName) AS FullName " +
                                " FROM Member" +
                              $" WHERE FullName like '%{name}%'" ;

                Log.Debug(query);

                using (SqlCommand command = new SqlCommand(query, con))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Member table = new Member();
                            table.MemberID = (string)reader["MemberID"];
                            table.Email = (string)reader["Email"];
                            table.Password = (string)reader["Password"];
                            table.FirstName = (string)reader["FirstName"];
                            table.LastName = (string)reader["LastName"];
                            table.Role = (string)reader["Role"];
                            table.Phone = (reader["Phone"] != System.DBNull.Value) ? (string)reader["Phone"] : "";
                            table.Tags = (reader["Tags"]!= System.DBNull.Value) ? (string)reader["Tags"] : "";
                            table.CustomerID = (reader["CustomerID"] != System.DBNull.Value) ? (string)reader["CustomerID"] : "";
                            table.BrainzPoint = (reader["BrainzPoint"] != System.DBNull.Value) ? (decimal)reader["BrainzPoint"] : 0;

                            table.Address = (reader["Address"] != System.DBNull.Value) ? (string)reader["Address"] : "";
                            table.Address2 = (reader["Address2"] != System.DBNull.Value) ? (string)reader["Address2"] : "";
                            table.City = (reader["City"] != System.DBNull.Value) ? (string)reader["City"] : "";
                            table.Region = (reader["Region"] != System.DBNull.Value) ? (string)reader["Region"] : "";
                            table.PostCode = (reader["PostCode"] != System.DBNull.Value) ? (string)reader["PostCode"] : "";

                            table.ImagePathFile = (reader["ImagePathFile"] != System.DBNull.Value) ? (string)reader["ImagePathFile"] : "";
                            table.RegisteredDate = (DateTime)reader["RegisteredDate"];
                            table.ModifiedDate = (DateTime)reader["ModifiedDate"];

                            members.Add(table);

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

        return members;
    }

    public bool DeleteMemberByEmail(string email)
    {
        bool bTrue = true;
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();

            SqlCommand command = con.CreateCommand();
            SqlTransaction transaction;
            transaction = con.BeginTransaction("DeleteMember");
            command.Connection = con;
            command.Transaction = transaction;

            try
            {
                string query = $"DELETE Member WHERE Email = '{email}'";
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

    public bool DeleteAllMembers()
    {
        bool bTrue = true;
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();

            SqlCommand command = con.CreateCommand();
            SqlTransaction transaction;
            transaction = con.BeginTransaction("DeleteAllMembers");
            command.Connection = con;
            command.Transaction = transaction;

            try
            {
                string query = $"DELETE Member";
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



    public bool IsUserExist(string email)
    {
        bool isExist = false;
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query = $"SELECT MemberID FROM Member WHERE Email = '{email}'";
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

    #region UserAction

    public bool AddUserAction(UserAction userAction)
    {
        bool bTrue = true;
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();

            SqlCommand command = con.CreateCommand();
            SqlTransaction transaction;
            transaction = con.BeginTransaction("AddUserActionTransaction");
            command.Connection = con;
            command.Transaction = transaction;

            try
            {
                //string insertDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                string queryHeader =
                    "INSERT INTO UserAction (" +
                            "UserID, Action, CreatedDate ) VALUES " +
                    "( " +
                            $"'{userAction.UserID}', '{userAction.Action}', '{userAction.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss")}' " +
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


    #endregion UserAction



    public bool AddLogin2FA(string email, string secretKey)
    {
        bool bTrue = true;
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();

            SqlCommand command = con.CreateCommand();
            SqlTransaction transaction;
            transaction = con.BeginTransaction("AddLogin2FATransaction");
            command.Connection = con;
            command.Transaction = transaction;

            try
            {
                string insertDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                string query = $"INSERT INTO Login2FA (MemberEmail, SecretKey, CreatedDate) " +
                     $" VALUES " +
                $"(" +
                $"'{email}','{secretKey}','{insertDate}'" +
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

    public Login2FA GetLogin2FA(string email)
    {
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        Login2FA login2FA = new Login2FA();

        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query = "SELECT MemberEmail, SecretKey, CreatedDate " +
                              "  FROM Login2FA" +
                             $" WHERE MemberEmail = '{email}'";

                Log.Debug(query);

                using (SqlCommand command = new SqlCommand(query, con))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Login2FA table = new Login2FA();
                            table.MemberEmail = (string)reader["MemberEmail"];
                            table.SecretKey = (string)reader["SecretKey"];
                            table.CreatedDate = (DateTime)reader["CreatedDate"];

                            login2FA = table;
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

        return login2FA;
    }


    public bool DeleteLogin2FA(string email)
    {
        bool bTrue = true;
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();

            SqlCommand command = con.CreateCommand();
            SqlTransaction transaction;
            transaction = con.BeginTransaction("DeleteLogin2FA");
            command.Connection = con;
            command.Transaction = transaction;

            try
            {
                string query = $"DELETE Login2FA WHERE MemberEmail = '{email}'";
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



    public bool AddResetPassword(string email, string resetPasswordID)
    {
        bool bTrue = true;
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();

            SqlCommand command = con.CreateCommand();
            SqlTransaction transaction;
            transaction = con.BeginTransaction("AddResetPasswordTransaction");
            command.Connection = con;
            command.Transaction = transaction;

            try
            {
                string insertDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                string query = $"INSERT INTO ResetPassword (MemberEmail, ResetPasswordID, CreatedDate) " +
                     $" VALUES " +
                $"(" +
                $"'{email}','{resetPasswordID}','{insertDate}'" +
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

    public ResetPassword GetResetPassword(string email, string resetPasswordID)
    {
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        ResetPassword resetPassword = null;

        try
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query = "SELECT MemberEmail, ResetPasswordID, CreatedDate " +
                               "  FROM ResetPassword" +
                              $" WHERE MemberEmail = '{email}'" +
                              $"   AND ResetPasswordID = '{resetPasswordID}'";

                Log.Debug(query);

                using (SqlCommand command = new SqlCommand(query, con))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        resetPassword = new ResetPassword();
                        while (reader.Read())
                        {
                            ResetPassword table = new ResetPassword();
                            table.MemberEmail = (string)reader["MemberEmail"];
                            table.ResetPasswordID = (string)reader["ResetPasswordID"];
                            table.CreatedDate = (DateTime)reader["CreatedDate"];

                            resetPassword = table;
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

        return resetPassword;
    }
    public bool DeleteResetPassword(string email)
    {
        bool bTrue = true;
        string connectionString = PortalDbConnectionSettings.GetDbConnectionString();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();

            SqlCommand command = con.CreateCommand();
            SqlTransaction transaction;
            transaction = con.BeginTransaction("DeleteResetPassword");
            command.Connection = con;
            command.Transaction = transaction;

            try
            {
                string query = $"DELETE ResetPassword WHERE MemberEmail = '{email}'";
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
}
