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
            string path;
            if (args.Length == 0 || !(CheckArguments(args[0])))
            {                
                var currentFolder = new DirectoryInfo(System.IO.Directory.GetCurrentDirectory());
                path = currentFolder.FullName;
            }

            else path = args[0];

            ProcessPDFs(GetPDFList(path));
        }

        public static bool CheckArguments(string arguments)
        {
            if (File.Exists(arguments) || Directory.Exists(arguments)) return true;            
            return false;
        }

        public static List<string> GetPDFList(string path)
        {
            List<String> PDFList = new List<string>();
            
            if (!(path.ToLower().Contains(".pdf")))
            {
                var folder = new DirectoryInfo(path);
                foreach (var fi in folder.EnumerateFiles("*.pdf"))
                {
                    PDFList.Add(fi.FullName);
                }
            }
            else PDFList.Add(path);
            return PDFList;
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
