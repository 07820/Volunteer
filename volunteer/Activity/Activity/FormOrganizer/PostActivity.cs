using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Model;
using Config;
using Map;
/*using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;*/

namespace Activity
{
    public partial class PostActivity : Form
    {
        private byte[] selectedImageBytes;
        public double lat = 51.303065;
        public double lng = 0.73175;
        public PostActivity()
        {
            InitializeComponent();

            // 初始化按钮样式
            InitializeButtonStyle(button_commit);
            InitializeButtonStyle(button_exit);





            // 为Load事件添加事件处理器
            this.Load += adjustEvent_Load;
            // 为panel2的Resize事件添加事件处理器
            this.panel2.Resize += panel2_Resize;
        }


        private void adjustEvent_Load(object sender, EventArgs e)
        {
           AdjustButtonsPosition(); // 在窗体加载时调整按钮位置
        }

        private void panel2_Resize(object sender, EventArgs e)
        {
            AdjustButtonsPosition(); // 当panel2的大小调整时，调整按钮位置
        }


        private void InitializeButtonStyle(System.Windows.Forms.Button btn)
        {
            // 设置按钮样式为Flat，边框大小为0
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            // 设置按钮的默认字体和颜色
            btn.ForeColor = Color.White;
            btn.Font = new Font(btn.Font.Name, btn.Font.Size, FontStyle.Regular);

            // 鼠标悬停时改变样式
            btn.MouseEnter += (sender, e) => {
                if (sender is System.Windows.Forms.Button button)
                {
                    button.ForeColor = Color.Yellow; // 示例：悬停时的字体颜色
                    button.Font = new Font(button.Font.Name, button.Font.Size, FontStyle.Bold); // 加粗字体以模拟阴影效果
                }
            };

            // 鼠标离开时恢复默认样式
            btn.MouseLeave += (sender, e) => {
                if (sender is System.Windows.Forms.Button button)
                {
                    button.ForeColor = Color.White; // 恢复默认字体颜色
                    button.Font = new Font(button.Font.Name, button.Font.Size, FontStyle.Regular); // 恢复默认字体样式
                }
            };

            // 鼠标按下时改变样式
            btn.MouseDown += (sender, e) => {
                if (sender is System.Windows.Forms.Button button)
                {
                    button.ForeColor = Color.Red; // 示例：按下时的字体颜色
                                                  // 可以在这里进一步自定义样式，例如改变背景色等
                }
            };

            // 鼠标释放时根据鼠标位置恢复样式
            btn.MouseUp += (sender, e) => {
                if (sender is System.Windows.Forms.Button button)
                {
                    // 根据鼠标是否仍然在按钮上来决定恢复到哪种样式
                    button.ForeColor = button.ClientRectangle.Contains(button.PointToClient(Control.MousePosition)) ? Color.Yellow : Color.White;
                    button.Font = new Font(button.Font.Name, button.Font.Size, FontStyle.Regular); // 假设鼠标释放后总是恢复到默认字体样式
                }
            };
        }






        private void AdjustButtonsPosition()
        {
            // 确保btnCommit和btnCancel存在
            if (button_commit == null || button_exit == null) return;

            // 计算panel2每个区块的中心点位置
            int leftCenter = panel2.Width / 4;
            int rightCenter = leftCenter * 3;

            // 设置btnCommit位置（左区块的中心）
            button_commit.Left = leftCenter - (button_commit.Width / 2);
            button_commit.Top = (panel2.Height - button_commit.Height) / 2;

            // 设置btnCancel位置（右区块的中心）
            button_exit.Left = rightCenter - (button_exit.Width / 2);
            button_exit.Top = (panel2.Height - button_exit.Height) / 2;


            panel3.Left = 30;

            panel3.Top = panel1.Bottom + 10;

            panel4.Left = this.ClientSize.Width / 2 + 30;

            panel4.Width = this.ClientSize.Width - panel4.Left - 30;

            panel4.Top = panel3.Top;

            txt_content.Width = panel4.Width - 20;

            int totalSpace = panel2.Top - panel3.Bottom - groupBox1.Height - groupBox2.Height - groupBox3.Height;

            int space = totalSpace / 4;

            groupBox1.Top = panel3.Bottom + space;

            labelService.Top = groupBox1.Top + 15;
            
            groupBox2.Top = groupBox1.Bottom + space;

            groupBox2.Left = groupBox1.Left;

            label6.Top = groupBox2.Top + 15;

            groupBox3.Top = groupBox2.Bottom + space;

            groupBox3.Left = groupBox1.Left;

            labelServicePlace.Top = groupBox3.Top + 15;

            //groupBox1.Top = panel3.Bottom + 
            labelService.Left = panel3.Left;

            label6.Left = labelService.Left;

            labelServicePlace.Left = label6.Left;

            panel5.Left = panel4.Right - panel4.Width / 2 - panel5.Width / 2;

            panel5.Top = groupBox1.Bottom + space / 2;


        }




























