using System.Collections.Generic;
using System.IO;

namespace SwikiToCsv
{
    public class SwikiReader
    {
        public List<string> Lines { get; set; }

        public SwikiReader(
            string filePath)
        {
            var swikiFile = File.ReadAllLines(
                filePath);
            Lines = new List<string>(
                swikiFile);
        }

    }
}