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

namespace Activity
{
    public partial class PieChart : System.Windows.Forms.Form
    {
       


        private int StuID;
        private List<string> serviceDomains;
        private List<int> countOfServiceDomains;
        private List<string> serviceRecipients;
        private List<int> countOfServiceRecipients;
        private List<string> serviceLocations;
        private List<int> countOfServiceLocations;

        // 新增属性来指定饼状图的大小和位置
        public Size PieChartSize1 { get; set; }
        public Point PieChartLocation1 { get; set; }


        public Size PieChartSize2 { get; set; }
        public Point PieChartLocation2 { get; set; }
        public Size PieChartSize3 { get; set; }
        public Point PieChartLocation3 { get; set; }


        






        public PieChart(int stuID)
        {
            InitializeComponent();

            WindowState = FormWindowState.Maximized;

            PieChartSize1 = new Size(200, 200);
            PieChartLocation1 = new Point(50, 50);

            PieChartLocation2 = new Point(50, 300);

            PieChartLocation3 = new Point(50, 550);
            StuID = stuID;
            this.panel1.Paint += new PaintEventHandler(this.panel1_Paint);



            // 为Load事件添加事件处理器
            this.Load += (sender, e) => {
                AdjustLayout();
            };

            // 为panel1的Resize事件添加事件处理器
            this.Resize += (sender, e) => {
                AdjustLayout();
            };






        }

        public PieChart(int stuID, List<string> serviceDomain, List<int> countOfServiceDomain, List<string> serviceRecipient, List<int> countOfServiceRecipient, List<string> serviceLocation, List<int> countOfServiceLocation)
        {
            InitializeComponent();
            StuID = stuID;
            serviceDomains = serviceDomain;
            countOfServiceDomains = countOfServiceDomain;
            serviceRecipients = serviceRecipient;
            countOfServiceRecipients = countOfServiceRecipient;
            serviceLocations = serviceLocation;
            countOfServiceLocations = countOfServiceLocation;

            PieChartSize1 = new Size(200, 200);
            PieChartLocation1 = new Point(50, 250);

            PieChartLocation2 = new Point(50, 550);

            PieChartLocation3 = new Point(50, 850);

            //this.panel1.Paint += new PaintEventHandler(this.panel1_Paint);


            // 为Load事件添加事件处理器
            this.Load += (sender, e) => {
                AdjustLayout();
            };

            // 为panel1的Resize事件添加事件处理器
            this.Resize += (sender, e) => {
                AdjustLayout();
            };

        }





        private void AdjustLayout()
        {
            // 调整panel1的大小以适应Form的新大小
            // 假设你希望留出一些边距
            

            panel1.Width = this.ClientSize.Width;

            panel1.Height = this.ClientSize.Height - panel2.Height;

            panel1.Left = 0;

            panel1.Top = panel2.Height;

            int Space = (this.ClientSize.Width - panel3.Width * 3) / 4;


            panel3.Left = Space;

            panel4.Left = panel3.Right + Space;

            panel5.Left = panel4.Right + Space;



            // 调整内部Panel控件的位置
            AdjustPanelsPosition();

            // 强制panel1重新绘制
            panel1.Invalidate();
        }





        private void AdjustPanelsPosition()
        {
            //panel1.Left = 0;

            //panel1.Top = panel2.Bottom;

            //panel1.Width = this.ClientSize.Width;

            //panel1.Height = this.ClientSize.Height - panel2.Height;

            // 计算三等分区域的起始点和高度
            int startHeight = panel3.Bottom + 20;
            int availableHeight = panel1.Height - startHeight;
            int sectionHeight = availableHeight / 2;

            label7.Left = 40;

            label8.Left = 40;

            label9.Left = this.ClientSize.Width / 2;

            label7.Top = startHeight;

            label8.Top = startHeight + sectionHeight;

            label9.Top = startHeight;
        }



       


























        /*protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // 绘制饼状图
            DrawPieChart(e.Graphics, PieChartLocation1, PieChartSize1, countOfServiceDomains);
            // 绘制注解
            DrawLegend(e.Graphics, PieChartLocation1, PieChartSize1, serviceDomains);

            DrawPieChart(e.Graphics, PieChartLocation2, PieChartSize1, countOfServiceRecipients);
            DrawLegend(e.Graphics, PieChartLocation2, PieChartSize1, serviceRecipients);

            DrawPieChart(e.Graphics, PieChartLocation3, PieChartSize1, countOfServiceLocations);
            DrawLegend(e.Graphics, PieChartLocation3, PieChartSize1, serviceLocations);


        }*/

