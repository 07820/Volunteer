using Activity;
using Config;
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

namespace volunterSystem
{
    public partial class FormMyActivities : System.Windows.Forms.Form
    {
        private List<Topic3> topics = new List<Topic3>();

        private int currentY = 0;
        private int panelSpacing = 10;

        //int currentY = 10; // 用于计算下一个话题 Panel 的 Y 坐标位置
        //int panelSpacing = 20; // 话题 Panel 之间的间距
        private Color colorForLabel = Color.Orange;
        private Color colorForFront = Color.Black;

        Font font1 = new Font("Arial", 12, FontStyle.Bold); // 主要信息使用加粗字体
        Font font2 = new Font("Arial", 10); // 次要信息使用普通字体

        int panelHeight = 150; // Panel高度固定



        public FormMyActivities()
        {
            InitializeComponent();

            this.Load += new System.EventHandler(this.FormMyActivities_Load);
            this.Resize += new System.EventHandler(this.FormMyActivities_Resize);
        }
        private void FormMyActivities_Load(object sender, EventArgs e)
        {
            // 假设您已经有办法获取当前学生的ID，这里使用0作为示例
            int studentId = FormLogin.stuID; // 应替换为实际获取学生ID的方法
            LoadTopicsFromDatabase(studentId);
            PositionPanel();
            DisplayTopics(topics); // Now passing the entire list to be displayed


        }

        private void FormMyActivities_Resize(object sender, EventArgs e)
        {
            // 初始化控件位置
            PositionSearchBox();

            PositionPanel();

            DisplayTopics(this.topics);
        }


        private void PositionPanel()
        {
            label1.Left = 10;

            label1.Top = 76;

            activitiesPanel.Left = 10;

            activitiesPanel.Top = label1.Bottom + 5;

            activitiesPanel.Width = this.ClientSize.Width - 20;

            activitiesPanel.Height = this.ClientSize.Height - 10 - activitiesPanel.Top;
        }


        // 创建一个新的方法来设置searchBox的位置和大小
        private void PositionSearchBox()
        {
            activitiesPanel.Left = 0;// = new Point(5, searchBox.Bottom + 3);

            activitiesPanel.Top = 0;

            activitiesPanel.Width = this.ClientSize.Width; // 假设您希望左右各保持与 searchBox 一样的边距

            activitiesPanel.Height = this.ClientSize.Height - 10;
        }



        private void LoadTopicsFromDatabase(int studentId)
        {
            topics.Clear(); // 清除旧数据

            string connectionString = ConfigInfo.GetDbConnectionString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sqlQuery = @"
SELECT 
    E.ActivityID, 
    E.ActivityName, 
    E.ActivityDate, 
    E.ActivityPopulation, 
    E.Status, 
    E.ActivityType, 
    E.ActivityServePeople, 
    E.ActivityPlace, 
    SIA.StuID,
    E.ActivityGraph
FROM 
    Events E
INNER JOIN 
    StuInvolvedActiv SIA ON E.ActivityID = SIA.ActivityID
WHERE 
    SIA.StuID = @StudentId";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@StudentId", studentId);

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        topics.Add(new Topic3
                        {
                            ActivityID = (int)reader["ActivityID"],
                            ActivityName = reader["ActivityName"].ToString(),
                            ActivityDate = Convert.ToDateTime(reader["ActivityDate"]).ToString("yyyy-MM-dd"),
                            ActivityPopulation = (int)reader["ActivityPopulation"],
                            Status = reader["Status"].ToString(),
                            ActivityType = reader["ActivityType"].ToString(),
                            ActivityServePeople = reader["ActivityServePeople"].ToString(),
                            ActivityPlace = reader["ActivityPlace"].ToString(),
                            NumOfInv = 0, // 默认值，因为这个字段不在Events表中
                            ImageData = reader["ActivityGraph"] as byte[]
                        });
                    }
                }
            }
        }








        private void DisplayTopics(List<Topic3> topics)
        {
            activitiesPanel.Controls.Clear(); // 清空现有的显示元素

            currentY = 10;

            Font font1 = new Font("Arial", 12, FontStyle.Bold); // 主要信息使用加粗字体
            Font font2 = new Font("Arial", 10); // 次要信息使用普通字体

            //int panelWidth = panel_out.Width - 20; // Panel宽度固定
            int panelHeight = 150; // Panel高度固定
            //int spacing = 10; // 控件之间的间距


            foreach (var topic in topics)
            {
                Panel topicPanel = new CustomBorderPanel()
                {
                    BorderStyle = BorderStyle.None,

                    BorderColor = Color.Black,

                    BorderThickness = 2,

                    BackColor = Color.White,

                    Size = new Size(activitiesPanel.ClientSize.Width - 50, panelHeight),

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










                ImageLabel ActivityType = new ImageLabel()
                {
                    Text = $"     {topic.ActivityType}",

                    Image = Properties.Resources.C,
                    //Location = new Point(15, 12), // 这行可能不再需要，因为我们会使用Dock属性
                    Location = new Point(ActivityName.Left + ActivityName.Width + 10, ActivityName.Top),

                    ImageAlign = ContentAlignment.MiddleLeft,
                    TextAlign = ContentAlignment.MiddleLeft, // 确保文本也左对齐
                    //Dock = DockStyle.Left, // 确保整个控件位于父控件的左侧

                    Font = font1,
                    ForeColor = Color.Black,
                    BackColor = Color.Transparent, // 要设置为浅橙色，你需要改变这里的颜色
                    AutoSize = true,

                };

                if (topic.ActivityType == "Health Care")
                {
                    ActivityType.Image = Properties.Resources.C;
                }

                if (topic.ActivityType == "Children Care")
                {
                    ActivityType.Image = Properties.Resources._1Children;
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




                activitiesPanel.Controls.Add(topicPanel);

                topicPanel.Location = new Point(10, currentY);


                // 更新currentY，为下一个topicPanel的位置做准备
                currentY += topicPanel.Height + panelSpacing;

            }
            // 确保 mainPanel 允许滚动以容纳所有话题
            activitiesPanel.AutoScroll = true;
        }




        private void ShowActivityDetails(int activityId)
        {
            // 这里创建并显示对应活动详情的Form
            // 例如：
            ActivityDetails displayForm = new ActivityDetails(activityId); // receive activityId
            displayForm.Show();
        }




        private void activitiesPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void activitiesPanel_Paint_1(object sender, PaintEventArgs e)
        {

        }
    }


}

