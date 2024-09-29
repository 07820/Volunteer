using Form.Page.MainMenu;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Form
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static string UserId = "";
        public static string userName = "";
        protected override void OnStartup(StartupEventArgs e)
        {
            string[] args = e.Args;
            if (args.Length > 0)
            {
                UserId = args[0];
                userName = args[1];
            }
            else
            {
                UserId = "1111111";
            }
            base.OnStartup(e);
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Growl.InfoGlobal(e.Exception.Message);
            e.Handled = true;
        }
    }
}
