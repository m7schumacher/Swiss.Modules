using ClosedXML.Excel;
using Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace Swiss
{
    /// <summary>
    /// Various utility methods for reading CSV and XLSX files
    /// </summary>
    public class ExcelUtility
    {
        private static string[] validExtensions = new string[] { ".csv", ".xls", ".xlsx" };

        /// <summary>
        /// Method reads a XLSX file, parses content, and wraps it in an ExcelSheet
        /// Accepts .csv, .xls, and .xlsx files
        /// </summary>
        public static List<ExcelSheet> ReadXLSX(string path, bool isFirstRowColumnNames)
        {
            List<DataTable> sheets = new List<DataTable>();
            string extension = Path.GetExtension(path);

            if(validExtensions.Contains(extension))
            {
                Stream filestream = File.OpenRead(path);

                using (var reader = ExcelReaderFactory.CreateOpenXmlReader(filestream))
                {
                    reader.IsFirstRowAsColumnNames = false;
                    DataSet data = reader.AsDataSet();

                    sheets = new List<DataTable>(data.Tables.Cast<DataTable>());
                }
            }
            else
            {
                throw new Exception("Invalid File Type: " + extension);
            }

            var excelSheets = sheets.Select(sheet => ReadDataTable(sheet)).ToList();
            
            return excelSheets;
        }

        /// <summary>
        /// Method reads a CSV file, parses content, and wraps it in an ExcelSheet
        /// </summary>
        public static ExcelSheet ReadCSV(string pathToFile, char delimeter = ',', bool hasHeader = true)
        {
            var lines = File.ReadAllLines(pathToFile);

            string[] hd = hasHeader ? lines[0].Split(delimeter).WhereNotEmpty().ToArray() : null;
            string[][] body = lines.Skip(hasHeader ? 1 : 0)
                .Select(line => line.Split(delimeter).WhereNotEmpty().ToArray())
                .ToArray();

            return new ExcelSheet(hd, body);
        }

        /// <summary>
        /// Method writes a DataTable to a new Excel Workbook (.XLSX) file at the specified location
        /// </summary>
        public static void WriteDataTable(DataTable table, string path)
        {
            if (string.IsNullOrEmpty(table.TableName))
                table.TableName = "Default";

            XLWorkbook workbook = new XLWorkbook();
            workbook.Worksheets.Add(table);
            workbook.SaveAs(path);
        }

        /// <summary>
        /// Method reads a DataTable into a two-dimensional array and then wraps it in an ExcelSheet
        /// </summary>
        public static ExcelSheet ReadDataTable(DataTable table)
        {
            string[][] grid = new string[table.Rows.Count][];
            int[] range = Enumerable.Range(0, table.Rows.Count).ToArray();

            foreach(var index in range)
            {
                grid[index] = new string[table.Columns.Count];

                for (int j = 0; j < grid[index].Length; j++)
                {
                    grid[index][j] = table.Rows[index][j].ToString();
                }
            }

            var header = grid.First();
            var body = grid.Skip(1).Where(row => !row.AreAllEmpty()).ToArray();

            return new ExcelSheet(header, body);
        }
    }
}
