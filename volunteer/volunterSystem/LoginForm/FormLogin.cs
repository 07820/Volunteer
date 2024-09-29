using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Model;
using WindowsFormsApp1;

namespace volunterSystem
{
    public partial class FormLogin : System.Windows.Forms.Form
    {
        public static int stuID;
        public static string orgUserName;
        public static int orgID;



        public FormLogin()
        {
            InitializeComponent();

            //this.WindowState = FormWindowState.Maximized; // 设置窗体最大化

            // 绑定事件处理器
            //this.Load += Form1_Load;
            this.Resize += FormLogin_Resize;


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = "ID:";

            
            CenterControlsVertically();



            PositionLabelToLeftOfTextBox(label1, txtId);
            PositionLabelToLeftOfTextBox(label3, txtPwd);



            CenterButtonsWithMiddleOneCentered(rbtUser, rbtOrg, rbtAdmin);

            PositionControlsBelowTxtPwd();


            PositionLoginAndExitButtons();


            PositionButtonsNearCenter(btnLogin, btnExit);




            PositionControlsEquidistantHorizontally(linkFindPwd, userRegister, orgRegister);


            PositionLinkLabelsAtBottom();

            AdjustTextBoxesPosition();
            AdjustLinkLabelPositions();

            PositionLabelToLeftOfTextBox(label1, txtId);
            PositionLabelToLeftOfTextBox(label3, txtPwd); 

            PositionLinkLabelsInsidePanel(panel2, linkFindPwd, userRegister, orgRegister);

            AdjustTextBox1Position();

            set();
        }

        

        private void FormLogin_Resize(object sender, EventArgs e)
        {
            
            CenterControlsVertically();

            CenterButtonsWithMiddleOneCentered(rbtUser, rbtOrg, rbtAdmin);

            PositionControlsBelowTxtPwd();


            PositionLoginAndExitButtons();


            PositionButtonsNearCenter(btnLogin, btnExit);




            PositionControlsEquidistantHorizontally(linkFindPwd, userRegister, orgRegister);


            PositionLinkLabelsAtBottom();

            AdjustTextBoxesPosition();
            AdjustLinkLabelPositions();

            PositionLabelToLeftOfTextBox(label1, txtId);
            PositionLabelToLeftOfTextBox(label3, txtPwd); 

            PositionLinkLabelsInsidePanel(panel2, linkFindPwd, userRegister, orgRegister);

            AdjustTextBox1Position();

            set();
        }



        
        

        private void CenterControlsVertically()
        {
            int verticalSpacing = 20; // 控件之间的垂直间距

            // 居中第一个控件（txtId）
            txtId.Top = (this.ClientSize.Height - txtId.Height - txtPwd.Height - verticalSpacing) / 2;

            // 将第二个控件（txtPwd）放在第一个控件下方并添加间距
            txtPwd.Top = txtId.Bottom + verticalSpacing;
        }







        private void PositionLabelToLeftOfTextBox(Label label, System.Windows.Forms.TextBox textBox)
        {
            int space = 16; // 你可以根据需要调整间距
            label.Left = textBox.Left - label.Width - space;
            label.Top = textBox.Top + (textBox.Height - label.Height) / 2; // 可选，如果你想垂直居中
        }


        private void CenterButtonsWithMiddleOneCentered(Control leftControl, Control middleControl, Control rightControl)
        {
            //int space = 45; // 控件之间的间距

            int space = this.ClientSize.Width / 6;

            // 居中中间控件
            middleControl.Left = this.ClientSize.Width / 2 - rbtOrg.Width  - label4.Width / 3;


            // 设置左侧控件位置，以确保它在中间控件左边
            leftControl.Left = middleControl.Left - leftControl.Width - space;


            // 设置右侧控件位置，以确保它在中间控件右边
            rightControl.Left = middleControl.Right + space;

        }


