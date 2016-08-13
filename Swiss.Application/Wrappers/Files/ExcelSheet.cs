using System.Collections.Generic;
using System.Linq;
using System.IO;
using ClosedXML.Excel;
using System.Data;

namespace Swiss
{
    /// <summary>
    /// Wrapper class stores contents of Excel or CSV file as arrays of strings
    /// </summary>
    public class ExcelSheet
    { 
        //content is stored as Lists for dynamic addition and removal
        private List<string> _header { get; set; }
        private List<string[]> _grid { get; set; }

        //content is accesses as arrays
        public string[] Header { get { return _header.ToArray(); } }
        public string[][] Grid { get { return _grid.ToArray(); } }

        public ExcelSheet()
        {
            _header = new List<string>();
            _grid = new List<string[]>();
        }

        public ExcelSheet(string[] header, string[][] grid)
        {
            _header = header == null ? new List<string>() : header.ToList();
            _grid = grid == null ? new List<string[]>() : grid.ToList();
        }

        public void SetHeader(string[] header)
        {
            _header = header.ToList();
        }

        public void SetGrid(string[][] grid)
        {
            _grid = grid.ToList();
        }

        public void AddRow(string[] row)
        {
            _grid.Add(row);
        }

        public void AddMultipleRows(IEnumerable<string[]> rows)
        {
            _grid.AddRange(rows);
        }

        /// <summary>
        /// Method returns specific column of table based on index
        /// </summary>
        public string[] GetColumnByIndex(int index)
        {
            return Grid.GetColumn(index);
        }

        /// <summary>
        /// Method gets specific column of table based on the name of that column (returns null if no such column is found)
        /// </summary>
        public string[] GetColumnByName(string name)
        {
            int index = _header.IndexOf(name);

            if(index > 0)
            {
                return Grid.ToArray().GetColumn(index);
            }

            return null;
        }

        /// <summary>
        /// Method combines header and grid to compose the complete content of the Excel Sheet
        /// </summary>
        /// <returns></returns>
        private string[][] GetCompleteContent()
        {
            string[][] body = new string[_grid.Count + 1][];
            body[0] = Header.ToArray();

            for (int i = 0; i < _grid.Count; i++)
            {
                body[i + 1] = Grid[i].Select(elem => elem.Replace("\n", "\r\n")).ToArray();
            }

            return body;
        }

        /// <summary>
        /// Writes the contents of the ExcelSheet into a .CSV file
        /// </summary>
        public void WriteToCSV(string path)
        {
            var body = GetCompleteContent().Select(row => row.JoinOnDelimeter(",")).ToArray();
            File.WriteAllLines(path, body);
        }

        /// <summary>
        /// Writes the contents of the ExcelSheet into a .XLSX file
        /// </summary>
        public void WriteToXLSX(string path)
        {
            XLWorkbook workbook = new XLWorkbook();

            var body = GetCompleteContent();

            if(Header.Length == 0)
            {
                Enumerable.Range(0, body.Width()).ForEach(index => _header.Add(index.ToString()));
            }

            DataTable table = body.ToDataTable("Test");
            workbook.Worksheets.Add(table);

            workbook.SaveAs(path);
        }
    }
}
