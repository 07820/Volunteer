using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace volunterSystem
{
    public partial class FormSearchOrg : System.Windows.Forms.Form
    {
        private FormOrgInfo myFormOrgInfo;
        public FormSearchOrg(FormOrgInfo formOrgInfo)
        {
            InitializeComponent();
            myFormOrgInfo = formOrgInfo;

            this.Load += FormSearchStu_Load;

            this.Resize += FormSearchStu_Resize;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormSearchOrg_Load(object sender, EventArgs e)
        {

        }

        private void FormSearchStu_Load(object sender, EventArgs e)
        {
            // 窗体加载时调整label1的位置
            AdjustLabel1Position();

            AdjustTextBoxPositions(); // 调整文本框位置

            AlignLabelsWithTextboxes();

            AdjustButtonPositions(); // 调整按钮位置

        }

        private void FormSearchStu_Resize(object sender, EventArgs e)
        {
            // 窗体大小调整时再次调整label1的位置
            AdjustLabel1Position();

            AdjustTextBoxPositions(); // 调整文本框位置

            AlignLabelsWithTextboxes();

            AdjustButtonPositions(); // 调整按钮位置
        }



        private void AdjustLabel1Position()
        {
            // 直接设置label1距离窗体顶部的偏移量为 81 像素
            int topOffset = 66; // 距离窗体顶部的偏移量

            int space = 15;

            // 计算 label1 的左边距，使其水平居中
            label1.Left = (this.ClientSize.Width - label1.Width) / 2;

            // 设置 label1 的顶部位置为窗体顶部以下 81 像素的位置
            label1.Top = topOffset + space;
        }


        private void AdjustTextBoxPositions()
        {
            int topOffset = 81; // 从窗体顶部开始的偏移量
            int availableHeight = this.ClientSize.Height - topOffset; // 可用的高度范围
            int space = 25;

            // 计算两个三等分点的位置
            int firstThird = topOffset + (availableHeight / 3);
            int secondThird = topOffset + (2 * availableHeight / 3) - space;

            // 设置textBox1和textBox2的顶部位置
            textBox1.Top = firstThird - (textBox1.Height / 2);
            textBox2.Top = secondThird - (textBox2.Height / 2);

            // 使textBox1和textBox2水平居中
            textBox1.Left = (this.ClientSize.Width - textBox1.Width) / 2;
            textBox2.Left = (this.ClientSize.Width - textBox2.Width) / 2;
        }


        private void AlignLabelsWithTextboxes()
        {
            // 假设 label2 与 textBox1 对齐，label4 与 textBox2 对齐

            // 调整 label2 位置
            label3.Top = textBox1.Top; // 使 label2 与 textBox1 在垂直方向上对齐
            label3.Left = textBox1.Left - label3.Width - 10; // 将 label2 放置在 textBox1 左侧，间隔 10px

            // 调整 label4 位置
            label2.Top = textBox2.Top; // 使 label4 与 textBox2 在垂直方向上对齐
            label2.Left = textBox2.Left - label2.Width - 10; // 将 label4 放置在 textBox2 左侧，间隔 10px
        }



        private void AdjustButtonPositions()
        {
            // 计算两个三等分点的水平位置
            int firstThirdPoint = this.ClientSize.Width / 3;
            int secondThirdPoint = 2 * this.ClientSize.Width / 3;
            int space = 19;

            // 将 btn1 移动到第一个三等分点，水平居中于这一点
            button1.Left = firstThirdPoint - button1.Width / 2;
            // 将 btn2 移动到第二个三等分点，水平居中于这一点
            button2.Left = secondThirdPoint - button2.Width / 2;

            // 设置按钮的顶部位置为窗体底部以上 15 像素的位置
            button1.Top = this.ClientSize.Height - button1.Height - space;
            button2.Top = this.ClientSize.Height - button2.Height - space;
        }























        private void button2_Click(object sender, EventArgs e)
        {
            // 从输入框中获取搜索条件
            string userName = textBox1.Text.Trim();
            string companyName = textBox2.Text.Trim();

            myFormOrgInfo.LoadDataIntoDataGridView(userName, companyName);

            // 关闭搜索窗体
            this.Close();
        }
    }
}
