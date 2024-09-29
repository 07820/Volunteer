using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class MessageBox2 : Form
    {
        public event EventHandler RequestCloseMainForm;
        public MessageBox2(string Message)
        {
            InitializeComponent();
            // 为Form的Load事件添加事件处理器
            this.Load += (sender, e) => {
                AdjustPanelPosition(); // 调整panel1位置
            };

            // 为Form的Resize事件添加事件处理器
            this.Resize += (sender, e) => {
                AdjustPanelPosition(); // 当窗体大小改变时，重新调整panel1位置

            };
            label1.Text = Message;
            // 设置AutoSize为false以禁用自动调整大小
            label1.AutoSize = false;

            // 设置标签的宽度与窗体一致
            label1.Width = this.ClientSize.Width - 20; // 留出一些边距

            // 设置标签的最大宽度，使其能够在达到这个宽度后自动换行
            label1.MaximumSize = new Size(label1.Width, 1000);

            // 确保标签的高度足够
            label1.Height = 100; // 设置一个初始高度
            label1.Font = new Font("Times New Roman", 12, FontStyle.Regular);

            label1.TextAlign = ContentAlignment.MiddleCenter;
        }
        private void AdjustPanelPosition()
        {
            label1.Left = this.ClientSize.Width / 2 - label1.Width / 2;

            label1.Top = this.ClientSize.Height / 2 - label1.Height / 2;


            button2.Left = (this.ClientSize.Width / 2 - button2.Width) / 2;
            button1.Left = this.ClientSize.Width / 2 + (this.ClientSize.Width / 2 - button1.Width) / 2;

            button1.Top = this.ClientSize.Height - button1.Height - 10;

            button2.Top = button1.Top;
            //button2.Left = this.ClientSize.Width / 2 - button2.Width / 2;

            label1.Left = this.ClientSize.Width / 2 - label1.Width / 2;

            label1.Top = ( button1.Top) / 2 - label1.Height / 2;


        }


        private void MessageBox2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RequestCloseMainForm?.Invoke(this, EventArgs.Empty);
            this.Close();
        }
    }
}
