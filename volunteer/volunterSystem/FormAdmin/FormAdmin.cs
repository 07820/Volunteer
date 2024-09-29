using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1;

namespace volunterSystem
{
    public partial class FormAdmin : System.Windows.Forms.Form
    {
        FormStuInfo FormStuInfo = new FormStuInfo();
        public FormAdmin()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;

            button1.FlatStyle = FlatStyle.Flat;
            button1.FlatAppearance.BorderSize = 0;

            menuStrip1.BackColor = Color.White;

            RSToolStripMenuItem.BackColor = Color.White;

            RSToolStripMenuItem.Font = new Font("Arial", 12, FontStyle.Bold);

            OIToolStripMenuItem.BackColor = Color.White;

            OIToolStripMenuItem.Font = new Font("Arial", 12, FontStyle.Bold);

            AOAToolStripMenuItem.BackColor = Color.White;

            AOAToolStripMenuItem.Font = new Font("Arial", 12, FontStyle.Bold);

            allStudentsToolStripMenuItem.BackColor = Color.White;

            allStudentsToolStripMenuItem.Font = new Font("Arial", 12, FontStyle.Bold);



            Load_Form(FormStuInfo, RSToolStripMenuItem);

            // 绑定事件处理器
            //this.Load += FormAdmin_Load;
            this.Resize += FormAdmin_Resize;
        }

        private void FormAdmin_Load(object sender, EventArgs e)
        {
            button1.Left = this.ClientSize.Width - button1.Width - 8;

            button1.Top = menuStrip1.Bottom / 2 - button1.Height / 2;

            panel1.Top = menuStrip1.Bottom + 8;

            panel1.Left = 8;

            panel1.Width = this.ClientSize.Width - 16;

            panel1.Height = this.ClientSize.Height - panel1.Top - 8;

            panel1.BackColor = Color.AliceBlue;

            
        }



        private void FormAdmin_Resize(object sender, EventArgs e)
        {

            button1.Left = this.ClientSize.Width - button1.Width - 8;

            button1.Top = menuStrip1.Bottom / 2 - button1.Height / 2;

            panel1.Top = menuStrip1.Bottom + 8;

            panel1.Left = 8;

            panel1.Width = this.ClientSize.Width - 16;

            panel1.Height = this.ClientSize.Height - panel1.Top - 8;
        }






        private void Load_Form(System.Windows.Forms.Form form, ToolStripMenuItem clickedMenuItem)
        {
            if (panel1.Controls.Count > 0)
            {
                panel1.Controls.Clear();
            }

            // 重置所有MenuStripItem的状态
            ResetMenuStripItems();
            clickedMenuItem.BackColor = SystemColors.ActiveCaption; // 模拟按下效果
            // 加载窗体
            form.TopLevel = false;
            form.Dock = DockStyle.Fill;
            panel1.Controls.Add(form);
            panel1.Tag = form;
            form.Show();
        }

        private void ResetMenuStripItems()
        {
            foreach (ToolStripMenuItem item in menuStrip1.Items)
            {
                // 需要递归重置所有子菜单项的样式
                ResetMenuItem(item);
            }
        }


        private void ResetMenuItem(ToolStripMenuItem menuItem)
        {
            menuItem.BackColor = SystemColors.Control; // 或其他默认颜色

            foreach (ToolStripItem subItem in menuItem.DropDownItems)
            {
                if (subItem is ToolStripMenuItem)
                {
                    // 递归调用以处理子菜单项
                    ResetMenuItem((ToolStripMenuItem)subItem);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (MessageBox2 form = new MessageBox2("Are you sure to quit?"))
            {
                // 订阅事件
                form.RequestCloseMainForm += (senderObj, args) =>
                {
                    this.Close();
                };

                // 显示窗体作为模态对话框
                form.ShowDialog();
            }
            /*if (DialogResult.Yes == MessageBox.Show("Are you sure to quit?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                this.Close();
            }*/
        }

        private void 审核组织者申请ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAuditOrganizer form = new FormAuditOrganizer();
            ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender; // 获取点击的菜单项
            Load_Form(form, clickedItem); // 传递点击的菜单项
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        

        private void 组织者信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormOrgInfo form = new FormOrgInfo();
            ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender; // 获取点击的菜单项
            Load_Form(form, clickedItem); // 传递点击的菜单项
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void 学生信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormStuInfo form = new FormStuInfo();
            ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender; // 获取点击的菜单项
            Load_Form(form, clickedItem); // 传递点击的菜单项
        }

        private void allStudentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAllStudents form = new FormAllStudents();
            ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender; // 获取点击的菜单项
            Load_Form(form, clickedItem);
        }
    }
}
