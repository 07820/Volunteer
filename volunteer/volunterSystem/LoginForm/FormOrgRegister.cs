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
using System.IO;
using WindowsFormsApp1;

namespace volunterSystem
{
    public partial class FormOrgRegister : System.Windows.Forms.Form
    {
        private byte[] imageBytes;

        public FormOrgRegister()
        {
            InitializeComponent();

            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.BackgroundImageLayout = ImageLayout.Zoom;

            this.Load += FormOrgDetail_Load;
            this.Resize += FormOrgDetail_Resize;
        }


        private void FormOrgRegister_Load(object sender, EventArgs e)
        {

        }



        private void FormOrgDetail_Resize(object sender, EventArgs e)
        {
            //AdjustControls(); // 调整控件位置
            AdjustControlsAndLabels();

            AdjustButtonsPosition();

            AdjustButtonUpLoadPosition();
        }

        private void FormOrgDetail_Load(object sender, EventArgs e)
        {
            //AdjustControls(); // 调整控件位置
            AdjustControlsAndLabels();

            AdjustButtonsPosition();

            AdjustButtonUpLoadPosition();
        }





        private void AdjustControls()
        {
            // 控件数组
            Control[] controls = {
        txtOrgName, txtCompany, txtEmail, txtPwd, txtRePwd, comSecProb, txtAnswer
    };

            int topSpace = 75; // 顶部空间
            int bottomSpace = 200; // 底部空间
            int totalHeight = this.ClientSize.Height; // 窗体的总高度

            // 计算控件应该占据的空间（总可用空间的三分之二）
            int controlSpaceHeight = (totalHeight - topSpace - bottomSpace) * 2 / 3;

            // 控件起始顶部位置（考虑顶部空间后的位置）


            // 控件之间的间距（根据控件的数量动态计算）
            int spaceBetweenControls = controlSpaceHeight / (controls.Length - 1);

            int startTop = topSpace + 10;

            // 计算并设置每个控件的位置
            for (int i = 0; i < controls.Length; i++)
            {

                controls[i].Top = startTop + i * spaceBetweenControls; // 设置顶部位置
                                                                       // 控件水平居中后向右移动40像素
                controls[i].Left = (this.ClientSize.Width - controls[0].Width) / 2 + 100;
            }
        }
        private void AdjustControlsAndLabels()
        {
            // 假设 controls 数组已经按照之前的逻辑排序和调整
            Control[] controls = {
        txtOrgName, txtCompany, txtEmail, txtPwd, txtRePwd, comSecProb, txtAnswer
    };

            // 对应的 Label 控件数组，顺序与 TextBox 控件相匹配
            System.Windows.Forms.Label[] labels = {
        label1, label2, label3, label4, label5, label8, label9
    };

            // 根据之前的逻辑调整 TextBox 控件的位置
            // 这里直接调用之前实现的 AdjustControls() 方法
            // 或者将之前调整控件位置的代码直接集成到这个方法中
            AdjustControls();

            // 遍历 Label 控件数组，调整每个 Label 的位置，使其与对应的 TextBox 水平对齐
            for (int i = 0; i < labels.Length; i++)
            {
                // 假设 Label 应该放置在对应的 TextBox 的左侧，我们这里简单地设置为向左偏移10个像素
                labels[i].Top = controls[i].Top + (controls[i].Height - labels[i].Height) / 2; // 调整 Top 使 Label 垂直居中对齐
                labels[i].Left = controls[i].Left - labels[i].Width - 20; // TextBox 左侧10像素处放置 Label
            }




            // 获取最后一个 label 的引用
            System.Windows.Forms.Label lastLabel = labels[labels.Length - 1];

            // 计算 label6 的 Top 位置，使其放在最后一个 label 的下方
            // 假设我们希望 label6 与其他 label 之间有一定的垂直间距，这里我们设为 20 像素
            int verticalSpacing = 40;
            label6.Top = lastLabel.Top + lastLabel.Height + verticalSpacing;

            // 使 label6 的右边与最后一个 label 的右边对齐
            // 我们需要考虑 label6 的宽度，来正确地设置它的 Left 属性
            label6.Left = 40;


        }



        private void AdjustButtonsPosition()
        {
            int bottomSpace = 20; // 按钮距离底部的距离
            int halfSpaceBetweenButtons = this.ClientSize.Width / 6; // 两个按钮之间一半的空间（使得它们能够围绕中心线对称）

            // 计算按钮的中心位置
            int centerLine = this.ClientSize.Width / 2;

            // 根据中心线和间隙调整btnRegister的位置
            btnRegister.Left = centerLine - btnRegister.Width - halfSpaceBetweenButtons;
            btnRegister.Top = this.ClientSize.Height - btnRegister.Height - bottomSpace;

            // 根据中心线和间隙调整btnCancel的位置
            btnCancel.Left = centerLine + halfSpaceBetweenButtons;
            btnCancel.Top = this.ClientSize.Height - btnCancel.Height - bottomSpace;
        }




