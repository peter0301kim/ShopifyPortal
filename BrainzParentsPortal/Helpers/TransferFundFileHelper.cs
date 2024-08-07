using HtmlAgilityPack;
using NLog;
using NLog.Fluent;

namespace BrainzParentsPortal.Helpers
{
    public class TransferFundFileHelper
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();
        public static bool WriteCsvFile(string trCsvPath,
                                      string paytecTrID, string memberName,  string date, string point, 
                                      string payeeReference, string payerReference)
        {
            bool bTrue = true;

            try
            {
                string csvFileName = $"{DateTime.Now.ToString("yyyyMMdd")}.csv";
                string csvPathFile = Path.Combine(trCsvPath, csvFileName);

                string csvContents = $"{memberName},{paytecTrID},{date},{payeeReference},{payerReference},{point}";

                if (File.Exists(csvPathFile))
                {
                    File.WriteAllText(csvPathFile, csvContents);
                }
                else
                {
                    File.AppendAllText(csvPathFile, csvContents);
                }
                
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                bTrue = false;
            }

            return bTrue;
        }
    }
}
