using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SwikiToCsv
{
    public class SwikiTable
    {
        public List<List<string>> Rows { get; set; }

        public SwikiTable()
        {
            Rows = new List<List<string>>();
        }

        internal void SaveToCsv(string filePath)
        {
            var csv = new StringBuilder();
            csv.AppendLine( HeaderLine() );
            foreach (var row in Rows)
            {
                var columnNumber = 0;
                foreach (var cell in row)
                {
                    if (!string.IsNullOrEmpty(cell))
                    {
                        columnNumber++;
                        var csvCell = CleanCell(
                            cell, 
                            columnNumber);
                        csv.Append(csvCell);
                        csv.Append(",");
                    }
                }
                csv.AppendLine();
            };
            File.WriteAllText(filePath, csv.ToString());
        }

        private string HeaderLine()
        {
            var sb = new StringBuilder();
            string[] columnNames = {
                "Date",
                "Reports Served",
                "Weekly",
                "401 outages",
                "Disconnects",
                "Disconnect Percentage",
                "Max Concurrent Users",
                "Slowest Report",
                "Slowest Report Timing",
                "Most Popular Report",
                "Reports",
                "Unused Reports",
                "New Subscriptions",
                "Subs Downloaded"
            };
            foreach (var item in columnNames)
            {
                sb.Append('"' + item + '"' + ',');
            }
            return sb.ToString();
        }

        private string CleanCell(
            string cell,
            int columnNumber)
        {
            cell = cell.Trim();
            if (ColumnContainsReportNames(columnNumber))
            {
                cell = cell.Replace("<nowiki> ", string.Empty);
                cell = cell.Replace(" </nowiki>", string.Empty);
                if (ColumnContainsTiming(columnNumber))
                {
                    cell = SplitOutTiming(cell);
                }
            }
            if (ColumnContainsDayNames(columnNumber))
            {
                cell = cell.Replace("Mon", string.Empty);
                cell = cell.Replace("Tue", string.Empty);
                cell = cell.Replace("Wed", string.Empty);
                cell = cell.Replace("Thu", string.Empty);
                cell = cell.Replace("Fri", string.Empty);
                cell = cell.Replace("Sat", string.Empty);
                cell = cell.Replace("Sun", string.Empty);
            }
            if (ColumnContainsPercentage(columnNumber))
            {
                cell = SplitPercentage(cell);
            }
            return cell;
        }

        private string SplitOutTiming(
            string cell)
        {
            var reportName = cell.Substring(0, cell.Length - 9);
            var timing = cell.Substring(cell.Length - 8, 8);
            return $"{reportName},{timing}";
        }

        private bool ColumnContainsTiming(
            int columnNumber)
        {
            return columnNumber.Equals(7);
        }

        private string SplitPercentage(
            string cell)
        {
            string[] parts = cell.Split(' ');
            parts[1] = parts[1].Replace("(", string.Empty);
            parts[1] = parts[1].Replace("%)", string.Empty);
            return $"{parts[0]},{parts[1]}";
        }

        private bool ColumnContainsPercentage(int columnNumber)
        {
            return columnNumber.Equals(5);
        }

        private bool ColumnContainsDayNames(
            int columnNumber)
        {
            return columnNumber.Equals(1);
        }

        private static bool ColumnContainsReportNames(
            int columnNumber)
        {
            return columnNumber.Equals(7)
                || columnNumber.Equals(8);
        }

        public SwikiLineType LineType(
            string line)
        {
            if (line.Contains("||"))
            {
                if (line.Contains("**"))
                    return SwikiLineType.TableHeader;
                else
                    return SwikiLineType.TableRow;
            }
            else
                return SwikiLineType.Text;
        }

        public void AddLine(
            string line)
        {
            //  split line on the double pipe
            string[] cells = line.Split("||");
            var row = new List<string>(cells);
            Rows.Add(row);
        }
    }
}
