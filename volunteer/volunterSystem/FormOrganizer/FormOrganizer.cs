using Activity;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1;

namespace volunterSystem
{
    public partial class FormOrganizer : System.Windows.Forms.Form
    {
        OrgPost form = new OrgPost();
        private System.Windows.Forms.Timer showTimer;
        public FormOrganizer()
        {
            InitializeComponent();


            // 初始化计时器
            showTimer = new System.Windows.Forms.Timer();
            showTimer.Interval = 1500; // 设置时间间隔为6秒
            showTimer.Tick += new EventHandler(showTimer_Tick);

            this.Opacity = 0; // 初始时窗体完全透明

            showTimer.Start(); // 启动计时器，开始计时




            this.WindowState = FormWindowState.Maximized;

            InitializeButtonBorders();
           

            //this.Load += new System.EventHandler(this.FormOrganizer_Load);

            this.Resize += new System.EventHandler(this.FormOrganizer_Resize);
        }


        private void InitializeButtonBorders()
        {
            // 初始化按钮边框样式
            button1.FlatAppearance.BorderSize = 0;
            button2.FlatAppearance.BorderSize = 0;
            button3.FlatAppearance.BorderSize = 0;
            button4.FlatAppearance.BorderSize = 0;
        }


        private void showTimer_Tick(object sender, EventArgs e)
        {
            this.Opacity = 1; // 将窗体透明度设置为完全不透明
            this.Show(); // 显示窗体
            showTimer.Stop(); // 停止计时器
        }



        // Event handler for form's Resize event
        private void FormOrganizer_Resize(object sender, EventArgs e)
        {
            

            SetPanelPositionAndSize();

            PositionButtonsInCenter();

            AdjustButtonSizes(); // 调整按钮大小



            // 在构造函数或Form_Load事件中添加
            //panel2.Paint += panel2_Paint;
            panel3.Invalidate(); // 强制panel2重绘



        }




        

        private void SetPanelPositionAndSize()
        {
            // 这里假设您想要panel1距离窗体顶部、左边和右边各10像素，底部留出足够空间
            int margin = 10;
            panel1.Location = new Point(284, 0); // 设置panel1的位置

            // 设置panel1的大小，这里使其宽度随窗体宽度变化，高度固定或随窗体高度变化
            panel1.Size = new Size(this.ClientSize.Width - 284, this.ClientSize.Height);

            // 如果您还有其他控件需要调整位置，可以在这里添加相应的代码

            panel3.Location = new Point(0, 0);

            panel3.Size = new Size(284, this.ClientSize.Height + 10);

            panel3.BackColor = Color.White;

        }



        private void PositionButtonsInCenter()
        {
            var buttons = panel3.Controls.OfType<Button>().OrderBy(button => button.Name).ToList();
            int numberOfModules = 4; // 模块数量
            List<Color> moduleColors = new List<Color>()
    {
        Color.FromArgb(204, 255, 255), // 浅Cyan
        Color.FromArgb(178, 235, 242), // 天空Cyan
        Color.FromArgb(153, 216, 225), // 暗淡Cyan
        Color.FromArgb(102, 204, 207), // 深Cyan
        
    };

            int moduleHeight = panel3.Height / numberOfModules;

            for (int i = 0; i < buttons.Count; i++)
            {
                int moduleTopY = i * moduleHeight;
                int buttonY = moduleTopY + (moduleHeight - buttons[i].Height) / 2;
                buttons[i].Location = new Point((panel3.Width - buttons[i].Width) / 2, buttonY);
                // 为按钮设置与其模块相同的背景颜色
                if (i < moduleColors.Count) // 确保颜色列表中有足够的颜色
                {
                    buttons[i].BackColor = moduleColors[i];
                }
            }
        }



        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            List<Color> moduleColors = new List<Color>()
             {
                Color.FromArgb(204, 255, 255), // 浅Cyan
                Color.FromArgb(178, 235, 242), // 天空Cyan
                Color.FromArgb(153, 216, 225), // 暗淡Cyan
                Color.FromArgb(102, 204, 207), // 深Cyan
               
                  };


            int numberOfModules = 4; // 模块数量
            int moduleHeight = panel3.Height / numberOfModules; // 每个模块的高度

            // 绘制模块背景颜色
            for (int i = 0; i < numberOfModules; i++)
            {
                // 计算当前模块的顶部和底部Y坐标
                int moduleTopY = i * moduleHeight;

                // 创建用于填充模块背景的画刷
                using (Brush brush = new SolidBrush(moduleColors[i]))
                {
                    // 填充模块背景颜色
                    e.Graphics.FillRectangle(brush, 0, moduleTopY, panel3.Width, moduleHeight);
                }

                // 如果不是第一个模块，绘制分割线
                if (i > 0)
                {
                    // 绘制分割线
                    e.Graphics.DrawLine(Pens.Black, 0, moduleTopY, panel3.Width, moduleTopY);
                }
            }
        }

        private void AdjustButtonSizes()
        {
            int numberOfModules = 4; // 模块数量
            int moduleHeight = panel3.Height / numberOfModules; // 每个模块的高度
            int buttonWidth = (int)(panel3.Width * 1); // 假设按钮宽度为panel2宽度的80%
            int buttonHeight = (int)(moduleHeight * 1); // 假设按钮高度为模块高度的60%

            // 调整每个按钮的大小
            var buttons = panel3.Controls.OfType<Button>().OrderBy(button => button.Name).ToList();
            foreach (var button in buttons)
            {
                button.Size = new Size(buttonWidth, buttonHeight);
                // 可能还需要重新调整按钮位置，使其居中于模块
                int moduleTopY = (buttons.IndexOf(button) * moduleHeight) + (moduleHeight - buttonHeight) / 2;
                button.Location = new Point((panel3.Width - buttonWidth) / 2, moduleTopY);
            }
        }

































        private void Load_Form(object page)
        {
            if (this.panel1.Controls.Count > 0)
            {
                this.panel1.Controls.Clear();
            }

            if (page is System.Windows.Forms.Form form)
            {
                form.TopLevel = false;
                form.Dock = DockStyle.Fill;
                this.panel1.Controls.Add(form);
                this.panel1.Tag = form;
                form.Show();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (MessageBox2 form = new MessageBox2("Are you sure to quit?"))
            {
                // 订阅事件
                form.RequestCloseMainForm += (senderObj, args) =>
                {
                    // 关闭主窗体
                    this.Close();
                };

                // 显示窗体作为模态对话框
                form.ShowDialog();
            }
        }

        private void FormOrganizer_Load(object sender, EventArgs e)
        {
            Load_Form(form);
            SetPanelPositionAndSize();

            PositionButtonsInCenter();

            AdjustButtonSizes(); // 调整按钮大小



            // 在构造函数或Form_Load事件中添加
            //panel2.Paint += panel2_Paint;
            panel3.Invalidate(); // 强制panel2重绘
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

       


        private void button2_Click(object sender, EventArgs e)
        {
            Load_Form(form);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FormOrganizerInfo organizerInfoForm = new FormOrganizerInfo();
            Load_Form(organizerInfoForm);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            string wpfAppPath = @"./Form.exe";
            string[] arguments = { FormLogin.orgID + "", SharedData.userName + "" };

            // 创建启动进程的信息
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = wpfAppPath,
                Arguments = string.Join(" ", arguments),
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            Process.Start(startInfo);
        }
    }
}
