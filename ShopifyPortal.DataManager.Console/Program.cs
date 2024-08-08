

using ShopifyPortal.Integration.PortalDb;
using ShopifyPortal.Integration.PortalDb.Models;
using ShopifyPortal.Integration.PortalDb.Services;
using ShopifyPortal.Shared;
using ShopifyPortal.Shared.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using Peter.Integration.Shopify;
using Peter.Integration.Shopify.Models;
using System;
using System.Diagnostics.Metrics;
using System.Reflection;
using System.Transactions;
using System.Xml.Linq;

namespace ShopifyPortal.DataManager.Console;

internal class Program
{
    private static Logger Log = LogManager.GetCurrentClassLogger();
    private static PortalDbConnectionSettings PortalDbConnectionSettings { get; set; }
    private static ShopifySettings ShopifySettings { get; set; }


    static void Main(string[] args)
    {
        if (!LoadApplication()) { return; }

        while (true)
        {
            System.Console.WriteLine($"\r\n");
            System.Console.WriteLine($"-------------------------------------------------------------------------");
            System.Console.WriteLine($"--------------------- ShopifyPortal.DataManager  ------------------");
            System.Console.WriteLine($"-------------------------------------------------------------------------");
            System.Console.WriteLine("1.Insert Point levels");
            System.Console.WriteLine("11.Insert Default Organizations,   12.Insert Default vendors,     13.Insert Default Mmebers");
            System.Console.WriteLine("14.Insert Sample OrderTx");

            System.Console.WriteLine("31.Copy Shopify customers to BrainzParentsPortal members");
            System.Console.WriteLine("");

            System.Console.WriteLine("0.Exit");

            System.Console.Write("\r\nSelect number (Please use Number KeyPad) : ");
            string selectedMenu = System.Console.ReadLine();
            if (selectedMenu == "0") break;

            switch (selectedMenu)
            {
                case "1":
                    {
                        InsertPointLevels();

                        break;
                    }
                case "11":
                    {
                        InsertDefaultOrganizations();

                        break;
                    }
                case "12":
                    {
                        InsertDefaultVendors();

                        break;
                    }
                case "13":
                    {
                        InsertDefaultMembers();

                        break;
                    }
                case "14":
                    {
                        InsertSampleOrderTx();

                        break;
                    }

                case "31":
                    {
                        CopyShopifyCustomersToPortalDbUsers();

                        break;
                    }


            }
        }
    }

    static bool LoadApplication()
    {
        bool bTrue = true;

        ShopifySettings = new ShopifySettings();
        if (File.Exists(GlobalSettings.Instance.PortalDbConnectionSettingsPathFile))
        {
            ShopifySettings
                = JsonConvert.DeserializeObject<ShopifySettings>(File.ReadAllText(GlobalSettings.Instance.ShopifySettingsPathFile));
        }
        else
        {
            System.Console.WriteLine("No PortalDbConnectionSettings file", "ERROR");
            bTrue = false;
            return bTrue;
        }

        PortalDbConnectionSettings = new PortalDbConnectionSettings();
        if (File.Exists(GlobalSettings.Instance.PortalDbConnectionSettingsPathFile))
        {
            PortalDbConnectionSettings
                = JsonConvert.DeserializeObject<PortalDbConnectionSettings>(File.ReadAllText(GlobalSettings.Instance.PortalDbConnectionSettingsPathFile));
        }
        else
        {
            System.Console.WriteLine("No PortalDbConnectionSettings file", "ERROR");
            bTrue = false;
            return bTrue;
        }


        return bTrue;
    }

    static bool CopyShopifyCustomersToPortalDbUsers()
    {

        return ShopifyToPortalDbHelper.CopyShopifyCustomerToPortalDb(PortalDbConnectionSettings, ShopifySettings);
    }

