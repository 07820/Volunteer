using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace volunterSystem
{
    public partial class SearchAllStu : System.Windows.Forms.Form
    {
        private FormAllStudents formAllStudents;
        public SearchAllStu(FormAllStudents formAllStudents)
        {
            InitializeComponent();
            this.formAllStudents = formAllStudents;

            this.Load += SearchAllStu_Load;
            this.Resize += SearchAllStu_Resize;
        }

        private void SearchAllStu_Load(object sender, EventArgs e)
        {


            label2.Top = this.ClientSize.Height / 2 - label2.Height - 15;

            label4.Top = this.ClientSize.Height / 2 + 15;


            textBox1.Left = (this.ClientSize.Width - textBox1.Width) / 2;

            textBox1.Top = label2.Top;

            textBox2.Left = (this.ClientSize.Width - textBox2.Width) / 2;

            textBox2.Top = label4.Top;


            label2.Left = textBox1.Left - label2.Width - 8;

            label4.Left = textBox2.Left - label4.Width - 8;


            label1.Top = label2.Top - label1.Height - 30;

            label1.Left = this.ClientSize.Width / 2 - label1.Width / 2;
        }


        private void SearchAllStu_Resize(object sender, EventArgs e)
        {
            label2.Top = this.ClientSize.Height / 2 - label2.Height - 20;

            label4.Top = this.ClientSize.Height / 2 + 15;


            textBox1.Left = (this.ClientSize.Width - textBox1.Width) / 2;

            textBox1.Top = label2.Top;

            textBox2.Left = (this.ClientSize.Width - textBox2.Width) / 2;

            textBox2.Top = label4.Top;


            label2.Left = textBox1.Left - label2.Width - 8;

            label4.Left = textBox2.Left - label4.Width - 8;


            label1.Top = label2.Top - label1.Height - 30;

            label1.Left = this.ClientSize.Width / 2 - label1.Width / 2;
        }









        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string stuName = textBox1.Text.Trim();
            string id = textBox2.Text.Trim();
            formAllStudents.LoadDataIntoDataGridView(stuName, id);
            this.Close();
        }
    }
}
