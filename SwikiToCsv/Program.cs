using System;

namespace SwikiToCsv
{
    class Program
    {
        static void Main(string[] args)
        {
            //  Read Swiki page file
            var swikiTable = new SwikiTable();
            var swikiReader = new SwikiReader(
                "dailystats.swiki");
            var rowCount = 0;
            foreach (var line in swikiReader.Lines)
            {
                if (swikiTable.LineType(
                    line)
                    .Equals(
                        SwikiLineType.TableRow))
                {
                    Console.WriteLine(line);
                    swikiTable.AddLine(line);
                    rowCount++;
                }
            }
            Console.WriteLine(
                $"Table rows found : {rowCount}");

            //  Spit out .CSV
            swikiTable.SaveToCsv(
                "dailystats.csv");
        }
    }

}
