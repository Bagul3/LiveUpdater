using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace LiveUpdate
{
    public class HDFParser
    {
        private LiveStockRepo liveStockRepo;
        private string guid;

        public HDFParser()
        {
            var ticks = new DateTime(2016, 1, 1).Ticks;
            var ans = DateTime.Now.Ticks - ticks;
            guid = ans.ToString("x");

            liveStockRepo = new LiveStockRepo();
        }

        public void ParseFileToUPD(string fullPath, string fileName, string location)
        {
            SaveHDFFile(fileName, location);
            var content = File.ReadAllText(fullPath);
            string[] transactionKeys = { "TheRecordSale", "TheRecordLayawaySale", "TheRecordApproAppro" };
            bool containTransactionKey = false;

            foreach(var key in transactionKeys)
            {
                if (IsRecordSale(content, key))
                {
                    if(key == "TheRecordApproAppro")
                    {
                        Console.WriteLine("Processing:" + key); 
                    }

                    containTransactionKey = true;
                    break;
                }
            }

            if (!containTransactionKey)
            {
                new LogWriter("File: " + fileName + " contains no transaction keys for processing.");
                return;
            }

            var skus = RegexMatch(content, "Code\\s*.\\d{12}");

            if (skus.Count() == 0) {
                new LogWriter("No SKUs found for HDF file: " + fileName);
                return;
            }

            
            for(var i=0; i< skus.Count();i++)
            {
                var sku = skus[i].Split(new string[] { "Code \"" }, StringSplitOptions.None)?[1];
                if (sku != null && sku?.Length > 11)
                {
                    var hdfModel = new HDFModel
                    {
                        FileName = fileName,
                        Location = location,
                        SKU = sku
                    };

                    AppendToUPDFile(hdfModel);
                }                
            }
        }

        private void SaveHDFFile(string fileName, string location)
        {
            var hdf = new HDF
            {
                hdf_name = fileName,
                branch_location = location
            };

            liveStockRepo.Insert(hdf);
        }

        private void AppendToUPDFile(HDFModel hdfModel)
        {
            File.AppendAllText($"{ System.Configuration.ConfigurationManager.AppSettings["UPD"]}{guid}{".upd"}", hdfModel.ToString());
        }
        
        private bool IsRecordSale(string content, string transactionKey)
        {
            return Regex.Match(content, transactionKey, RegexOptions.IgnoreCase).Success;
        }

        private List<string> RegexMatch(string content, string pattern)
        {
            var result = new List<string>();
            var match = new Regex(pattern);
            foreach(Match capture in match.Matches(content))
            {
                result.Add(capture.Value);
            }
            
            return result;
        }

        public bool DoesFileExist(string filename)
        {
            var result = liveStockRepo.DoesHDFExist(filename);
            return result == null;
        }

    }
}
