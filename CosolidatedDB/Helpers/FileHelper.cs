using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CosolidatedDB.Helpers
{
    public class FileHelper
    {

        public static IEnumerable<string> LoadItemsLines(string fileName)
        {
            StreamReader streamReader = File.OpenText(fileName);
            return LoadItemsLines(streamReader);
        }

        public static IEnumerable<string> LoadItemsLines(StreamReader streamReader)
        {
            string line = "";

            while ((line = streamReader.ReadLine()) != null)
            {
                if(line.Trim().Length == 0) continue;

                yield return line;
            }
        }
    }
}
