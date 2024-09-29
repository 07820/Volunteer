using Config;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1;



namespace volunterSystem
{
    public partial class FormStudentInfo : System.Windows.Forms.Form
    {
        public FormStudentInfo()
        {
            InitializeComponent();

            this.Load += new EventHandler(FormStudentInfo_Load);


            this.Resize += new EventHandler(FormStudentInfo_Resize);
        }


        private void FormStudentInfo_Load(object sender, EventArgs e)
        {
            int studentId = SharedData.StuID;

            LoadStudentInfo(studentId);

            //panel1.Width = this.Width * 3 / 4;

            //panel1.Height = this.Height - 90 - this.Height / 7;

            CenterPanelOnForm();


            
        }



        private void FormStudentInfo_Resize(object sender, EventArgs e)
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


        private void PositionAvatarControls()
        {
            // 假设您希望将avatarPictureBox放置在窗体顶部中央位置
            avatarPictureBox.Left = panel1.Width / 12;
            avatarPictureBox.Top = panel1.Width / 12; ; // 或者根据需要来设置顶部位置

            // 将uploadAvatarButton放置在avatarPictureBox下方并水平居中
            uploadAvatarButton.Left = avatarPictureBox.Left + (avatarPictureBox.Width - uploadAvatarButton.Width) / 2;
            uploadAvatarButton.Top = avatarPictureBox.Bottom + 10; // 与avatarPictureBox保持10像素的距离
        }

        private void PositionButtonsAtBottom()
        {
            // 计算每个按钮宽度的三等分点
            int thirdWidth = panel1.Width / 3;

            // 按钮1在第一个三等分点上水平居中
            btnEditInfo.Left = (thirdWidth - btnEditInfo.Width) / 2;
            // 按钮2在第二个三等分点上水平居中
            btnViewActivities.Left = thirdWidth + (thirdWidth - btnViewActivities.Width) / 2;
            // 按钮3在第三个三等分点上水平居中
            button1.Left = 2 * thirdWidth + (thirdWidth - button1.Width) / 2;

            // 设置按钮垂直居于窗体底部上方的位置
            int buttonBottomMargin = 10; // 假设距离底部10像素
            
            btnViewActivities.Top = panel1.Height - btnViewActivities.Height - buttonBottomMargin;
            
            btnEditInfo.Top = btnViewActivities.Top;
            
            button1.Top = btnViewActivities.Top;
        }


        private void PositionLabelsInPanel()
        {
            // 计算控件间的垂直间距
            int totalLabelsHeight = label1.Height * 5; // 假设有五个同样大小的label控件
            int usableHeight = (int)(panel1.Height * 2 / 3); // 使用panel1高度的三分之二
            int spacing = (usableHeight - totalLabelsHeight) / 6; // 有六个间隔

            // 计算起始位置
            int startY = panel1.Top + (panel1.Height - usableHeight) / 2 + spacing;

            // 定义label控件数组，便于操作
            Label[] labels = { label2, label4, label1, label3, label5 }; // 用您实际的控件名替换这些

            // 循环设置每个控件的位置
            for (int i = 0; i < labels.Length; i++)
            {
                labels[i].Top = startY + i * (label1.Height + spacing);
                labels[i].Left = panel1.Left + (panel1.Width - labels[i].Width) / 2; // 水平居中于panel1
            }
        }
























