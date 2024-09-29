namespace volunterSystem
{
    partial class FormLogin
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogin));
            this.label1 = new System.Windows.Forms.Label();
            this.txtId = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtPwd = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnLogin = new System.Windows.Forms.Button();
            this.rbtUser = new System.Windows.Forms.RadioButton();
            this.rbtOrg = new System.Windows.Forms.RadioButton();
            this.rbtAdmin = new System.Windows.Forms.RadioButton();
            this.linkFindPwd = new System.Windows.Forms.LinkLabel();
            this.userRegister = new System.Windows.Forms.LinkLabel();
            this.orgRegister = new System.Windows.Forms.LinkLabel();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(172, 162);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(165, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "ID/Username:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // txtId
            // 
            this.txtId.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtId.Location = new System.Drawing.Point(387, 157);
            this.txtId.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtId.Name = "txtId";
            this.txtId.Size = new System.Drawing.Size(234, 37);
            this.txtId.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Impact", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(426, 104);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(152, 41);
            this.label7.TabIndex = 2;
            this.label7.Text = "Welcome";
            this.label7.Click += new System.EventHandler(this.label2_Click);
            // 
            // txtPwd
            // 
            this.txtPwd.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtPwd.Location = new System.Drawing.Point(387, 247);
            this.txtPwd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.Size = new System.Drawing.Size(234, 37);
            this.txtPwd.TabIndex = 4;
            this.txtPwd.UseSystemPasswordChar = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(214, 247);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(133, 29);
            this.label3.TabIndex = 3;
            this.label3.Text = "Password:";
            // 
            // btnLogin
            // 
            this.btnLogin.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogin.Location = new System.Drawing.Point(217, 410);
            this.btnLogin.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(135, 53);
            this.btnLogin.TabIndex = 5;
            this.btnLogin.Text = "Confirm";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // rbtUser
            // 
            this.rbtUser.AutoSize = true;
            this.rbtUser.Checked = true;
            this.rbtUser.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtUser.Image = ((System.Drawing.Image)(resources.GetObject("rbtUser.Image")));
            this.rbtUser.Location = new System.Drawing.Point(151, 343);
            this.rbtUser.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rbtUser.Name = "rbtUser";
            this.rbtUser.Size = new System.Drawing.Size(81, 50);
            this.rbtUser.TabIndex = 7;
            this.rbtUser.TabStop = true;
            this.rbtUser.UseVisualStyleBackColor = true;
            this.rbtUser.CheckedChanged += new System.EventHandler(this.rbtUser_CheckedChanged);
            // 
            // rbtOrg
            // 
            this.rbtOrg.AutoSize = true;
            this.rbtOrg.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtOrg.Image = ((System.Drawing.Image)(resources.GetObject("rbtOrg.Image")));
            this.rbtOrg.Location = new System.Drawing.Point(369, 343);
            this.rbtOrg.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rbtOrg.Name = "rbtOrg";
            this.rbtOrg.Size = new System.Drawing.Size(81, 50);
            this.rbtOrg.TabIndex = 8;
            this.rbtOrg.UseVisualStyleBackColor = true;
            this.rbtOrg.CheckedChanged += new System.EventHandler(this.rbtOrg_CheckedChanged);
            // 
            // rbtAdmin
            // 
            this.rbtAdmin.AutoSize = true;
            this.rbtAdmin.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtAdmin.Image = ((System.Drawing.Image)(resources.GetObject("rbtAdmin.Image")));
            this.rbtAdmin.Location = new System.Drawing.Point(584, 343);
            this.rbtAdmin.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rbtAdmin.Name = "rbtAdmin";
            this.rbtAdmin.Size = new System.Drawing.Size(68, 50);
            this.rbtAdmin.TabIndex = 9;
            this.rbtAdmin.UseVisualStyleBackColor = true;
            this.rbtAdmin.CheckedChanged += new System.EventHandler(this.rbtAdmin_CheckedChanged);
            // 
            // linkFindPwd
            // 
            this.linkFindPwd.AutoSize = true;
            this.linkFindPwd.BackColor = System.Drawing.Color.Transparent;
            this.linkFindPwd.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkFindPwd.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkFindPwd.LinkColor = System.Drawing.Color.LightSeaGreen;
            this.linkFindPwd.Location = new System.Drawing.Point(29, 6);
            this.linkFindPwd.Name = "linkFindPwd";
            this.linkFindPwd.Size = new System.Drawing.Size(207, 29);
            this.linkFindPwd.TabIndex = 10;
            this.linkFindPwd.TabStop = true;
            this.linkFindPwd.Text = "Forget password";
            this.linkFindPwd.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // userRegister
            // 
            this.userRegister.AutoSize = true;
            this.userRegister.BackColor = System.Drawing.Color.Transparent;
            this.userRegister.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userRegister.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.userRegister.LinkColor = System.Drawing.Color.LightSeaGreen;
            this.userRegister.Location = new System.Drawing.Point(299, 6);
            this.userRegister.Name = "userRegister";
            this.userRegister.Size = new System.Drawing.Size(250, 29);
            this.userRegister.TabIndex = 11;
            this.userRegister.TabStop = true;
            this.userRegister.Text = "Student Registration";
            this.userRegister.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.userRegister_LinkClicked);
            // 
            // orgRegister
            // 
            this.orgRegister.AutoSize = true;
            this.orgRegister.BackColor = System.Drawing.Color.Transparent;
            this.orgRegister.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.orgRegister.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.orgRegister.LinkColor = System.Drawing.Color.LightSeaGreen;
            this.orgRegister.Location = new System.Drawing.Point(623, 6);
            this.orgRegister.Name = "orgRegister";
            this.orgRegister.Size = new System.Drawing.Size(271, 29);
            this.orgRegister.TabIndex = 12;
            this.orgRegister.TabStop = true;
            this.orgRegister.Text = "Organizer Registration";
            this.orgRegister.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.orgRegister_LinkClicked);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.Location = new System.Drawing.Point(584, 410);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(143, 53);
            this.btnExit.TabIndex = 13;
            this.btnExit.Text = "Quit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(939, 79);
            this.panel1.TabIndex = 14;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, -12);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(524, 88);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Menu;
            this.panel2.Controls.Add(this.linkFindPwd);
            this.panel2.Controls.Add(this.userRegister);
            this.panel2.Controls.Add(this.orgRegister);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 553);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(939, 48);
            this.panel2.TabIndex = 15;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(238, 358);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 24);
            this.label2.TabIndex = 16;
            this.label2.Text = "Student";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(446, 358);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 24);
            this.label4.TabIndex = 17;
            this.label4.Text = "Organizer";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(658, 358);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(138, 24);
            this.label5.TabIndex = 18;
            this.label5.Text = "Administrator";
            // 
            // FormLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(939, 601);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.rbtAdmin);
            this.Controls.Add(this.rbtOrg);
            this.Controls.Add(this.rbtUser);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.txtPwd);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtId);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FormLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Volunteers";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtPwd;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.RadioButton rbtUser;
        private System.Windows.Forms.RadioButton rbtOrg;
        private System.Windows.Forms.RadioButton rbtAdmin;
        private System.Windows.Forms.LinkLabel linkFindPwd;
        private System.Windows.Forms.LinkLabel userRegister;
        private System.Windows.Forms.LinkLabel orgRegister;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}

