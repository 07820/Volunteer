using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace volunterSystem
{
    public partial class FormOrgAppDetail : System.Windows.Forms.Form
    {
        public FormOrgAppDetail()
        {
            InitializeComponent();

            this.Resize += new EventHandler(FormOrgAppDetail_Load);

            // 注册Resize事件处理器
            this.Resize += new EventHandler(FormOrgAppDetail_Resize);
        }



        private void FormOrgAppDetail_Load(object sender, EventArgs e)
        {
            CenterLabel1();

            AdjustControlsPosition();

            AlignLabelsToLeftControls();


        }



        private void FormOrgAppDetail_Resize(object sender, EventArgs e)
        {
            CenterLabel1();

            AdjustControlsPosition();

            AlignLabelsToLeftControls();


        }

        private void CenterLabel1()
        {
            int offtop = 66;
            int space = 15;

            // 将label1水平居中于窗体
            label1.Left = (this.ClientSize.Width - label1.Width) / 2;
            // 设置label1距离窗体顶端的距离为40像素
            label1.Top = offtop + space;
        }

        // ... 其他的方法和事件处理器


        private void AdjustControlsPosition()
        {
            // 列出需要调整位置的控件
            Control[] controls = new Control[] { lbUserName, lbCompany, lbemail, lbPwd, lbsec, lbans };

            // 获取Label1的底部位置作为起始点
            int startTop = label1.Bottom;

            // 计算从Label1底部到Form底部的距离
            int totalHeight = this.ClientSize.Height - startTop;

            // 计算在Label1底部到Form底部的前三分之二范围内分布控件
            int twoThirdsHeight = (int)(totalHeight * 2 / 3);

            // 计算控件间的间距
            int spaceBetweenControls = (twoThirdsHeight - controls.Sum(c => c.Height)) / (controls.Length + 1);

            // 初始位置设置为Label1底部加上第一个间隔
            int currentTop = startTop + spaceBetweenControls - 20;

            foreach (Control control in controls)
            {
                // 控件右对齐，位于窗体右侧的左边40px处
                control.Left = this.ClientSize.Width - control.Width - 40;
                // 设置控件的顶部位置
                control.Top = currentTop;
                // 更新下一个控件的顶部位置
                currentTop += control.Height + spaceBetweenControls;
            }

            
        }



        private void AlignLabelsToLeftControls()
        {
            // 右侧控件列表
            Control[] rightControls = { lbUserName, lbCompany, lbemail, lbPwd, lbsec, lbans };
            // 左侧对应的标签列表
            System.Windows.Forms.Label[] leftLabels = { label7, label2, label3, label4, label8, label9 };

            int leftSpace = 40; // 左边的间距

            for (int i = 0; i < rightControls.Length; i++)
            {
                // 确保右侧控件列表和左侧标签列表的长度相同
                if (i < leftLabels.Length)
                {
                    // 将左侧标签的顶部位置与对应的右侧控件对齐
                    leftLabels[i].Top = rightControls[i].Top;
                    // 设置左侧标签的左边距，以便它们位于窗体左侧的40空间处
                    leftLabels[i].Left = leftSpace;
                }
            }

            int upSpace = label9.Top - label8.Top;


            label6.Left = leftSpace;

            label6.Top = label9.Top + upSpace;

            materialBox.Left = leftSpace;

            materialBox.Top = label6.Top + label6.Height + 5;

            btnReturn.Top = this.ClientRectangle.Height - btnReturn.Height - leftSpace;

            btnReturn.Left = (this.ClientRectangle.Width - btnReturn.Width) / 2;


        }
















        public string lbUserNameText
        {
            get { return lbUserName.Text; }
            set { lbUserName.Text = value; }
        }

        public string lbCompanyText
        { 
            get { return lbCompany.Text; }
            set { lbCompany.Text = value; }
        }

        public string lbEmailText
        {
            get { return lbemail.Text; }
            set { lbemail.Text = value; }
        }

        public string lbPasswordText
        {
            get { return lbPwd.Text; }
            set { lbPwd.Text = value; }
        }
        public string lbSecText
        {
            get { return lbsec.Text; }
            set { lbsec.Text = value; }
        }

        public string lbAnsTxt
        {
            get { return lbans.Text; }
            set { lbans.Text = value; }
        }

        public Image ImageSource
        {
            get { return materialBox.Image; }
            set { materialBox.Image = value;
                materialBox.SizeMode = PictureBoxSizeMode.Zoom;
            }
            
        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

      

        private void btnReturn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void materialBox_Click(object sender, EventArgs e)
        {

        }

        private void lbUserName_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
