using System.Collections.Generic;
using System.IO;
using System.Text;
using OfficeOpenXml;

namespace BeyondIT.MicroLoan.Infrastructure.Extensions
{
    internal static class StringUtils
    {
        private static string DuplicateTicksForSql(this string s)
        {
            return s.Replace("'", "''");
        }

        internal static string ToDelimitedString(this List<string> list, string delimiter = ":", bool insertSpaces = false, string qualifier = "", bool duplicateTicksForSql = false)
        {
            var result = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                string initialStr = duplicateTicksForSql ? list[i].DuplicateTicksForSql() : list[i];
                result.Append(qualifier == string.Empty ? initialStr : string.Format("{1}{0}{1}", initialStr, qualifier));
                if (i < list.Count - 1)
                {
                    result.Append(delimiter);
                    if (insertSpaces)
                    {
                        result.Append(' ');
                    }
                }
            }
            return result.ToString();
        }
    }

    public static class EpplusCsvConverter
    {
        public static byte[] ConvertToCsv(this ExcelPackage package, string delimiter = ",", string qualifier = "")
        {
            var worksheet = package.Workbook.Worksheets[1];

            var maxColumnNumber = worksheet.Dimension.End.Column;
            var currentRow = new List<string>(maxColumnNumber);
            var totalRowCount = worksheet.Dimension.End.Row;
            var currentRowNum = 1;

            var memory = new MemoryStream();

            using (var writer = new StreamWriter(memory, Encoding.ASCII))
            {
                while (currentRowNum <= totalRowCount)
                {
                    BuildRow(worksheet, currentRow, currentRowNum, maxColumnNumber, delimiter, qualifier);
                    WriteRecordToFile(currentRow, writer, currentRowNum, totalRowCount, delimiter);
                    currentRow.Clear();
                    currentRowNum++;
                }
            }

            return memory.ToArray();
        }

        private static void WriteRecordToFile(List<string> record, TextWriter sw, int rowNumber, int totalRowCount, string delimiter)
        {
            var delimitedRecord = record.ToDelimitedString(delimiter);

            if (rowNumber == totalRowCount)
            {
                sw.Write(delimitedRecord);
            }
            else
            {
                sw.WriteLine(delimitedRecord);
            }
        }

        private static void BuildRow(ExcelWorksheet worksheet, ICollection<string> currentRow, int currentRowNum, int maxColumnNumber, string delimiter, string qualifier)
        {
            for (int i = 1; i <= maxColumnNumber; i++)
            {
                var cell = worksheet.Cells[currentRowNum, i];
                AddCellValue(cell == null ? string.Empty : GetCellText(cell), currentRow, delimiter, qualifier);
            }
        }

        /// <summary>
        /// Can't use .Text: http://epplus.codeplex.com/discussions/349696
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private static string GetCellText(ExcelRangeBase cell)
        {
            return cell.Value == null ? string.Empty : cell.Text;
        }

        private static void AddCellValue(string text, ICollection<string> record, string delimiter, string fieldQulifier)
        {
            if (!string.IsNullOrEmpty(fieldQulifier) && text.Contains(fieldQulifier))
            {
                text = text.Replace("\"", "\"\"");
            }

            if (text.Contains(delimiter) || text.Contains("\""))
            {
                text = text.Replace(delimiter, $"\"{text}\"");
            }

            text = text.Replace("\n", string.Empty);

            record.Add(text);
        }
    }
}