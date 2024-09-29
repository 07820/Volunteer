using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config
{
    public class ConfigInfo
    {
        
         public static string DbConnectionString = "Data Source =.; Initial Catalog = Volunteer; Integrated Security = True";
        //public static string DbConnectionString = "Server=8.208.98.57,1433;Database=Volunteer;User Id=sa;Password=Abc394639.;";
        public static string TestDbConnectionString = "Data Source =.; Initial Catalog = Volunteer; Integrated Security = True";
        //public static string TestDbConnectionString = "Server=8.208.98.57,1433;Database=Volunteer;User Id=sa;Password=Abc394639.;";

        public static string VdoConnectionString = "http://8.208.98.57:8080";
        public static string TestVdoConnectionString = "http://8.208.98.57:8080";
        public static string GetDbConnectionString()
        {
            if (Environment.UserName.Contains("wtl"))
            {
                return TestDbConnectionString;
            }
            else
            {
                return DbConnectionString;
            }
        }

        public static string GetVdoHttp()
        {
            if (Environment.UserName.Contains("wtl"))
            {
                return VdoConnectionString;
            }
            else
            {
                return VdoConnectionString;
            }
        }
    }
}
