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
    public partial class FormOrgInfo : System.Windows.Forms.Form
    {
        // 声明类的成员变量来存储DataTable
        private DataTable dataTable;
        public FormOrgInfo()
        {
            InitializeComponent();

            // 绑定事件处理器
            this.Load += FormOrgInfo_Load;
            this.Resize += FormOrgInfo_Resize;
        }



        private void FormOrgInfo_Load(object sender, EventArgs e)
        {
            // 设置 AutoGenerateColumns 为 false
            dataGridView1.AutoGenerateColumns = false;

            // 手动设置每列的 DataPropertyName
            dataGridView1.Columns["Column1"].DataPropertyName = "userName";
            dataGridView1.Columns["Column2"].DataPropertyName = "companyName";
            dataGridView1.Columns["Column3"].DataPropertyName = "email";
            dataGridView1.Columns["Column4"].DataPropertyName = "state";

            // 加载数据到 DataGridView
            LoadDataIntoDataGridView();

            comboBox1.Left = this.ClientSize.Width - comboBox1.Width - 8;

            comboBox1.Top = 8;

            button2.Left = comboBox1.Left - button2.Width - 8;

            button2.Top = comboBox1.Top + comboBox1.Height / 2 - button2.Height / 2;


            dataGridView1.Left = 8;

            dataGridView1.Top = button2.Bottom + 8;

            dataGridView1.Width = this.ClientSize.Width - 16;

            dataGridView1.Height = this.ClientSize.Height - dataGridView1.Top - 60;


            button3.Top = dataGridView1.Bottom + 30 - button3.Height / 2;

            button3.Left = 20;


            button6.Left = button3.Right + 10;

            button6.Top = dataGridView1.Bottom + 30 - button6.Height / 2;



            button5.Top = button3.Top;

            button5.Left = this.ClientSize.Width - button5.Width - 20;

            button4.Top = button5.Top;

            button4.Left = button5.Left - button4.Width - 10;


        }


        private void FormOrgInfo_Resize(object sender, EventArgs e)
        {


            comboBox1.Left = this.ClientSize.Width - comboBox1.Width - 8;

            comboBox1.Top = 8;

            button2.Left = comboBox1.Left - button2.Width - 8;

            button2.Top = comboBox1.Top + comboBox1.Height / 2 - button2.Height / 2;


            dataGridView1.Left = 8;

            dataGridView1.Top = button2.Bottom + 8;

            dataGridView1.Width = this.ClientSize.Width - 16;

            dataGridView1.Height = this.ClientSize.Height - dataGridView1.Top - 60;


            button3.Top = dataGridView1.Bottom + 30 - button3.Height / 2;

            button3.Left = 20;


            button6.Left = button3.Right + 10;

            button6.Top = dataGridView1.Bottom + 30 - button6.Height / 2;


            button5.Top = button3.Top;

            button5.Left = this.ClientSize.Width - button5.Width - 20;

            button4.Top = button5.Top;

            button4.Left = button5.Left - button4.Width - 10;


        }







        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        

        public void LoadDataIntoDataGridView(string userName = null, string companyName = null)
        {
            string connectionString = ConfigInfo.GetDbConnectionString();
            string query = "SELECT userName, companyName, email, state FROM organizer_audited WHERE 1 = 1"; // 注意这里的 WHERE 1 = 1，它允许我们更容易地添加后续条件

            // 创建并填充 DataTable
            dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                // 添加参数化查询，防止 SQL 注入
                if (!string.IsNullOrEmpty(userName))
                {
                    command.Parameters.Add(new SqlParameter("@userName", $"%{userName}%"));
                    query += " AND userName LIKE @userName";
                }
                if (!string.IsNullOrEmpty(companyName))
                {
                    command.Parameters.Add(new SqlParameter("@companyName", $"%{companyName}%"));
                    query += " AND companyName LIKE @companyName";
                }


                // 更新 command 的 CommandText 属性
                command.CommandText = query;

                // 打开数据库连接并执行查询
                try
                {
                    connection.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable); // 使用数据适配器填充 dataTable
                    }
                }
                catch (Exception ex)
                {
                    MessageBox1 form = new MessageBox1("Error: " + ex.Message);
                    form.ShowDialog();
                    //MessageBox.Show("Error: " + ex.Message);
                }
            }

            // 将 dataTable 设置为 dataGridView1 的数据源
            dataGridView1.DataSource = dataTable;
        }

        public void SortDataIntoDataGridView(string sortField, string sortOrder = "ASC")
        {
            if (dataTable != null)
            {
                string actualSortField = sortField == "Username" ? "userName" : "companyName"; // 确保这里使用的字段名与 DataTable 中的列名相匹配
                dataTable.DefaultView.Sort = $"{actualSortField} {sortOrder}";
                dataGridView1.DataSource = dataTable.DefaultView;
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            using (MessageBox2 form = new MessageBox2("Are you sure to quit?"))
            {
                // 订阅事件
                form.RequestCloseMainForm += (senderObj, args) =>
                {
                    this.Close();
                };

                // 显示窗体作为模态对话框
                form.ShowDialog();
            }

            /*if (DialogResult.Yes == MessageBox.Show("Are you sure to quit?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                this.Close();
            }*/
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null || dataGridView1.CurrentRow.Cells[0].Value == null)
            {
                MessageBox1 form = new MessageBox1("Please select a row first");
                form.ShowDialog();
                //MessageBox.Show("Please select a row first", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                // 假设您的 DataGridView 中学生 ID 在第二列
                string email = dataGridView1.CurrentRow.Cells["Column3"].Value.ToString();
                FormOrgDetail form = new FormOrgDetail(email);
                form.ShowDialog();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadDataIntoDataGridView();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 假设comboBox1有两个选项：Name和ID
            string sortField = comboBox1.SelectedItem.ToString() == "Username" ? "userName" : "companyName";
            SortDataIntoDataGridView(sortField, "ASC");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FormSearchOrg searchForm = new FormSearchOrg(this);
            searchForm.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null || dataGridView1.CurrentRow.Cells["Column1"].Value == null)
            {
                MessageBox1 form = new MessageBox1("Please select a row first");
                form.ShowDialog();
                //MessageBox.Show("Please select a row first", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            string userName = dataGridView1.CurrentRow.Cells["Column1"].Value.ToString();
            string connectionString = @ConfigInfo.GetDbConnectionString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "UPDATE organizer_audited SET state = @NewState WHERE userName = @userName";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@NewState", "Locked");
                    command.Parameters.AddWithValue("@userName", userName);

                    try
                    {
                        connection.Open();
                        int result = command.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox1 form = new MessageBox1("The organizer's status has been updated to 'Locked'.");
                            form.ShowDialog();
                            //MessageBox.Show("The organizer's status has been updated to 'Locked'.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox1 form = new MessageBox1("No records were updated.");
                            form.ShowDialog();
                            //MessageBox.Show("No records were updated.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox1 form = new MessageBox1("Error: " + ex.Message);
                        form.ShowDialog();
                        //MessageBox.Show("Error: " + ex.Message);
                    }
                }
                LoadDataIntoDataGridView();


            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null || dataGridView1.CurrentRow.Cells["Column1"].Value == null)
            {
                MessageBox1 form = new MessageBox1("Please select a row first");
                form.ShowDialog();
                //MessageBox.Show("Please select a row first", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            string userName = dataGridView1.CurrentRow.Cells["Column1"].Value.ToString();
            string connectionString = @ConfigInfo.GetDbConnectionString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "UPDATE organizer_audited SET state = @NewState WHERE userName = @userName";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@NewState", "Normal");
                    command.Parameters.AddWithValue("@userName", userName);

                    try
                    {
                        connection.Open();
                        int result = command.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox1 form = new MessageBox1("The organizer's status has been updated to 'Normal'.");
                            form.ShowDialog();
                            //MessageBox.Show("The organizer's status has been updated to 'Normal'.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox1 form = new MessageBox1("No records were updated.");
                            form.ShowDialog();
                            //MessageBox.Show("No records were updated.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            LoadDataIntoDataGridView();
        }
    }
}