        private void AdjustButtonUpLoadPosition()
        {
            int spaceRightOfLabel6 = 20; // btnUpLoad 距离 label6 右边的距离

            // 设置 btnUpLoad 的 Top 使其与 label6 在同一水平线上
            btnUpLoad.Top = label6.Top + (label6.Height - btnUpLoad.Height) / 2;

            // 设置 btnUpLoad 的 Left 为 label6 的右边界加上指定的间距
            btnUpLoad.Left = label6.Right + spaceRightOfLabel6;

            pictureBox1.Top = btnUpLoad.Top + btnUpLoad.Height + spaceRightOfLabel6;

            pictureBox1.Left = btnUpLoad.Left;
        }




















        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (txtOrgName.Text == "" || txtCompany.Text == "" || txtEmail.Text == "" || txtPwd.Text == "" || txtRePwd.Text == "" || comSecProb.Text == "" || txtAnswer.Text == "")
            {
                MessageBox1 form = new MessageBox1("Empty text exists!");
                form.ShowDialog();
                //MessageBox.Show("Empty text exists!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (imageBytes == null)
            {
                MessageBox1 form = new MessageBox1("No picture has been uploaded!");
                form.ShowDialog();
                //MessageBox.Show("No picture has been uploaded!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string name = txtOrgName.Text;
            string companyName = txtCompany.Text;
            string email = txtEmail.Text;
            string pwd = txtPwd.Text;
            string rePwd = txtRePwd.Text;
            string secProb = comSecProb.Text;
            string answer = txtAnswer.Text;

            Dao dao = new Dao();
            dao.connect();
            string sql = $"SELECT email, userName FROM organizer_audited WHERE email = '{email}' OR userName = '{name}';";
            SqlDataReader reader = dao.read(sql);

            if (reader.Read())
            {
                MessageBox1 form = new MessageBox1("The email or username already exists!");
                form.ShowDialog();
                //MessageBox.Show("The email or username already exists!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                reader.Close();
                dao.DaoClose();
                return;
            }

            if (txtPwd.Text.Trim() != txtRePwd.Text.Trim())
            {
                MessageBox1 form = new MessageBox1("The passwords entered twice do not match!");
                //MessageBox.Show("The passwords entered twice do not match!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime currentTime = DateTime.Now;
            string time = currentTime.ToString("yyyy-MM-dd HH:mm:ss");

            sql = "INSERT INTO organizer_unaudited (userName, companyName, email, orgPwd, secProblem, answer, image_data, time) VALUES (@name, @companyName, @email, @pwd, @secProb, @answer, @image, @time);";
            using (SqlConnection conn = dao.connect())
            {
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    command.Parameters.Add("@name", SqlDbType.VarChar).Value = name;
                    command.Parameters.Add("@companyName", SqlDbType.VarChar).Value = companyName;
                    command.Parameters.Add("@email", SqlDbType.VarChar).Value = email;
                    command.Parameters.Add("@pwd", SqlDbType.VarChar).Value = pwd;
                    command.Parameters.Add("@secProb", SqlDbType.VarChar).Value = secProb;
                    command.Parameters.Add("@answer", SqlDbType.VarChar).Value = answer;
                    command.Parameters.Add("@image", SqlDbType.VarBinary).Value = imageBytes;
                    command.Parameters.Add("@time", SqlDbType.VarChar).Value = time;

                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox1 form = new MessageBox1("Registration application submitted successfully, the auditing will be completed within 24 hours.");
                        form.ShowDialog();
                        //MessageBox.Show("Registration application submitted successfully, the auditing will be completed within 24 hours.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox1 form = new MessageBox1("Registration Failure!");
                        form.ShowDialog();
                        //MessageBox.Show("Registration Failure!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }

            imageBytes = null;
            reader.Close();
            dao.DaoClose();
        }

        private void comSecProb_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnUpLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg, *.png, *.gif, *.bmp)|*.jpg; *.png; *.gif; *.bmp";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string imagePath = openFileDialog.FileName;

                    // Load image data into memory
                    imageBytes = File.ReadAllBytes(imagePath);
                    // 将选定的图片显示在PictureBox控件中
                    pictureBox1.Image = Image.FromFile(imagePath);
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

                    MessageBox1 form = new MessageBox1("Image selected successfully!");
                    form.ShowDialog();
                    //MessageBox.Show("Image selected successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox1 form = new MessageBox1("Image selected successfully!");
                    form.ShowDialog();
                    //MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

    }
}
