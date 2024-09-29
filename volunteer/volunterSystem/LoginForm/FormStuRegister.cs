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
using WindowsFormsApp1;

namespace volunterSystem
{
    public partial class FormStuRegister : System.Windows.Forms.Form
    {

        public FormStuRegister()
        {
            InitializeComponent();

            // 注册Load事件
            this.Load += new EventHandler(FormStuRegister_Load);

            this.Resize += new EventHandler(FormStuRegister_Resize);
        }




        private void FormStuRegister_Load(object sender, EventArgs e)
        {
            AdjustControlsPosition();
            AdjustLabelsPosition();
            AdjustButtonsPosition();
        }

        private void FormStuRegister_Resize(object sender, EventArgs e)
        {
            AdjustControlsPosition();
            AdjustLabelsPosition();
            AdjustButtonsPosition();
        }

        private void AdjustControlsPosition()
        {
            Control[] controlsToAdjust = new Control[]
            {
        txtStuName, txtStuNick, txtStuId, txtPwd, txtRePwd,txtTel, txtEmail, comSecProb, txtAnswer// 添加或移除控件以匹配您的界面
            };

            // 计算垂直方向上的等距分布
            int totalAvailableHeight = this.ClientSize.Height - 116; // 窗体高度上下各减去40像素
            int spaceBetween = totalAvailableHeight / (controlsToAdjust.Length + 1); // 计算等距分布的间距

            for (int i = 0; i < controlsToAdjust.Length; i++)
            {
                if (controlsToAdjust[i] != null)
                {
                    // 水平方向：居中后向右移动40像素
                    controlsToAdjust[i].Left = (this.ClientSize.Width - controlsToAdjust[0].Width) / 2 + 40;

                    // 垂直方向：等距分布
                    controlsToAdjust[i].Top = (i + 1) * spaceBetween - (spaceBetween / 2) - (controlsToAdjust[i].Height / 2) + 76;
                }
            }
        }


        private void AdjustLabelsPosition()
        {
            // 将每个标签和对应的控件配对
            var labelControlPairs = new (Label, Control)[]
            {
        (label1, txtStuName),
        (label2, txtStuNick),
        (label3, txtStuId),
        (label4, txtPwd),
        (label5, txtRePwd),
        (label6, txtTel),
        (label7, txtEmail),
        (label8, comSecProb),
        (label9, txtAnswer)
            };

            const int spacing = 8; // 标签和对应控件之间的水平间距

            foreach (var (label, control) in labelControlPairs)
            {
                if (label != null && control != null)
                {
                    // 设置标签的位置，使其位于对应控件的左侧，并与控件垂直居中对齐
                    label.Left = control.Left - label.Width - spacing;
                    label.Top = control.Top + (control.Height - label.Height) / 2;
                }
            }
        }

        private void AdjustButtonsPosition()
        {
            // 计算txtAnswer底部到窗体底部的距离
            int spaceBelowAnswer = this.ClientSize.Height - txtAnswer.Bottom;

            // 计算按钮应该位于的中间位置
            int buttonsMidPoint = txtAnswer.Bottom + (spaceBelowAnswer / 2);

            // 窗体的水平中心线
            int formCenterX = this.ClientSize.Width / 2;

            // 设置btnRegister的位置
            // 它应该位于水平中心线的左侧，距离中心线40像素
            btnRegister.Left = formCenterX - btnRegister.Width - 40;
            btnRegister.Top = buttonsMidPoint - (btnRegister.Height / 2);

            // 设置btnCancel的位置
            // 它应该位于水平中心线的右侧，距离中心线40像素
            btnCancel.Left = formCenterX + 40;
            btnCancel.Top = buttonsMidPoint - (btnCancel.Height / 2);
        }


















        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtStuName.Text=="" || txtStuNick.Text=="" || txtStuId.Text == "" || txtPwd.Text == "" || txtRePwd.Text == "" || txtTel.Text == "" || txtEmail.Text == "" || comSecProb.Text == "" || txtAnswer.Text == "")
            {
                MessageBox1 form = new MessageBox1("Empty text exsists!");
                form.ShowDialog();
                //MessageBox.Show("Empty text exsists!","Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string name = txtStuName.Text;
            string nickName = txtStuNick.Text;
            string ID = txtStuId.Text;
            string pwd = txtPwd.Text;
            string rePwd = txtRePwd.Text;
            string tel = txtTel.Text;
            string email = txtEmail.Text;
            string secProb = comSecProb.Text;
            string answer = txtAnswer.Text;

            Dao dao = new Dao();
            dao.connect();
            string sql = "select stuId,stuName from student_already where stuId = @ID AND stuName = @name";
            SqlCommand command = new SqlCommand(sql, dao.connect());
            command.Parameters.AddWithValue("@ID", ID);
            command.Parameters.AddWithValue("@name", name);
            SqlDataReader reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                MessageBox1 form = new MessageBox1("The ID or student name doesn't exsist in the system!");
                form.ShowDialog();
                //MessageBox.Show("The ID or student name doesn't exsist in the system!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                reader.Close();
                dao.DaoClose();
                return;
            }
            reader.Close();

            sql = "select stuId from student_information where stuId = @ID";
            command = new SqlCommand(sql, dao.connect());
            command.Parameters.AddWithValue("@ID", ID);
            reader = command.ExecuteReader();
            // string stuId = reader[0].ToString();

            if (reader.HasRows)
            {
                MessageBox1 form = new MessageBox1("The account already exsists!");
                form.ShowDialog();
                //MessageBox.Show("The account already exsists!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                reader.Close();
                dao.DaoClose();
                return;
            }
            reader.Close();

            if (txtPwd.Text.Trim() != txtRePwd.Text.Trim())
            {
                MessageBox1 form = new MessageBox1("The passwords entered twice do not match!");
                form.ShowDialog();
                //MessageBox.Show("The passwords entered twice do not match!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            sql = "insert into student_information (stuName, nickName, stuId, stuPwd, tel, email, secProblem, answer, credit, state) " +
                "values (@name, @nickName, @ID, @pwd, @tel, @email, @secProb, @answer, 0, 'Normal');" +
                "Update student_already set registerStatus = 1 Where stuId = @ID";

            command = new SqlCommand(sql, dao.connect());
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@nickName", nickName);
            command.Parameters.AddWithValue("@ID", ID);
            command.Parameters.AddWithValue("@pwd", pwd);
            command.Parameters.AddWithValue("@tel", tel);
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@secProb", secProb);
            command.Parameters.AddWithValue("@answer", answer);

            int result = command.ExecuteNonQuery();
            if (result > 0)
            {
                MessageBox1 form = new MessageBox1("Register successfully");
                form.ShowDialog();
                //MessageBox.Show("Register successfully", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox1 form = new MessageBox1("Registration Failure!");
                form.ShowDialog();
                //MessageBox.Show("Registration Failure!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            dao.DaoClose();

        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comSecProb_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        
    }
}
