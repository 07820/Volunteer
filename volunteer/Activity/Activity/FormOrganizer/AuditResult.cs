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
using WindowsFormsApp1;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Activity
{
    public partial class AuditResult : Form
    {
        private int activityID;

        private List<Topic> topics;

        int currentY = 0; // 用于计算下一个话题 Panel 的 Y 坐标位置
        int panelSpacing = 10; // 话题 Panel 之间的间距
        public AuditResult(int ID)
        {
            activityID = ID;
            topics = new List<Topic>(); // 初始化 topics
            InitializeComponent();
            RefreshTopics();
            this.comboBox_sort.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);




            // 绑定事件处理器
            this.Load += YourForm_Load;
            this.Resize += YourForm_Resize;



        }



        private void YourForm_Load(object sender, EventArgs e)
        {
            // 初始化时调整 panel_out 的大小和位置
            AdjustPanelOut();

            LoadTopicsFromDatabase();

            DisplayTopics();
        }

        private void YourForm_Resize(object sender, EventArgs e)
        {
            // 窗体大小变化时调整 panel_out 的大小和位置
            AdjustPanelOut();

            DisplayTopics();
        }



        private void AdjustPanelOut()
        {


            label1.Left = 8;

            label1.Top = panel1.Bottom + 8;

            comboBox_sort.Left = label1.Right + 8;

            comboBox_sort.Top = label1.Bottom - label1.Height / 2 - comboBox_sort.Height / 2;

            label2.Left = comboBox_sort.Right + 150;

            label2.Top = label1.Top;

            comboBox1.Left = label2.Right + 8;

            comboBox1.Top = label1.Bottom - label1.Height / 2 - comboBox1.Height / 2;

            button1.Left = this.ClientSize.Width - button1.Width - 20;

            button1.Top = label1.Bottom - label1.Height / 2 - button1.Height / 2;

            // 这里是一个示例，根据你的具体布局调整
            panel_out.Location = new Point(12, comboBox_sort.Bottom + 10); // 保持一定的边距

            panel_out.Size = new Size(this.ClientSize.Width - 20, this.ClientSize.Height - panel_out.Top - 10); // 根据 Form 的大小调整，保持边距


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
            WHERE ActivityDate > @CurrentDate OR DATEADD(day, duration, ActivityDate) < GETDATE()";

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

        private void LoadTopicsFromDatabase(string Resultstatus = "Unaudited", string sortBy = "Student name")
        {
            topics.Clear(); // 清空全局话题列表，准备加载新数据
            string connectionString = ConfigInfo.GetDbConnectionString();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string orgUserName = SharedData.userName;

                    // 保持原始 SQL 查询不变
                    StringBuilder sqlQuery = new StringBuilder(@"
SELECT s.ActivityID, s.ActivityName, s.StuID, s.StuName,s.ActivityStatus, e.ActivityDate,e.ActivityPopulation,o.userName from StuInvolvedActiv s
join Events e on e.ActivityID = s.ActivityID
join organizer_audited o on e.email = o.email
WHERE o.userName = @orgUserName
AND e.ActivityID = @activityID
AND s.ApplyStatus = 1
");

                    if (Resultstatus == "Unaudited")
                    {
                        sqlQuery.Append(" AND s.FinishStatus = 0");
                    }
                    else
                    {
                        sqlQuery.Append(" AND (s.FinishStatus = 1 OR s.FinishStatus = 2)");
                    }

                    // 保持原始排序条件不变
                    string orderByClause = sortBy == "Student name" ? "ORDER BY s.StuName" : "ORDER BY s.AppTime";
                    sqlQuery.Append($" {orderByClause}");

                    using (SqlCommand command = new SqlCommand(sqlQuery.ToString(), connection))
                    {
                        command.Parameters.AddWithValue("@orgUserName", orgUserName);
                        command.Parameters.AddWithValue("@activityID", activityID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                topics.Add(new Topic
                                {
                                    ActivityID = (int)reader["ActivityID"],
                                    ActivityName = reader["ActivityName"].ToString(),
                                    StuID = int.Parse(reader["StuID"].ToString()),
                                    StuName = reader["StuName"].ToString(),
                                    Status = reader["ActivityStatus"].ToString(),
                                    ActivityDate = reader["ActivityDate"].ToString(),
                                    ActivityPopulation = reader["ActivityPopulation"].ToString(),
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 在这里处理异常，例如显示错误消息
                MessageBox1 form = new MessageBox1($"An error occurred while loading topics from the database: {ex.Message}");
                form.ShowDialog();
                //MessageBox.Show($"An error occurred while loading topics from the database: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayTopics()
        {
            // 清空当前话题列表
            panel_out.Controls.Clear();
            currentY = 10; // 重置 Y 坐标

            Font font1 = new Font("Times New Roman", 12);
            Font font2 = new Font("Times New Roman", 10);
            foreach (var topic in topics)
            {
                Panel topicPanel = new Panel();
                topicPanel.BorderStyle = BorderStyle.FixedSingle;
                topicPanel.Height = 150; // 设置固定高度
                topicPanel.Width = panel_out.Width - 20; // panel_out 的宽度减去一定的边距值，使 topicPanel 不会触及 panel_out 的滚动条



                // 创建顶部的 Cyan Panel
                Panel topCyanPanel = new Panel();
                topCyanPanel.Height = 20; // 设置高度为 20
                topCyanPanel.Dock = DockStyle.Top; // 设置 Dock 为 Top
                topCyanPanel.BackColor = Color.Turquoise; // 设置背景颜色为 Cyan
                topCyanPanel.BorderStyle = BorderStyle.None; // 设置边框为不可见

                // 将 Cyan Panel 添加到 topicPanel
                topicPanel.Controls.Add(topCyanPanel);







                





                








                //活动名称
                LinkLabel ActivityName = new LinkLabel();
                ActivityName.Text = topic.ActivityName; // 从数据库获取的昵称
                ActivityName.Location = new Point(400, 45);
                ActivityName.Font = font1;
                ActivityName.LinkBehavior = LinkBehavior.NeverUnderline; // 不显示下划线
                ActivityName.LinkColor = Color.Black; // 设置字体颜色为黑色
                ActivityName.AutoSize = true;
                ActivityName.ActiveLinkColor = Color.Red; // 设置鼠标点击时的颜色
                // 为了实现鼠标悬停效果，使用 MouseEnter 和 MouseLeave 事件
                ActivityName.MouseEnter += (s, e) => { ActivityName.LinkColor = Color.Green; }; // 鼠标悬停时颜色变化
                ActivityName.MouseLeave += (s, e) => { ActivityName.LinkColor = Color.Black; }; // 鼠标离开时恢复原色



                topicPanel.Controls.Add(ActivityName);

                ActivityName.Location = new Point(30, topCyanPanel.Bottom + 10);


                //添加姓名
                Label stuName = new Label();
                stuName.Text = $"Name: {topic.StuName}"; // 使用从数据库加载的Status值
                stuName.Location = new Point(100, 20); // 位置调整为总人数的正后方
                stuName.Font = font2;
                stuName.AutoSize = true;


                topicPanel.Controls.Add(stuName);

                stuName.Location = new Point(250, ActivityName.Bottom + 8); // 位置调整为总人数的正后方



                // 活动日期
                LinkLabel ActivityDate = new LinkLabel();
                ActivityDate.Text = topic.ActivityDate; // 从数据库获取的话题内容
                ActivityDate.Location = new Point(100, 45);
                ActivityDate.Size = new Size(235, 65); // 根据需要调整
                ActivityDate.AutoSize = true;
                ActivityDate.Font = font2;
                ActivityDate.LinkBehavior = LinkBehavior.NeverUnderline; // 不显示下划线
                ActivityDate.LinkColor = Color.Black; // 设置字体颜色为黑色
                ActivityDate.ActiveLinkColor = Color.Red; // 设置鼠标点击时的颜色
                ActivityDate.MouseEnter += (s, e) => { ActivityDate.LinkColor = Color.Green; }; // 鼠标悬停时颜色变化
                ActivityDate.MouseLeave += (s, e) => { ActivityDate.LinkColor = Color.Black; }; // 鼠标离开时恢复原色


                topicPanel.Controls.Add(ActivityDate);

                ActivityDate.Location = new Point(250, stuName.Bottom + 10);






                // 添加detail
                LinkLabel detail = new LinkLabel();
                detail.Text = "Detail";
                //detail.Location = new Point(1130, 40);
                detail.Font = font2;
                detail.AutoSize = true;
                detail.Click += (sender, e) => { Detail(topic); };

                int finishStatus;
                string connectionString = ConfigInfo.GetDbConnectionString();
                
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // 构建基本的SQL查询语句
                    StringBuilder sqlQuery = new StringBuilder(@"
        SELECT FinishStatus FROM StuInvolvedActiv WHERE ActivityID = @activityID AND StuID = @stuID");
                    using (SqlCommand command = new SqlCommand(sqlQuery.ToString(), connection))
                    {
                        command.Parameters.AddWithValue("@activityID", activityID);
                        command.Parameters.AddWithValue("@stuID", topic.StuID);

                        SqlDataReader reader = command.ExecuteReader();
                        reader.Read();
                        finishStatus = reader.GetInt32(0);
                    }

                }

                topicPanel.Controls.Add(detail);

                detail.Location = new Point(topicPanel.Width - detail.Width - 20, (topicPanel.Height + topCyanPanel.Bottom) / 2 - detail.Height / 2);










                //添加label
                Label resultStatus = new Label();
                resultStatus.Visible = false;
                
                resultStatus.Font = font2;
                resultStatus.AutoSize = true;
                if (comboBox1.Text == "Audited")
                {
                    if(finishStatus == 1)
                    {
                        resultStatus.Text = "Completed";
                        resultStatus.Location = new Point(detail.Left - 160, detail.Location.Y);
                    }
                    else
                    {
                        resultStatus.Text = "Not completed";
                        resultStatus.Location = new Point(detail.Left - 175, detail.Location.Y);
                    }
                    resultStatus.Visible = true;

                }



                topicPanel.Controls.Add(resultStatus);






                // 添加拒绝
                LinkLabel reject = new LinkLabel();
                reject.Text = "Activity not completed";
                //reject.Location = new Point(1000, 70);
                reject.Font = font2;
                reject.AutoSize = true;
                reject.LinkBehavior = LinkBehavior.NeverUnderline;
                reject.LinkColor = reject.ForeColor; // 设置链接颜色为前景色
                reject.ActiveLinkColor = Color.Red; // 设置活动链接颜色为前景色
                reject.MouseEnter += (s, e) => { reject.LinkColor = Color.Green; }; // 鼠标悬停时颜色变化
                reject.MouseLeave += (s, e) => { reject.LinkColor = Color.Black; }; // 鼠标离开时恢复原色
                reject.Click += (sender, e) => { Reject(topic); };
                if (comboBox1.Text == "Audited")
                {
                    reject.Visible = false;
                }


                topicPanel.Controls.Add(reject);


                reject.Location = new Point(topicPanel.Width - reject.Width - 20, topicPanel.Height - reject.Height - 15);








                //添加label
                Label label = new Label();
                label.Text = "/";
                label.Location = new Point(reject.Left - 15, reject.Location.Y);
                label.Font = font2;
                label.AutoSize = true;
                if (comboBox1.Text == "Audited")
                {
                    label.Visible = false;
                }

                topicPanel.Controls.Add(label);


                label.Location = new Point(reject.Left - 15, reject.Location.Y);









                // 添加接受
                LinkLabel accept = new LinkLabel();
                accept.Text = "Confirm completion";
                accept.Font = font2;
                accept.AutoSize = true;
                accept.LinkBehavior = LinkBehavior.NeverUnderline;
                accept.LinkColor = accept.ForeColor; // 设置链接颜色为前景色
                accept.ActiveLinkColor = Color.Red; // 设置活动链接颜色为前景色
                // 为了实现鼠标悬停效果，使用 MouseEnter 和 MouseLeave 事件
                accept.MouseEnter += (s, e) => { accept.LinkColor = Color.Green; }; // 鼠标悬停时颜色变化
                accept.MouseLeave += (s, e) => { accept.LinkColor = Color.Black; }; // 鼠标离开时恢复原色
                accept.Click += (sender, e) => { Accept(topic); };
                if (comboBox1.Text == "Audited")
                {
                    accept.Visible = false;
                }


                topicPanel.Controls.Add(accept);

                accept.Location = new Point(label.Left - accept.Width - 8, reject.Location.Y);





                // 将控件添加到topicPanel
                //topicPanel.Controls.Add(avatar);



                // 计算topicPanel的位置并添加到mainPanel中
                topicPanel.Location = new Point(0, currentY);
                panel_out.Controls.Add(topicPanel);

                // 更新currentY，为下一个topicPanel的位置做准备
                currentY += topicPanel.Height + panelSpacing;

            }
            // 确保 mainPanel 允许滚动以容纳所有话题
            panel_out.AutoScroll = true;
        }

        private void Detail(Topic topic)
        {
            ClosedStuDetail form = new ClosedStuDetail(topic);
            form.ShowDialog();
        }

        private void Reject(Topic topic)
        {

            using (MessageBox2 form = new MessageBox2("Whether the student has not completed the activity?"))
            {
                // 订阅事件
                form.RequestCloseMainForm += (senderObj, args) =>
                {
                    UpdateDatabase(topic.StuID, activityID, 2);
                };

                // 显示窗体作为模态对话框
                form.ShowDialog();
            }

            /*DialogResult dialogResult = MessageBox.Show(
        "Whether the student has not completed the activity?",
        "Message",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question);

            // 检查用户是否点击了"Yes"
            if (dialogResult == DialogResult.Yes)
            {
                // 执行数据库更新操作
                UpdateDatabase(topic.StuID, activityID, 2);
            }*/
            RefreshTopicsBasedOnFilters();

        }

        private void Accept(Topic topic)
        {
            using (MessageBox2 form = new MessageBox2("Whether the student has not completed the activity?"))
            {
                // 订阅事件
                form.RequestCloseMainForm += (senderObj, args) =>
                {
                    UpdateDatabase(topic.StuID, activityID, 1);
                };

                // 显示窗体作为模态对话框
                form.ShowDialog();
            }

           /* DialogResult dialogResult = MessageBox.Show(
        "Whether the student has completed the activity?",
        "Message",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question);

            // 检查用户是否点击了"Yes"
            if (dialogResult == DialogResult.Yes)
            {
                // 执行数据库更新操作
                UpdateDatabase(topic.StuID, activityID, 1);
            }*/
            RefreshTopicsBasedOnFilters();

        }

        private void UpdateDatabase(int Stuid, int ActivityID, int decision)
        {
            // 假设这是连接字符串，您需要根据您的数据库进行修改
            string connectionString = ConfigInfo.GetDbConnectionString();
            string updateStatement;
            int point;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string orgUserName = SharedData.userName;

                // 构建基本的SQL查询语句
                StringBuilder sqlQuery = new StringBuilder(@"
        SELECT point FROM Events WHERE ActivityID = @activityID");
                using (SqlCommand command = new SqlCommand(sqlQuery.ToString(), connection))
                {
                    command.Parameters.AddWithValue("@activityID", activityID);

                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    point = reader.GetInt32(0);
                    
                }

            }

                if (decision == 1)
            {
                // SQL 更新语句，假设您正在更新一个名为 'YourTable' 的表
                updateStatement = "UPDATE StuInvolvedActiv SET FinishStatus = 1 WHERE StuID = @StuID AND ActivityID = @ActivityID;" +
                    "UPDATE student_information SET credit = credit + @point WHERE stuId = @StuID";
            }
            else
            {
                updateStatement = "UPDATE StuInvolvedActiv SET FinishStatus = 2 WHERE StuID = @StuID AND ActivityID = @ActivityID";
            }

            // 创建数据库连接和命令对象
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(updateStatement, connection))
            {
                command.Parameters.AddWithValue("@StuID", Stuid);
                command.Parameters.AddWithValue("@ActivityID", ActivityID);
                command.Parameters.AddWithValue("@point", point);

                // 打开连接
                connection.Open();
                // 执行更新操作并返回受影响的行数
                command.ExecuteNonQuery();

            }
        }

        private void AuditDetail(Topic topic)
        {
            
            //auditForm.ShowDialog(); // 显示窗体
            

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
            //  List<Topic> topic = LoadTopicsFromDatabase();
           LoadTopicsFromDatabase();

            // 显示新加载的话题
            DisplayTopics();
        }

        private void DisplaySearchResults()
        {
            panel_out.Controls.Clear(); // 清空现有的搜索结果
            currentY = 0; // 初始的 Y 位置，用于定位每个结果的 Panel

            DisplayTopics();
        }

        private void RefreshTopicsBasedOnFilters()
        {
            // 确保comboBox1和comboBox_sort不是null
            if (comboBox1 != null && comboBox_sort != null)
            {
                string selectedStatus = comboBox1.SelectedItem?.ToString(); // 使用null条件运算符防止空引用
                string selectedSortOption = comboBox_sort.SelectedItem?.ToString(); // 使用null条件运算符防止空引用

                // 如果selectedStatus或selectedSortOption为null，则不继续执行
                if (selectedStatus == null || selectedSortOption == null)
                {
                    return;
                }

                string sortBy = selectedSortOption == "Student name" ? "Student name" : "Application time";
                 LoadTopicsFromDatabase(selectedStatus, sortBy); // 直接更新全局变量 topics
                DisplaySearchResults(); // 显示搜索结果
            }
        }




        private void AuditResult_Load(object sender, EventArgs e)
        {

        }

        private void searchButton_Click(object sender, EventArgs e)
        {
           
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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
