using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data.SQLite;
using static Dapper.SimpleCRUD;

namespace Dapper
{
    public class DBHelper
    {
        private static SimpleCRUD.Dialect DbType;
        private static string Conn;

        public static void Init(Dialect type,string conn)
        {
            DbType = type; 
            Conn = conn;
        }

        public static IDbConnection GetConn()
        {
            IDbConnection connection;
            if (DbType == SimpleCRUD.Dialect.SQLite)
            {
                connection = new SQLiteConnection(Conn);
            }
            else if (DbType == SimpleCRUD.Dialect.MySQL)
            {
                connection = new MySqlConnection(Conn);
            }
            else
            {
                connection = new SqlConnection(Conn);
            }
            SimpleCRUD.SetDialect(DbType);
            connection.Open();
            return connection;
        }
    }
}
