using Config;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using WindowsFormsApp1;

namespace Activity
{
    public partial class ApplyAudit : Form
    {
        private int activityID;
        private List<Topic> topics;
        int currentY = 0; // 用于计算下一个话题 Panel 的 Y 坐标位置
        int panelSpacing = 10; // 话题 Panel 之间的间距
        public ApplyAudit(int ID)
        {
            activityID = ID;
            topics = new List<Topic>(); // 初始化全局变量
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


            //DisplayTopics(globalTopics);
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

        private void LoadTopicsFromDatabase(string ApplyStatus = "Unaudited", string sortBy = "Student name")
        {
            topics.Clear(); // 清空全局列表，准备加载新数据
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
        AND e.ActivityID = @activityID");

                if (ApplyStatus == "Unaudited")
                {
                    // 添加状态过滤条件
                    sqlQuery.Append(" AND s.ApplyStatus = 0");
                }

                if (ApplyStatus == "Accepted")
                {
                    // 添加状态过滤条件
                    sqlQuery.Append(" AND s.ApplyStatus = 1");
                }



                // 添加排序条件
                string orderByClause = sortBy == "Student name" ? "ORDER BY s.StuName" : "ORDER BY s.AppTime";
                sqlQuery.Append($" {orderByClause}");

                using (SqlCommand command = new SqlCommand(sqlQuery.ToString(), connection))
                {
                    command.Parameters.AddWithValue("@orgUserName", orgUserName);
                    command.Parameters.AddWithValue("@activityID", activityID);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        DateTime activityDate = Convert.ToDateTime(reader["ActivityDate"]);
                        topics.Add(new Topic
                        {
                            // 假设你的Topic类有这些属性
                            ActivityID = (int)reader["ActivityID"],
                            ActivityName = reader["ActivityName"].ToString(),
                            StuID = (int)reader["StuID"],
                            StuName = reader["StuName"].ToString(),
                            Status = reader["ActivityStatus"].ToString(),
                            ActivityDate = activityDate.Date.ToString("yyyy-MM-dd"),
                            ActivityPopulation = reader["ActivityPopulation"].ToString(),
                        });
                    }
                }
            }
            
        }

        private void DisplayTopics()
        {
            panel_out.Controls.Clear(); // 清空现有的显示元素

            currentY = 10;

            Font font1 = new Font("Arial", 12, FontStyle.Bold);
            Font font2 = new Font("Arial", 10, FontStyle.Bold);
            
            foreach (var topic in topics)
            {
                CustomBorderPanel topicPanel = new CustomBorderPanel();
                topicPanel.BorderThickness = 2; // 边框粗细设置为2
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

                stuName.Font = font2;
                stuName.AutoSize = true;

                topicPanel.Controls.Add(stuName);

                stuName.Location = new Point(250,ActivityName.Bottom + 8); // 位置调整为总人数的正后方







                // 活动日期
                LinkLabel ActivityDate = new LinkLabel();
                ActivityDate.Text = $"Date: {topic.ActivityDate}"; // 从数据库获取的话题内容

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










                // 添加Compute按钮
                System.Windows.Forms.Button computeButton = new System.Windows.Forms.Button();
                computeButton.Text = "Calculate Matching";
                computeButton.Font = font2;
                //computeButton.Size = new Size(110, 30); // 可根据需要调整大小
                computeButton.AutoSize = true;

                topicPanel.Controls.Add(computeButton);

                computeButton.Location = new Point(panel_out.Width - computeButton.Width - 250, topCyanPanel.Height + (topicPanel.Height - topCyanPanel.Height) / 2 - computeButton.Height / 2 - 8);





                // 添加label
                Label match = new Label();
                match.Text = "";

                match.Font = font2;
                match.AutoSize = true;

                topicPanel.Controls.Add(match);

                match.Location = new Point(computeButton.Right + 20, computeButton.Top + computeButton.Height / 2 - match.Height / 2);



                // 添加detail
                LinkLabel detail = new LinkLabel();
                detail.Text = "Detail";

                detail.Font = font2;
                detail.AutoSize = true;
                detail.Visible = false;
                detail.Click += (sender, e) => { Detail(topic); };

                computeButton.Click += (sender, e) => { Compute(match, detail, topic); };

                topicPanel.Controls.Add(detail);

                detail.Location = new Point(topicPanel.Width - detail.Width - 20, computeButton.Top + computeButton.Height / 2 - detail.Height / 2);

                



               






                // 添加拒绝
                LinkLabel reject = new LinkLabel();
                reject.Text = "Reject";

                reject.Font = font2;
                reject.AutoSize = true;
                reject.LinkBehavior = LinkBehavior.NeverUnderline;
                reject.LinkColor = reject.ForeColor; // 设置链接颜色为前景色
                reject.ActiveLinkColor = Color.Red; // 设置活动链接颜色为前景色
                reject.MouseEnter += (s, e) => { reject.LinkColor = Color.Green; }; // 鼠标悬停时颜色变化
                reject.MouseLeave += (s, e) => { reject.LinkColor = Color.Black; }; // 鼠标离开时恢复原色
                reject.Click += (sender, e) => { Reject(topic); };
                if (comboBox1.Text == "Accepted")
                {
                    reject.Visible = false;
                }

                topicPanel.Controls.Add(reject);

                reject.Location = new Point(detail.Location.X, topicPanel.Height - reject.Height - 15);






                //添加label
                Label label = new Label();
                label.Text = "/";

                label.Font = font2;
                label.AutoSize = true;
                if (comboBox1.Text == "Accepted")
                {
                    label.Visible = false;
                }

                topicPanel.Controls.Add(label);

                label.Location = new Point(reject.Left - 15, reject.Location.Y);









                // 添加接受
                LinkLabel accept = new LinkLabel();
                accept.Text = "Accept";

                accept.Font = font2;
                accept.AutoSize = true;
                accept.LinkBehavior = LinkBehavior.NeverUnderline;
                accept.LinkColor = accept.ForeColor; // 设置链接颜色为前景色
                accept.ActiveLinkColor = Color.Red; // 设置活动链接颜色为前景色
                // 为了实现鼠标悬停效果，使用 MouseEnter 和 MouseLeave 事件
                accept.MouseEnter += (s, e) => { accept.LinkColor = Color.Green; }; // 鼠标悬停时颜色变化
                accept.MouseLeave += (s, e) => { accept.LinkColor = Color.Black; }; // 鼠标离开时恢复原色
                accept.Click += (sender, e) => { Accept(topic); };
                if (comboBox1.Text == "Accepted")
                {
                    accept.Visible = false;
                }

                topicPanel.Controls.Add(accept);

                accept.Location = new Point(label.Left - accept.Width - 8, reject.Location.Y);





                // 计算topicPanel的位置并添加到mainPanel中
                topicPanel.Location = new Point(0, currentY);
                panel_out.Controls.Add(topicPanel);

                // 更新currentY，为下一个topicPanel的位置做准备
                currentY += topicPanel.Height + panelSpacing;

            }
            // 确保 mainPanel 允许滚动以容纳所有话题
            panel_out.AutoScroll = true;
        }

