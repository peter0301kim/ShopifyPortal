using BrainzParentsPortal.Shared;
using HiQPdf;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using NLog;

namespace BrainzParentsPortal.Helpers;

public static class ReceiptHelper
{
    private static Logger Log = LogManager.GetCurrentClassLogger();

    public static string CreateReceipt(string trReceiptPath, string trReceiptTemplatePathFile,
                                      string memberName, string date, string point, string payeeReference, string payerReference,
                                      string bankTxID)
    {
        Log.Info("Start of CreateReceipt - Station : {0}, UserName : {1}, Date : {2}, Payment type : {3}, Credit added : {4}, FinalBalance : {5}",
                                 memberName, date, point, payeeReference, payerReference, bankTxID);

        HtmlDocument hDoc = new HtmlDocument();

        string trReceiptFullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", trReceiptTemplatePathFile);

        hDoc.Load(trReceiptFullPath);

        HtmlTextNode nodeName = hDoc.DocumentNode.SelectSingleNode("//label[@id='lblMemberName']//text()") as HtmlTextNode;
        nodeName.Text = memberName;

        HtmlTextNode nodeDate = hDoc.DocumentNode.SelectSingleNode("//label[@id='lblDate']//text()") as HtmlTextNode;
        nodeDate.Text = date;

        HtmlTextNode nodePoint = hDoc.DocumentNode.SelectSingleNode("//label[@id='lblPoint']//text()") as HtmlTextNode;
        nodePoint.Text = point;

        HtmlTextNode nodePayeeRef = hDoc.DocumentNode.SelectSingleNode("//label[@id='lblPayeeReference']//text()") as HtmlTextNode;
        nodePayeeRef.Text = payeeReference;

        HtmlTextNode nodePayerRef = hDoc.DocumentNode.SelectSingleNode("//label[@id='lblPayerReference']//text()") as HtmlTextNode;
        nodePayerRef.Text = payerReference;

        HtmlTextNode nodeBankTxID = hDoc.DocumentNode.SelectSingleNode("//label[@id='lblBankTxID']//text()") as HtmlTextNode;
        nodeBankTxID.Text = bankTxID;

        string fileName = $"{DateTime.Now.ToString("yyyyMMddHHmmss")}-{memberName}" + ".html";

        string newReceiptFullFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", trReceiptPath, fileName);
        Log.Info($"--- End of CreateReceipt - ReceiptFullFilePath:{newReceiptFullFilePath}---");

        hDoc.Save(newReceiptFullFilePath);

        /*
        //Create new receipt file
        System.IO.FileInfo fileinfo = new System.IO.FileInfo(GlobalSettings.Instance.ReceiptPaymentFilePath);
        string newReceiptFilePath = fileinfo.Directory.Name + @"/" + fileName;
        Log.Info("--- ReceiptFilePath : {0} ---", newReceiptFilePath);
        hDoc.Save(newReceiptFilePath);

        string newReceiptFullFilePath = Path.GetFullPath(newReceiptFilePath);
        Log.Info("--- newReceiptFullFilePath ---{0} ----------", newReceiptFullFilePath);
        */

        return newReceiptFullFilePath;
    }



    public static string ConvertHtmlToPdf(string htmlFilePath, string destDir)
    {
        Log.Info($"--- Start of ConvertHtmlToPdf - HtmlFileName:{htmlFilePath}, DestDir:{destDir} ---");
        string fileName = Path.GetFileName(htmlFilePath);
        string fileNameWithoutExtention = Path.GetFileNameWithoutExtension(fileName);
        string pdfFileName = fileNameWithoutExtention + ".pdf";
        string pdfFilePathFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", destDir, pdfFileName);

        Uri uri = new Uri(htmlFilePath);

        HtmlToPdf htmlToPdfConverter = new HtmlToPdf();

        // set PDF page size, orientation and margins 
        //htmlToPdfConverter.Document.PageSize = PdfPageSize.A4;
        htmlToPdfConverter.Document.PageOrientation = PdfPageOrientation.Portrait;
        htmlToPdfConverter.Document.Margins = new PdfMargins(0);
        // convert HTML to PDF 
        byte[] pdfBuffer = htmlToPdfConverter.ConvertUrlToMemory(uri.ToString());

        File.WriteAllBytes(pdfFilePathFile, pdfBuffer); // Requires System.IO
                                                        ////////////////////////End of converting from html to PDF
        Log.Info($"--- End of ConvertHtmlToPdf - PdfFilePath:{pdfFilePathFile} ---");
        return pdfFilePathFile;
    }



}