        private void DrawPieChart(Graphics g, Point location, Size size, List<int> list)
        {
            // 准备数据
            // float[] data = { 20, 30, 50, 38, 39, 29, 83 }; // 饼状图数据（百分比）

            float[] data = new float[list.Count()];

            // 准备颜色
            Color[] colors = { Color.Cyan, Color.Gray, Color.Yellow, Color.Purple, Color.Brown, Color.Cyan, Color.Magenta };

            // 计算饼状图的矩形区域
            Rectangle rect = new Rectangle(location, size);

            int total = 0;
            foreach (int val in list)
                total += val;

            float total1 = 0;

            for (int i = 0; i < list.Count; i++)
            {
                if (i != list.Count - 1)
                {
                    float ratio = list[i] / (float)total;
                    data[i] = (float)Math.Round(ratio * 100, 2);
                    total1 += data[i];
                }
                else
                {
                    float ratio = 100 - total1;
                    data[i] = ratio;
                }

            }


            float startAngle = 0;

            for (int i = 0; i < list.Count; i++)
            {
                float sweepAngle = list[i] / (float)total * 360;
                using (SolidBrush brush = new SolidBrush(colors[i]))
                {
                    g.FillPie(brush, rect, startAngle, sweepAngle);
                }

                // 计算扇形的中心点
                float midAngle = startAngle + sweepAngle / 2;
                float midX = (float)(location.X + size.Width / 2 + (size.Width / 2 - 30) * Math.Cos(midAngle * Math.PI / 180));
                float midY = (float)(location.Y + size.Height / 2 + (size.Height / 2 - 30) * Math.Sin(midAngle * Math.PI / 180));

                // 准备文本
                string text = $"{data[i]}%";

                // 计算文本位置
                SizeF textSize = g.MeasureString(text, Font);
                PointF textLocation = new PointF(midX - textSize.Width / 2, midY - textSize.Height / 2);

                // 创建Arial字体实例，假设使用12pt大小
                Font arialFont = new Font("Arial", 8);

                // 使用新字体绘制文本
                g.DrawString(text, arialFont, Brushes.Black, textLocation);

                startAngle += sweepAngle;
            }


        }

        private void DrawLegend(Graphics g, Point location, Size size, List<string> annotations)
        {
            // 准备数据
            // string[] annotations = { "Social Support Sector (Emergency Response, Public Safety Management, Environmental Cleanliness)", "国家2", "国家3" };
            Color[] colors = { Color.Cyan, Color.Gray, Color.Yellow, Color.Purple, Color.Brown, Color.Cyan, Color.Magenta };

            int annotationHeight = 20;
            int annotationMargin = 5;

            for (int i = 0; i < annotations.Count(); i++)
            {
                int x = location.X + size.Width + annotationMargin;
                int y = location.Y + i * (annotationHeight + annotationMargin);

                Rectangle rect = new Rectangle(x, y, annotationHeight, annotationMargin);

                // 绘制颜色块
                using (SolidBrush brush = new SolidBrush(colors[i]))
                {
                    g.FillRectangle(brush, rect);
                }

                // 创建Arial字体实例，假设使用12pt大小
                Font arialFont = new Font("Arial", 8);

                // 使用新字体绘制文本
                g.DrawString(annotations[i], arialFont, Brushes.Black, x + annotationHeight + annotationMargin, y - 2);
            }
        }





        private void PieChart_Load(object sender, EventArgs e)
        {
            

            string connectionString = ConfigInfo.GetDbConnectionString();
            string query = "SELECT stuId, stuName, tel, email, state, credit FROM student_information WHERE stuId = @StuID";
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StuID", StuID);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            lbStu.Text = reader["stuName"].ToString();
                            lbTel.Text = reader["tel"].ToString();
                            lbEmail.Text = reader["email"].ToString();
                            lbStatus.Text = reader["state"].ToString();
                            lbCredit.Text = reader["credit"].ToString();
                            lbID.Text = reader["stuId"].ToString();
                        }
                    }
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            base.OnPaint(e);

            // 假设饼状图的大小保持不变，仅调整位置
            int chartOffsetX = 160; // 饼状图相对于标签的X轴偏移量

            int DOWN = 40;

            // 更新饼状图的位置，使之位于相应标签的后方并与顶部对齐
            PieChartLocation1 = new Point(label7.Left + chartOffsetX, label7.Top + DOWN);
            PieChartLocation2 = new Point(label8.Left + chartOffsetX, label8.Top + DOWN);
            PieChartLocation3 = new Point(label9.Left + chartOffsetX, label9.Top + DOWN);

            // 绘制饼状图和注解
            DrawPieChart(e.Graphics, PieChartLocation1, PieChartSize1, countOfServiceDomains);
            DrawLegend(e.Graphics, PieChartLocation1, PieChartSize1, serviceDomains);

            DrawPieChart(e.Graphics, PieChartLocation2, PieChartSize1, countOfServiceRecipients);
            DrawLegend(e.Graphics, PieChartLocation2, PieChartSize1, serviceRecipients);

            DrawPieChart(e.Graphics, PieChartLocation3, PieChartSize1, countOfServiceLocations);
            DrawLegend(e.Graphics, PieChartLocation3, PieChartSize1, serviceLocations);
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
