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

namespace Form.Page.Progress
{
    /// <summary>
    /// 进度条界面的交互逻辑
    /// </summary>
    public partial class ProgressView : Window
    {
        /// <summary>
        /// 获取或设置一个值，指示窗口是否已关闭。
        /// </summary>
        public bool IsClose = false;
        /// <summary>
        /// 初始化 ProgressView 类的新实例。
        /// </summary>
        /// <param name="title">窗口标题。</param>
        /// <remarks>
        /// 构造函数通过调用 InitializeComponent 方法进行界面初始化，并设置窗口的标题。
        /// </remarks>
        public ProgressView(string title)
        {
            InitializeComponent();
            this.txtTitle.Text = title;
        }
        /// <summary>
        /// 处理鼠标左键在窗口上按下并移动时的事件，实现窗体拖动功能。
        /// </summary>
        /// <param name="sender">触发事件的对象。</param>
        /// <param name="e">事件参数。</param>
        /// <remarks>
        /// 通过捕捉异常来处理可能的异常情况，让窗体随着鼠标拖拽进行移动。
        /// </remarks>
        public void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                // 让窗体随着拖拽移动
                ((System.Windows.Window)sender).DragMove();
            }
            catch
            {

            }

        }

        /// <summary>
        /// 处理关闭按钮点击事件，关闭窗口。
        /// </summary>
        /// <param name="sender">触发事件的对象。</param>
        /// <param name="e">事件参数。</param>
        public void CloseClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 处理窗口关闭事件，将 IsClose 属性设置为 true。
        /// </summary>
        /// <param name="sender">触发事件的对象。</param>
        /// <param name="e">事件参数。</param>
        public void Window_Closed(object sender, EventArgs e)
        {
            IsClose = true;
        }
    }
}