        private void Compute(Label label, LinkLabel linklabel, Topic topic)
        {
            // 定义要传递给 Python 程序的参数
            string arg1;
            string arg2;
            string arg3;
            string arg4;
            // 92.24, 50.43, 68.8, 337

            string activityType;
            string activityServePeople;
            string activityPlace;

            string connectionString = ConfigInfo.GetDbConnectionString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                StringBuilder sqlQuery = new StringBuilder(@"
        SELECT distinct ActivityType, ActivityServePeople, ActivityPlace FROM StuInvolvedActiv s, Events e WHERE s.ActivityID = e.ActivityID AND s.ActivityID = @ActivityID");
                using (SqlCommand command = new SqlCommand(sqlQuery.ToString(), connection))
                {
                    command.Parameters.AddWithValue("@ActivityID", topic.ActivityID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();

                        activityType = reader.GetString(0); // 读取第一列的值
                        activityServePeople = reader.GetString(1); // 读取第二列的值
                        activityPlace = reader.GetString(2); // 读取第三列的值

                    }
                }
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                StringBuilder sqlQuery = new StringBuilder(@"SELECT credit FROM student_information WHERE stuId = @stuId");

                using (SqlCommand command = new SqlCommand(sqlQuery.ToString(), connection))
                {
                    command.Parameters.AddWithValue("@stuId", topic.StuID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();

                        arg4 = reader.GetInt32(0).ToString(); // 读取第一列的值
                    }
                }
            }

            string type1 = "ActivityType";
            string type2 = "ActivityServePeople";
            string type3 = "ActivityPlace";

            string[] typeNum1OfType1 = { "Cultural Education" };
            string[] typeNum2OfType1 = { "Civic Counseling" };
            string[] typeNum3OfType1 = { "Cultural Tourism Services" };
            string[] typeNum4OfType1 = { "Emergency Response", "Healthcare Services", "Environmental Cleanliness" };
            string[] typeNum5OfType1 = { "Elderly Assistance", "Childcare Services", "Neighborhood Watch" };
            string[] typeNum6OfType1 = { "Online Activities" };

            string[] typeNum1OfType2 = { "Families with Children" };
            string[] typeNum2OfType2 = { "Children" };
            string[] typeNum3OfType2 = { "Adolescents" };
            string[] typeNum4OfType2 = { "Middle-aged Individuals" };
            string[] typeNum5OfType2 = { "Elderly Individuals" };
            string[] typeNum6OfType2 = { "General Public" };

            string[] typeNum1OfType3 = { "Campus" };
            string[] typeNum2OfType3 = { "Museum", "Tourist Attraction" };
            string[] typeNum3OfType3 = { "Elderly Care Institution", "Hospital" };
            string[] typeNum4OfType3 = { "Rural Community" };
            string[] typeNum5OfType3 = { "Transportation Hub" };
            string[] typeNum6OfType3 = { "Online Network" };
            string[] typeNum7OfType3 = { "Other" };

            List<int> numOfDomains = new List<int>();
            List<int> numOfRecipients = new List<int>();
            List<int> numOfLocations = new List<int>();

            numOfDomains.Add(readFromDatabase(type1, typeNum1OfType1, topic));
            numOfDomains.Add(readFromDatabase(type1, typeNum2OfType1, topic));
            numOfDomains.Add(readFromDatabase(type1, typeNum3OfType1, topic));
            numOfDomains.Add(readFromDatabase(type1, typeNum4OfType1, topic));
            numOfDomains.Add(readFromDatabase(type1, typeNum5OfType1, topic));
            numOfDomains.Add(readFromDatabase(type1, typeNum6OfType1, topic));

            numOfRecipients.Add(readFromDatabase(type2, typeNum1OfType2, topic));
            numOfRecipients.Add(readFromDatabase(type2, typeNum2OfType2, topic));
            numOfRecipients.Add(readFromDatabase(type2, typeNum3OfType2, topic));
            numOfRecipients.Add(readFromDatabase(type2, typeNum4OfType2, topic));
            numOfRecipients.Add(readFromDatabase(type2, typeNum5OfType2, topic));
            numOfRecipients.Add(readFromDatabase(type2, typeNum6OfType2, topic));

            numOfLocations.Add(readFromDatabase(type3, typeNum1OfType3, topic));
            numOfLocations.Add(readFromDatabase(type3, typeNum2OfType3, topic));
            numOfLocations.Add(readFromDatabase(type3, typeNum3OfType3, topic));
            numOfLocations.Add(readFromDatabase(type3, typeNum4OfType3, topic));
            numOfLocations.Add(readFromDatabase(type3, typeNum5OfType3, topic));
            numOfLocations.Add(readFromDatabase(type3, typeNum6OfType3, topic));
            numOfLocations.Add(readFromDatabase(type3, typeNum7OfType3, topic));

            int totalNumOfDomains = 0;
            int totalNumOfRecipients = 0;
            int totalNumOfLocations = 0;

            foreach (int val in numOfDomains)
                totalNumOfDomains += val;

            foreach (int val in numOfRecipients)
                totalNumOfRecipients += val;

            foreach (int val in numOfLocations)
                totalNumOfLocations += val;

            if (totalNumOfDomains != 0)
            {
                if (activityType == "Cultural Education")
                {
                    float ratio = numOfDomains[0] / (float)totalNumOfDomains;
                    arg1 = Math.Round(ratio * 100, 2).ToString();
                }
                else if (activityType == "Civic Counseling")
                {
                    float ratio = numOfDomains[1] / (float)totalNumOfDomains;
                    arg1 = Math.Round(ratio * 100, 2).ToString();
                }
                else if (activityType == "Cultural Tourism Services")
                {
                    float ratio = numOfDomains[2] / (float)totalNumOfDomains;
                    arg1 = Math.Round(ratio * 100, 2).ToString();
                }
                else if (activityType == "Emergency Response" || activityType == "Healthcare Services" || activityType == "Environmental Cleanliness")
                {
                    float ratio = numOfDomains[3] / (float)totalNumOfDomains;
                    arg1 = Math.Round(ratio * 100, 2).ToString();
                }
                else if (activityType == "Elderly Assistance" || activityType == "Childcare Services" || activityType == "Neighborhood Watch")
                {
                    float ratio = numOfDomains[4] / (float)totalNumOfDomains;
                    arg1 = Math.Round(ratio * 100, 2).ToString();
                }
                else
                {
                    float ratio = numOfDomains[5] / (float)totalNumOfDomains;
                    arg1 = Math.Round(ratio * 100, 2).ToString();
                }
            }
            else
            {
                arg1 = "0.00";
            }

            if (totalNumOfRecipients != 0)
            {
                if (activityServePeople == "Families with Children")
                {
                    float ratio = numOfRecipients[0] / (float)totalNumOfRecipients;
                    arg2 = Math.Round(ratio * 100, 2).ToString();
                }
                else if (activityServePeople == "Children")
                {
                    float ratio = numOfRecipients[1] / (float)totalNumOfRecipients;
                    arg2 = Math.Round(ratio * 100, 2).ToString();
                }
                else if (activityServePeople == "Adolescents")
                {
                    float ratio = numOfRecipients[2] / (float)totalNumOfRecipients;
                    arg2 = Math.Round(ratio * 100, 2).ToString();
                }
                else if (activityServePeople == "Middle-aged Individuals")
                {
                    float ratio = numOfRecipients[3] / (float)totalNumOfRecipients;
                    arg2 = Math.Round(ratio * 100, 2).ToString();
                }
                else if (activityServePeople == "Elderly Individuals")
                {
                    float ratio = numOfRecipients[4] / (float)totalNumOfRecipients;
                    arg2 = Math.Round(ratio * 100, 2).ToString();
                }
                else
                {
                    float ratio = numOfRecipients[5] / (float)totalNumOfRecipients;
                    arg2 = Math.Round(ratio * 100, 2).ToString();
                }
            }
            else
            {
                arg2 = "0.00";
            }

           if (totalNumOfLocations != 0)
            {
                if (activityPlace == "Campus")
                {
                    float ratio = numOfLocations[0] / (float)totalNumOfLocations;
                    arg3 = Math.Round(ratio * 100, 2).ToString();
                }
                else if (activityPlace == "Museum" || activityPlace == "Tourist Attraction")
                {
                    float ratio = numOfLocations[1] / (float)totalNumOfLocations;
                    arg3 = Math.Round(ratio * 100, 2).ToString();
                }
                else if (activityPlace == "Elderly Care Institution" || activityPlace == "Hospital")
                {
                    float ratio = numOfLocations[2] / (float)totalNumOfLocations;
                    arg3 = Math.Round(ratio * 100, 2).ToString();
                }
                else if (activityPlace == "Rural Community")
                {
                    float ratio = numOfLocations[3] / (float)totalNumOfLocations;
                    arg3 = Math.Round(ratio * 100, 2).ToString();
                }
                else if (activityPlace == "Transportation Hub")
                {
                    float ratio = numOfLocations[4] / (float)totalNumOfLocations;
                    arg3 = Math.Round(ratio * 100, 2).ToString();
                }
                else if (activityPlace == "Online Network")
                {
                    float ratio = numOfLocations[5] / (float)totalNumOfLocations;
                    arg3 = Math.Round(ratio * 100, 2).ToString();
                }
                else
                {
                    float ratio = numOfLocations[6] / (float)totalNumOfLocations;
                    arg3 = Math.Round(ratio * 100, 2).ToString();
                }
            }
            else
            {
                arg3 = "0.00";
            }



            string sArgName = @"exercise3.py";
            string path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + sArgName;

            string interpreter = @"myenv\python.exe";
            string interpreterPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + interpreter;
            // 启动外部的 Python 程序
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = $"{interpreterPath}"; // Python 解释器的路径
            startInfo.Arguments = $"-u {path} {arg1} {arg2} {arg3} {arg4}"; // Python 程序的路径以及参数
                                                                                      // startInfo.Arguments = $"-u C:\\Users\\Alienware\\PycharmProjects\\pythonProject3\\main.py {arg1} {arg2}"; // Python 程序的路径以及参数
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;

            // 创建进程对象并启动
            using (Process process = new Process())
            {
                process.StartInfo = startInfo;
                process.Start();

                // 读取 Python 程序的输出
                string output = process.StandardOutput.ReadToEnd();
                if (output.Contains("[0]"))
                {
                    label.Text = "perfect fit";
                }
                else if (output.Contains("[1]"))
                {
                    label.Text = "fit";
                }
                else if (output.Contains("[2]"))
                {
                    label.Text = "normal";
                }
                else if (output.Contains("[3]"))
                {
                    label.Text = "not fit";
                }
                else
                {
                    label.Text = "bad fit";
                }
                linklabel.Visible = true;


                

                // 设置match的背景颜色
                UpdateMatchBackgroundColor(label);

            }


        }


