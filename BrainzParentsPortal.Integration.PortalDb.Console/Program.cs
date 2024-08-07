
using BrainzParentsPortal.Integration.PortalDb;
using BrainzParentsPortal.Integration.PortalDb.Models;
using BrainzParentsPortal.Integration.PortalDb.Services;
using BrainzParentsPortal.Shared;
using Newtonsoft.Json;
using NLog;

namespace BrainzParentsPortal.Integration.PortalDb.Console
{
    internal class Program
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();
        private static PortalDbConnectionSettings PortalDbConnectionSettings { get; set; }
        static void Main(string[] args)
        {
            if (!LoadApplication()) { return; }

            while (true)
            {
                System.Console.WriteLine($"\r\n");
                System.Console.WriteLine($"-------------------------------------------------------------------------");
                System.Console.WriteLine($"-------------------------- Portal DB Manager ----------------------------");
                System.Console.WriteLine($"-------------------------------------------------------------------------");
                System.Console.WriteLine("11.Get a user by email");
                System.Console.WriteLine("");

                System.Console.WriteLine("0.Exit");

                System.Console.Write("\r\nSelect number (Please use Number KeyPad) : ");
                string selectedMenu = System.Console.ReadLine();
                if (selectedMenu == "0") break;

                switch (selectedMenu)
                {
                    case "11":
                        {

                            System.Console.Write("\r\nPlease input email : ");
                            string email = System.Console.ReadLine();
                            var portalUser = GetPortalUserByEmail(email);
                            if (portalUser != null)
                            {
                                System.Console.Write(JsonConvert.SerializeObject(portalUser, Formatting.Indented));
                            }
                            else
                            {
                                System.Console.WriteLine($" There is no data associated with email : {email}");
                            }

                            break;
                        }
                    
                }
            }
        }

        static bool LoadApplication()
        {
            bool bTrue = true;

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
            }

            return bTrue;
        }

        static Member GetPortalUserByEmail(string email)
        {
            IPortalDbMemberService portalDbService = new PortalDbMemberService(PortalDbConnectionSettings);
            var portalUser = portalDbService.GetMemberByEmail(email);

            return portalUser;
        }

    }
}
