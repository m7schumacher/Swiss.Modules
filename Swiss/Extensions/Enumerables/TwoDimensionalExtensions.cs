using System.Data;
using System.Linq;

namespace Swiss
{
    public static class TwoDimensionalExtensions
    {
        /// <summary>
        /// Method returns the contents of a column, given an index, from this grid
        /// </summary>
        public static T[] GetColumn<T>(this T[][] grid, int index)
        {
            if (index < 0) return null;

            T[] result = new T[grid.Length];

            for (int i = 0; i < grid.Length; i++)
            {
                if(grid[i].Length > index)
                {
                    result[i] = grid[i][index];
                }
            }

            return result;
        }

        /// <summary>
        /// Method returns the height of this grid (# of rows)
        /// </summary>
        public static int Height<T>(this T[][] grid)
        {
            return grid.Length;
        }

        /// <summary>
        /// Method returns the width of this grid (# of columns in largest row)
        /// </summary>
        public static int Width<T>(this T[][] grid)
        {
            return grid.GetLengthOfLongestRow();
        }

        /// <summary>
        /// Method returns the number of columns in a row, given an index
        /// </summary>
        public static int GetLengthOfRow<T>(this T[][] grid, int row)
        {
            return grid[row].Length;
        }

        /// <summary>
        /// Method returns the number of columns in the largest row in this grid
        /// </summary>
        public static int GetLengthOfLongestRow<T>(this T[][] grid)
        {
            return grid.Max(row => row.Length);
        }

        /// <summary>
        /// Method converts this grid into a DataTable
        /// </summary>
        public static DataTable ToDataTable<T>(this T[][] grid, string nameOfTable)
        {
            DataTable table = new DataTable();
            table.TableName = nameOfTable;

            if (grid.Length > 0)
            {
                var names = grid.First().Select(nm => nm.ToString());

                foreach (var name in names)
                {
                    DataColumn col = new DataColumn(name, typeof(string));
                    table.Columns.Add(col);
                }

                foreach (T[] r in grid.Skip(1))
                {
                    DataRow row = table.NewRow();

                    for(int i = 0; i < r.Length; i++)
                    {
                        var element = r[i].ToString();
                        row[names.ElementAt(i)] = element;
                    }

                    table.Rows.Add(row);
                }
            }

            return table;
        }

        /// <summary>
        /// Method inverts this grid on the y = x axis
        /// </summary>
        public static T[][] Invert<T>(this T[][] grid)
        {
            int heightOfGrid = grid.Length;
            int widthOfGrid = grid.GetLengthOfLongestRow();

            T[][] inverted = new T[widthOfGrid][];

            for(int i = 0; i < widthOfGrid; i++)
            {
                inverted[i] = new T[heightOfGrid];

                for(int j = 0; j < heightOfGrid; j++)
                {
                    inverted[i][j] = grid[j][i];
                }
            }

            return inverted;
        }
    }
}
