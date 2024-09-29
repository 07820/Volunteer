using Config;
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

namespace volunterSystem
{
    public partial class FormAllStudents : System.Windows.Forms.Form
    {
        // 声明类的成员变量来存储DataTable
        private DataTable dataTable;
        public FormAllStudents()
        {
            InitializeComponent();
            // 订阅 CellFormatting 事件
            this.dataGridView1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView1_CellFormatting);

            // 绑定事件处理器
            this.Load += FormAllStudents_Load;
            this.Resize += FormAllStudents_Resize;
        }



        private void FormAllStudents_Load(object sender, EventArgs e)
        {
            // 设置 AutoGenerateColumns 为 false
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns["Column1"].DataPropertyName = "stuName";
            dataGridView1.Columns["Column2"].DataPropertyName = "stuId";
            dataGridView1.Columns["Column3"].DataPropertyName = "registerStatus";
            LoadDataIntoDataGridView();





            comboBox1.Left = this.ClientSize.Width - comboBox1.Width - 8;

            comboBox1.Top = 8;

            button4.Left = comboBox1.Left - button4.Width - 8;

            button4.Top = comboBox1.Top + comboBox1.Height / 2 - button4.Height / 2;


            dataGridView1.Left = 8;

            dataGridView1.Top = button4.Bottom + 8;

            dataGridView1.Width = this.ClientSize.Width - 16;

            dataGridView1.Height = this.ClientSize.Height - dataGridView1.Top - 60;

            button3.Top = dataGridView1.Bottom + 30 - button3.Height / 2;

            button3.Left = 20;

            button2.Top = button3.Top;

            button2.Left = this.ClientSize.Width - button2.Width - 20;

            button1.Top = button2.Top;

            button1.Left = button2.Left - button1.Width - 10;


        }



        private void FormAllStudents_Resize(object sender, EventArgs e)
        {


            comboBox1.Left = this.ClientSize.Width - comboBox1.Width - 8;

            comboBox1.Top = 8;

            button4.Left = comboBox1.Left - button4.Width - 8;

            button4.Top = comboBox1.Top + comboBox1.Height / 2 - button4.Height / 2;


            dataGridView1.Left = 8;

            dataGridView1.Top = button4.Bottom + 8;

            dataGridView1.Width = this.ClientSize.Width - 16;

            dataGridView1.Height = this.ClientSize.Height - dataGridView1.Top - 60;

            button3.Top = dataGridView1.Bottom + 30 - button3.Height / 2;

            button3.Left = 20;

            button2.Top = button3.Top;

            button2.Left = this.ClientSize.Width - button2.Width - 20;

            button1.Top = button2.Top;

            button1.Left = button2.Left - button1.Width - 10;


        }





















        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // 假设 "Column3" 是 registerStatus 对应的列名称
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Column3" && e.RowIndex >= 0)
            {
                if (e.Value != null && !string.IsNullOrEmpty(e.Value.ToString()))
                {
                    // 仅当e.Value确实是一个表示状态的字符串时才进行转换
                    e.Value = e.Value.ToString() == "0" ? "No" : "Yes";
                    e.FormattingApplied = true; // 表明格式化已应用
                }
                else
                {
                    // 对于空或无效的值，可以选择不显示任何内容或显示特定文本
                    e.Value = ""; // 或者 e.Value = "N/A";
                    e.FormattingApplied = true; // 也标记为格式化已应用
                }
            }
        }

        public void LoadDataIntoDataGridView(string studentName = null, string studentId = null)
        {
            string connectionString = ConfigInfo.GetDbConnectionString();
            string query = "SELECT stuName, stuId, registerStatus FROM student_already WHERE 1 = 1 "; // 注意这里的 WHERE 1 = 1，它允许我们更容易地添加后续条件

            // 创建并填充 DataTable
            dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                // 添加参数化查询，防止 SQL 注入
                if (!string.IsNullOrEmpty(studentName))
                {
                    command.Parameters.Add(new SqlParameter("@StudentName", $"%{studentName}%"));
                    query += " AND stuName LIKE @StudentName";
                }
                if (!string.IsNullOrEmpty(studentId))
                {
                    command.Parameters.Add(new SqlParameter("@StudentId", $"%{studentId}%"));
                    query += " AND stuId LIKE @StudentId";
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

        

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null || dataGridView1.CurrentRow.Cells["Column2"].Value == null)
            {
                MessageBox1 form = new MessageBox1("Please select a row first");
                form.ShowDialog();
                //MessageBox.Show("Please select a row first", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            using (MessageBox2 form = new MessageBox2("Are you sure you want to delete this student?"))
            {
                // 订阅事件
                form.RequestCloseMainForm += (senderObj, args) =>
                {
                    return;
                };

                // 显示窗体作为模态对话框
                form.ShowDialog();
            }

            /*DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this student?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult != DialogResult.Yes)
            {
                return; // 如果用户选择“No”，则不继续执行删除操作
            }*/

            string studentId = dataGridView1.CurrentRow.Cells["Column2"].Value.ToString();
            string connectionString = ConfigInfo.GetDbConnectionString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "Delete from student_information WHERE stuId = @StuID;" +
                             "Delete from student_already WHERE stuId = @StuID";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@StuID", studentId);

                    try
                    {
                        connection.Open();
                        int result = command.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox1 form = new MessageBox1("Delete student successfully!");
                            form.ShowDialog();
                            //MessageBox.Show("Delete student successfully!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void AddStudentForm_StudentAdded(object sender, EventArgs e)
        {
            // 刷新显示学生信息的数据源，例如 DataGridView
            LoadDataIntoDataGridView();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var form = new FormAddStu();
            form.StudentAdded += AddStudentForm_StudentAdded;
            form.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoadDataIntoDataGridView();
        }

        public void SortDataIntoDataGridView(string sortField, string sortOrder = "ASC")
        {
            if (dataTable != null)
            {
                string actualSortField;
                switch (sortField)
                {
                    case "Name":
                        actualSortField = "stuName";
                        break;
                    case "ID":
                        actualSortField = "stuId";
                        break;
                    case "Status":
                        actualSortField = "registerStatus";
                        break;
                    default:
                        actualSortField = "stuName"; // 默认排序字段
                        break;
                }
                dataTable.DefaultView.Sort = $"{actualSortField} {sortOrder}";
                dataGridView1.DataSource = dataTable.DefaultView;
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedSortOption = comboBox1.SelectedItem.ToString();
            string sortField;
            switch (selectedSortOption)
            {
                case "Name":
                    sortField = "Name";
                    break;
                case "ID":
                    sortField = "ID";
                    break;
                case "Status":
                    sortField = "Status";
                    break;
                default:
                    sortField = "Name"; // 默认排序选项
                    break;
            }
            SortDataIntoDataGridView(sortField, "ASC");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SearchAllStu searchAllStu = new SearchAllStu(this);
            searchAllStu.Show();
        }
    }
}
