using Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1;

namespace volunterSystem
{
    public partial class FormEditStudentInfo : System.Windows.Forms.Form
    {
        public FormEditStudentInfo()
        {
            InitializeComponent();

            // 为 Load 和 Resize 事件添加事件处理器
            this.Load += FormEditStudentInfo_Load;
            this.Resize += FormEditStudentInfo_Resize;
        }

        private void FormEditStudentInfo_Load(object sender, EventArgs e)
        {
            int studentId = FormLogin.stuID; // 假设这是获取学生ID的方式
            


            AdjustLabelsDynamically();


            // 调整文本框位置
            AdjustTextBoxesPosition();
        }

        private void FormEditStudentInfo_Resize(object sender, EventArgs e)
        {
            AdjustLabelsDynamically();


            // 调整文本框位置
            AdjustTextBoxesPosition();
        }








        // 定义一个方法来动态调整标签位置和间距
        private void AdjustLabelsDynamically()
        {
            // 定义标签列表
            List<Label> labels = new List<Label> { label1, label2, label3, label4, label5, label6 };

            // 定义起始位置
            int currentY = 105; // 初始Y位置设置为第一个间距的大小

            // 计算可用于间距的总高度（窗体高度减去所有标签的总高度）
            int availableSpace = this.ClientSize.Height - currentY - 200;

            // 计算每个间距（标签之间的空间），分配给标签之间的间距
            // 注意：标签数量减1是因为间距的数量总是比标签少一个
            int spacing = availableSpace / (labels.Count + 1);


            foreach (var label in labels)
            {
                // 设置标签的新位置
                label.Left = this.ClientSize.Width / 2 - label.Width - 50; // 标签的右侧与中轴线对齐
                label.Top = currentY;

                // 更新下一个标签的Y位置
                currentY += label.Height + spacing;
            }

            int SPACE = 20;

            btnSave.Left = this.ClientSize.Width / 2 - btnSave.Width - SPACE;

            btnSave.Top = this.ClientSize.Height - btnSave.Height - 10;

            button1.Left = this.ClientSize.Width / 2 + SPACE;

            button1.Top = btnSave.Top;

            
            //label7.Left = this.ClientSize.Width / 2 - label7.Width - SPACE + 8;

            //label8.Left = this.ClientSize.Width / 2 - label8.Width - SPACE + 8;

            
        }




        private void AdjustTextBoxesPosition()
        {
            // 为每个文本框设置水平对齐的位置
            txtNickName.Top = label1.Top;
            txtNickName.Left = label1.Right + 8;

            txtStuPwd.Top = label2.Top;
            txtStuPwd.Left = label2.Right + 8;

            txtEmail.Top = label3.Top;
            txtEmail.Left = label3.Right + 8;

            txtTel.Top = label4.Top; // 假设没有label4，直接跳到label5
            txtTel.Left = label4.Right + 8;

            // 假设txtSecProblem是ComboBox而不是TextBox，但处理方法相同
            txtSecProblem.Top = label5.Top;
            txtSecProblem.Left = label5.Right + 8;

            txtAnswer.Top = label6.Top; // 假设txtAnswer应该与某个标签对齐，这里示例为label6
            txtAnswer.Left = label6.Right + 8;
        }






































        public void LoadStudentDetails(int studentId)
        {
            string connectionString = @ConfigInfo.GetDbConnectionString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM student_information WHERE stuId = @StudentId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentId", studentId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // 假设你有名为txtEmail, txtTel等的文本框用于编辑

                            txtStuPwd.Text = reader["stuPwd"] as string;

                            //txtSecProblem1.Text = reader["secProblem"] as string;
                            txtNickName.Text = reader["nickName"] as string;
                            txtSecProblem.Text = reader["secProblem"] as string;
                            txtAnswer.Text = reader["answer"] as string;
                            txtEmail.Text = reader["email"] as string;
                            txtTel.Text = reader["tel"] as string;


                        }
                    }
                }
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        

        

        private void btnSave_Click(object sender, EventArgs e)
        {
            // 假设你已经有了连接字符串和获取studentId的逻辑
            string connectionString = ConfigInfo.GetDbConnectionString();
            int studentId = FormLogin.stuID; // 获取学生ID的方法;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();



                string query1 = @"UPDATE student_information 
                         SET tel = @Tel, email = @Email, secProblem = @SecProblem, answer = @Answer, nickName =@NickName
                         WHERE stuId = @StudentId
                         Update T_Post
                         Set nickname = @NickName WHERE UserID =  @StudentId ";

                using (SqlCommand command = new SqlCommand(query1, connection))
                {
                    // 使用TextBox控件中的值设置参数
                    command.Parameters.AddWithValue("@Tel", txtTel.Text);
                    command.Parameters.AddWithValue("@Email", txtEmail.Text);
                    command.Parameters.AddWithValue("@SecProblem", txtSecProblem.Text);
                    command.Parameters.AddWithValue("@Answer", txtAnswer.Text);

                    command.Parameters.AddWithValue("@NickName", txtNickName.Text);

                    command.Parameters.AddWithValue("@StudentId", studentId);

                    command.ExecuteNonQuery();
                }
            }

            MessageBox1 form = new MessageBox1("Information has been updated!");
            form.ShowDialog();
            //MessageBox.Show("Information has been updated!");
            this.Close(); // 关闭窗体
        }

        private void txtSecProblem_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void stuName_Click(object sender, EventArgs e)
        {

        }

        private void txtNickName_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
