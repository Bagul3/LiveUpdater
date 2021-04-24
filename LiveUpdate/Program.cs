using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LiveUpdate
{
    class Program
    {
        static HDFParser HDFParser;
        static LogWriter LogWriter;

        static void Main(string[] args)
        {
            try
            {
                LogWriter = new LogWriter();


                Console.WriteLine("Locating Bmoney files " + System.Configuration.ConfigurationManager.AppSettings["Ballymoney"] + DateTime.Now.ToString("yyyy-MM-dd") + "\\");
                LogWriter.LogWrite("Locating Bmoney files " + System.Configuration.ConfigurationManager.AppSettings["Ballymoney"] + DateTime.Now.ToString("yyyy-MM-dd") + "\\");                                

                var bmoney = System.Configuration.ConfigurationManager.AppSettings["Ballymoney"] + DateTime.Now.ToString("yyyy-MM-dd") + "\\";
                Console.WriteLine("Collecting files");
                var hdfFullPaths = GetFullPaths(bmoney);
                LogWriter.LogWrite("Locating Bmoney files complete, number of files found: " + hdfFullPaths.Count);               

                Console.WriteLine("Till files achieved. Checking for new entry.");
                LogWriter.LogWrite("Till files achieved. Checking for new entry.");
                ProcessFiles(hdfFullPaths, "Bmoney");

                Console.WriteLine("Processing Nards files" + System.Configuration.ConfigurationManager.AppSettings["Ballymoney"] + DateTime.Now.ToString("yyyy-MM-dd") + "\\");
                LogWriter.LogWrite("Processing Nards files" + System.Configuration.ConfigurationManager.AppSettings["NArds"] + DateTime.Now.ToString("yyyy-MM-dd") + "\\");

                var nards = System.Configuration.ConfigurationManager.AppSettings["NArds"] + DateTime.Now.ToString("yyyy-MM-dd") + "\\";
                hdfFullPaths = GetFullPaths(nards);
                Console.WriteLine("Till files achieved. Checking for new entry.");
                LogWriter.LogWrite("Till files achieved. Checking for new entry.");
                ProcessFiles(hdfFullPaths, nards);
                LogWriter.LogWrite("Locating NArds files complete, number of files found: " + hdfFullPaths.Count);

                Console.WriteLine("Task completed successfully.");
                LogWriter.LogWrite("Task completed successfully.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: Unable to process files.");
                LogWriter.LogWrite("Error: Unable to process files.");
                LogWriter.LogWrite(e.Message);
                LogWriter.LogWrite(e.StackTrace);
            }

        }

        private static void ProcessFiles(List<string> hdfFullPaths, string location)
        {
            
            foreach (var fullPaths in hdfFullPaths)
            {
                var fileName = Path.GetFileNameWithoutExtension(fullPaths);
                HDFParser = new HDFParser();
                if (HDFParser.DoesFileExist(fileName))                    
                    HDFParser.ParseFileToUPD(fullPaths, fileName, location);
            }
        }

        private static List<string> GetFullPaths(string locationPath)
        {
            return Directory.GetFiles(locationPath, "*.hdf")
                                     .Select(Path.GetFullPath)
                                     .ToList();    
        }
    }
}
