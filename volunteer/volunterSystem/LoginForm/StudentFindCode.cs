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
    public partial class StudentFindCode : System.Windows.Forms.Form
    {
        public StudentFindCode()
        {
            InitializeComponent();

            label1.Text = "Student ID:";

            this.Load += new EventHandler(Form1_Load);

            this.Resize += new EventHandler(Form1_Resize);
        }

        public static int id;
        public static string email;
        public static string userName;
        public static bool IsStudent { get; set; }





        private void Form1_Load(object sender, EventArgs e)
        {
            // 调整文本框位置
            AdjustTextBoxPositions();


            AlignLabelsToTextBoxes();

            AdjustControlsPositions(); // 调整控件位置

            AdjustButtonsPosition(); // 在窗体尺寸变化时调整按钮位置
            // 这里还可以添加其他控件初始化代码
        }


        private void Form1_Resize(object sender, EventArgs e)
        {
            AdjustTextBoxPositions(); // 在窗体尺寸变化时调整文本框位置


            AlignLabelsToTextBoxes();

            AdjustControlsPositions(); // 调整控件位置

            AdjustButtonsPosition(); // 在窗体尺寸变化时调整按钮位置

        }




        private void AdjustTextBoxPositions()
        {
            // 设置TextStudentEmail在Form高度的四分之一处
            TextStudentID.Top = this.ClientSize.Height / 4 - TextStudentEmail.Height / 2 + 15;
            TextStudentID.Left = (this.ClientSize.Width - TextStudentEmail.Width) / 2; // 水平居中

            // 设置TextStudentID在Form高度的二分之一处
            TextStudentEmail.Top = this.ClientSize.Height / 2 - TextStudentID.Height / 2 - 15;
            TextStudentEmail.Left = (this.ClientSize.Width - TextStudentID.Width) / 2; // 水平居中
        }


        private void AlignLabelsToTextBoxes()
        {
            // 对齐label1与textStudentEmail
            label1.Top = TextStudentID.Top + (TextStudentID.Height - label1.Height) / 2;
            label1.Left = TextStudentID.Left - label1.Width - 5; // 假设标签和文本框之间的间距为5px

            // 对齐label2与textStudentID
            label2.Top = TextStudentEmail.Top + (TextStudentEmail.Height - label2.Height) / 2;
            label2.Left = TextStudentEmail.Left - label2.Width - 5; // 同上，间距为5px
        }


        private void AdjustButtonsPosition()
        {
            // 假设radioButtonReference是您要参考的单选按钮
            int radioButtonBottom = btnOrganizer.Bottom;
            int spaceFromCenter = 40; // 到中心线的距离

            // 计算这两个按钮应该位于的Y坐标
            int yPos = radioButtonBottom + (this.ClientSize.Height - radioButtonBottom) / 2 - btnReturn.Height / 2;

            // 设置btnReturn的位置
            btnReturn.Top = yPos;

            btnReturn.Left = this.ClientSize.Width / 2 + spaceFromCenter;
            btnFinish.Left = this.ClientSize.Width / 2 - btnFinish.Width - spaceFromCenter;

            // 设置btnFinish的位置
            btnFinish.Top = yPos;
            btnFinish.Left = this.ClientSize.Width / 2 - btnFinish.Width - spaceFromCenter;

        }


        private void AdjustControlsPositions()
        {
            int centerLine = this.ClientSize.Width / 2;
            int twoThirdsHeight = (int)(this.ClientSize.Height * 2 / 3);

            // 设置单选按钮位于窗体高度的三分之二处
            btnStudent.Top = twoThirdsHeight - btnStudent.Height / 2;
            btnStudent.Left = centerLine - btnStudent.Width - 40; // 在中心线左侧40像素

            btnOrganizer.Top = twoThirdsHeight - btnOrganizer.Height / 2;
            btnOrganizer.Left = centerLine + 40; // 在中心线右侧40像素

            // 计算btnReturn和btnFinish的顶部位置
            int lowestRadioButtonBottom = Math.Max(btnStudent.Bottom, btnOrganizer.Bottom);
            int bottomAreaHeight = this.ClientSize.Height - lowestRadioButtonBottom;
            int buttonsTopPosition = lowestRadioButtonBottom + ((bottomAreaHeight - 25 - Math.Max(btnReturn.Height, btnFinish.Height)) / 2) + 25;

            // 设置按钮位置
            btnReturn.Top = buttonsTopPosition;

            btnReturn.Left = centerLine + 40; // 在中心线右侧40像素

            btnFinish.Top = buttonsTopPosition;
            
            btnFinish.Left = centerLine - btnFinish.Width - 40; // 在中心线左侧40像素
        }





























        private void stuAccountIdentify()
        {
            int stuid;
            if (!int.TryParse(TextStudentID.Text, out stuid))
            {
                MessageBox1 form = new MessageBox1("Student ID must be a number");
                form.ShowDialog();
                //MessageBox.Show("Student ID must be a number", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string email = TextStudentEmail.Text;
            Dao studentInformation = new Dao();

            try
            {
                string sql = $"select * from student_information where stuId = @id and email = @email";
                SqlCommand cmd = studentInformation.command(sql);
                cmd.Parameters.AddWithValue("@id", stuid);
                cmd.Parameters.AddWithValue("@email", email);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        StudentFindCode.id = stuid;
                        StudentFindCode.email = email;

                        TextStudentID.Text = "";
                        TextStudentEmail.Text = "";

                        reader.Close();
                        studentInformation.DaoClose();

                        StudentResetCode form = new StudentResetCode();
                        form.ShowDialog();
                    }
                    else
                    {
                        MessageBox1 form = new MessageBox1("Account or email wrong!");
                        form.ShowDialog();
                        //MessageBox.Show("Account or email wrong!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox1 form = new MessageBox1($"An error occurred: {ex.Message}");
                form.ShowDialog();
                //MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // 确保数据库连接被关闭
                studentInformation.DaoClose();
            }

        }

        private void orgAccountIdentify()
        {
            string userName = TextStudentID.Text; // 这里假设TextStudentID实际上是用户名的输入框
            string email = TextStudentEmail.Text;

            Dao studentInformation = new Dao();

            string sql = $"select * from organizer_audited where userName = @userName and email = @email";
            SqlCommand cmd = studentInformation.command(sql);
            cmd.Parameters.AddWithValue("@userName", userName);
            cmd.Parameters.AddWithValue("@email", email);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    StudentFindCode.userName = userName;
                    StudentFindCode.email = email;

                    TextStudentID.Text = "";
                    TextStudentEmail.Text = "";

                    reader.Close();
                    studentInformation.DaoClose();

                    StudentResetCode form = new StudentResetCode();
                    form.ShowDialog();
                }
                else
                {
                    MessageBox1 form = new MessageBox1("Account or email wrong!");
                    form.ShowDialog();
                    //MessageBox.Show("Account or email wrong!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                reader.Close(); // 确保在逻辑的每个分支上reader都被关闭
                studentInformation.DaoClose();
            }
                

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void TextStudentID_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            StudentFindCode.IsStudent = btnStudent.Checked;
            if (TextStudentID.Text == "" || TextStudentEmail.Text == "")
            {
                MessageBox1 form = new MessageBox1("Please enter the information completely");
                form.ShowDialog();
                //MessageBox.Show("Please enter the information completely", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (btnStudent.Checked)  // Assuming 'radioButtonStudent' is the name of your student radio button
            {
                stuAccountIdentify();
            }
            else if (btnOrganizer.Checked) // Assuming 'radioButtonOrganizer' is the name of your organizer radio button
            {
                orgAccountIdentify(); // Replace with the actual function you want to call for 'Organizer'
            }

            this.Close();
        }

        private void StudentFindCode_Load(object sender, EventArgs e)
        {

        }

        private void btnStudent_CheckedChanged(object sender, EventArgs e)
        {
            label1.Text = "Student ID:";

            AlignLabelsToTextBoxes();
        }

        private void btnOrganizer_CheckedChanged(object sender, EventArgs e)
        {
            label1.Text = "User Name:";

            AlignLabelsToTextBoxes();
        }
    }
}
