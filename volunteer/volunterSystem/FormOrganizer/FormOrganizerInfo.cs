using Model;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using Config;
using WindowsFormsApp1;

namespace volunterSystem

{
    public partial class FormOrganizerInfo : System.Windows.Forms.Form
    {
        private string userName = SharedData.userName;
        public FormOrganizerInfo()
        {
            InitializeComponent();
            LoadOrganizerInfo();

            this.Load += new EventHandler(FormOrganizerInfo_Load);


            this.Resize += new EventHandler(FormOrganizerInfo_Resize);
        }



        private void FormOrganizerInfo_Load(object sender, EventArgs e)
        {
            int studentId = SharedData.StuID;


            CenterPanelOnForm();



        }



        private void FormOrganizerInfo_Resize(object sender, EventArgs e)
        {
            // ...其他调整大小的逻辑
            //panel1.Width = this.Width * 3 / 4;

            //panel1.Height = this.Height - 90 - this.Height / 7;



            CenterPanelOnForm();



        }





        private void CenterPanelOnForm()
        {

            // 计算窗体的中心点
            int formCenterX = this.ClientSize.Width / 2;
            int formCenterY = this.ClientSize.Height / 2;

            // 更新panel1的位置，使其中心与窗体的中心对齐
            panel1.Left = formCenterX - panel1.Width / 2;
            panel1.Top = formCenterY - panel1.Height / 2 + 30;
        }







        private void LoadOrganizerInfo()
        {
            byte[] avatarBytes = null;
            string connectionString = ConfigInfo.GetDbConnectionString();
            string query = @"
            SELECT userName, companyName, email, state,avatar
            FROM organizer_audited WHERE userName = @userName";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@userName", userName);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read()) // 如果有数据返回
                        {
                            userNameLabel.Text = reader["userName"].ToString();
                            companyNameLabel.Text = reader["companyName"].ToString();
                            emailLabel.Text = reader["email"].ToString();
                            lbStatus.Text = reader["state"].ToString();
                            avatarBytes = reader["avatar"] as byte[];
                        }
                        reader.Close();
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
            else
            {
                // 设置默认头像
                pictureBox1.Image = Properties.Resources.DefaultAvatar;
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormEditOrganizerInfo organizerEditInfoForm = new FormEditOrganizerInfo();

            // 订阅 FormClosed 事件
            organizerEditInfoForm.FormClosed += OrganizerEditInfoForm_FormClosed;

            organizerEditInfoForm.Show();
        }

        private void OrganizerEditInfoForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // 当 FormEditOrganizerInfo 窗体关闭时，重新加载组织者信息
            LoadOrganizerInfo();
        }

      

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Picture files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFileName = openFileDialog.FileName;
                byte[] imageBytes;
                using (FileStream fs = new FileStream(selectedFileName, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        imageBytes = br.ReadBytes((int)fs.Length);
                    }
                }

                //string connectionString = "Server=8.208.98.57,1433;Database=Volunteer;User Id=sa;Password=Abc394639.;";
                string connectionString = "Data Source =.; Initial Catalog = Volunteer; Integrated Security = True";
                
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sqlQuery = "UPDATE organizer_audited SET avatar = @Avatar WHERE userName = @userName";
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Avatar", imageBytes);

                        string userName = SharedData.userName;
                        command.Parameters.AddWithValue("@userName", userName);
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox1 form = new MessageBox1("Avatar uploaded successfully!");
                form.ShowDialog();
                //MessageBox.Show("Avatar uploaded successfully!");

                // 刷新UI以显示新头像
                LoadOrganizerInfo(); // 确保在头像上传后重新加载并显示新头像

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Group_template.ViewPosts form = new Group_template.ViewPosts();
            form.ShowDialog();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
