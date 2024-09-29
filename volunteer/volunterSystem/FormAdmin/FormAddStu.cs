using Config;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace volunterSystem
{
    public partial class FormAddStu : System.Windows.Forms.Form
    {
        public event EventHandler StudentAdded;

        protected void OnStudentAdded()
        {
            StudentAdded?.Invoke(this, EventArgs.Empty);
        }

        public FormAddStu()
        {
            InitializeComponent();

            this.Load += FormAddStu_Load;
            this.Resize += FormAddStu_Resize;
        }




        private void FormAddStu_Load(object sender, EventArgs e)
        {
            // 窗体加载时调整label1的位置
            AdjustLabel1Position();

          

        }

        private void FormAddStu_Resize(object sender, EventArgs e)
        {
            // 窗体加载时调整label1的位置
            AdjustLabel1Position();

           

        }





        private void AdjustLabel1Position()
        {
            int spaceRest = this.ClientSize.Height - 66;


            
            textBox1.Top = 66 + spaceRest / 3 - textBox1.Height;

            textBox2.Top = 66 + spaceRest / 3 + 40;

            textBox1.Left = this.ClientSize.Width / 2 - 40;

            textBox2.Left = textBox1.Left;

            label1.Top = textBox1.Top;

            label3.Top = textBox2.Top;

            label1.Left = textBox1.Left - label1.Width - 8;

            label3.Left = textBox2.Left - label3.Width - 8; 


            button1.Top = textBox2.Bottom + 50;

            button2.Top = button1.Top;

            button1.Left = this.ClientSize.Width / 2 - button1.Width - 40;

            button2.Left = this.ClientSize.Width / 2 + 40;

        }
















        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void AddStudentDetails()
        {
            string studentId = textBox1.Text;
            string connectionString = ConfigInfo.GetDbConnectionString();

            string checkQuery = "SELECT COUNT(*) FROM student_already WHERE stuId = @StudentId";
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
            {
                checkCommand.Parameters.AddWithValue("@StudentId", studentId);
                connection.Open();
                int exists = (int)checkCommand.ExecuteScalar();
                if (exists > 0)
                {
                    // 如果学生已存在，弹出警告对话框并返回
                    MessageBox1 form = new MessageBox1("This student already exists in the database!");
                    form.ShowDialog();
                    //MessageBox.Show("This student already exists in the database!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }


            string query = "INSERT INTO student_already(stuName, stuId, registerStatus) VALUES (@stuName, @StudentId, @registerStatus)";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // 将参数添加到SQL查询
                    command.Parameters.AddWithValue("@StudentId", studentId);
                    command.Parameters.AddWithValue("@stuName", textBox2.Text);
                    command.Parameters.AddWithValue("@registerStatus", 0);

                    try
                    {
                        connection.Open();
                        int result = command.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox1 form = new MessageBox1("Student added successfully!");
                            form.ShowDialog();
                            //MessageBox.Show("Student added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox1 form = new MessageBox1("Error: " + ex.Message);
                        form.ShowDialog();
                        //MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (MessageBox2 form = new MessageBox2("Are you sure you want to add this student?"))
            {
                // 订阅事件
                form.RequestCloseMainForm += (senderObj, args) =>
                {
                    AddStudentDetails();
                    OnStudentAdded();
                };

                // 显示窗体作为模态对话框
                form.ShowDialog();
            }
            /*DialogResult dialogResult = MessageBox.Show("Are you sure you want to add this student?", "Confirm Addition", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                // 如果用户确认，继续添加操作
                AddStudentDetails();
                OnStudentAdded();
            }*/
        }
    }
}