        private void PositionButtonsNearCenter(System.Windows.Forms.Button btnLeft, System.Windows.Forms.Button btnRight)
        {
            int space = 20; // 控件边缘到中心线的距离

            // 计算窗体中心线位置
            int centerLine = this.ClientSize.Width / 2;

            // 设置左按钮的右边界与中心线的距离
            btnLeft.Left = centerLine - btnLeft.Width - space;

            // 设置右按钮的左边界与中心线的距离
            btnRight.Left = centerLine + space;


        }

        
        private void PositionControlsBelowTxtPwd()
        {
            
            int verticalSpacing = 1; // 控件下方的垂直间隔

            // 将第一个RadioButton控件放置在txtPwd下方25像素的位置
            rbtUser.Top = txtPwd.Bottom + verticalSpacing;
            // 如果RadioButton是并排排列的，那么它们的Top位置将是相同的
            rbtOrg.Top = rbtUser.Top;
            rbtAdmin.Top = rbtUser.Top;
            
            
        }
       


        private void PositionLoginAndExitButtons()
        {
            int verticalSpacing = 30; // 登录和退出按钮下方的垂直间隔

            // 获取三个单选按钮中最低的一个的底部位置
            int lowestRadioButtonBottom = Math.Max(Math.Max(rbtUser.Bottom, rbtOrg.Bottom), rbtAdmin.Bottom);

            // 然后根据这个位置和间隔来设置登录和退出按钮的顶部位置
            btnLogin.Top = lowestRadioButtonBottom + verticalSpacing;
            btnExit.Top = btnLogin.Top; // 让退出按钮和登录按钮在同一水平线上
        }







        private void PositionControlsEquidistantHorizontally(Control leftControl, Control centerControl, Control rightControl)
        {
            int space = 100; // 控件边缘到中心线的距离

            // 获取窗体的总宽度
            int formWidth = this.ClientSize.Width;

            // 将每个控件的中心点放置在等分点

            centerControl.Left = (formWidth / 2) - (centerControl.Width / 2);
            leftControl.Left = centerControl.Left - leftControl.Width - space;
            rightControl.Left = (formWidth / 2) + (centerControl.Width / 2) + space;


        }


        private void PositionLinkLabelsAtBottom()
        {
            int bottomSpacing = 15; // 距离窗体底部的间距

            // 计算垂直位置
            int topPosition = this.ClientSize.Height - linkFindPwd.Height - bottomSpacing;

            // 计算水平位置，使三个 LinkLabel 水平等距排列
            int totalWidth = linkFindPwd.Width + userRegister.Width + orgRegister.Width;
            int spacing = (this.ClientSize.Width - totalWidth) / 4; // 四个空间分布在三个控件之间及两边

            linkFindPwd.Top = topPosition;
            userRegister.Top = topPosition;
            orgRegister.Top = topPosition;

            linkFindPwd.Left = spacing;
            userRegister.Left = linkFindPwd.Right + spacing;
            orgRegister.Left = userRegister.Right + spacing;
        }

        
        private void AdjustTextBoxesPosition()
        {
            int verticalOffset = 50; // 向上移动的像素值

            // 水平居中
            txtId.Left = (this.ClientSize.Width - txtId.Width) / 2;
            txtPwd.Left = (this.ClientSize.Width - txtPwd.Width) / 2;

            // 计算不居中而是向上移动50像素的顶部位置
            int txtIdTopPosition = (this.ClientSize.Height - txtId.Height - txtPwd.Height) / 2 - verticalOffset;
            txtId.Top = txtIdTopPosition > 0 ? txtIdTopPosition : 0;  // 确保不会出现负值

            // 将txtPwd放置在txtId下方，使用您之前定义的间距
            int verticalSpacing = 20; // 两个文本框之间的间距
            txtPwd.Top = txtId.Bottom + verticalSpacing;
        }
        
        private void AdjustLinkLabelPositions()
        {
            // 确定 LinkLabel 控件距离窗体底部的距离
            int bottomSpacing = 10;
            int sideSpacing = this.ClientSize.Width / 4; // 窗体宽度的四分之一

            // 设置每个 LinkLabel 控件的顶部位置
            int linkLabelTop = this.ClientSize.Height - linkFindPwd.Height - bottomSpacing;

            linkFindPwd.Top = linkLabelTop;
            userRegister.Top = linkLabelTop;
            orgRegister.Top = linkLabelTop;

            // 水平位置调整
            linkFindPwd.Left = sideSpacing - (linkFindPwd.Width / 2);
            userRegister.Left = (this.ClientSize.Width / 2) - (userRegister.Width / 2);
            orgRegister.Left = this.ClientSize.Width - sideSpacing - (orgRegister.Width / 2);
        }


