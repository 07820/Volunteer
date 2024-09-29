using Activity;
using Config;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace Activity
{
    public partial class OrgPost : Form
    {
        int currentY = 0; // 用于计算下一个话题 Panel 的 Y 坐标位置
        int panelSpacing = 10; // 话题 Panel 之间的间距
        public OrgPost()
        {
            InitializeComponent();
            RefreshTopics();
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            this.comboBox_sort.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);

            this.Load += new System.EventHandler(this.OrgPost_signup_Load);
            this.Resize += new System.EventHandler(this.OrgPost_signup_Resize);
        }


        private void OrgPost_signup_Load(object sender, EventArgs e)
        {
            // 初始化控件位置
            PositionSearchBox();


        }

        private void OrgPost_signup_Resize(object sender, EventArgs e)
        {
            // 初始化控件位置
            PositionSearchBox();


        }

        // 创建一个新的方法来设置searchBox的位置和大小
        private void PositionSearchBox()
        {
            // 设置searchBox距离窗体顶部的位置（例如，如果你有一个菜单栏或其他控件）
            int topOffset = 10; // 如果需要的话，根据窗体上方的其他控件调整这个值
            searchBox.Location = new Point(10, panel1.Top + panel1.Height + topOffset);

            // 设置searchBox的宽度以填充窗体宽度，减去左右的间距
            searchBox.Width = 244; // 减去的20是左右两边各10像素的间距

            int controlSpacing = 15;

            // 根据 searchBox 的位置和大小设置 searchButton 的位置
            searchButton.Location = new Point(searchBox.Right + 8, searchBox.Top + searchBox.Height / 2 - searchButton.Height / 2);
            comboBox_sort.Location = new Point(searchButton.Right + controlSpacing, searchBox.Top + searchBox.Height / 2 - comboBox_sort.Height / 2);
            comboBox1.Location = new Point(comboBox_sort.Right + controlSpacing, searchBox.Top + searchBox.Height / 2 - comboBox1.Height / 2);

            postActivity.Location = new Point(this.ClientSize.Width - postActivity.Width - 10, searchButton.Top);

            button1.Location = new Point(postActivity.Left - button1.Width - 8, postActivity.Top);

            panel_out.Left = 5;// = new Point(5, searchBox.Bottom + 3);

            panel_out.Top = searchBox.Bottom + 15;

            panel_out.Width = this.ClientSize.Width; // 假设您希望左右各保持与 searchBox 一样的边距

            panel_out.Height = this.ClientSize.Height - panel_out.Top - 10;

            RefreshTopicsBasedOnFilters();
        }








        private void UpdateStatusForPastEvents()
        {

            string connectionString = ConfigInfo.GetDbConnectionString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // 定义SQL命令，将活动日期晚于当前日期的活动状态更新为"Not available"
                string sqlUpdate = @"
            UPDATE Events
            SET Status = CASE 
                            WHEN ActivityDate > @CurrentDate THEN 'Not yet started'
                            WHEN DATEADD(day, duration, ActivityDate) < GETDATE() THEN 'Activity closed'
                            ELSE Status
                        END
            WHERE ActivityDate > @CurrentDate OR DATEADD(day, duration, ActivityDate) < GETDATE();

             UPDATE StuInvolvedActiv SET ApplyStatus = 2 FROM StuInvolvedActiv s JOIN Events e ON s.ActivityID = e.ActivityID AND 
             e.ActivityDate < @CurrentDate AND s.ApplyStatus = 0";

                using (SqlCommand command = new SqlCommand(sqlUpdate, connection))
                {
                    // 设置当前日期参数
                    command.Parameters.AddWithValue("@CurrentDate", DateTime.Now);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery(); // 执行更新操作
                    //Console.WriteLine($"Rows affected: {rowsAffected}"); // 输出受影响的行数，可选
                }
            }
        }

        private Dictionary<int, int> GetActivityParticipantCounts(string connectionString)
        {
            var participantCounts = new Dictionary<int, int>();
            string participantCountSql = "SELECT ActivityID, COUNT(*) AS ParticipantCount FROM StuInvolvedActiv WHERE ApplyStatus = 1 GROUP BY ActivityID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(participantCountSql, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int activityId = reader.GetInt32(reader.GetOrdinal("ActivityID"));
                            int count = reader.GetInt32(reader.GetOrdinal("ParticipantCount"));
                            participantCounts.Add(activityId, count);
                        }
                    }
                }
            }

            return participantCounts;
        }

        private List<Topic> LoadTopicsFromDatabase(string searchTerm = "", string status = "", string sortBy = "ActivityName")
        {
            List<Topic> topics = new List<Topic>();
            string connectionString = ConfigInfo.GetDbConnectionString();
            var participantCounts = GetActivityParticipantCounts(connectionString);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string orgUserName = SharedData.userName;

                // 构建基本的SQL查询语句
                StringBuilder sqlQuery = new StringBuilder(@"
        SELECT e.ActivityID, e.ActivityName, e.ActivityDate, e.ActivityPopulation, e.Status, e.ActivityType, e.ActivityServePeople, e.ActivityPlace, e.NumOfInvolved, e.ActivityGraph
        FROM Events e
        JOIN organizer_audited o ON e.email = o.email
        WHERE o.userName = @orgUserName
        AND (@searchTerm = '' OR 
               ActivityName LIKE '%' + @searchTerm + '%' OR
               CONVERT(VARCHAR, ActivityDate, 120) LIKE '%' + @searchTerm + '%' OR
               CONVERT(VARCHAR, ActivityPopulation) LIKE '%' + @searchTerm + '%')");



                // 添加状态过滤条件
                if (!string.IsNullOrEmpty(status) && status != "All")
                {
                    sqlQuery.Append(" AND e.Status = @Status");
                }

                // 添加排序条件
                string orderByClause = sortBy == "ActivityName" ? "ORDER BY ActivityName" : "ORDER BY ActivityDate";
                sqlQuery.Append($" {orderByClause}");

                using (SqlCommand command = new SqlCommand(sqlQuery.ToString(), connection))
                {
                    command.Parameters.AddWithValue("@orgUserName", orgUserName);
                    command.Parameters.AddWithValue("@searchTerm", searchTerm);
                    if (!string.IsNullOrEmpty(status))
                    {
                        command.Parameters.AddWithValue("@Status", status);
                    }

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int activityId = (int)reader["ActivityID"];
                        int numOfInv = participantCounts.ContainsKey(activityId) ? participantCounts[activityId] : 0;

                        DateTime activityDate = Convert.ToDateTime(reader["ActivityDate"]);
                    topics.Add(new Topic
                        {
                            ActivityID = (int)reader["ActivityID"],
                            ActivityName = reader["ActivityName"].ToString(),
                            ActivityDate = activityDate.Date.ToString("yyyy-MM-dd"),
                            ActivityPopulation = reader["ActivityPopulation"].ToString(),
                            Status = reader["Status"].ToString(),
                            // 为新属性赋值
                            ActivityType = reader["ActivityType"].ToString(),
                            ActivityServePeople = reader["ActivityServePeople"].ToString(),
                            ActivityPlace = reader["ActivityPlace"].ToString(),
                            NumOfInv = numOfInv,
                            ImageData = reader["ActivityGraph"] as byte[]
                    });
                    }
                }


            }
            return topics;
        }

        private void RefreshTopicsBasedOnFilters()
        {
            // 确保comboBox1和comboBox_sort不是null
            if (comboBox1 != null && comboBox_sort != null && searchBox != null)
            {
                string selectedStatus = comboBox1.SelectedItem?.ToString(); // 使用null条件运算符防止空引用
                string selectedSortOption = comboBox_sort.SelectedItem?.ToString(); // 使用null条件运算符防止空引用
                string searchTerm = searchBox.Text;

                // 如果selectedStatus或selectedSortOption为null，则不继续执行
                if (selectedStatus == null || selectedSortOption == null)
                {
                    return;
                }

                string sortBy = selectedSortOption == "ActivityName" ? "ActivityName" : "ActivityDate";
                List<Topic> filteredTopics = LoadTopicsFromDatabase(searchTerm, selectedStatus, sortBy);
                DisplaySearchResults(filteredTopics);
            }
        }


        public void RefreshTopics()
        {
            UpdateStatusForPastEvents();
            // 清空当前话题列表
            panel_out.Controls.Clear();
            currentY = 0; // 重置 Y 坐标

            // 重置下拉框的选项为初始状态
            comboBox1.SelectedIndex = 0;
            comboBox_sort.SelectedIndex = 0;

            // 重新加载话题
            List<Topic> topics = LoadTopicsFromDatabase();

            // 显示新加载的话题
            DisplayTopics(topics);
        }



        private Color colorForLabel = Color.Orange;
        private Color colorForFront = Color.Black;

        private void DisplayTopics(List<Topic> topics)
        {

            Font font1 = new Font("Arial", 12, FontStyle.Bold); // 主要信息使用加粗字体
            Font font2 = new Font("Arial", 10); // 次要信息使用普通字体

            //int panelWidth = panel_out.Width - 20; // Panel宽度固定
            int panelHeight = 150; // Panel高度固定
            //int spacing = 10; // 控件之间的间距

            foreach (var topic in topics)
            {
                CustomBorderPanel topicPanel = new CustomBorderPanel()
                {
                    BorderStyle = BorderStyle.None,

                    BorderColor = Color.Black,

                    BorderThickness = 2,

                    BackColor = Color.White,

                    Size = new Size(panel_out.ClientSize.Width - 50, panelHeight),

                    Dock = DockStyle.None,

                    Anchor = AnchorStyles.Left | AnchorStyles.Top,

                    Location = new Point(10, currentY)


                };



                Panel topPanel = new Panel
                {
                    Dock = DockStyle.Top,
                    Height = 30, // 设置顶部Panel的高度
                    //BackColor = Color.Aquamarine, // 可以设置不同的背景色
                    BackColor = Color.Turquoise,
                };

                // 可以在这个topPanel上添加更多控件，比如标题、图标等

                // 将顶部Panel添加到topicPanel中
                topicPanel.Controls.Add(topPanel);





                //昵称
                RoundedLabel ActivityName = new RoundedLabel()
                {
                    Text = topic.ActivityName,
                    Location = new Point(15, 38), // 靠上放置
                    Font = font1,
                    AutoSize = true,
                    //LinkBehavior = LinkBehavior.NeverUnderline,
                    ForeColor = colorForFront,
                    BackColor = Color.White, // 设置背景颜色为浅橙色
                    //ActiveLinkColor = Color.Red
                };


                // 为了实现鼠标悬停效果，使用 MouseEnter 和 MouseLeave 事件
                ActivityName.MouseEnter += (s, e) => { ActivityName.ForeColor = Color.Green; }; // 鼠标悬停时颜色变化
                ActivityName.MouseLeave += (s, e) => { ActivityName.ForeColor = Color.Black; }; // 鼠标离开时恢复原色
                topicPanel.Controls.Add(ActivityName);






                PictureBox PopImage = new PictureBox()
                {
                    Image = Properties.Resources.POPULATION,
                    Location = new Point(ActivityName.Left + 5, ActivityName.Top + ActivityName.Height + 20), // 继续下移40个单位

                    SizeMode = PictureBoxSizeMode.AutoSize, // 确保图像以其原始大小显示，PictureBox调整大小以适应图像
                };


                topicPanel.Controls.Add(PopImage);







                // 在ActivityDate下方或右侧添加显示总人数的Label
                Label ActivityPopulation = new Label()
                {
                    Text = $"{topic.NumOfInv} / {topic.ActivityPopulation}",
                    Location = new Point(PopImage.Left + PopImage.Width + 5, PopImage.Top), // 继续下移40个单位
                    Font = font1,
                    ForeColor = Color.Black,
                    BackColor = Color.White, // 设置背景颜色为浅橙色
                    AutoSize = true

                };


                topicPanel.Controls.Add(ActivityPopulation);







                LinkLabel ActivityDate = new LinkLabel()
                {
                    Text = $"Date: {topic.ActivityDate}",
                    Location = new Point(315, ActivityPopulation.Top),
                    Font = font2,
                    LinkBehavior = LinkBehavior.NeverUnderline,
                    LinkColor = colorForFront,
                    BackColor = Color.White, // 设置背景颜色为浅橙色
                    ActiveLinkColor = Color.Red,
                    AutoSize = true
                };

                ActivityDate.MouseEnter += (s, e) => { ActivityDate.LinkColor = Color.Green; }; // 鼠标悬停时颜色变化
                ActivityDate.MouseLeave += (s, e) => { ActivityDate.LinkColor = Color.Black; }; // 鼠标离开时恢复原色

                topicPanel.Controls.Add(ActivityDate);










                PictureBox pixturebox2 = new PictureBox()
                {
                    Size = new Size(30, 30), // 设置控件大小为30x30
                    SizeMode = PictureBoxSizeMode.Zoom, // 设置图片模式为Zoom，使图片按比例缩放以完全填充PictureBox
                    BackColor = Color.White, // 可选，设置背景色

                    Location = new Point(ActivityName.Left + ActivityName.Width + 10, ActivityName.Top)
                };

                topicPanel.Controls.Add((PictureBox)pixturebox2);


                ImageLabel ActivityType = new ImageLabel()
                {
                    Text = $"{topic.ActivityType}",

                    //Image = Properties.Resources.C,
                    //Location = new Point(15, 12), // 这行可能不再需要，因为我们会使用Dock属性
                    Location = new Point(pixturebox2.Left + pixturebox2.Width + 10, ActivityName.Top),

                    ImageAlign = ContentAlignment.MiddleLeft,
                    TextAlign = ContentAlignment.MiddleLeft, // 确保文本也左对齐
                    //Dock = DockStyle.Left, // 确保整个控件位于父控件的左侧

                    Font = font1,
                    ForeColor = Color.Black,
                    BackColor = Color.Transparent, // 要设置为浅橙色，你需要改变这里的颜色
                    AutoSize = true,

                };

                topicPanel.Controls.Add(ActivityType);

                if (topic.ActivityType == "Cultural Education")
                {
                    pixturebox2.Image = Properties.Resources.CulturalEducation;
                }

                if (topic.ActivityType == "Civic Counseling")
                {
                    pixturebox2.Image = Properties.Resources.CHAT;
                }

                if (topic.ActivityType == "Cultural Tourism Services")
                {
                    pixturebox2.Image = Properties.Resources.CULTURALTOURISM;
                }

                if (topic.ActivityType == "Childcare Services")
                {
                    pixturebox2.Image = Properties.Resources.Children;
                }

                if (topic.ActivityType == "Environmental Cleanliness")
                {
                    pixturebox2.Image = Properties.Resources.environment;
                }


                if (topic.ActivityType == "Elderly Assistance")
                {
                    pixturebox2.Image = Properties.Resources.EDERLY;
                }

                if (topic.ActivityType == "Online Activities")
                {
                    pixturebox2.Image = Properties.Resources.INTERNET;
                }

                if (topic.ActivityType == "Neighborhood Watch")
                {
                    pixturebox2.Image = Properties.Resources.D;
                }

                if (topic.ActivityType == "Emergency Response")
                {
                    pixturebox2.Image = Properties.Resources.C;
                }

                if (topic.ActivityType == "Healthcare Services")
                {
                    pixturebox2.Image = Properties.Resources.HEALTHCARE;
                }


                topicPanel.Controls.Add(ActivityType);

                ActivityDate.MouseEnter += (s, e) => { ActivityDate.LinkColor = Color.Green; }; // 鼠标悬停时颜色变化
                ActivityDate.MouseLeave += (s, e) => { ActivityDate.LinkColor = Color.Black; }; // 鼠标离开时恢复原色

                topicPanel.Controls.Add(ActivityType);












                LinkLabel ActivityPlace = new LinkLabel()
                {
                    Text = $"Place: {topic.ActivityPlace}",
                    Location = new Point(ActivityDate.Left + ActivityDate.Width + 30, ActivityDate.Top), // 下移40个单位
                    Font = font2,
                    LinkBehavior = LinkBehavior.NeverUnderline,
                    LinkColor = colorForFront,
                    BackColor = Color.White, // 设置背景颜色为浅橙色
                    ActiveLinkColor = Color.Red,
                    AutoSize = true
                };

                ActivityDate.MouseEnter += (s, e) => { ActivityDate.LinkColor = Color.Green; }; // 鼠标悬停时颜色变化
                ActivityDate.MouseLeave += (s, e) => { ActivityDate.LinkColor = Color.Black; }; // 鼠标离开时恢复原色

                topicPanel.Controls.Add(ActivityPlace);









                LinkLabel ActivityServePeople = new LinkLabel()
                {
                    Text = $"Service Recipients: {topic.ActivityServePeople}",
                    Location = new Point(ActivityDate.Left, ActivityDate.Top + ActivityDate.Height + 10), // 下移40个单位
                    Font = font2,
                    LinkBehavior = LinkBehavior.NeverUnderline,
                    LinkColor = colorForFront,
                    BackColor = Color.White, // 设置背景颜色为浅橙色
                    ActiveLinkColor = Color.Red,
                    AutoSize = true
                };

                ActivityServePeople.MouseEnter += (s, e) => { ActivityDate.LinkColor = Color.Green; }; // 鼠标悬停时颜色变化
                ActivityServePeople.MouseLeave += (s, e) => { ActivityDate.LinkColor = Color.Black; }; // 鼠标离开时恢复原色

                topicPanel.Controls.Add(ActivityServePeople);





                // 新增：状态显示
                Label statusLabel = new Label()
                {

                    Text = $"{topic.Status}",
                    //Location = new Point(panelWidth - 30 - ActivityName.Width, ActivityName.Top), // 继续下移40个单位
                    Font = new Font(font2.FontFamily, font2.Size, FontStyle.Bold),
                    ForeColor = Color.Black,
                    AutoSize = true
                };



                if (topic.Status == "In progress")
                {
                    statusLabel.BackColor = Color.Lime;
                }

                if (topic.Status == "Activity closed")
                {
                    statusLabel.BackColor = Color.Red;
                }

                if (topic.Status == "Not yet started")
                {
                    statusLabel.BackColor = Color.Orange;
                }




                topicPanel.Controls.Add(statusLabel);

                statusLabel.Location = new Point(topicPanel.Width - 80 - statusLabel.Width / 2, ActivityName.Top);





                // 添加详情查看Label
                Label detailLabel = new Label()
                {
                    Text = "Details",
                    Location = new Point(topicPanel.Width - 80, panelHeight - 40), // 靠右下角放置
                    Font = new Font("Arial", 10, FontStyle.Underline),
                    ForeColor = Color.Blue,

                    Cursor = Cursors.Hand,
                    AutoSize = true
                };

                detailLabel.Click += (sender, e) => ShowActivityDetails(topic.ActivityID);
                topicPanel.Controls.Add(detailLabel);




                // 新增：edit 按钮
                Button editButton = new Button();
                editButton.Text = "Edit";
                editButton.Font = new Font("Arial", 10);

                editButton.AutoSize = true;
                //signUpButton.Size = new Size(100, 40); // 可根据需要调整大小
                editButton.Left = detailLabel.Left - editButton.Width - 8;
                editButton.Top = (detailLabel.Top + detailLabel.Bottom) / 2 - editButton.Height / 2 - 6;
                editButton.Tag = topic.ActivityID;
                editButton.Click += (sender, e) => EditActivity((int)((Button)sender).Tag); 


                topicPanel.Controls.Add(editButton);




                Button actionButton = new Button();
                //actionButton.Size = new Size(30, 40); // 设置统一的按钮大小
                
                actionButton.Font = new Font("Arial", 10);

                actionButton.AutoSize = true;

                


                if (topic.Status == "Not yet started")
                {
                    actionButton.Text = "Audit Enrollment";
                    actionButton.Click += (sender, e) => { AuditEnrollment(topic.ActivityID); };
                }
                else if (topic.Status == "In progress")
                {
                    actionButton.Text = "View Students";
                    actionButton.Click += (sender, e) => { ViewStudents(topic.ActivityID); };
                }
                else if (topic.Status == "Activity closed")
                {
                    actionButton.Text = "Audit Result";
                    actionButton.Click += (sender, e) => { AuditResult(topic.ActivityID); };
                }
                else
                {
                    // 如果状态不匹配上面的任何一个，可以选择不添加按钮或者添加一个默认的按钮
                    actionButton.Visible = false; // 或者可以设置为不可见
                }

                

                // 将按钮添加到 Panel
                topicPanel.Controls.Add(actionButton);



                actionButton.Location = new Point(editButton.Left - actionButton.Width - 8, editButton.Top); // 设置统一的按钮位置









                if (topic.ImageData != null && topic.ImageData.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream(topic.ImageData))
                    {
                        PictureBox pictureBox = new PictureBox();
                        pictureBox.Location = new Point(145, 70);
                        pictureBox.Image = Image.FromStream(ms);
                        pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                        BackgroundImageLayout = ImageLayout.Zoom;
                        pictureBox.Height = 70; // 根据需要设置适当的高度
                        pictureBox.Width = 130;

                        topicPanel.Controls.Add(pictureBox);
                    }
                }





                panel_out.Controls.Add(topicPanel);

                topicPanel.Location = new Point(10, currentY);


                // 更新currentY，为下一个topicPanel的位置做准备
                currentY += topicPanel.Height + panelSpacing;

            }
            // 确保 mainPanel 允许滚动以容纳所有话题
            panel_out.AutoScroll = true;
        }





        private void AuditEnrollment(int activityID)
        {
            List<Topic> topics = new List<Topic>();
            string connectionString = ConfigInfo.GetDbConnectionString();

            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                StringBuilder sqlQuery = new StringBuilder(@"
                SELECT e.ActivityID, e.ActivityName, e.ActivityDate, e.ActivityPopulation, e.Status, s.StuID, s.StuName 
         FROM Events e
         JOIN organizer_audited o ON e.email = o.email
         JOIN StuInvolvedActiv s ON s.ActivityID = e.ActivityID
         WHERE s.ActivityID = @activityID");
                using (SqlCommand command = new SqlCommand(sqlQuery.ToString(), connection))
                {
                    command.Parameters.AddWithValue("@activityID", activityID);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        topics.Add(new Topic
                        {
                            ActivityID = (int)reader["ActivityID"],
                            ActivityName = reader["ActivityName"].ToString(),
                            ActivityDate = reader["ActivityDate"].ToString(),
                            ActivityPopulation = reader["ActivityPopulation"].ToString(),
                            StuID = (int)reader["StuID"],
                            Status = reader["Status"].ToString(),
                            StuName = reader["StuName"].ToString(),

                        });

                    }

                }

            }
            ApplyAudit form = new ApplyAudit(activityID);
            form.ShowDialog();
        }

        private void ViewStudents(int activityId)
        {

            inProgressCheck form = new inProgressCheck(activityId);
            form.ShowDialog();
        }

        private void AuditResult(int activityId)
        {
            AuditResult form = new AuditResult(activityId);
            form.ShowDialog();
        }

        private void EditActivity(int activityId)
        {
            // 打开一个新窗口或表单来修改活动
            // 这里我们将使用 adjustEvent 窗体来编辑活动，确保你已经有了这个窗体，并且它接受一个活动ID作为参数
            adjustEvent editForm = new adjustEvent(activityId); // 使用活动ID创建窗体
            editForm.ShowDialog(); // 显示窗体

            // 修改完成后刷新列表以显示最新数据
            // 请确保你的 RefreshTopics 方法能够正确地从数据库加载和显示最新的活动列表
            RefreshTopics();
        }

        private void ShowActivityDetails(int activityId)
        {
            // 这里创建并显示对应活动详情的Form
            // 例如：
            ActivityDetails displayForm = new ActivityDetails(activityId); // receive activityId
            displayForm.Show();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            RefreshTopics();
        }


        private void searchButton_Click(object sender, EventArgs e)
        {
            RefreshTopicsBasedOnFilters();
        }

        private void DisplaySearchResults(List<Topic> topics)
        {
            panel_out.Controls.Clear(); // 清空现有的搜索结果
            currentY = 0; // 初始的 Y 位置，用于定位每个结果的 Panel

            DisplayTopics(topics);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshTopicsBasedOnFilters();
        }



        private void postActivity_Click_1(object sender, EventArgs e)
        {
            PostActivity form1 = new PostActivity();
            form1.ShowDialog();
            RefreshTopics();
        }

        private void panel_out_Paint_1(object sender, PaintEventArgs e)
        {
            
        }

        private void Form3_Load_1(object sender, EventArgs e)
        {  

            RefreshTopics(); // 初始化筛选和显示
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel_out_Paint(object sender, PaintEventArgs e)
        {

        }



        private void searchBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 重置下拉框的选项为初始状态
            comboBox1.SelectedIndex = 0;
            comboBox_sort.SelectedIndex = 0;
            RefreshTopics();
        }

        private void comboBox_sort_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            RefreshTopicsBasedOnFilters();
        }

        private void searchBox_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void searchButton_Click_1(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

