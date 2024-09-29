using System;
using Dapper;


namespace Rank.Config
{
    public class Boot
    {
        //public static string connstr = $@"Server=8.208.98.57,1433;Database=Volunteer;User Id=sa;Password=Abc394639.;";
        public static string connstr = "Data Source =.; Initial Catalog = Volunteer; Integrated Security = True";
        
        public static string vdoHttp = $@"8.208.98.57:8080";
    }
}