    static bool InsertPointLevels()
    {
        List<PointLevel> pointLevels = new List<PointLevel>()
        {
            new PointLevel { PointLevelID = Guid.NewGuid().ToString(), PointLevelCode = "Diamond", PointLevelValue = 800000  },
            new PointLevel { PointLevelID = Guid.NewGuid().ToString(), PointLevelCode = "Platinum", PointLevelValue = 400000  },
            new PointLevel { PointLevelID = Guid.NewGuid().ToString(), PointLevelCode = "Gold", PointLevelValue = 200000  },
            new PointLevel { PointLevelID = Guid.NewGuid().ToString(), PointLevelCode = "Silver", PointLevelValue = 100000  },
            new PointLevel { PointLevelID = Guid.NewGuid().ToString(), PointLevelCode = "Bronze", PointLevelValue = 50000  },
        };

        IPortalDbService portalDbService = new PortalDbService(PortalDbConnectionSettings);

        foreach (var pointLevel in pointLevels)
        {
            portalDbService.AddPointLevel(pointLevel);
        }
        return true;
    }

    static bool InsertDefaultOrganizations()
    {
        DateTime currentDate = DateTime.UtcNow;
        List<Organization> orgs = new List<Organization>()
        {
            new Organization { 
                OrganizationID = Guid.NewGuid().ToString(), OrganizationType = "School", 
                OrganizationCode = "AUCKLAND_GRAMMAR", OrganizationName = "Auckland Grammar", Phone = "", Email = "", ContactPerson = "", 
                ParentCommission = 0.40m, PayerRefHelpText = "AUCKLAND_GRAMMAR-Payer Ref help text", Memo = "", 
                ImagePathFile = $"Images/Organization/Auckland_Grammar.png", CreatedDate = currentDate, ModifiedDate= currentDate },
            new Organization { 
                OrganizationID = Guid.NewGuid().ToString(), OrganizationType = "School",
                OrganizationCode = "MASSEY_HIGH", OrganizationName = "Massey High School", Phone = "",Email = "", ContactPerson = "", 
                ParentCommission = 0.40m, PayerRefHelpText = "MASSEY_HIGH-Payer Ref help text", Memo = "",
                ImagePathFile = $"Images/Organization/Massey_High_School.png", CreatedDate = currentDate,ModifiedDate = currentDate },
            new Organization { 
                OrganizationID = Guid.NewGuid().ToString(), OrganizationType = "School",
                OrganizationCode = "OTAGO_BOYS_HIGH",OrganizationName = "Otago Boys' High School", Phone = "",Email = "",ContactPerson = "",
                ParentCommission = 0.40m, PayerRefHelpText = "OTAGO_BOYS_HIGH-Payer Ref help text", Memo = "",
                ImagePathFile = $"Images/Organization/Otago_Boys_High_School.png", CreatedDate = currentDate,ModifiedDate = currentDate },
            new Organization { 
                OrganizationID = Guid.NewGuid().ToString(), OrganizationType = "School",
                OrganizationCode = "WAITAKERE_COLLEGE",OrganizationName = "Waitakere College", Phone = "",Email = "",ContactPerson = "",
                ParentCommission = 0.40m, PayerRefHelpText = "WAITAKERE_COLLEGE-Payer Ref help text", Memo = "",
                ImagePathFile = $"Images/Organization/Waitakere_College.png", CreatedDate = currentDate,ModifiedDate = currentDate }

        };

        IPortalDbService portalDbService = new PortalDbService(PortalDbConnectionSettings);

        foreach(var org in orgs)
        {

            if (portalDbService.IsOrganizationExist(org.OrganizationName)) { continue; }


            portalDbService.AddOrganization(org);
        }
        return true;
    }

