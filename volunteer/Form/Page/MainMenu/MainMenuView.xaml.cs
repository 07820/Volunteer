using Form.Control;
using Form.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Form.Page.MainMenu
{
    /// <summary>
    /// TestView.xaml 的交互逻辑
    /// </summary>
    public partial class MainMenuView: Window
    {
        public MainMenuView()
        {
            InitializeComponent();
        }

        private void Max(object sender, MouseButtonEventArgs e)
        {
            if(WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                WindowState = WindowState.Maximized;
            }
        }

        private void Close(object sender, MouseButtonEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Min(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void TextBlock_MouseDown_1(object sender, MouseButtonEventArgs e)
        {

        }

        private void TextBlock_MouseDown_2(object sender, MouseButtonEventArgs e)
        {

        }

        private void Path_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}