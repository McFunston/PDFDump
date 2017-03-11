using System;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace PDFDump
{
    class Program
    {
        static void Main(string[] args)
        {
            ProcessPDFs(GetPDFList());
            //string fileName;

            //fileName = @"Resume_MicaFunston.pdf";
            
            //var results = GetTextFromAllPages(fileName);
            //WriteText(fileName, results);
        }

        public static List<string> GetPDFList()
        {
            List<String> PDFs = new List<string>();
            var folder = new DirectoryInfo(System.IO.Directory.GetCurrentDirectory());
            foreach (var fi in folder.EnumerateFiles("*.pdf"))
            {
                PDFs.Add(fi.Name);
            }
            return PDFs;
        }

        public static void ProcessPDFs(List<string> PDFList)
        {
            foreach(var pdf in PDFList)
            {
                var txt = GetTextFromAllPages(pdf);
                WriteText(pdf, txt);
            }
        }

        public static void WriteText(string fileName, string textToWrite)
        {
            string txtFile = fileName.Remove(fileName.Length - 4);
            txtFile = txtFile + ".txt";

            var file = System.IO.File.CreateText(@txtFile);
            file.Write(textToWrite);
            file.Dispose();
        }

        public static string GetTextFromAllPages(String pdfPath)
        {            
            StringWriter output = new StringWriter();

            try
            {
                PdfReader reader = new PdfReader(pdfPath);
                for (int i = 1; i <= reader.NumberOfPages; i++)
                    output.WriteLine(PdfTextExtractor.GetTextFromPage(reader, i, new SimpleTextExtractionStrategy()));
                return output.ToString();
            }

            catch (Exception e)
            {
                output.Write($"{pdfPath} was not able to be processed. The following exception was thrown: <{e.Message}>");
                return output.ToString();
            }
            
        }
    }

}