    static bool InsertDefaultVendors()
    {
        DateTime defaultDate = DateTime.UtcNow;
        List<Vendor> vendors = new List<Vendor>()
        {

            new Vendor { VendorID = "DEFAULT_VENDOR", VendorCode = "DEF_VENDOR", VendorName = "DEFAULT VENDOR", Phone="", Email = "", ContactPerson = "", 
                Memo = "", VendorCommission = 0.04m, ParentCommission = 0.80m, ImagePathFile = $"",
                ModifiedDate = defaultDate },
            new Vendor { VendorID = "5af73382-82a5-4cb1-a0f7-b67427fb8ae4", VendorCode = "TWG", VendorName = "The Warehouse Group", Phone = "", Email = "", ContactPerson = "",
                Memo = "", VendorCommission = 0.04m, ParentCommission = 0.80m, ImagePathFile = $"",
                ModifiedDate = defaultDate },
        };

        IPortalDbService portalDbService = new PortalDbService(PortalDbConnectionSettings);

        foreach (var vendor in vendors)
        {
            portalDbService.AddVendor(vendor);
        }
        return true;
    }

    static bool InsertDefaultMembers()
    {
        DateTime defaultDate = DateTime.UtcNow;
        List<Member> members = new List<Member>()
        {
            new Member
            {
                MemberID = "DEFAULT_ADMIN_11",Email = "peter@myshopify.com.au",FirstName = "Admin-Peter",LastName = "Kim",
                Password = EncryptionHelper.SHA512("Brainz123!@#"), Phone = "", Role = "Administrator",
                CustomerID = "DEFAULT_ADMIN_11",  BrainzPoint = 0, ImagePathFile = "",  ModifiedDate = DateTime.UtcNow,    RegisteredDate = DateTime.UtcNow
            },
            new Member
            {
                MemberID = "DEFAULT_ADMIN_12",Email = "michael@myshopify.com.au",FirstName = "Michael",LastName = "Smith",
                Password = EncryptionHelper.SHA512("Brainz123!@#"), Phone = "", Role = "Administrator",
                CustomerID = "DEFAULT_ADMIN_12",  BrainzPoint = 0, ImagePathFile = "", ModifiedDate = DateTime.UtcNow,    RegisteredDate = DateTime.UtcNow
            },
            new Member
            {
                MemberID = "DEFAULT_MEMBER_11",Email = "john@myshopify.com.au",FirstName = "John",LastName = "Williams",
                Password = EncryptionHelper.SHA512("Brainz123!@#"), Phone = "", Role = "User",
                CustomerID = "DEFAULT_MEMBER_11",  BrainzPoint = 0, ImagePathFile = "", ModifiedDate = DateTime.UtcNow,    RegisteredDate = DateTime.UtcNow
            },


        };

        IPortalDbMemberService portalDbMemberService = new PortalDbMemberService(PortalDbConnectionSettings);

        foreach (var member in members)
        {
            portalDbMemberService.AddMember(member);
        }
        return true;
    }

    static bool InsertSampleOrderTx()
    {
        IPortalDbService portalDbService = new PortalDbService(PortalDbConnectionSettings);

        //DateTime defaultDate = DateTime.UtcNow;

        Random random = new Random();

        for(int i = 0; i < 1000; i++)
        {
            DateTime? txDate = null;

            if (i <= 200)
            {
                txDate = DateTime.Today.AddMonths(-4);
            }
            else if( i > 200 && i <= 400)
            {
                txDate = DateTime.Today.AddMonths(-3);
            }
            else if (i > 400 && i <= 600)
            {
                txDate = DateTime.Today.AddMonths(-2);
            }
            else if (i > 600 && i <= 800)
            {
                txDate = DateTime.Today.AddMonths(-1);
            }
            else if(i > 800 && i <= 1000 )
            {
                txDate = DateTime.Today;
            }

            var orderTx = MakeBaseOrderTx(txDate);

            //var brainzPoint = CalculateBrainzPoint(portalDbService, orderTx);
            string pointCalc = "";
            var organization = portalDbService.GetOrganizationByOrganizationCode(orderTx.OrganizationCode);
            var vendor = portalDbService.GetVendorByVendorCode(orderTx.VendorCode);

            decimal purchasedPrice = orderTx.Total;
            pointCalc = $"Purchased Price [{purchasedPrice}] * ";
            decimal vendorCommission = vendor.VendorCommission;
            pointCalc += $"VendorCommission [{vendor.VendorCode} - {vendorCommission}] * ";

            decimal parentCommission = organization.ParentCommission;

            if (parentCommission != 0)
            {
                pointCalc += $"ParentCommission [Organization : {organization.OrganizationCode} - {parentCommission}]  ";
            }
            else
            {
                parentCommission = vendor.ParentCommission;
                pointCalc += $"ParentCommission [Vendor : {vendor.VendorCode} - {parentCommission}]  ";
            }
            
            var tmpPoint = purchasedPrice * vendorCommission * parentCommission;

            pointCalc += $" = {tmpPoint} * 100";
            tmpPoint = tmpPoint * 100;
            
            pointCalc += $" = {tmpPoint}";

            int brainzPoint = (int)tmpPoint;
            //var brainzPoint = Math.Truncate(100 * tmpPoint) / 100; //Truncate Two decimal places without rounding

            pointCalc += $" = Brainz Point [{brainzPoint}] ";


            // BrainzPoint
            orderTx.BrainzPoint = brainzPoint;
            orderTx.BrainzPointCalc = pointCalc;

            portalDbService.AddOrderTx(orderTx);

            IPortalDbMemberService portalDbMemberService = new PortalDbMemberService(PortalDbConnectionSettings);
            var newPoint = portalDbMemberService.AddMemberBrainzPoint(orderTx.BrainzCustomerID, orderTx.BrainzPoint);
        }

        return true;
    }