        private void LoadStudentInfo(int studentId)
        {
            byte[] avatarBytes = null;
            string nickName = "";

            avatarBytes = GetStudentAvatarBytes(studentId);
            nickName = GetStudentNickName(studentId);

            string connectionString = ConfigInfo.GetDbConnectionString();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT stuId, stuName,state, credit FROM student_information WHERE stuId = @StuID";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@StuID", studentId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lbname.Text = reader["stuName"].ToString();
                            lbID.Text = reader["stuId"].ToString() ;
                            lbCredit.Text = reader["credit"].ToString();
                            lbStatus.Text = reader["state"].ToString();
                        }
                    }
                }
            }

                if (avatarBytes != null && avatarBytes.Length > 0)
            {
                using (var ms = new MemoryStream(avatarBytes))
                {
                    avatarPictureBox.Image = Image.FromStream(ms);
                    avatarPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
            else
            {
                // 设置默认头像
                avatarPictureBox.Image = Properties.Resources.DefaultAvatar;
                avatarPictureBox.SizeMode = PictureBoxSizeMode.Zoom;

            }
            nickNameLabel.Text = nickName;
        }

        private string GetStudentNickName(int studentId)
        {
            string connectionString = ConfigInfo.GetDbConnectionString();
            string nickName = "";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT nickName FROM student_information WHERE stuId = @StudentId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentId", studentId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            nickName = reader["nickName"] as string;
                        }
                    }
                }
            }

            return nickName;
        }

        private byte[] GetStudentAvatarBytes(int studentId)
        {
            // 从配置文件或代码中获取数据库连接字符串
            string connectionString = ConfigInfo.GetDbConnectionString();

            // 定义一个存储头像字节数组的变量
            byte[] avatarBytes = null;

            // 使用using语句确保SqlConnection对象在使用完毕后能被正确地释放
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // 打开数据库连接
                connection.Open();

                // 定义查询字符串，使用参数化查询防止SQL注入攻击
                string query = "SELECT avatar FROM student_information WHERE stuId = @StudentId";

                // 创建SqlCommand对象
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // 定义查询参数并赋值
                    command.Parameters.AddWithValue("@StudentId", studentId);

                    // 执行查询并获取结果
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // 检查是否有结果
                        if (reader.Read())
                        {
                            // 判断结果是否为DBNull，如果不是，则读取数据
                            if (!reader.IsDBNull(0))
                            {
                                avatarBytes = reader["avatar"] as byte[];
                            }
                        }
                    }
                }
            }

            // 返回头像字节数据
            return avatarBytes;
        }







        

        private void uploadAvatarButton_Click(object sender, EventArgs e)
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
                    string sqlQuery = "UPDATE student_information SET avatar = @Avatar WHERE stuId = @UserID";
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Avatar", imageBytes);
                        // 确保你有正确的方式来获取学生ID
                        int studentId = SharedData.StuID;
                        command.Parameters.AddWithValue("@UserID", studentId);
                        command.ExecuteNonQuery();
                    }
                }

                MessageBox1 form = new MessageBox1("Avatar uploaded successfully!");
                form.ShowDialog();
                //MessageBox.Show("Avatar uploaded successfully!");

                // 刷新UI以显示新头像
                LoadStudentInfo(SharedData.StuID); // 确保在头像上传后重新加载并显示新头像
            }
        }

        private void btnEditInfo_Click(object sender, EventArgs e)
        {
            FormEditStudentInfo editForm = new FormEditStudentInfo();
            int studentId = FormLogin.stuID; // 获取当前学生ID
            editForm.LoadStudentDetails(studentId); // 调用方法以加载并显示学生详细信息

            // 订阅 FormClosed 事件
            editForm.FormClosed += EditForm_FormClosed;

            editForm.ShowDialog(); // 显示编辑窗体作为对话框
        }

        private void EditForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // 当 FormEditStudentInfo 窗体关闭时，重新加载学生信息
            int studentId = FormLogin.stuID; // 确保有正确的逻辑来获取当前学生ID
            LoadStudentInfo(studentId);
        }

        private void btnViewActivities_Click(object sender, EventArgs e)
        {
            int studentId = FormLogin.stuID; // 获取当前学生的ID
            FormMyActivities myActivitiesForm = new FormMyActivities();
            //myActivitiesForm.LoadMyActivities(studentId);
            myActivitiesForm.ShowDialog(); // 显示FormMyActivities

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Group_template.ViewPosts form = new Group_template.ViewPosts();
            form.ShowDialog();
        }

        private void nickNameLabel_Click(object sender, EventArgs e)
        {

        }

        private void avatarPictureBox_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void lbname_Click(object sender, EventArgs e)
        {

        }

        private void lbID_Click(object sender, EventArgs e)
        {

        }

        private void lbCredit_Click(object sender, EventArgs e)
        {

        }

        private void lbStatus_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
