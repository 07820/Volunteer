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

namespace volunterSystem
{
    public partial class FormEditOrganizerInfo : System.Windows.Forms.Form
    {
        public FormEditOrganizerInfo()
        {
            InitializeComponent();

            // 为 Load 和 Resize 事件订阅 PositionControls 方法
            this.Load += FormEditOrganizerInfo_Load;
            this.Resize += FormEditOrganizerInfo_Resize;
        }
        private void FormEditOrganizerInfo_Load(object sender, EventArgs e)
        {
            // 使用组织者的 userName 来加载其详细信息
            string organizerUserName = FormLogin.orgUserName; // 假设这是获取组织者 userName 的方式
            LoadOrganizerDetails(organizerUserName);

            PositionLabelsEqually(); // 在窗体加载时调整控件位置
        }

        private void FormEditOrganizerInfo_Resize(object sender, EventArgs e)
        {
            PositionLabelsEqually(); // 在窗体大小改变时调整控件位置
        }


     
        private void PositionLabelsEqually()
        {
            // 定义标签数组，便于操作
            Label[] labels = { label2, label4, label5, label6 }; // 使用您实际的控件名称替换

            // 窗体的五分之一高度和三分之二高度
            int oneFifthHeight = 85;
            int twoThirdsHeight = (int)(this.ClientSize.Height * 2 / 3);

            // 计算标签间的垂直间距
            int totalLabelsSpacing = twoThirdsHeight - oneFifthHeight;
            int spacingBetweenLabels = totalLabelsSpacing / (labels.Length + 1); // +1 为了在顶部和底部都有间隔

            // 计算起始位置
            int currentY = oneFifthHeight + spacingBetweenLabels;

            // 窗体中心线左边距离为20像素的位置
            int rightAlignPosition = (this.ClientSize.Width / 2) - 40;

            foreach (Label label in labels)
            {
                label.Top = currentY;

                // 设置标签的右侧到中心线的左边距离为20
                label.Left = rightAlignPosition - label.Width;

                // 更新下一个标签的Y位置
                currentY += spacingBetweenLabels;
            }

            txtCompanyName.Left = label2.Right + 8;
            txtCompanyName.Top = label2.Top;

            txtOrgPwd.Left = label4.Right + 8;
            txtOrgPwd.Top = label4.Top;

            txtSecProblem.Left = label5.Right + 8;
            txtSecProblem.Top = label5.Top;

            txtAnswer.Left = label6.Right + 8;
            txtAnswer.Top = label6.Top;

            button1.Top = this.ClientSize.Height - button1.Height - spacingBetweenLabels;

            button2.Top = button1.Top;

            button1.Left = this.ClientSize.Width / 2 - button1.Width - 40;

            button2.Left = this.ClientSize.Width / 2 + 40;
        }























        public void LoadOrganizerDetails(string organizerUserName)
        {
            string connectionString = ConfigInfo.GetDbConnectionString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT userName, companyName, orgPwd, secProblem, answer FROM organizer_audited WHERE userName = @OrganizerUserName";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OrganizerUserName", organizerUserName);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtCompanyName.Text = reader["companyName"].ToString();
                            txtOrgPwd.Text = reader["orgPwd"].ToString();
                            txtSecProblem.Text = reader["secProblem"].ToString() ;
                            txtAnswer.Text = reader["answer"].ToString();
                        }
                    }
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string organizerUserName = FormLogin.orgUserName; // 用于获取组织者 userName 的逻辑

            string connectionString = ConfigInfo.GetDbConnectionString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"UPDATE organizer_audited
                                 SET companyName = @CompanyName, orgPwd = @OrgPwd, secProblem = @SecProblem, answer = @Answer
                                 WHERE userName = @OrganizerUserName; ";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CompanyName", txtCompanyName.Text);
                    command.Parameters.AddWithValue("@OrgPwd", txtOrgPwd.Text);
                    command.Parameters.AddWithValue("@SecProblem", txtSecProblem.Text);
                    command.Parameters.AddWithValue("@Answer", txtAnswer.Text);
                    command.Parameters.AddWithValue("@OrganizerUserName", organizerUserName);

                    command.ExecuteNonQuery();
                }
            }

            MessageBox1 form = new MessageBox1("Information has been updated!");
            form.ShowDialog();
            //MessageBox.Show("Information has been updated!");
            this.Close(); // 关闭窗体
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
