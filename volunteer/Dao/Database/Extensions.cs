using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper
{
    public static class Extensions
    {
        public static DataTable QueryTable(this IDbConnection cnn, string sql)
        {
            if (cnn is MySqlConnection)
            {
                using (MySqlCommand command = new MySqlCommand(sql, (MySqlConnection)cnn))
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
            else if (cnn is SqlConnection)
            {
                using (SqlCommand command = new SqlCommand(sql, (SqlConnection)cnn))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
            else if (cnn is SQLiteConnection)
            {
                using (SQLiteCommand command = new SQLiteCommand(sql, (SQLiteConnection)cnn))
                {
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
            else
            {
                return null;
            }
        }
    }
}
