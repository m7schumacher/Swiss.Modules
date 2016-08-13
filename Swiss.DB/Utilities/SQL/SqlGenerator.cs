using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swiss;

namespace Swiss.DB
{
    public class SqlGenerator
    {
        public static string ConnectionString(string srvr, string uid, string pss, string db)
        {
            return "SERVER=" + srvr + ";" + "DATABASE=" + db + ";" + "UID=" + uid + ";" + "PASSWORD=" + pss + ";";
        }

        private static string MakeSqlValue(object val)
        {
            var type = val.GetType();
            return type == typeof(String) ? "'" + val.ToString() + "'" : val.ToString();
        }

        public static string InsertString(object input)
        {
            var type = input.GetType();

            StringBuilder builder = new StringBuilder();

            builder.Append("INSERT INTO ");
            builder.Append(type.Name);

            var properties = type.GetPropertyNamesAndTypes()
                                 .Where(pair => !pair.Value.IsEnumerable())
                                 .Select(pair => pair.Key)
                                 .Where(prop => !prop.EqualsIgnoreCase("ID"));

            var firstProperty = properties.First();

            StringBuilder varNames = new StringBuilder();
            varNames.Append("(" + firstProperty);

            StringBuilder varValues = new StringBuilder();
            varValues.Append("(" + MakeSqlValue(input.GetProperty(firstProperty)));

            properties.Skip(1).ForEach(prop =>
            {
                var value = input.GetProperty(prop);

                varNames.Append(", " + prop);
                varValues.Append(", " + MakeSqlValue(value));
            });

            varNames.Append(")");
            varValues.Append(")");

            builder.Append(varNames.ToString() + " VALUES " + varValues.ToString() + ";");

            return builder.ToString();
        }

        public static string TableString(Type type)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("CREATE TABLE ");
            builder.Append(type.Name + "(");
            builder.Append("ID int NOT NULL, ");

            var info = type.GetPropertyNamesAndTypes()
                           .Where(pr => !pr.Value.IsEnumerable());

            foreach (var pair in info)
            {
                var notNull = pair.Value.IsNullable() ? string.Empty : " NOT NULL";
                builder.Append(pair.Key + " " + pair.Value.ToSqlType().ToString() + notNull + ", ");
            }

            builder.Append("PRIMARY KEY(ID));");

            return builder.ToString();
        }
    }
}
