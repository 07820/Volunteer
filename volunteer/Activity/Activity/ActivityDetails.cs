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
using System.Xml;
using System.IO;
using Config;
using Map;

namespace Activity
{



    public partial class ActivityDetails : Form
    {
        public double lat = 51.303065;
        public double lng = 0.73175;

        public ActivityDetails(int activityId)
        {/*public Form2(string eventName, string eventPopulation, string eventDescription, DateTime eventDate, string servicePlace)*/
            InitializeComponent();

            this.label3.MouseEnter += new EventHandler(label3_MouseEnter);
            this.label3.MouseLeave += new EventHandler(label3_MouseLeave);

            panel2.BackColor = Color.FromArgb(240, 255, 240); // 浅绿色
            panel3.BackColor = Color.FromArgb(255, 255, 224); // 浅黄色
            panel4.BackColor = Color.FromArgb(255, 224, 224); // 浅红色


            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;

            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;

            pictureBox5.SizeMode = PictureBoxSizeMode.StretchImage;

            pictureBox6.SizeMode = PictureBoxSizeMode.StretchImage;

            pictureBox7.SizeMode = PictureBoxSizeMode.StretchImage;

            pictureBox8.SizeMode = PictureBoxSizeMode.StretchImage;
            //panel5.BackColor = Color.FromArgb(245, 245, 245);


            // 绑定事件处理器
            linkLabel2.MouseEnter += linkLabel_MouseEnter;
            linkLabel2.MouseLeave += linkLabel_MouseLeave;


            // 为Form的Load事件添加事件处理器
            this.Load += (sender, e) => {
                AdjustPanelPosition(); // 调整panel1位置
                AdjustLinkLabelPosition(); // 调整linkLabel2位置
            };

            // 为Form的Resize事件添加事件处理器
            this.Resize += (sender, e) => {
                AdjustPanelPosition(); // 当窗体大小改变时，重新调整panel1位置
                AdjustLinkLabelPosition(); // 当窗体大小改变时，重新调整linkLabel2位置
            };



            LoadActivityData(activityId);
        }


        private void label3_MouseEnter(object sender, EventArgs e)
        {
            Label lbl = sender as Label;
            if (lbl != null)
            {
                lbl.BackColor = Color.LightBlue; // 鼠标悬停时的背景色
                lbl.ForeColor = Color.Gray;     // 鼠标悬停时的文字颜色
            }
        }

        // 鼠标离开时触发的事件
        private void label3_MouseLeave(object sender, EventArgs e)
        {
            Label lbl = sender as Label;
            if (lbl != null)
            {
                lbl.BackColor = Color.Transparent; // 鼠标离开时恢复背景色
                lbl.ForeColor = Color.Black;       // 鼠标离开时恢复文字颜色
            }
        }

        private void AdjustLinkLabelPosition()
        {
            // 计算panel6的中心位置
            int centerPanelX = panel6.Width / 2;
            int centerPanelY = panel6.Height / 2;

            // 计算linkLabel2的中心位置
            int centerLinkLabelX = linkLabel2.Width / 2;
            int centerLinkLabelY = linkLabel2.Height / 2;

            // 设置linkLabel2的新位置，使其位于panel6的正中心
            linkLabel2.Location = new Point(centerPanelX - centerLinkLabelX, centerPanelY - centerLinkLabelY);

            // 确保linkLabel2是panel6的子控件
            linkLabel2.Parent = panel6;
        }



        private void AdjustPanelPosition()
        {

            panel1.Width = this.ClientSize.Width * 2 / 3;

            // 计算窗体的水平中心点
            int centerFormX = this.ClientSize.Width / 2;

            // 计算panel1的宽度的一半
            int halfPanelWidth = panel1.Width / 2;

            // 设置panel1的新位置，使其水平居中
            panel1.Location = new Point(centerFormX - halfPanelWidth, panel1.Location.Y);




            panel5.Left = 0;
            panel5.Top = 0;

            panel5.Width = this.ClientSize.Width;

            panel5.Height = this.ClientSize.Height;

            pictureBox1.Left = 0;

            pictureBox1.Top = 66;

            pictureBox1.Width = this.ClientSize.Width;

            pictureBox1.Height = this.ClientSize.Height * 5 / 12;

            panel1.Top = pictureBox1.Bottom - panel1.Height * 10 / 24 ;



            int totalSpace = panel1.Width - panel2.Width * 3;

            int space = totalSpace / 4;

            panel2.Left = space;

            panel3.Left = space + panel2.Width + space;

            panel4.Left = space + panel3.Width + panel2.Width + space + space;

            //panel2.Top = panel1.Height - panel2.Height - space;

            //panel3.Top = panel2.Top;

            //panel4.Top = panel2.Top;


            label8.Top = panel1.Bottom + 8;

            textBox1.Top = label8.Bottom + 8;

            label8.Left = 10;

            textBox1.Left = panel5.Left + 8;

            textBox1.Width = panel5.Width - textBox1.Left * 2;

            textBox1.Height = panel5.Height - textBox1.Top - panel6.Height - 10;

        }