        private void UpdateMatchBackgroundColor(Label match)
        {
            switch (match.Text)
            {
                case "perfect fit":
                    match.BackColor = Color.LimeGreen; // 或者使用任何你喜欢的颜色
                    match.ForeColor = Color.White; // 设置字体颜色以保证可读性
                    break;
                case "fit":
                    match.BackColor = Color.Green;
                    match.ForeColor = Color.White;
                    break;
                case "normal":
                    match.BackColor = Color.Yellow;
                    match.ForeColor = Color.Black;
                    break;
                case "not fit":
                    match.BackColor = Color.Orange;
                    match.ForeColor = Color.White;
                    break;
                case "bad fit":
                    match.BackColor = Color.Red;
                    match.ForeColor = Color.White;
                    break;
                default:
                    match.BackColor = Color.Transparent; // 未匹配任何条件时的默认颜色
                    match.ForeColor = Color.Black;
                    break;
            }
        }



        private void Detail(Topic topic)
        {
            string activityType;
            string activityServePeople;
            string activityPlace;

            string connectionString = ConfigInfo.GetDbConnectionString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                StringBuilder sqlQuery = new StringBuilder(@"
        SELECT distinct ActivityType, ActivityServePeople, ActivityPlace FROM StuInvolvedActiv s, Events e WHERE s.ActivityID = e.ActivityID AND s.ActivityID = @ActivityID");
                using (SqlCommand command = new SqlCommand(sqlQuery.ToString(), connection))
                {
                    command.Parameters.AddWithValue("@ActivityID", topic.ActivityID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();

                        activityType = reader.GetString(0); // 读取第一列的值
                        activityServePeople = reader.GetString(1); // 读取第二列的值
                        activityPlace = reader.GetString(2); // 读取第三列的值

                    }
                }
            }

            string type1 = "ActivityType";
            string type2 = "ActivityServePeople";
            string type3 = "ActivityPlace";

            string[] typeNum1OfType1 = { "Cultural Education" };
            string[] typeNum2OfType1 = { "Civic Counseling" };
            string[] typeNum3OfType1 = { "Cultural Tourism Services" };
            string[] typeNum4OfType1 = { "Emergency Response", "Healthcare Services", "Environmental Cleanliness" };
            string[] typeNum5OfType1 = { "Elderly Assistance", "Childcare Services", "Neighborhood Watch" };
            string[] typeNum6OfType1 = { "Online Activities" };

            string[] typeNum1OfType2 = { "Families with Children" };
            string[] typeNum2OfType2 = { "Children" };
            string[] typeNum3OfType2 = { "Adolescents" };
            string[] typeNum4OfType2 = { "Middle-aged Individuals" };
            string[] typeNum5OfType2 = { "Elderly Individuals" };
            string[] typeNum6OfType2 = { "General Public" };

            string[] typeNum1OfType3 = { "Campus" };
            string[] typeNum2OfType3 = { "Museum", "Tourist Attraction" };
            string[] typeNum3OfType3 = { "Elderly Care Institution", "Hospital" };
            string[] typeNum4OfType3 = { "Rural Community" };
            string[] typeNum5OfType3 = { "Transportation Hub" };
            string[] typeNum6OfType3 = { "Online Network" };
            string[] typeNum7OfType3 = { "Other" };

            List<string> serviceDomains = new List<string>();

            // 添加元素到列表中
            serviceDomains.Add("Education Sector (Cultural Education)");
            serviceDomains.Add("Social Civility Sector (Civic Counseling)");
            serviceDomains.Add("Tourism Services Sector (Cultural Tourism Services)");
            serviceDomains.Add("Social Support Sector (Emergency Response, Healthcare Services, Environmental Cleanliness)");
            serviceDomains.Add("Social Welfare Sector (Elderly Assistance, Childcare Services, Neighborhood Watch)");
            serviceDomains.Add("Digital Services Sector (Online Activities)");

            List<string> serviceRecipients = new List<string>();
            serviceRecipients.Add("Families with Children");
            serviceRecipients.Add("Children");
            serviceRecipients.Add("Adolescents");
            serviceRecipients.Add("Middle-aged Individuals");
            serviceRecipients.Add("Elderly Individuals");
            serviceRecipients.Add("General Public");

            List<string> serviceLocations = new List<string>();
            serviceLocations.Add("Educational Facilities (Campus)");
            serviceLocations.Add("Cultural Facilities (Museum, Tourist Attraction)");
            serviceLocations.Add("Social Welfare Facilities (Elderly Care Institution, Hospital)");
            serviceLocations.Add("Community Facilities (Rural Community)");
            serviceLocations.Add("Public Spaces (Transportation Hub)");
            serviceLocations.Add("Digital Platforms (Online Network)");
            serviceLocations.Add("Other");

            List<int> numOfDomains = new List<int>();
            List<int> numOfRecipients = new List<int>();
            List<int> numOfLocations = new List<int>();

            numOfDomains.Add(readFromDatabase(type1, typeNum1OfType1, topic));
            numOfDomains.Add(readFromDatabase(type1, typeNum2OfType1, topic));
            numOfDomains.Add(readFromDatabase(type1, typeNum3OfType1, topic));
            numOfDomains.Add(readFromDatabase(type1, typeNum4OfType1, topic));
            numOfDomains.Add(readFromDatabase(type1, typeNum5OfType1, topic));
            numOfDomains.Add(readFromDatabase(type1, typeNum6OfType1, topic));

            numOfRecipients.Add(readFromDatabase(type2, typeNum1OfType2, topic));
            numOfRecipients.Add(readFromDatabase(type2, typeNum2OfType2, topic));
            numOfRecipients.Add(readFromDatabase(type2, typeNum3OfType2, topic));
            numOfRecipients.Add(readFromDatabase(type2, typeNum4OfType2, topic));
            numOfRecipients.Add(readFromDatabase(type2, typeNum5OfType2, topic));
            numOfRecipients.Add(readFromDatabase(type2, typeNum6OfType2, topic));

            numOfLocations.Add(readFromDatabase(type3, typeNum1OfType3, topic));
            numOfLocations.Add(readFromDatabase(type3, typeNum2OfType3, topic));
            numOfLocations.Add(readFromDatabase(type3, typeNum3OfType3, topic));
            numOfLocations.Add(readFromDatabase(type3, typeNum4OfType3, topic));
            numOfLocations.Add(readFromDatabase(type3, typeNum5OfType3, topic));
            numOfLocations.Add(readFromDatabase(type3, typeNum6OfType3, topic));
            numOfLocations.Add(readFromDatabase(type3, typeNum7OfType3, topic));

            string str;
            int num;
            if (activityType == "Cultural Education") { }
            else if (activityType == "Civic Counseling")
            {
                str = serviceDomains[1];
                serviceDomains[1] = serviceDomains[0];
                serviceDomains[0] = str;

                num = numOfDomains[1];
                numOfDomains[1] = numOfDomains[0];
                numOfDomains[0] = num;
            }
            else if (activityType == "Cultural Tourism Services")
            {
                str = serviceDomains[2];
                serviceDomains[2] = serviceDomains[0];
                serviceDomains[0] = str;

                num = numOfDomains[2];
                numOfDomains[2] = numOfDomains[0];
                numOfDomains[0] = num;
            }
            else if (activityType == "Emergency Response" || activityType == "Healthcare Services" || activityType == "Environmental Cleanliness")
            {
                str = serviceDomains[3];
                serviceDomains[3] = serviceDomains[0];
                serviceDomains[0] = str;

                num = numOfDomains[3];
                numOfDomains[3] = numOfDomains[0];
                numOfDomains[0] = num;
            }
            else if (activityType == "Elderly Assistance" || activityType == "Childcare Services" || activityType == "Neighborhood Watch")
            {
                str = serviceDomains[4];
                serviceDomains[4] = serviceDomains[0];
                serviceDomains[0] = str;

                num = numOfDomains[4];
                numOfDomains[4] = numOfDomains[0];
                numOfDomains[0] = num;
            }
            else
            {
                str = serviceDomains[5];
                serviceDomains[5] = serviceDomains[0];
                serviceDomains[0] = str;

                num = numOfDomains[5];
                numOfDomains[5] = numOfDomains[0];
                numOfDomains[0] = num;
            }

            if (activityServePeople == "Families with Children") { }
            else if (activityServePeople == "Children")
            {
                str = serviceRecipients[1];
                serviceRecipients[1] = serviceRecipients[0];
                serviceRecipients[0] = str;

                num = numOfRecipients[1];
                numOfRecipients[1] = numOfRecipients[0];
                numOfRecipients[0] = num;
            }
            else if (activityServePeople == "Adolescents")
            {
                str = serviceRecipients[2];
                serviceRecipients[2] = serviceRecipients[0];
                serviceRecipients[0] = str;

                num = numOfRecipients[2];
                numOfRecipients[2] = numOfRecipients[0];
                numOfRecipients[0] = num;
            }
            else if (activityServePeople == "Middle-aged Individuals")
            {
                str = serviceRecipients[3];
                serviceRecipients[3] = serviceRecipients[0];
                serviceRecipients[0] = str;

                num = numOfRecipients[3];
                numOfRecipients[3] = numOfRecipients[0];
                numOfRecipients[0] = num;
            }
            else if (activityServePeople == "Elderly Individuals")
            {
                str = serviceRecipients[4];
                serviceRecipients[4] = serviceRecipients[0];
                serviceRecipients[0] = str;

                num = numOfRecipients[4];
                numOfRecipients[4] = numOfRecipients[0];
                numOfRecipients[0] = num;
            }
            else
            {
                str = serviceRecipients[5];
                serviceRecipients[5] = serviceRecipients[0];
                serviceRecipients[0] = str;

                num = numOfRecipients[5];
                numOfRecipients[5] = numOfRecipients[0];
                numOfRecipients[0] = num;
            }

            if (activityPlace == "Campus") { }
            else if (activityPlace == "Museum" || activityPlace == "Tourist Attraction")
            {
                str = serviceLocations[1];
                serviceLocations[1] = serviceLocations[0];
                serviceLocations[0] = str;

                num = numOfLocations[1];
                numOfLocations[1] = numOfLocations[0];
                numOfLocations[0] = num;
            }
            else if (activityPlace == "Elderly Care Institution" || activityPlace == "Hospital")
            {
                str = serviceLocations[2];
                serviceLocations[2] = serviceLocations[0];
                serviceLocations[0] = str;

                num = numOfLocations[2];
                numOfLocations[2] = numOfLocations[0];
                numOfLocations[0] = num;
            }
            else if (activityPlace == "Rural Community")
            {
                str = serviceLocations[3];
                serviceLocations[3] = serviceLocations[0];
                serviceLocations[0] = str;

                num = numOfLocations[3];
                numOfLocations[3] = numOfLocations[0];
                numOfLocations[0] = num;
            }
            else if (activityPlace == "Transportation Hub")
            {
                str = serviceLocations[4];
                serviceLocations[4] = serviceLocations[0];
                serviceLocations[0] = str;

                num = numOfLocations[4];
                numOfLocations[4] = numOfLocations[0];
                numOfLocations[0] = num;
            }
            else if (activityPlace == "Online Network")
            {
                str = serviceLocations[5];
                serviceLocations[5] = serviceLocations[0];
                serviceLocations[0] = str;

                num = numOfLocations[5];
                numOfLocations[5] = numOfLocations[0];
                numOfLocations[0] = num;
            }
            else
            {
                str = serviceLocations[6];
                serviceLocations[6] = serviceLocations[0];
                serviceLocations[0] = str;

                num = numOfLocations[6];
                numOfLocations[6] = numOfLocations[0];
                numOfLocations[0] = num;
            }


            for (int i = numOfDomains.Count - 1; i >= 0; i--)
            {
                if (numOfDomains[i] == 0)
                {
                    numOfDomains.RemoveAt(i);
                    serviceDomains.RemoveAt(i);
                }
            }

            for (int i = numOfRecipients.Count - 1; i >= 0; i--)
            {
                if (numOfRecipients[i] == 0)
                {
                    numOfRecipients.RemoveAt(i);
                    serviceRecipients.RemoveAt(i);
                }
            }

            for (int i = numOfLocations.Count - 1; i >= 0; i--)
            {
                if (numOfLocations[i] == 0)
                {
                    numOfLocations.RemoveAt(i);
                    serviceLocations.RemoveAt(i);
                }
            }



            PieChart form = new PieChart(topic.StuID, serviceDomains, numOfDomains, serviceRecipients, numOfRecipients, serviceLocations, numOfLocations);
            form.ShowDialog();
        }


