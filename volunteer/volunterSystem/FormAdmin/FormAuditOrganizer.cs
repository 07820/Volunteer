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
using WindowsFormsApp1;

namespace volunterSystem
{
    public partial class FormAuditOrganizer : System.Windows.Forms.Form
    {
        private BindingSource bindingSource = new BindingSource();
        private SqlConnection connection = new SqlConnection(ConfigInfo.GetDbConnectionString());
        public FormAuditOrganizer()
        {
            InitializeComponent();

            dgv.DataSource = bindingSource;

            // 调用 LoadData 方法加载数据到数据源
            LoadData();


            // 绑定事件处理器
            this.Load += FormAuditOrganizer_Load;
            this.Resize += FormAuditOrganizer_Resize;
        }


        private void FormAuditOrganizer_Load(object sender, EventArgs e)
        {
            LoadData();

            btnRefresh.Left = this.ClientSize.Width - btnRefresh.Width - 8;

            btnRefresh.Top = 8;

           


            dgv.Left = 8;

            dgv.Top = btnRefresh.Bottom + 8;

            dgv.Width = this.ClientSize.Width - 16;

            dgv.Height = this.ClientSize.Height - dgv.Top - 60;

            btnCheck.Top = dgv.Bottom + 30 - btnCheck.Height / 2;

            btnCheck.Left = 20;

            btnReject.Top = btnCheck.Top;

            btnReject.Left = this.ClientSize.Width - btnReject.Width - 20;

            btnPass.Top = btnReject.Top;

            btnPass.Left = btnReject.Left - btnPass.Width - 10;
        }


        private void FormAuditOrganizer_Resize(object sender, EventArgs e)
        {


            btnRefresh.Left = this.ClientSize.Width - btnRefresh.Width - 8;

            btnRefresh.Top = 8;




            dgv.Left = 8;

            dgv.Top = btnRefresh.Bottom + 8;

            dgv.Width = this.ClientSize.Width - 16;

            dgv.Height = this.ClientSize.Height - dgv.Top - 60;

            btnCheck.Top = dgv.Bottom + 30 - btnCheck.Height / 2;

            btnCheck.Left = 20;

            btnReject.Top = btnCheck.Top;

            btnReject.Left = this.ClientSize.Width - btnReject.Width - 20;

            btnPass.Top = btnReject.Top;

            btnPass.Left = btnReject.Left - btnPass.Width - 10;


        }




















        private void LoadData()
        {
            try
            {
                // 打开数据库连接
                connection.Open();

                // 执行 SQL 查询
                SqlCommand command = new SqlCommand("SELECT time, userName, companyName, email FROM organizer_unaudited", connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // 设置 DataGridView 控件的 AutoGenerateColumns 为 false
                dgv.AutoGenerateColumns = false;

                // 为每个 DataGridViewColumn 指定 DataPropertyName
                dgv.Columns["Column1"].DataPropertyName = "time";
                dgv.Columns["Column2"].DataPropertyName = "userName";
                dgv.Columns["Column3"].DataPropertyName = "companyName";
                dgv.Columns["Column4"].DataPropertyName = "email";

                // 将 DataTable 设置为 BindingSource 的数据源
                bindingSource.DataSource = dataTable;
                dgv.DataSource = bindingSource;
            }
            catch (Exception ex)
            {
                MessageBox1 form = new MessageBox1("Error: " + ex.Message);
                form.ShowDialog();
                //MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                // 关闭数据库连接
                connection.Close();
            }
        }

        /* private void LoadOrgInfo()
        {
            Dao dao = new Dao();
            dao.connect();

            string sql = "select * from organizer_unaudited";
            SqlDataReader reader = dao.read(sql);

            while (reader.Read())
            {
                dgv.Rows.Add(reader[7].ToString(), reader[0].ToString(), reader[1].ToString(), reader[2].ToString());
            }

            reader.Close();
            dao.DaoClose();
        } */

        
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            if (dgv.CurrentRow == null || dgv.CurrentRow.Cells[0].Value == null)
            {
                MessageBox1 form = new MessageBox1("Please select a user first");
                form.ShowDialog();
                //MessageBox.Show("Please select a user first", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string email = dgv.CurrentRow.Cells[3].Value.ToString();

            string str = @ConfigInfo.GetDbConnectionString();

            // 准备 SQL 查询语句
            string query = $"SELECT * FROM organizer_unaudited WHERE email = '{email}'";

            // 创建连接对象和命令对象
            using (SqlConnection connection = new SqlConnection(str))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                // 打开连接
                connection.Open();

                // 执行查询
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // 检查是否有结果
                    if (reader.Read())
                    {
                        // 读取 ImageData 列的数据并转换为字节数组

                        // 现在可以使用 imageData 变量了，进行需要的操作
                        // 例如，在 PictureBox 控件中显示图像
                        // pictureBox1.Image = Image.FromStream(new MemoryStream(imageData));

                        // 获取其他列的数据示例
                        string name = reader["userName"].ToString();
                        string companyName = reader["companyName"].ToString();
                        email = reader["email"].ToString();
                        string pwd = reader["orgPwd"].ToString();
                        string secProb = reader["secProblem"].ToString();
                        string ans = reader["answer"].ToString();
                        byte[] imageData = (byte[])reader["image_data"];

                        Image image = Image.FromStream(new MemoryStream(imageData));

                        FormOrgAppDetail form = new FormOrgAppDetail();

                        form.lbUserNameText = name;
                        form.lbCompanyText = companyName;
                        form.lbEmailText = email;
                        form.lbPasswordText = pwd;
                        form.lbSecText = secProb;
                        form.lbAnsTxt = ans;
                        form.ImageSource = image;

                        form.ShowDialog();
                        // 获取其他列的数据，以此类推
                    }


                }

            }

        }

