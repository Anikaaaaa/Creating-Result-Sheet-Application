using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ResultSheet
{
    public class Helper
    {
        public IEnumerable<string[]> ReadFile(string fileName)
        {
            try
            {
                var lines = File.ReadAllText(fileName).Split('\n');
                var contentInLines = lines.Skip(1).Select(l => l.Split(','));

                return contentInLines;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        public void Print(List<string> contentsToWrite)
        {
            try
            {
                File.WriteAllLines("SortedResult.csv", contentsToWrite.ToArray());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
