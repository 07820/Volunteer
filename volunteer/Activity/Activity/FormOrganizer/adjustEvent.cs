using Config;
using Map;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
/*using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;*/

namespace Activity
{
    public partial class adjustEvent : Form
    {
        private int activityId; // 活动ID
        private byte[] selectedImageBytes; // 选中的图片字节
        public double lat = 51.303065;
        public double lng = 0.73175;


        // 修改构造函数以接受活动ID
        public adjustEvent(int activityId)
        {
            InitializeComponent();
            this.activityId = activityId; // 设置活动ID


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

            panel3.Top =  panel1.Bottom + 10;

            panel4.Left = this.ClientSize.Width / 2 + 30;

            panel4.Width = this.ClientSize.Width - panel4.Left - 30;

            panel4.Top = panel3.Top;

            txt_content.Width = panel4.Width - 20;

            int totalSpace = panel2.Top - panel3.Bottom - groupBox1.Height - groupBox2.Height - groupBox3.Height;

            int space = totalSpace / 4;

            groupBox1.Top = panel3.Bottom + space;

            labelService.Top = groupBox1.Top + 15;

            groupBox2.Top = groupBox1.Bottom + space;

            label6.Top = groupBox2.Top + 15;

            groupBox3.Top = groupBox2.Bottom + space;

            labelServicePlace.Top = groupBox3.Top + 15;

            //groupBox1.Top = panel3.Bottom + 
            labelService.Left = panel3.Left;

            label6.Left = labelService.Left;

            labelServicePlace.Left = label6.Left;

            panel5.Left = panel4.Right - panel4.Width / 2 - panel5.Width / 2;

            panel5.Top = groupBox1.Bottom + space / 2;


        }




















        // Utility method to select the correct radio button based on its text.
        private void SetSelectedRadioButton(GroupBox groupBox, string value)
        {
            foreach (RadioButton rb in groupBox.Controls)
            {
                if (rb.Text == value)
                {
                    rb.Checked = true;
                    break;
                }
            }
        }


        private string GetSelectedRadioButtonValue(GroupBox groupBox)
        {
            foreach (RadioButton rb in groupBox.Controls.OfType<RadioButton>())
            {
                if (rb.Checked)
                {
                    return rb.Text;
                }
            }
            return null; // Or handle this scenario as needed
        }



      

        private void button_commit_Click_1(object sender, EventArgs e)
        {
            string connectionString = ConfigInfo.GetDbConnectionString();
            string activityName = txt_title.Text;
            string activityAddress = txt_address.Text;
            string activityContent = txt_content.Text;
            decimal activityPopulation = numericUpDown1_population.Value;
            int duration = int.Parse(textBox1.Text);
            DateTime activityDate = dateTimePicker1.Value;

            string activityType = GetSelectedRadioButtonValue(groupBox1);
            string activityServePeople = GetSelectedRadioButtonValue(groupBox2);
            string activityPlace = GetSelectedRadioButtonValue(groupBox3);
            string status;

            if (activityDate.AddDays(duration) < DateTime.Now)
            {
                status = "Activity closed";
            }
            else
            {
                status = "In progress";
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "UPDATE Events SET ActivityName = @activityName, ActivityPopulation = @activityPopulation, duration = @duration, ActivityDate = @activityDate, ActivityAddress = @activityAddress, ActivityContent = @activityContent, " +
                    "ActivityType = @activityType, ActivityServePeople = @activityServePeople, ActivityPlace = @activityPlace, ActivityGraph = @activityGraph ,Status = @Status WHERE ActivityID = @activityId;" +
                    "Update StuInvolvedActiv set ActivityStatus = @status, ActivityName = @activityName where ActivityID = @activityId;" +
                    "Update Events set Lat = @lat, Lng = @lng where ActivityID = @activityId;";


                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@activityId", this.activityId);
                    cmd.Parameters.AddWithValue("@activityName", activityName);
                    cmd.Parameters.AddWithValue("@activityPopulation", activityPopulation);
                    cmd.Parameters.AddWithValue("@duration", duration);
                    cmd.Parameters.AddWithValue("@activityDate", activityDate);
                    cmd.Parameters.AddWithValue("@activityAddress", activityAddress);
                    cmd.Parameters.AddWithValue("@activityContent", activityContent);
                    cmd.Parameters.AddWithValue("@activityType", activityType);
                    cmd.Parameters.AddWithValue("@activityServePeople", activityServePeople);
                    cmd.Parameters.AddWithValue("@activityPlace", activityPlace);
                    cmd.Parameters.AddWithValue("@lat", lat);
                    cmd.Parameters.AddWithValue("@lng", lng);
                    cmd.Parameters.Add("@activityGraph", SqlDbType.Image).Value = (selectedImageBytes != null && selectedImageBytes.Length > 0) ? (object)selectedImageBytes : DBNull.Value;
                    cmd.Parameters.AddWithValue("@status", status);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            this.Close(); // Close the form after updating
        }

        private void button_exit_Click_1(object sender, EventArgs e)
        {

            this.Close();
        }

        private void adjustEvent_Load_1(object sender, EventArgs e)
        {
            string connectionString = ConfigInfo.GetDbConnectionString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT Lat,Lng,ActivityName, ActivityPopulation, ActivityDate, ActivityAddress, ActivityContent, ActivityType, ActivityServePeople, ActivityPlace, ActivityGraph,duration FROM Events WHERE ActivityID = @activityId";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@activityId", this.activityId);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txt_title.Text = reader["ActivityName"].ToString();
                            numericUpDown1_population.Value = Convert.ToDecimal(reader["ActivityPopulation"]);
                            dateTimePicker1.Value = Convert.ToDateTime(reader["ActivityDate"]);
                            txt_address.Text = reader["ActivityAddress"].ToString();
                            txt_content.Text = reader["ActivityContent"].ToString();
                            textBox1.Text = reader["duration"].ToString();
                            SetSelectedRadioButton(groupBox1, reader["ActivityType"].ToString());
                            SetSelectedRadioButton(groupBox2, reader["ActivityServePeople"].ToString());
                            SetSelectedRadioButton(groupBox3, reader["ActivityPlace"].ToString());
                            try
                            {
                                this.lat = Convert.ToDouble(reader["Lat"]);
                                this.lng = Convert.ToDouble(reader["Lng"]);
                            }
                            catch (Exception ex)
                            {

                            }
                            if (reader["ActivityGraph"] != DBNull.Value)
                            {
                                selectedImageBytes = (byte[])reader["ActivityGraph"];
                                pictureBox.Image = Image.FromStream(new MemoryStream(selectedImageBytes));
                                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                            }
                        }
                    }
                }
            }
        }

        private void B_uploadImage_Click_1(object sender, EventArgs e)
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

        private void pictureBox_Click(object sender, EventArgs e)
        {

        }

        private void txt_content_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
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
