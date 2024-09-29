using Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace volunterSystem
{
    public partial class FormStuDetail : System.Windows.Forms.Form
    {
        private string studentId;
        public FormStuDetail(string studentId)
        {
            InitializeComponent();
            this.studentId = studentId;

            this.Load += new EventHandler(FormStuDetail_Load);
            this.Resize += new EventHandler(FormStuDetail_Resize);
        }



        private void FormStuDetail_Load(object sender, EventArgs e)
        {
            // 加载学生的详细信息
            LoadStudentDetails();

            //AlignLabels();
            AlignControlsEqually();
            //AlignLabelsVertically();


        }




        private void FormStuDetail_Resize(object sender, EventArgs e)
        {
            //AlignLabels();
            AlignControlsEqually();
            //AlignLabelsVertically();
        }








        private void AlignControlsEqually()
        {
            System.Windows.Forms.Label[] leftLabels = { label7, label1, label2, label3, label4, label5, label6, label8, label9, label10 };
            Control[] rightControls = { lbStuName, lbUserName, lbID, lbPwd, lbTel, lbEmail, lbQues, lbAns, lbCredit, lbStatus };

            int spaceFromTop = 90; // 到Form顶部的距离
            int spaceFromBottom = 40; // 到Form底部的距离
            int space = 60; // 到Form左右边的距离

            // 计算总的可用高度，即窗体高度减去顶部和底部的间距
            int totalAvailableHeight = this.ClientSize.Height - spaceFromTop - spaceFromBottom;

            // 计算每个控件之间的间距
            int verticalSpaceBetweenControls = totalAvailableHeight / (leftLabels.Length + 1);

            int labelTop = spaceFromTop;

            for (int i = 0; i < leftLabels.Length; i++)
            {
                // 设置左边Label的位置
                leftLabels[i].Left = space; // 左边的间距
                leftLabels[i].Top = labelTop;

                // 设置右边控件的位置
                if (i < rightControls.Length) // 防止数组越界
                {
                    rightControls[i].Top = labelTop;
                    // 保持右侧控件到Form右边的距离与左边Label到Form左边的距离一样
                    rightControls[i].Left = this.ClientSize.Width - rightControls[i].Width - space;
                }


                // 更新下一个label的顶部位置
                labelTop += verticalSpaceBetweenControls;
            }

            // 特殊处理button1
            button1.Top = labelTop;
            button1.Left = this.ClientSize.Width / 2 - button1.Width / 2;
        }




        private void AlignLabelsVertically()
        {
            System.Windows.Forms.Label[] leftLabels = { label7, label1, label2, label3, label4, label5, label6, label8, label9, label10 };
            System.Windows.Forms.Label[] rightLabels = { lbStuName, lbUserName, lbID, lbPwd, lbTel, lbEmail, lbQues, lbAns, lbCredit, lbStatus };

            int spaceFromTop = 70; // 第一个label距离Form顶部的间距
            int verticalSpaceBetweenLabels = 30; // Label控件之间的垂直间距

            int space = 60;

            int labelTop = spaceFromTop;

            for (int i = 0; i < leftLabels.Length; i++)
            {
                // 设置左边Label的位置
                leftLabels[i].Left = space; // 左边的间距
                leftLabels[i].Top = labelTop;

                // 设置右边Label的位置
                rightLabels[i].Left = this.ClientSize.Width - rightLabels[i].Width - space; // 右边的间距
                rightLabels[i].Top = labelTop;

                // 更新下一个label的顶部位置
                labelTop += leftLabels[i].Height + verticalSpaceBetweenLabels;
            }

            // 现在，设置label6的位置
            // label6将放在最后一个label的下面，并与其保持相同的间距
            label6.Left = space; // 左边的间距与其他label保持一致
            label6.Top = labelTop;
        }






















        private void LoadStudentDetails()
        {
            string connectionString = ConfigInfo.GetDbConnectionString();
            string query = "SELECT stuName, nickname, stuId, stuPwd, tel, email, secProblem, answer, credit, state FROM student_information WHERE stuId = @StudentId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // 将参数添加到SQL查询
                    command.Parameters.AddWithValue("@StudentId", studentId);

                    try
                    {
                        // 打开数据库连接
                        connection.Open();

                        // 执行查询
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // 假设你的Label控件名称分别是labelName, labelID, ...
                                lbStuName.Text = reader["stuName"].ToString();
                                lbUserName.Text = reader["nickname"].ToString();
                                lbID.Text = reader["stuId"].ToString();
                                lbPwd.Text = reader["stuPwd"].ToString();
                                lbTel.Text = reader["tel"].ToString();
                                lbEmail.Text = reader["email"].ToString();
                                lbQues.Text = reader["secProblem"].ToString();
                                lbAns.Text = reader["answer"].ToString();
                                lbCredit.Text = reader["credit"].ToString();
                                lbStatus.Text = reader["state"].ToString();
                                // ...为其他标签赋值
                            }
                            else
                            {
                                MessageBox.Show("No student details found.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