    static OrderTx MakeBaseOrderTx(DateTime? txDate)
    {
        Random random = new Random();
        string txID = Guid.NewGuid().ToString();
        string vendorCode = "TWG";

        string organizationCode = "";
        int orgCode = random.Next(1, 5);
        if(orgCode == 1) { organizationCode = "MASSEY_HIGH"; } 
        else if (orgCode == 2) { organizationCode = "WAITAKERE_COLLEGE"; }
        else if (orgCode == 3) { organizationCode = "AUCKLAND_GRAMMAR"; }
        else if (orgCode == 4) { organizationCode = "OTAGO_BOYS_HIGH"; }

        string brainzCustomerID = "";
        int randCustomerID = random.Next(1, 4);
        if (randCustomerID == 1) { brainzCustomerID = "DEFAULT_ADMIN_01"; }
        else if (randCustomerID == 2) { brainzCustomerID = "DEFAULT_MEMBER_01"; }
        else if (randCustomerID == 3) { brainzCustomerID = "DEFAULT_MEMBER_02"; }

        int qty1 = random.Next(1, 5); int qty2 = random.Next(1, 5); int qty3 = random.Next(1, 5);
        decimal unitPrice1 = random.Next(10, 100);
        //decimal unitPrice2 = random.Next(10, 100);
        //decimal unitPrice3 = random.Next(10, 100);

        OrderTx transaction = new OrderTx
        {
            OrderTxID = txID,
            OrderTxDate = txDate.Value,
            VendorCode = vendorCode,
            VendorOrderID = random.Next(10000, 9999999).ToString(),
            OrderName = $"Test Order : {txID}",
            Total = unitPrice1,
            BrainzCustomerID = brainzCustomerID.ToString(),
            OrganizationCode = organizationCode,
            Comments = "",

            //OrderItems = new List<OrderItem>
            //{ 
            //    new OrderItem {
            //        OrderItemID = txID + "-1", OrderItemName = "ItemName1", ProductName = "", Quantity = qty1, UnitPrice = unitPrice1, Amount = qty1 * unitPrice1
            //    },
            //    new OrderItem {
            //        OrderItemID = txID + "-2", OrderItemName = "ItemName2", ProductName = "", Quantity = qty2, UnitPrice = unitPrice2, Amount = qty2 * unitPrice2
            //    },
            //    new OrderItem {
            //        OrderItemID = txID + "-3", OrderItemName = "ItemName3", ProductName = "", Quantity = qty3, UnitPrice = unitPrice3, Amount = qty3 * unitPrice3
            //    },
            //},
        };

        return transaction;
    }

    
}