        private void pictureBox_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
                this.Close();
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            
        }

        private void 审核组织者申请ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void btnPass_Click(object sender, EventArgs e)
        {
            if (dgv.CurrentRow == null || dgv.CurrentRow.Cells[0].Value == null)
            {
                MessageBox1 form = new MessageBox1("Please select a user first!");
                form.ShowDialog();
                //MessageBox.Show("Please select a user first!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string email = dgv.CurrentRow.Cells[3].Value.ToString();

            string str = @ConfigInfo.GetDbConnectionString();

            // 准备 SQL 查询语句
            string query = $"SELECT * FROM organizer_unaudited WHERE email = '{email}'";

            string name;
            string companyName;
            string pwd;
            string secProb;
            string ans;
            byte[] imageData;

            // 创建连接对象和命令对象
            using (SqlConnection connection = new SqlConnection(str))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                // 打开连接
                connection.Open();

                // 执行查询
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    reader.Read();
                    // 检查是否有结果

                    // 读取 ImageData 列的数据并转换为字节数组

                    // 现在可以使用 imageData 变量了，进行需要的操作
                    // 例如，在 PictureBox 控件中显示图像
                    // pictureBox1.Image = Image.FromStream(new MemoryStream(imageData));

                    // 获取其他列的数据示例
                    name = reader["userName"].ToString();
                    companyName = reader["companyName"].ToString();
                    email = reader["email"].ToString();
                    pwd = reader["orgPwd"].ToString();
                    secProb = reader["secProblem"].ToString();
                    ans = reader["answer"].ToString();
                    imageData = (byte[])reader["image_data"];

                }
            }

            string query2 = "INSERT INTO organizer_audited (userName, companyName, email, orgPwd, secProblem, answer, image_data, state) " +
               "VALUES (@Name, @CompanyName, @Email, @Password, @SecurityQuestion, @Answer, @ImageData, 'Normal')";

            using (SqlConnection connection = new SqlConnection(str))
            using (SqlCommand command = new SqlCommand(query2, connection))
            {
                // 添加参数
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@CompanyName", companyName);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", pwd);
                command.Parameters.AddWithValue("@SecurityQuestion", secProb);
                command.Parameters.AddWithValue("@Answer", ans);
                command.Parameters.AddWithValue("@ImageData", imageData);

                // 打开连接并执行插入操作
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox1 form = new MessageBox1("Approved successfully!");
                    form.ShowDialog();
                    //MessageBox.Show("Approved successfully!");
                }
            }

            // 准备 SQL 删除语句，用于删除指定邮箱的数据行
            string deleteQuery = $"DELETE FROM organizer_unaudited WHERE email = '{email}'";

            // 创建连接对象和命令对象
            using (SqlConnection connection = new SqlConnection(str))
            using (SqlCommand command = new SqlCommand(deleteQuery, connection))
            {
                try
                {
                    // 打开连接
                    connection.Open();

                    // 执行删除命令
                    int rowsAffected = command.ExecuteNonQuery();

                    /*if (rowsAffected > 0)
                    {
                        MessageBox.Show("数据删除成功!");
                    }
                    else
                    {
                        MessageBox.Show("未找到与该邮箱相关的数据行.");
                    }*/
                }
                catch (Exception ex)
                {
                    MessageBox1 form = new MessageBox1("Error while deleting data: " + ex.Message);
                    form.ShowDialog();
                    //MessageBox.Show("Error while deleting data: " + ex.Message);
                }
            }

            LoadData();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnReject_Click(object sender, EventArgs e)
        {
            if (dgv.CurrentRow == null || dgv.CurrentRow.Cells[0].Value == null)
            {
                MessageBox1 form = new MessageBox1("Please select a user first!");
                form.ShowDialog();
                //MessageBox.Show("Please select a user first!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string email = dgv.CurrentRow.Cells[3].Value.ToString();

            string str = @ConfigInfo.GetDbConnectionString();

            // 准备 SQL 删除语句，用于删除指定邮箱的数据行
            string deleteQuery = $"DELETE FROM organizer_unaudited WHERE email = '{email}'";

            // 创建连接对象和命令对象
            using (SqlConnection connection = new SqlConnection(str))
            using (SqlCommand command = new SqlCommand(deleteQuery, connection))
            {
                try
                {
                    // 打开连接
                    connection.Open();

                    // 执行删除命令
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox1 form = new MessageBox1("Reject successfully!");
                        form .ShowDialog();
                        //MessageBox.Show("Reject successfully!");
                    }
                    /*else
                    {
                        MessageBox.Show("未找到与该邮箱相关的数据行.");
                    }*/
                }
                catch (Exception ex)
                {
                    MessageBox1 form = new MessageBox1("Error while deleting data:" + ex.Message);
                    form .ShowDialog();
                    //MessageBox.Show("Error while deleting data:" + ex.Message);
                }
            }

            LoadData();
        }
    }
}
