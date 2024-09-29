using Config;
using Model;
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

namespace Activity
{
    public partial class inProgressUserDetail : Form
    {
        private Topic _Topic;
        public inProgressUserDetail(Topic topic)
        {
            InitializeComponent();
            _Topic = topic;

            button1.FlatStyle = FlatStyle.Flat;
            button1.FlatAppearance.BorderSize = 0; // 正确的方式来去除边框
            // 设置按钮文字颜色为白色
            button1.ForeColor = Color.White;

            // 订阅 Load 和 Resize 事件
            this.Load += inProgressUserDetail_Load;
            this.Resize += inProgressUserDetail_Resize;
        }

        private void inProgressUserDetail_Load(object sender, EventArgs e)
        {
            lb1.Text = _Topic.StuName;
            string connectionString = ConfigInfo.GetDbConnectionString();
            string query = "SELECT s1.state, s1.credit, s1.tel, s1.email FROM student_information s1, StuInvolvedActiv s2 " +
                "WHERE s1.stuId = @StuID AND s2.ActivityID = @activityID AND s2.ApplyStatus = 1";
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StuID", _Topic.StuID);
                    command.Parameters.AddWithValue("@activityID", _Topic.ActivityID);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string tel = reader["tel"].ToString();
                            string email = reader["email"].ToString();
                            string state = reader["state"].ToString();
                            string credit = reader["credit"].ToString();

                            lb2.Text = tel;
                            lb3.Text = email;
                            lb4.Text = state;
                            lb5.Text = credit;
                        }
                    }
                }
            }

            panel2.Location = new Point((this.ClientSize.Width - panel2.Width) / 2, (this.ClientSize.Height - panel2.Height) / 2);

            button1.Height = panel2.Height;
            button1.Location = new Point((panel2.Width - button1.Width) / 2, (panel2.Height - button1.Height) / 2);

            AdjustControls();


        }

        private void inProgressUserDetail_Resize(object sender, EventArgs e)
        {
            panel2.Location = new Point((this.ClientSize.Width - panel2.Width) / 2, (this.ClientSize.Height - panel2.Height) / 2);

            button1.Height = panel2.Height;
            button1.Location = new Point((panel2.Width - button1.Width) / 2, (panel2.Height - button1.Height) / 2);

            AdjustControls();
        }



        private void AdjustControls()
        {
            // 保证 panel3 位于窗体中心
            panel3.Location = new Point((this.ClientSize.Width - panel3.Width) / 2, (this.ClientSize.Height - panel3.Height) / 2);

            // 设置 button1 的大小和位置，使其高度与 panel3 相同，且位于 panel3 的中心
            button1.Size = new Size(button1.Width, panel3.Height);
            button1.Location = new Point((panel3.Width - button1.Width) / 2, 0); // 注意这里 Y 坐标为 0，因为我们希望按钮在 panel3 的垂直中心
        }














        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
