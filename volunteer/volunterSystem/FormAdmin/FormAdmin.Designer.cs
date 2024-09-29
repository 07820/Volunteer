namespace volunterSystem
{
    partial class FormAdmin
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAdmin));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.RSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AOAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allStudentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Times New Roman", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RSToolStripMenuItem,
            this.OIToolStripMenuItem,
            this.AOAToolStripMenuItem,
            this.allStudentsToolStripMenuItem,
            this.toolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1201, 34);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // RSToolStripMenuItem
            // 
            this.RSToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("RSToolStripMenuItem.Image")));
            this.RSToolStripMenuItem.Name = "RSToolStripMenuItem";
            this.RSToolStripMenuItem.Size = new System.Drawing.Size(250, 30);
            this.RSToolStripMenuItem.Text = "Registered students";
            this.RSToolStripMenuItem.Click += new System.EventHandler(this.学生信息ToolStripMenuItem_Click);
            // 
            // OIToolStripMenuItem
            // 
            this.OIToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("OIToolStripMenuItem.Image")));
            this.OIToolStripMenuItem.Name = "OIToolStripMenuItem";
            this.OIToolStripMenuItem.Size = new System.Drawing.Size(282, 30);
            this.OIToolStripMenuItem.Text = "Organizer information";
            this.OIToolStripMenuItem.Click += new System.EventHandler(this.组织者信息ToolStripMenuItem_Click);
            // 
            // AOAToolStripMenuItem
            // 
            this.AOAToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("AOAToolStripMenuItem.Image")));
            this.AOAToolStripMenuItem.Name = "AOAToolStripMenuItem";
            this.AOAToolStripMenuItem.Size = new System.Drawing.Size(348, 30);
            this.AOAToolStripMenuItem.Text = "Audit organizer applications ";
            this.AOAToolStripMenuItem.Click += new System.EventHandler(this.审核组织者申请ToolStripMenuItem_Click);
            // 
            // allStudentsToolStripMenuItem
            // 
            this.allStudentsToolStripMenuItem.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.allStudentsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("allStudentsToolStripMenuItem.Image")));
            this.allStudentsToolStripMenuItem.Name = "allStudentsToolStripMenuItem";
            this.allStudentsToolStripMenuItem.Size = new System.Drawing.Size(160, 30);
            this.allStudentsToolStripMenuItem.Text = "All students";
            this.allStudentsToolStripMenuItem.Click += new System.EventHandler(this.allStudentsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(14, 30);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.White;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(1071, 0);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(118, 30);
            this.button1.TabIndex = 3;
            this.button1.Text = "Log Out";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.Location = new System.Drawing.Point(0, 60);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1201, 521);
            this.panel1.TabIndex = 6;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // FormAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkTurquoise;
            this.ClientSize = new System.Drawing.Size(1201, 739);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FormAdmin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Administrator";
            this.Load += new System.EventHandler(this.FormAdmin_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem RSToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OIToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AOAToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem allStudentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
    }
}