        private static int readFromDatabase(string type, string[] typeNum, Topic topic) 
        {
            int count = 0;
            string connectionString = ConfigInfo.GetDbConnectionString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                StringBuilder sqlQuery;

                connection.Open();
                if(type == "ActivityType")
                {
                    if(typeNum.Length == 1)
                    {
                    sqlQuery = new StringBuilder(@"SELECT COUNT(*) FROM StuInvolvedActiv s , Events e 
WHERE s.ActivityID = e.ActivityID AND StuID = @StuID AND ActivityType = @type AND s.FinishStatus = 1");
                    using (SqlCommand command = new SqlCommand(sqlQuery.ToString(), connection))
                        {
                            command.Parameters.AddWithValue("@StuID", topic.StuID);
                            command.Parameters.AddWithValue("@type", typeNum[0]);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    count = reader.GetInt32(0); // 读取第一列的值
                                }
                            }
                        }

                    }
                    else if(typeNum.Length == 2)
                    {
                        sqlQuery = new StringBuilder(@"SELECT COUNT(*) FROM StuInvolvedActiv s , Events e 
WHERE s.ActivityID = e.ActivityID AND StuID = @StuID AND (ActivityType = @type1 OR ActivityType = @type2)  AND s.FinishStatus = 1");
                        using (SqlCommand command = new SqlCommand(sqlQuery.ToString(), connection))
                        {
                            command.Parameters.AddWithValue("@StuID", topic.StuID);
                            command.Parameters.AddWithValue("@type1", typeNum[0]);
                            command.Parameters.AddWithValue("@type2", typeNum[1]);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    count = reader.GetInt32(0); // 读取第一列的值
                                }
                            }
                        }
                    }
                    else if(typeNum.Length == 3)
                    {
                        sqlQuery = new StringBuilder(@"SELECT COUNT(*) FROM StuInvolvedActiv s , Events e 
WHERE s.ActivityID = e.ActivityID AND StuID = @StuID AND (ActivityType = @type1 OR ActivityType = @type2 OR ActivityType = @type3)  AND s.FinishStatus = 1");
                        using (SqlCommand command = new SqlCommand(sqlQuery.ToString(), connection))
                        {
                            command.Parameters.AddWithValue("@StuID", topic.StuID);
                            command.Parameters.AddWithValue("@type1", typeNum[0]);
                            command.Parameters.AddWithValue("@type2", typeNum[1]);
                            command.Parameters.AddWithValue("@type3", typeNum[2]);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    count = reader.GetInt32(0); // 读取第一列的值
                                }
                            }
                        }
                    }

                }

                else if (type == "ActivityServePeople")
                {
                    sqlQuery = new StringBuilder(@"SELECT COUNT(*) FROM StuInvolvedActiv s , Events e 
WHERE s.ActivityID = e.ActivityID AND StuID = @StuID AND ActivityServePeople = @type  AND s.FinishStatus = 1");
                    using (SqlCommand command = new SqlCommand(sqlQuery.ToString(), connection))
                    {
                        command.Parameters.AddWithValue("@StuID", topic.StuID);
                        command.Parameters.AddWithValue("@type", typeNum[0]);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                count = reader.GetInt32(0); // 读取第一列的值
                            }
                        }
                    }
                }

                else if (type == "ActivityPlace")
                {
                    if (typeNum.Length == 1)
                    {
                        sqlQuery = new StringBuilder(@"SELECT COUNT(*) FROM StuInvolvedActiv s , Events e 
WHERE s.ActivityID = e.ActivityID AND StuID = @StuID AND ActivityPlace = @type  AND s.FinishStatus = 1");
                        using (SqlCommand command = new SqlCommand(sqlQuery.ToString(), connection))
                        {
                            command.Parameters.AddWithValue("@StuID", topic.StuID);
                            command.Parameters.AddWithValue("@type", typeNum[0]);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    count = reader.GetInt32(0); // 读取第一列的值
                                }
                            }
                        }
                    }

                    else if(typeNum.Length == 2)
                    {
                        sqlQuery = new StringBuilder(@"SELECT COUNT(*) FROM StuInvolvedActiv s , Events e 
WHERE s.ActivityID = e.ActivityID AND StuID = @StuID AND (ActivityPlace = @type1 OR ActivityPlace = @type2)  AND s.FinishStatus = 1");
                        using (SqlCommand command = new SqlCommand(sqlQuery.ToString(), connection))
                        {
                            command.Parameters.AddWithValue("@StuID", topic.StuID);
                            command.Parameters.AddWithValue("@type1", typeNum[0]);
                            command.Parameters.AddWithValue("@type2", typeNum[1]);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    count = reader.GetInt32(0); // 读取第一列的值
                                }
                            }
                        }
                    }
                }



            }


            return count;
        }



        private void Accept(Topic topic)
        {
            int numOfEnrolled;

            string connectionString = ConfigInfo.GetDbConnectionString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                StringBuilder sqlQuery = new StringBuilder(@"SELECT COUNT(*) FROM StuInvolvedActiv WHERE ActivityID = @activityID AND ApplyStatus = 1");

                using (SqlCommand command = new SqlCommand(sqlQuery.ToString(), connection))
                {
                    command.Parameters.AddWithValue("@activityID", topic.ActivityID);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        numOfEnrolled = reader.GetInt32(0);
                        if (numOfEnrolled >= int.Parse(topic.ActivityPopulation))
                        {
                            MessageBox.Show("The number of accepted students is full!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                }
            }

            using (MessageBox2 form = new MessageBox2("Are you sure to accept?"))
            {
                // 订阅事件
                form.RequestCloseMainForm += (senderObj, args) =>
                {
                    UpdateDatabase(topic.StuID, activityID, 1);
                };

                // 显示窗体作为模态对话框
                form.ShowDialog();
            }

            /*DialogResult dialogResult = MessageBox.Show(
        "Are you sure to accept?",
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

            if (decision == 1)
            {
                // SQL 更新语句，假设您正在更新一个名为 'YourTable' 的表
                updateStatement = "UPDATE StuInvolvedActiv SET ApplyStatus = 1 WHERE StuID = @StuID AND ActivityID = @ActivityID";
            }
            else
            {
                updateStatement = "UPDATE StuInvolvedActiv SET ApplyStatus = 2 WHERE StuID = @StuID AND ActivityID = @ActivityID";
            }

            // 创建数据库连接和命令对象
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(updateStatement, connection))
            {
                command.Parameters.AddWithValue("@StuID", Stuid);
                command.Parameters.AddWithValue("@ActivityID", ActivityID);

                // 打开连接
                connection.Open();
                    // 执行更新操作并返回受影响的行数
                    command.ExecuteNonQuery();

            }
        }

        private void Reject(Topic topic)
        {
            using (MessageBox2 form = new MessageBox2("Are you sure to reject?"))
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
        "Are you sure to reject?",
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
            if (comboBox1 != null && comboBox_sort != null )
            {
                string selectedStatus = comboBox1.SelectedItem?.ToString(); // 使用null条件运算符防止空引用
                string selectedSortOption = comboBox_sort.SelectedItem?.ToString(); // 使用null条件运算符防止空引用

                // 如果selectedStatus或selectedSortOption为null，则不继续执行
                if (selectedStatus == null || selectedSortOption == null)
                {
                    return;
                }

                string sortBy = selectedSortOption == "Student name" ? "Student name" : "Application time";

                // 直接调用LoadTopicsFromDatabase更新全局变量topics
                LoadTopicsFromDatabase(selectedStatus, sortBy);

                // 使用全局变量topics直接显示搜索结果
                DisplaySearchResults();
            }
        }



        private void ApplyAudit_Load(object sender, EventArgs e)
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

        private void searchBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel_out_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
