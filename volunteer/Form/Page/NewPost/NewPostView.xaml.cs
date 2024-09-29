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

namespace Form.Page.NewPost
{
    /// <summary>
    /// NewPostView.xaml 的交互逻辑
    /// </summary>
    public partial class NewPostView : Window
    {
        public NewPostView()
        {
            InitializeComponent();
        }
        private void Max(object sender, MouseButtonEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
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
            this.Close();
        }

        private void Min(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
