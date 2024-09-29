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
using Config;
using Model;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Activity
{
    public partial class inProgressCheck : Form
    {
        private int activityID;
        private List<Topic> topicsList = new List<Topic>(); // 全局列表用于存储话题信息
        int currentY = 0; // 用于计算下一个话题 Panel 的 Y 坐标位置
        int panelSpacing = 10; // 话题 Panel 之间的间距
        public inProgressCheck(int ID)
        {
            this.activityID = ID;
            InitializeComponent();
            LoadTopicsIntoGlobalList(); // 修改：首先加载话题到全局列表
            RefreshTopics();
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            this.comboBox_sort.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);

            // 订阅 Load 和 Resize 事件
            this.Load += inProgressCheck_Load;
            this.Resize += inProgressCheck_Resize;
        }

        private void inProgressCheck_Load(object sender, EventArgs e)
        {
            // 初始化控件位置
            PositionSearchBox();

            LoadTopicsIntoGlobalList(); // 修改：首先加载话题到全局列表

            // 调整 panel_out 的大小并加载话题
            AdjustPanelOutAndLoadTopics();
        }


        private void inProgressCheck_Resize(object sender, EventArgs e)
        {
            // 初始化控件位置
            PositionSearchBox();



            // 调整 panel_out 的大小并加载话题
            AdjustPanelOutAndLoadTopics();


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
            //button1.Location = new Point(comboBox_sort.Right + controlSpacing, searchBox.Top);

            button1.Location = new Point(this.ClientSize.Width - button1.Width - 10, searchButton.Top);


            panel_out.Left = 8;// = new Point(5, searchBox.Bottom + 3);

            panel_out.Top = searchBox.Bottom + 8;

            panel_out.Width = this.ClientSize.Width - 10; // 假设您希望左右各保持与 searchBox 一样的边距

            panel_out.BackColor = Color.White;

            panel_out.Height = this.ClientSize.Height - 130;
        }











        private void AdjustPanelOutAndLoadTopics()
        {
            // 调整 panel_out 的大小，这里可以根据需要设置边距
            panel_out.Width = this.ClientSize.Width - 20; // 假设左右各保留10像素的边距
            panel_out.Height = this.ClientSize.Height - panel_out.Location.Y - 10; // 根据 panel_out 的位置调整

            // 重新加载并显示话题，确保 topicsList 包含了当前的话题列表
            DisplayTopics(topicsList);
        }


        private void LoadTopicsIntoGlobalList(string searchTerm = "", string sortBy = "Student name")
        {
            topicsList.Clear(); // 清空列表以避免重复添加
            topicsList = LoadTopicsFromDatabase(searchTerm, sortBy); // 加载话题到全局列表
        }



        // 修改：刷新话题的逻辑，现在使用全局列表来刷新显示
        public void RefreshTopics()
        {
            UpdateStatusForPastEvents();
            // 清空当前话题列表
            panel_out.Controls.Clear();
            currentY = 0; // 重置 Y 坐标

            // 使用全局列表刷新显示
            DisplayTopics(topicsList);
        }



        // 修改：根据筛选条件重新加载全局列表并刷新显示
        private void RefreshTopicsBasedOnFilters()
        {
            if (comboBox_sort != null && searchBox != null)
            {
                string selectedSortOption = comboBox_sort.SelectedItem?.ToString(); // 使用null条件运算符防止空引用
                string searchTerm = searchBox.Text;

                string sortBy = selectedSortOption == "Student name" ? "StuName" : "ActivityDate";
                LoadTopicsIntoGlobalList(searchTerm, sortBy); // 根据筛选条件重新加载全局列表
                DisplaySearchResults(topicsList); // 使用全局列表刷新显示
            }
        }


        private void searchButton_Click(object sender, EventArgs e)
        {
            RefreshTopicsBasedOnFilters(); // 搜索按钮点击时，根据筛选条件刷新显示
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

        private List<Topic> LoadTopicsFromDatabase(string searchTerm = "", string sortBy = "Student name")
        {
            List<Topic> topic = new List<Topic>();
            string connectionString = ConfigInfo.GetDbConnectionString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string orgUserName = SharedData.userName;

                // 构建基本的SQL查询语句
                StringBuilder sqlQuery = new StringBuilder(@"
        SELECT s.ActivityID, s.ActivityName,s.StuID, s.StuName,s.ActivityStatus, e.ActivityDate,e.ActivityPopulation,o.userName from StuInvolvedActiv s
        join Events e on e.ActivityID = s.ActivityID
        join organizer_audited o on e.email = o.email
        WHERE o.userName = @orgUserName
        And s.ActivityID = @activityID
        AND s.ApplyStatus = 1
        AND (@searchTerm = '' OR 
               s.StuName LIKE '%' + @searchTerm + '%' OR
               CONVERT(VARCHAR, e.ActivityDate, 120) LIKE '%' + @searchTerm + '%' OR
               CONVERT(VARCHAR, e.ActivityPopulation) LIKE '%' + @searchTerm + '%')");


                // 添加排序条件
                string orderByClause = sortBy == "Student name" ? "ORDER BY s.StuName" : "ORDER BY e.ActivityDate";
                sqlQuery.Append($" {orderByClause}");

                using (SqlCommand command = new SqlCommand(sqlQuery.ToString(), connection))
                {
                    command.Parameters.AddWithValue("@orgUserName", orgUserName);
                    command.Parameters.AddWithValue("@activityID", activityID);
                    command.Parameters.AddWithValue("@searchTerm", searchTerm);


                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        DateTime activityDate = Convert.ToDateTime(reader["ActivityDate"]);
                        topic.Add(new Topic
                        {
                            // 假设你的Topic类有这些属性
                            ActivityID = (int)reader["ActivityID"],
                            ActivityName = reader["ActivityName"].ToString(),
                            StuID = int.Parse(reader["StuID"].ToString()),
                            StuName = reader["StuName"].ToString(),
                            Status = reader["ActivityStatus"].ToString(),
                            ActivityDate = activityDate.Date.ToString("yyyy-MM-dd"),
                            ActivityPopulation = reader["ActivityPopulation"].ToString(),
                        });
                    }
                }
            }
            return topic;
        }



        private void DisplayTopics(List<Topic> topics)
        {
            Font font1 = new Font("Arial", 12, FontStyle.Bold);
            Font font2 = new Font("Arial", 10, FontStyle.Bold);

            panel_out.Controls.Clear(); // 清除之前的所有话题Panel
            currentY = 10; // 重置 Y 坐标

            foreach (var topic in topics)
            {
                CustomBorderPanel topicPanel = new CustomBorderPanel();
                topicPanel.BorderThickness = 2; // 边框粗细设置为2
                topicPanel.BackColor = Color.White;
                topicPanel.Size = new Size(panel_out.ClientSize.Width - 20, 100);
                // 其他代码不变...



                // 创建顶部的 Cyan Panel
                Panel topCyanPanel = new Panel();
                topCyanPanel.Height = 20; // 设置高度为 20
                topCyanPanel.Dock = DockStyle.Top; // 设置 Dock 为 Top
                topCyanPanel.BackColor = Color.Turquoise; // 设置背景颜色为 Cyan
                topCyanPanel.BorderStyle = BorderStyle.None; // 设置边框为不可见

                // 将 Cyan Panel 添加到 topicPanel
                topicPanel.Controls.Add(topCyanPanel);



                //添加姓名
                Label stuName = new Label();
                stuName.Text = $"Name: {topic.StuName}"; // 使用从数据库加载的Status值
                stuName.Location = new Point(250, (topicPanel.Height - stuName.Height) / 2 - 8); // 位置调整为总人数的正后方
                stuName.Font = font2;
                stuName.AutoSize = true;

                // 活动日期
                LinkLabel ActivityDate = new LinkLabel();
                ActivityDate.Text = $"Date: {topic.ActivityDate}"; // 从数据库获取的话题内容
                ActivityDate.Location = new Point(250, stuName.Top + stuName.Height + 8);
                ActivityDate.Size = new Size(235, 65); // 根据需要调整
                ActivityDate.AutoSize = true;
                ActivityDate.Font = font2;
                ActivityDate.LinkBehavior = LinkBehavior.NeverUnderline; // 不显示下划线
                ActivityDate.LinkColor = Color.Black; // 设置字体颜色为黑色
                ActivityDate.ActiveLinkColor = Color.Red; // 设置鼠标点击时的颜色
                ActivityDate.MouseEnter += (s, e) => { ActivityDate.LinkColor = Color.Green; }; // 鼠标悬停时颜色变化
                ActivityDate.MouseLeave += (s, e) => { ActivityDate.LinkColor = Color.Black; }; // 鼠标离开时恢复原色



                //活动名称
                LinkLabel ActivityName = new LinkLabel();
                ActivityName.Text = topic.ActivityName; // 从数据库获取的昵称
                ActivityName.Location = new Point(20, topCyanPanel.Bottom + 8);
                ActivityName.Font = font1;
                ActivityName.LinkBehavior = LinkBehavior.NeverUnderline; // 不显示下划线
                ActivityName.LinkColor = Color.Black; // 设置字体颜色为黑色
                ActivityName.AutoSize = true;
                ActivityName.ActiveLinkColor = Color.Red; // 设置鼠标点击时的颜色
                // 为了实现鼠标悬停效果，使用 MouseEnter 和 MouseLeave 事件
                ActivityName.MouseEnter += (s, e) => { ActivityName.LinkColor = Color.Green; }; // 鼠标悬停时颜色变化
                ActivityName.MouseLeave += (s, e) => { ActivityName.LinkColor = Color.Black; }; // 鼠标离开时恢复原色



                // 添加按钮
                System.Windows.Forms.Button detialButton = new System.Windows.Forms.Button();
                detialButton.Text = "Detail";
                detialButton.Font = new Font("Arial", 12, FontStyle.Bold);
                detialButton.AutoSize = true;
                detialButton.BackColor = Color.White;
                detialButton.Size = new Size(90, 30); // 可根据需要调整大小
                detialButton.Location = new Point(topicPanel.Width - detialButton.Width - 10, (topicPanel.Height - detialButton.Height) / 2);                                     // 为按钮添加点击事件
                detialButton.Click += (sender, e) => { viewDetail(topic); };

                // 将控件添加到topicPanel
                //topicPanel.Controls.Add(avatar);

                topicPanel.Controls.Add(ActivityDate);
                topicPanel.Controls.Add(ActivityName);
                topicPanel.Controls.Add(stuName);
                topicPanel.Controls.Add(detialButton);

                // 计算topicPanel的位置并添加到mainPanel中
                topicPanel.Location = new Point(0, currentY);
                panel_out.Controls.Add(topicPanel);

                // 更新currentY，为下一个topicPanel的位置做准备
                currentY += topicPanel.Height + panelSpacing;

            }
            // 确保 mainPanel 允许滚动以容纳所有话题
            panel_out.AutoScroll = true;
        }

        private void viewDetail(Topic topic)
        {
            inProgressUserDetail form = new inProgressUserDetail(topic);
            form.ShowDialog();

        }

        private void DisplaySearchResults(List<Topic> topic)
        {
            panel_out.Controls.Clear(); // 清空现有的搜索结果
            currentY = 0; // 初始的 Y 位置，用于定位每个结果的 Panel

            DisplayTopics(topic);
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshTopicsBasedOnFilters();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RefreshTopicsBasedOnFilters();
        }

        private void comboBox_sort_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel_out_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