        private void B_uploadImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files (*.jpg;*.jpeg;*.png;*.gif;*.bmp)|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    pictureBox.Image = Image.FromFile(openFileDialog.FileName);
                    pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                    selectedImageBytes = File.ReadAllBytes(openFileDialog.FileName);
                }
            }
        }

        private string GetSelectedRadioButtonValue(GroupBox groupBox)
        {
            foreach (RadioButton rb in groupBox.Controls.OfType<RadioButton>())
            {
                if (rb.Checked)
                {
                    return rb.Text; // Or any other value that's relevant
                }
            }
            return string.Empty; // Or handle this scenario as needed
        }




        private void button_commit_Click_2(object sender, EventArgs e)
        {
            string connectionString = ConfigInfo.GetDbConnectionString();
            string activityName = txt_title.Text; // txtEventName
            string activityAddress = txt_address.Text;
            string activityContent = txt_content.Text;
            decimal activityPopulation = numericUpDown1_population.Value;
            int duration = int.Parse(textBox1.Text);
            DateTime activityDate = dateTimePicker1.Value;
            int point = int.Parse(textBox2.Text);

            string activityType = GetSelectedRadioButtonValue(groupBox1);
            string activityServePeople = GetSelectedRadioButtonValue(groupBox2);
            string activityPlace = GetSelectedRadioButtonValue(groupBox3);



            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string email = null;
                string sql1 = "SELECT email FROM organizer_audited WHERE userName = @userName";
                using (SqlCommand cmd1 = new SqlCommand(sql1, conn))
                {
                    cmd1.Parameters.AddWithValue("@userName", SharedData.userName);
                    using (SqlDataReader reader = cmd1.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            email = reader["email"].ToString();
                        }
                    }
                }

                string sql = "INSERT INTO Events ( Lat,Lng,ActivityName, ActivityPopulation, ActivityDate, ActivityAddress, ActivityContent, ActivityType, ActivityServePeople, ActivityPlace, ActivityGraph, Status, email, NumOfInvolved, duration, point) " +
                    "VALUES (@lat,@lng,@activityName,@activityPopulation,@activityDate, @activityAddress, @activityContent, @activityType, @activityServePeople, @activityPlace, @activityGraph, @Status, @email,@NumOfInvolved, @duration, @point); SELECT SCOPE_IDENTITY();";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@activityName", activityName);
                    cmd.Parameters.AddWithValue("@activityPopulation", activityPopulation);
                    cmd.Parameters.AddWithValue("@activityDate", activityDate);
                    cmd.Parameters.AddWithValue("@activityAddress", activityAddress);
                    cmd.Parameters.AddWithValue("@activityContent", activityContent);
                    cmd.Parameters.AddWithValue("@activityType", activityType);
                    cmd.Parameters.AddWithValue("@activityServePeople", activityServePeople);
                    cmd.Parameters.AddWithValue("@activityPlace", activityPlace);
                    cmd.Parameters.AddWithValue("@lat", lat);
                    cmd.Parameters.AddWithValue("@lng", lng);

                    if (selectedImageBytes != null && selectedImageBytes.Length > 0)
                    {
                        /*cmd.Parameters.AddWithValue("activityGraph", selectedImageBytes);*/
                        cmd.Parameters.Add("@activityGraph", SqlDbType.Image).Value = selectedImageBytes;
                    }
                    else
                    {
                        /*cmd.Parameters.AddWithValue("@activityGraph", DBNull.Value);*/
                        cmd.Parameters.Add("@activityGraph", SqlDbType.Image).Value = DBNull.Value;
                    }
                    cmd.Parameters.AddWithValue("@Status", "In progress");
                    cmd.Parameters.AddWithValue("@userName", SharedData.userName);
                    cmd.Parameters.AddWithValue("@NumOfInvolved", 0);
                    cmd.Parameters.AddWithValue("@duration", duration);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@point", point);


                    this.Close();

                    int activityId = Convert.ToInt32(cmd.ExecuteScalar());


                    // tranpoart
                    //ActivityDetails displayForm = new ActivityDetails(activityId); // receive activityId
                    //displayForm.Show();


                    /*Form3 Option = new Form3();
                    Option.RefreshTopics();

                    Option.ShowDialog();*/

                }
            }
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {

        }

        private void uploadimage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files (*.jpg;*.jpeg;*.png;*.gif;*.bmp)|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    pictureBox.Image = Image.FromFile(openFileDialog.FileName);
                    pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                    selectedImageBytes = File.ReadAllBytes(openFileDialog.FileName);
                }
            }
        }
        private void txt_title_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt_content_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {


        }
        private void txt_population_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private string selectedServicePlace;


        private void numericUpDown1_population_ValueChanged(object sender, EventArgs e)
        {

        }

        private string selectedServiceType;


        private string selectedServicePeople;



        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }


        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void B_uploadImage_Click_1(object sender, EventArgs e)
        {

        }

        private void button_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void labelServicePlace_Click(object sender, EventArgs e)
        {

        }

        private void txt_title_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SelectForm selectForm = new SelectForm(lat, lng);
            selectForm.MapAction = (lat, lng, place) =>
            {
                this.lat = lat;
                this.lng = lng;
                txt_address.Text = place;
            };
            selectForm.Show();
        }
    }
}