        private void linkLabel_MouseEnter(object sender, EventArgs e)
        {
            if (sender is LinkLabel linkLabel)
            {
                // 模拟阴影效果，例如通过加粗字体
                linkLabel.Font = new Font(linkLabel.Font, FontStyle.Bold);
                // 改变颜色以提供视觉反馈，假设默认颜色是黑色，悬停时改为灰色
                linkLabel.LinkColor = Color.White;
            }
        }

        private void linkLabel_MouseLeave(object sender, EventArgs e)
        {
            if (sender is LinkLabel linkLabel)
            {
                // 还原到默认样式
                linkLabel.Font = new Font(linkLabel.Font, FontStyle.Regular);
                // 假设默认颜色为黑色
                linkLabel.LinkColor = Color.White;
            }
        }





        private void LoadActivityData(int activityId)
        {
            byte[] avatarBytes = null;
            string connectionString = ConfigInfo.GetDbConnectionString();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "SELECT Lng,Lat,ActivityName, ActivityPopulation, ActivityDate, ActivityAddress, ActivityContent, ActivityType, ActivityServePeople, ActivityPlace, Status, ActivityGraph, duration, NumOfInvolved, point FROM Events WHERE ActivityID = @ActivityID";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ActivityID", activityId);
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            linkLabel1.Text = reader["ActivityName"].ToString();
                            label1.Text = reader["ActivityPopulation"].ToString();
                            DateTime activityDate = Convert.ToDateTime(reader["ActivityDate"]);
                            label2.Text = activityDate.Date.ToString("yyyy-MM-dd");
                            label3.Text = reader["ActivityAddress"].ToString();
                            label4.Text = reader["ActivityType"].ToString();
                            label5.Text = reader["ActivityServePeople"].ToString();
                            label6.Text = reader["ActivityPlace"].ToString();
                            textBox1.Text = reader["ActivityContent"].ToString();
                            try
                            {
                                this.lat = Convert.ToDouble(reader["Lat"]);
                                this.lng = Convert.ToDouble(reader["Lng"]);
                            }
                            catch (Exception ex)
                            {

                            }
                            avatarBytes = reader["ActivityGraph"] as byte[];
                        }
                    }
                }

                if (avatarBytes != null && avatarBytes.Length > 0)
                {
                    using (var ms = new MemoryStream(avatarBytes))
                    {
                        pictureBox1.Image = Image.FromStream(ms);
                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                }

            }
        }

        

        

        private void labelEventName_Click(object sender, EventArgs e)
        {

        }

        private void lableEventDescription_Click(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void lableEventDate_Click(object sender, EventArgs e)
        {

        }

        private void Population_Click(object sender, EventArgs e)
        {

        }

        private void Service_place_Click(object sender, EventArgs e)
        {

        }

        private void label_Address_Click(object sender, EventArgs e)
        {

        }

        private void label_ServePeople_Click(object sender, EventArgs e)
        {

        }

        private void label_ServiceType_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox_out_Click(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void Form2_Load_1(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        { /*
            ShowForm showForm = new ShowForm();
            showForm.SelectPoint(lat, lng);
            showForm.Show();
            */
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            /*ShowForm showForm = new ShowForm();
            showForm.SelectPoint(lat, lng);
            showForm.Show();*/
        }

        private void label3_Click(object sender, EventArgs e)
        {
            ShowForm showForm = new ShowForm();
            showForm.SelectPoint(lat, lng);
            showForm.Show();
        }
    }
}
