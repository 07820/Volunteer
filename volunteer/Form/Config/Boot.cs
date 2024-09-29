using System;
using System.Threading;
using Config;
using Dapper;
using Form.Page.MainMenu;
using HandyControl.Tools;
using Stylet;
using StyletIoC;

namespace Form.Config
{
    public class Boot : Bootstrapper<MainMenuViewModel>
    {
        public static IWindowManager Manager;
        public static IContainer IOC;
        //public static string connstr = $@"Server=8.208.98.57,1433;Database=Volunteer;User Id=sa;Password=Abc394639.;";
        public static string connstr = "Data Source =.; Initial Catalog = Volunteer; Integrated Security = True";
        
        public static string vdoHttp = $@"8.208.98.57:8080";
        protected override void ConfigureIoC(IStyletIoCBuilder builder)
        {
            // Configure the IoC container in here
        }

        protected override void Configure()
        {
            // Perform any other configuration before the application starts
            connstr = ConfigInfo.GetDbConnectionString();
            vdoHttp = ConfigInfo.GetVdoHttp();
            IOC = this.Container;
            Manager = IOC.Get<IWindowManager>();
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            DBHelper.Init(SimpleCRUD.Dialect.SQLServer, connstr);
        }
    }
}
