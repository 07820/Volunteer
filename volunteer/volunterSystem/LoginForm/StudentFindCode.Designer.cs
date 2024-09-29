namespace volunterSystem
{
    partial class StudentFindCode
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StudentFindCode));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TextStudentID = new System.Windows.Forms.TextBox();
            this.TextStudentEmail = new System.Windows.Forms.TextBox();
            this.btnStudent = new System.Windows.Forms.RadioButton();
            this.btnOrganizer = new System.Windows.Forms.RadioButton();
            this.btnReturn = new System.Windows.Forms.Button();
            this.btnFinish = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(105, 143);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(231, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Student ID/User ID:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(174, 246);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 29);
            this.label2.TabIndex = 1;
            this.label2.Text = "Email:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // TextStudentID
            // 
            this.TextStudentID.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextStudentID.Location = new System.Drawing.Point(434, 143);
            this.TextStudentID.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TextStudentID.Name = "TextStudentID";
            this.TextStudentID.Size = new System.Drawing.Size(265, 35);
            this.TextStudentID.TabIndex = 2;
            this.TextStudentID.TextChanged += new System.EventHandler(this.TextStudentID_TextChanged);
            // 
            // TextStudentEmail
            // 
            this.TextStudentEmail.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextStudentEmail.Location = new System.Drawing.Point(434, 240);
            this.TextStudentEmail.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TextStudentEmail.Name = "TextStudentEmail";
            this.TextStudentEmail.Size = new System.Drawing.Size(265, 35);
            this.TextStudentEmail.TabIndex = 3;
            // 
            // btnStudent
            // 
            this.btnStudent.AutoSize = true;
            this.btnStudent.Checked = true;
            this.btnStudent.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStudent.Image = ((System.Drawing.Image)(resources.GetObject("btnStudent.Image")));
            this.btnStudent.Location = new System.Drawing.Point(191, 338);
            this.btnStudent.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnStudent.Name = "btnStudent";
            this.btnStudent.Size = new System.Drawing.Size(81, 50);
            this.btnStudent.TabIndex = 4;
            this.btnStudent.TabStop = true;
            this.btnStudent.UseVisualStyleBackColor = true;
            this.btnStudent.CheckedChanged += new System.EventHandler(this.btnStudent_CheckedChanged);
            // 
            // btnOrganizer
            // 
            this.btnOrganizer.AutoSize = true;
            this.btnOrganizer.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOrganizer.Image = ((System.Drawing.Image)(resources.GetObject("btnOrganizer.Image")));
            this.btnOrganizer.Location = new System.Drawing.Point(476, 338);
            this.btnOrganizer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnOrganizer.Name = "btnOrganizer";
            this.btnOrganizer.Size = new System.Drawing.Size(81, 50);
            this.btnOrganizer.TabIndex = 5;
            this.btnOrganizer.TabStop = true;
            this.btnOrganizer.UseVisualStyleBackColor = true;
            this.btnOrganizer.CheckedChanged += new System.EventHandler(this.btnOrganizer_CheckedChanged);
            // 
            // btnReturn
            // 
            this.btnReturn.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReturn.Location = new System.Drawing.Point(210, 433);
            this.btnReturn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(134, 55);
            this.btnReturn.TabIndex = 6;
            this.btnReturn.Text = "Return";
            this.btnReturn.UseVisualStyleBackColor = true;
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // btnFinish
            // 
            this.btnFinish.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFinish.Location = new System.Drawing.Point(516, 433);
            this.btnFinish.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(134, 55);
            this.btnFinish.TabIndex = 7;
            this.btnFinish.Text = "Finish";
            this.btnFinish.UseVisualStyleBackColor = true;
            this.btnFinish.Click += new System.EventHandler(this.btnFinish_Click);
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(807, 79);
            this.panel1.TabIndex = 8;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, -12);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(489, 92);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // StudentFindCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(807, 540);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnFinish);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.btnOrganizer);
            this.Controls.Add(this.btnStudent);
            this.Controls.Add(this.TextStudentEmail);
            this.Controls.Add(this.TextStudentID);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "StudentFindCode";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Find Password";
            this.Load += new System.EventHandler(this.StudentFindCode_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TextStudentEmail;
        private System.Windows.Forms.TextBox TextStudentID;
        private System.Windows.Forms.RadioButton btnStudent;
        private System.Windows.Forms.RadioButton btnOrganizer;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.Button btnFinish;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}