        private void PositionLinkLabelsInsidePanel(Panel panel, LinkLabel leftLinkLabel, LinkLabel centerLinkLabel, LinkLabel rightLinkLabel)
        {
            int panelWidth = panel.Width;

            // 计算每个 LinkLabel 控件的宽度
            int totalWidth = leftLinkLabel.Width + centerLinkLabel.Width + rightLinkLabel.Width;
            int spacing = (panelWidth - totalWidth) / 4; // 计算四个空间分布在三个控件之间及两边

            // 设置 LinkLabel 控件的 Left 属性，以三等分空间
            leftLinkLabel.Left = spacing;
            centerLinkLabel.Left = leftLinkLabel.Right + spacing;
            rightLinkLabel.Left = centerLinkLabel.Right + spacing;

            // 将 LinkLabel 控件设置为在 Panel 中垂直居中
            leftLinkLabel.Top = (panel.Height - leftLinkLabel.Height) / 2;
            centerLinkLabel.Top = (panel.Height - centerLinkLabel.Height) / 2;
            rightLinkLabel.Top = (panel.Height - rightLinkLabel.Height) / 2;

            // 将 LinkLabel 控件添加到 Panel 中
            panel.Controls.Add(leftLinkLabel);
            panel.Controls.Add(centerLinkLabel);
            panel.Controls.Add(rightLinkLabel);
        }


        private void AdjustTextBox1Position()
        {
            // Ensure that label7 and txtId exist before proceeding
            if (label7 != null && txtId != null)
            {
                // Set label7's Left property to horizontally center it relative to the form's client area
                // This calculation considers the label's width and ensures it is centered by subtracting half its width from the midpoint of the form's width
                label7.Left = (this.ClientSize.Width - label7.Width) / 2;

                // Set label7's Top property to position it above txtId with a vertical gap
                // The gap is determined by subtracting a specific distance (e.g., 10 pixels) from txtId's Top property
                label7.Top = txtId.Top - label7.Height - 18; // Adjust the 10 pixels gap as needed
            }
        }


        private void panel2_Resize(object sender, EventArgs e)
        {
            // 每当Panel大小改变时，重新定位LinkLabel控件
            PositionLinkLabelsInsidePanel(panel2, linkFindPwd, userRegister, orgRegister);
        }



        private void set()
        {
            //rbtUser.Left -= 30;
            rbtOrg.Left = this.ClientSize.Width / 2 - rbtOrg.Width - label4.Width / 3;
            //rbtAdmin.Left -= 30;
            label2.Left = rbtUser.Right + 8;
            label2.Top = rbtUser.Top + rbtUser.Height / 2 - label2.Height / 2;
            label4.Left = rbtOrg.Right + 8;
            label4.Top = label2.Top;
            label5.Left = rbtAdmin.Right + 8;
            label5.Top = label2.Top;

        }




























