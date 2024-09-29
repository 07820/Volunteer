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
    public partial class StudentResetCode : System.Windows.Forms.Form
    {

        public static string question;
        public static string answer;
        public static int studentID;
        public static string userName;


        public StudentResetCode()
        {
            InitializeComponent();
            LoadSecurityQuestion();

            this.Load += new EventHandler(StudentResetCode_Load); // 确保这一行存在

            this.Resize += new EventHandler(StudentResetCode_Resize); // 确保这一行存在
        }

        private void StudentResetCode_Load(object sender, EventArgs e)
        {

            AdjustControlsPosition();

            AlignLabelsToTextBoxes();


        }




        private void StudentResetCode_Resize(object sender, EventArgs e)
        {

            AdjustControlsPosition();


            AlignLabelsToTextBoxes();

        }



        private void AdjustControlsPosition()
        {
            // 定义初始参数
            int topMargin = 100;
            int bottomMargin = 40;
            int centerLine = this.ClientSize.Width / 2;
            int spaceBetweenButtons = 80; // 按钮和中心线之间的间距

            // 设置第一个TextBox位置
            textNewpwd.Top = topMargin;
            textNewpwd.Left = (this.ClientSize.Width - textNewpwd.Width) / 2;

            // 设置按钮行位置
            btnConfirm.Top = this.ClientSize.Height - bottomMargin - btnConfirm.Height;
            btnConfirm.Left = centerLine - btnConfirm.Width - spaceBetweenButtons / 2;
            btnCancel.Top = btnConfirm.Top;
            btnCancel.Left = centerLine + spaceBetweenButtons / 2;

            // 计算剩余TextBoxes的可用高度和间距
            int availableHeight = btnConfirm.Top - textNewpwd.Bottom - topMargin;
            int numberOfTextBoxes = 3; // 中间的TextBox数量
            int spacing = availableHeight / (numberOfTextBoxes + 1);

            // 中间的TextBoxes等距分布
            textQuestion.Top = textNewpwd.Bottom + spacing;
            textQuestion.Left = (this.ClientSize.Width - textQuestion.Width) / 2;
            textAnswer.Top = textQuestion.Top + textQuestion.Height + spacing;
            textAnswer.Left = (this.ClientSize.Width - textAnswer.Width) / 2;
            textVerifypwd.Top = textAnswer.Top + textAnswer.Height + spacing;
            textVerifypwd.Left = (this.ClientSize.Width - textVerifypwd.Width) / 2;
        }





        private void AlignLabelsToTextBoxes()
        {
            // 标签与文本框的水平间距
            int horizontalSpacing = 10;

            // 对齐label1与txtBox1
            label1.Top = textQuestion.Top + (textNewpwd.Height - label1.Height) / 2;
            label1.Left = textQuestion.Left - label1.Width - horizontalSpacing;

            // 对齐label2与txtBox2
            label2.Top = textAnswer.Top + (textQuestion.Height - label2.Height) / 2;
            label2.Left = textAnswer.Left - label2.Width - horizontalSpacing;

            // 对齐label3与txtBox3
            label3.Top = textNewpwd.Top + (textAnswer.Height - label3.Height) / 2;
            label3.Left = textNewpwd.Left - label3.Width - horizontalSpacing;

            // 对齐label4与txtBox4
            label4.Top = textVerifypwd.Top + (textVerifypwd.Height - label4.Height) / 2;
            label4.Left = textVerifypwd.Left - label4.Width - horizontalSpacing;
        }












        private void SecurityCheck()
        {
            string q = textQuestion.Text;
            string a = textAnswer.Text;
            if (StudentFindCode.IsStudent)
            {
                Dao studentInformation = new Dao();
                studentInformation.connect();
                string sql = $"select * from student_information where secProblem ='{q}'and answer = '{a}'";
                SqlDataReader reader = studentInformation.read(sql);
                if (reader.Read() == true)
                {
                    StudentResetCode.question = q;
                    sql = $"select answer from student_information where secProblem = '{q}'";
                    reader = studentInformation.read(sql);
                    reader.Read();
                    StudentResetCode.answer = reader[0].ToString();

                    reader.Close();
                    studentInformation.DaoClose();

                    StudentResetCode form = new StudentResetCode();
                    form.ShowDialog();

                }
                else
                {
                    MessageBox1 form = new MessageBox1("Answer Wrong!");
                    form.ShowDialog();
                    //MessageBox.Show("Answer Wrong!", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                }

            }
            else
            {
                Dao studentInformation = new Dao();
                studentInformation.connect();
                string sql = $"select * from organizer_audited where secProblem ='{q}'and answer = '{a}'";
                SqlDataReader reader = studentInformation.read(sql);
                if (reader.Read() == true)
                {
                    StudentResetCode.question = q;
                    sql = $"select answer from organizer_audited where secProblem = '{q}'";
                    reader = studentInformation.read(sql);
                    reader.Read();
                    StudentResetCode.answer = reader[0].ToString();

                    reader.Close();
                    studentInformation.DaoClose();

                    StudentResetCode form = new StudentResetCode();
                    form.ShowDialog();

                }
                else
                {
                    MessageBox1 form = new MessageBox1("Answer Wrong!");
                    form.ShowDialog();
                    //MessageBox.Show("Answer Wrong!", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                }


            }
        }

        // 定义一个新方法用于重置密码
        private void ResetStudentPassword(string securityAnswer, string newPassword)
        {
            Dao studentInformation = new Dao();
            studentInformation.connect();
            string sql = $"SELECT * FROM student_information WHERE stuId = {StudentFindCode.id} AND answer = '{securityAnswer}'";
            SqlDataReader reader = studentInformation.read(sql);
            if (reader.Read())
            {
                reader.Close();
                // Update the password in the database
                sql = $"UPDATE student_information SET stuPwd = '{newPassword}' WHERE stuId = {StudentFindCode.id}";
                int rowsAffected = studentInformation.Execute(sql);
                if (rowsAffected > 0)
                {
                    MessageBox1 form = new MessageBox1("Password has been reset successfully.");
                    form.ShowDialog();
                    //MessageBox.Show("Password has been reset successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox1 form = new MessageBox1("There was a problem resetting the password.");
                    form.ShowDialog();
                    //MessageBox.Show("There was a problem resetting the password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox1 form = new MessageBox1("Security Answer Wrong!");
                form.ShowDialog();
                //MessageBox.Show("Security Answer Wrong!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            studentInformation.DaoClose();
        }

        // 定义一个新方法用于重置密码
        private void ResetOrganizerPassword(string securityAnswer, string newPassword)
        {

            Dao studentInformation = new Dao();
            studentInformation.connect();
            string sql = $"SELECT * FROM organizer_audited WHERE userName = '{StudentFindCode.userName}' AND answer = '{securityAnswer}'";
            SqlDataReader reader = studentInformation.read(sql);
            if (reader.Read())
            {
                reader.Close();
                // Update the password in the database
                sql = $"UPDATE organizer_audited SET orgPwd = '{newPassword}' WHERE userName = '{StudentFindCode.userName}'";
                int rowsAffected = studentInformation.Execute(sql);
                if (rowsAffected > 0)
                {
                    MessageBox1 form = new MessageBox1("Password has been reset successfully.");
                    form.ShowDialog();
                    //MessageBox.Show("Password has been reset successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox1 form = new MessageBox1("There was a problem resetting the password.");
                    form.ShowDialog();
                    //MessageBox.Show("There was a problem resetting the password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox1 form = new MessageBox1("Security Answer Wrong!");
                form.ShowDialog();
                //MessageBox.Show("Security Answer Wrong!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            studentInformation.DaoClose();
        }

        private void LoadSecurityQuestion()
        {
            if (StudentFindCode.IsStudent)
            {
                // Assuming the user's StudentID is available and set from the previous form
                int studentId = StudentFindCode.id;
                Dao studentInformation = new Dao();
                studentInformation.connect();
                string sql = $"SELECT secProblem FROM student_information WHERE stuId = {studentId}";
                SqlDataReader reader = studentInformation.read(sql);
                if (reader.Read())
                {
                    // Set the security question on the form
                    textQuestion.Text = reader["secProblem"].ToString();
                }
                reader.Close();
                studentInformation.DaoClose();
            }
            else
            {
                // Assuming the user's StudentID is available and set from the previous form
                string userName = StudentFindCode.userName;
                Dao studentInformation = new Dao();
                studentInformation.connect();
                string sql = $"SELECT secProblem FROM organizer_audited WHERE userName= '{userName}'";
                SqlDataReader reader = studentInformation.read(sql);
                if (reader.Read())
                {
                    // Set the security question on the form
                    textQuestion.Text = reader["secProblem"].ToString();
                }
                reader.Close();
                studentInformation.DaoClose();

            }
        }



        

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            // 检查密码是否为空和是否匹配
            string securityAnswer = textAnswer.Text;
            string newPassword = textNewpwd.Text;
            string verifyPassword = textVerifypwd.Text;

            if (string.IsNullOrEmpty(newPassword))
            {
                MessageBox1 form = new MessageBox1("New password cannot be empty!");
                form.ShowDialog();
                //MessageBox.Show("New password cannot be empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newPassword != verifyPassword)
            {
                MessageBox1 form = new MessageBox1("Passwords do not match!");
                form.ShowDialog();
                //MessageBox.Show("Passwords do not match!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (StudentFindCode.IsStudent)
            {
                ResetStudentPassword(securityAnswer, newPassword); // 学生重置密码
            }
            else
            {
                ResetOrganizerPassword(securityAnswer, newPassword); // 组织者重置密码
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
