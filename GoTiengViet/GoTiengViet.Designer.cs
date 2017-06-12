namespace GotiengVietApplication
{
    partial class GotiengVietForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GotiengVietForm));
            this.labelChuyenKieuGo = new System.Windows.Forms.Label();
            this.bogoviet = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.giớiThiệuToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.giớiThiệuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.thoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chkStartUp = new System.Windows.Forms.CheckBox();
            this.selectTypeInput = new System.Windows.Forms.ComboBox();
            this.buttonDong = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelChuyenKieuGo
            // 
            this.labelChuyenKieuGo.AutoSize = true;
            this.labelChuyenKieuGo.Location = new System.Drawing.Point(54, 156);
            this.labelChuyenKieuGo.Name = "labelChuyenKieuGo";
            this.labelChuyenKieuGo.Size = new System.Drawing.Size(391, 25);
            this.labelChuyenKieuGo.TabIndex = 0;
            this.labelChuyenKieuGo.Text = "Alt + Z để chuyển đổi kiểu gõ Việt - Anh";
            // 
            // bogoviet
            // 
            this.bogoviet.ContextMenuStrip = this.contextMenuStrip;
            this.bogoviet.Icon = ((System.Drawing.Icon)(resources.GetObject("bogoviet.Icon")));
            this.bogoviet.Text = "Bộ Gõ Việt (Bật)";
            this.bogoviet.Visible = true;
            this.bogoviet.MouseClick += new System.Windows.Forms.MouseEventHandler(this.bogoviet_MouseClick);
            this.bogoviet.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.giớiThiệuToolStripMenuItem1,
            this.giớiThiệuToolStripMenuItem,
            this.thoToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(243, 112);
            // 
            // giớiThiệuToolStripMenuItem1
            // 
            this.giớiThiệuToolStripMenuItem1.Name = "giớiThiệuToolStripMenuItem1";
            this.giớiThiệuToolStripMenuItem1.Size = new System.Drawing.Size(242, 36);
            this.giớiThiệuToolStripMenuItem1.Text = "Giới thiệu";
            this.giớiThiệuToolStripMenuItem1.Click += new System.EventHandler(this.giớiThiệuToolStripMenuItem1_Click);
            // 
            // giớiThiệuToolStripMenuItem
            // 
            this.giớiThiệuToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.giớiThiệuToolStripMenuItem.Name = "giớiThiệuToolStripMenuItem";
            this.giớiThiệuToolStripMenuItem.Size = new System.Drawing.Size(242, 36);
            this.giớiThiệuToolStripMenuItem.Text = "Chương trình";
            this.giớiThiệuToolStripMenuItem.Click += new System.EventHandler(this.giớiThiệuToolStripMenuItem_Click);
            // 
            // thoToolStripMenuItem
            // 
            this.thoToolStripMenuItem.Name = "thoToolStripMenuItem";
            this.thoToolStripMenuItem.Size = new System.Drawing.Size(242, 36);
            this.thoToolStripMenuItem.Text = "Thoát";
            this.thoToolStripMenuItem.Click += new System.EventHandler(this.thoToolStripMenuItem_Click);
            // 
            // chkStartUp
            // 
            this.chkStartUp.AutoSize = true;
            this.chkStartUp.Checked = true;
            this.chkStartUp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkStartUp.Location = new System.Drawing.Point(107, 238);
            this.chkStartUp.Name = "chkStartUp";
            this.chkStartUp.Size = new System.Drawing.Size(287, 29);
            this.chkStartUp.TabIndex = 6;
            this.chkStartUp.Text = "Khởi động cùng Windows";
            this.chkStartUp.UseVisualStyleBackColor = true;
            this.chkStartUp.CheckedChanged += new System.EventHandler(this.chkStartUp_CheckedChanged);
            // 
            // selectTypeInput
            // 
            this.selectTypeInput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.selectTypeInput.FormattingEnabled = true;
            this.selectTypeInput.Items.AddRange(new object[] {
            "Telex",
            "VNI",
            "VIQR"});
            this.selectTypeInput.Location = new System.Drawing.Point(251, 77);
            this.selectTypeInput.Name = "selectTypeInput";
            this.selectTypeInput.Size = new System.Drawing.Size(121, 33);
            this.selectTypeInput.TabIndex = 8;
            this.selectTypeInput.SelectedIndexChanged += new System.EventHandler(this.selectTypeInput_SelectedIndexChanged);
            // 
            // buttonDong
            // 
            this.buttonDong.Location = new System.Drawing.Point(156, 312);
            this.buttonDong.Name = "buttonDong";
            this.buttonDong.Size = new System.Drawing.Size(177, 53);
            this.buttonDong.TabIndex = 9;
            this.buttonDong.Text = "Đóng";
            this.buttonDong.UseVisualStyleBackColor = true;
            this.buttonDong.Click += new System.EventHandler(this.buttonDong_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(132, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 25);
            this.label1.TabIndex = 11;
            this.label1.Text = "Kiểu gõ";
            // 
            // GotiengVietForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 395);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonDong);
            this.Controls.Add(this.selectTypeInput);
            this.Controls.Add(this.chkStartUp);
            this.Controls.Add(this.labelChuyenKieuGo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "GotiengVietForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bộ Gõ Việt";
            this.Load += new System.EventHandler(this.GotiengVietForm_Load);
            this.Move += new System.EventHandler(this.Form1_Move);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelChuyenKieuGo;
        private System.Windows.Forms.NotifyIcon bogoviet;
        private System.Windows.Forms.CheckBox chkStartUp;
        private System.Windows.Forms.ComboBox selectTypeInput;
        private System.Windows.Forms.Button buttonDong;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem thoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem giớiThiệuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem giớiThiệuToolStripMenuItem1;
        private System.Windows.Forms.Label label1;
    }
}

