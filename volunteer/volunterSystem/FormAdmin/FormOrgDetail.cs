using Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using WindowsFormsApp1;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace volunterSystem
{
    public partial class FormOrgDetail : System.Windows.Forms.Form
    {
        private string email;
        public FormOrgDetail(string email)
        {
            InitializeComponent();
            this.email = email;

            this.Load += new EventHandler(FormOrgDetail_Load);
            this.Resize += new EventHandler(FormOrgDetail_Resize);
        }

        private void FormOrgDetail_Load(object sender, EventArgs e)
        {
            LoadStudentDetails();

            //AlignLabels();
            AlignLabelsVertically();

            AlignButton1();
        }


        private void FormOrgDetail_Resize(object sender, EventArgs e)
        {
            //AlignLabels();
            AlignLabelsVertically();

            AlignButton1();
        }


        private void AlignLabelsVertically()
        {
            System.Windows.Forms.Label[] leftLabels = { label7, label1, label2, label3, label4, label5, label10 };
            System.Windows.Forms.Label[] rightLabels = { lbUserName, lbCompanyName, lbEmail, lbOrgPwd, lbSecQues, lbAns, lbStatus };
            int spaceFromTop = 85; // 第一个label距离Form顶部的间距
            int verticalSpaceBetweenLabels = 30; // Label控件之间的垂直间距

            int space = 60;
            int labelTop = spaceFromTop;

            for (int i = 0; i < leftLabels.Length; i++)
            {
                // 设置左边Label的位置
                leftLabels[i].Left = space; // 左边的间距
                leftLabels[i].Top = labelTop;

                // 设置右边Label的位置
                rightLabels[i].Left = this.ClientSize.Width - rightLabels[i].Width - space; // 右边的间距
                rightLabels[i].Top = labelTop;

                // 更新下一个label的顶部位置
                labelTop += leftLabels[i].Height + verticalSpaceBetweenLabels;
            }

            // 现在，设置label6的位置
            // label6将放在最后一个label的下面，并与其保持相同的间距
            label6.Left = space; // 左边的间距与其他label保持一致
            label6.Top = labelTop;
        }


        private void AlignButton1()
        {
            int space = 60;
            // 设置button1到Form左边的距离为n
            button1.Left = space;
            // 设置button1到Form底部的距离为n
            button1.Top = this.ClientSize.Height - button1.Height - space;
        }
























        private void LoadStudentDetails()
        {
            string connectionString = ConfigInfo.GetDbConnectionString();
            string query = "SELECT userName, companyName, email, orgPwd, secProblem, email, answer, image_data, state FROM organizer_audited WHERE email = @email";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // 将参数添加到SQL查询
                    command.Parameters.AddWithValue("@email", email);

                    try
                    {
                        // 打开数据库连接
                        connection.Open();

                        // 执行查询
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // 假设你的Label控件名称分别是labelName, labelID, ...
                                lbUserName.Text = reader["userName"].ToString();
                                lbCompanyName.Text = reader["companyName"].ToString();
                                lbEmail.Text = reader["email"].ToString();
                                lbOrgPwd.Text = reader["orgPwd"].ToString();
                                lbSecQues.Text = reader["secProblem"].ToString();
                                lbAns.Text = reader["answer"].ToString();
                                lbStatus.Text = reader["state"].ToString();

                                // 处理图像数据
                                if (reader["image_data"] != DBNull.Value)
                                {
                                    byte[] imageData = (byte[])reader["image_data"];
                                    using (MemoryStream ms = new MemoryStream(imageData))
                                    {
                                        pictureBox1.Image = System.Drawing.Image.FromStream(ms);
                                    }
                                }
                                else
                                {
                                    pictureBox1.Image = null; // 如果没有图像数据，可以选择清除PictureBox或设置默认图像
                                }
                            }
                            else
                            {
                                MessageBox1 form = new MessageBox1("No organizer details found.");
                                form.ShowDialog();
                                //MessageBox.Show("No organizer details found.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox1 form = new MessageBox1("Error: " + ex.Message);
                        form.ShowDialog();
                        //MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
