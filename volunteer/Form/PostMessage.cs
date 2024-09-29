using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Form
{
    public partial class PostMessage : System.Windows.Forms.Form
    {
        public PostMessage()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PostMessage_Load(object sender, EventArgs e)
        {

        }
    }
}