        private void stuLogin()
        {
            int id = int.Parse(txtId.Text);
            stuID = id;
            SharedData.StuID = stuID;
            SharedData.orgID = 0;
            string pwd = txtPwd.Text;
            
            Dao dao = new Dao();
            dao.connect();
            string sql = $"select * from student_information where stuId = {id} and stuPwd = '{pwd}'";
            SqlDataReader reader = dao.read(sql);

            if (reader.Read() == true)
            {
                SharedData.userName = reader["nickName"].ToString();
                string state = reader["state"].ToString();
                if (state == "Normal")
                {
                    txtId.Text = "";
                    txtPwd.Text = "";

                    reader.Close();
                    dao.DaoClose();

                    FormStudent form = new FormStudent();
                    this.Hide();
                    form.ShowDialog();
                    this.Close();
                }
                else if (state == "Locked")
                {
                    MessageBox1 form = new MessageBox1("Sorry, your acount is locked, please contact the administrator!");
                    form.ShowDialog();
                    //MessageBox.Show("Sorry, your acount is locked, please contact the administrator!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                

            }
            else
            {
                MessageBox1 form = new MessageBox1("Incorrect ID or password");
                form.ShowDialog();
                //MessageBox.Show("Incorrect ID or password", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

        }

        private void orgLogin()
        {
            string name = txtId.Text;
            orgUserName = name;

            SharedData.userName = orgUserName;
           
            SharedData.StuID = 0;

            string pwd = txtPwd.Text;

            Dao dao = new Dao();
            dao.connect();
            string sql = $"select * from organizer_audited where userName = '{name}' and orgPwd = '{pwd}'";
            SqlDataReader reader = dao.read(sql);

            if (reader.Read() == true)
            {
                string state = reader["state"].ToString();
                orgID = (int)reader["orgID"];

                 SharedData.orgID = orgID;
               
                if (state == "Normal")
                {
                    txtId.Text = "";
                    txtPwd.Text = "";

                    reader.Close();
                    dao.DaoClose();

                    FormOrganizer form = new FormOrganizer();
                    this.Hide();
                    form.ShowDialog();
                    this.Close();
                }
                else if(state == "Locked")
                {
                    MessageBox1 form = new MessageBox1("Sorry, your acount is locked, please contact the administrator!");
                    form.ShowDialog();
                    //MessageBox.Show("Sorry, your acount is locked, please contact the administrator!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
            }
            else
            {
                MessageBox1 form = new MessageBox1("Incorrect username or password");
                form.ShowDialog();
                //MessageBox.Show("Incorrect username or password", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

        }

        private void adminLogin()
        {
            int id = int.Parse(txtId.Text);
            string pwd = txtPwd.Text;

            Dao dao = new Dao();
            dao.connect();
            string sql = $"select * from admin_information where adminId = {id} and adminPwd = '{pwd}'";
            SqlDataReader reader = dao.read(sql);

            if (reader.Read() == true)
            {
                txtId.Text = "";
                txtPwd.Text = "";

                reader.Close();
                dao.DaoClose();

                FormAdmin form = new FormAdmin();
                this.Hide();
                form.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox1 form = new MessageBox1("Incorrect ID or password");
                form.ShowDialog();
                //MessageBox.Show("Incorrect ID or password", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            StudentFindCode form = new StudentFindCode();
            form.ShowDialog();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void userRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormStuRegister form = new FormStuRegister();
            form.ShowDialog();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtId.Text == "" || txtPwd.Text == "")
            {
                MessageBox1 form = new MessageBox1("Empty text exists!");
                form.ShowDialog();
                //MessageBox.Show("Empty text exists!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (rbtUser.Checked == true)
            {
                stuLogin();
            }

            if (rbtOrg.Checked == true)
            {
                orgLogin();
            }

            if (rbtAdmin.Checked == true)
            {
                adminLogin();
            }

        }

        private void orgRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormOrgRegister form = new FormOrgRegister();
            form.ShowDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            using(MessageBox2 form = new MessageBox2("Are you sure to quit?"))
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
            
           /*if (DialogResult.Yes == MessageBox.Show("Are you sure to quit?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                this.Close();
            }*/
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void rbtOrg_CheckedChanged(object sender, EventArgs e)
        {
            label1.Text = "UserName:";

            
            PositionLabelToLeftOfTextBox(label1, txtId);
            PositionLabelToLeftOfTextBox(label3, txtPwd);
        }

        private void rbtUser_CheckedChanged(object sender, EventArgs e)
        {
            label1.Text = "ID:";
           
            PositionLabelToLeftOfTextBox(label1, txtId);
            PositionLabelToLeftOfTextBox(label3, txtPwd);
        }

        private void rbtAdmin_CheckedChanged(object sender, EventArgs e)
        {
            label1.Text = "ID:";
           
            PositionLabelToLeftOfTextBox(label1, txtId);
            PositionLabelToLeftOfTextBox(label3, txtPwd);
        }
    }
}
