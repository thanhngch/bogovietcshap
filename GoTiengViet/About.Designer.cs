namespace BoGoViet
{
    partial class About
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
            this.gioithieu = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // gioithieu
            // 
            this.gioithieu.BackColor = System.Drawing.SystemColors.Control;
            this.gioithieu.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gioithieu.Location = new System.Drawing.Point(61, 40);
            this.gioithieu.Name = "gioithieu";
            this.gioithieu.ReadOnly = true;
            this.gioithieu.Size = new System.Drawing.Size(388, 200);
            this.gioithieu.TabIndex = 2;
            this.gioithieu.Text = "Chương trình gõ Tiếng Việt cho Windows sử dụng .Net Framework\nTác giả: Nguyễn Chí" +
    " Thanh\nWebsite: https://bogoviet.com\nLiên hệ: thanhngch91@gmail.com\nPhiên bản: 1" +
    ".9.0\nNgày cập nhật: 18/06/2017";
            // 
            // About
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 298);
            this.Controls.Add(this.gioithieu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "About";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Giới thiệu";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.RichTextBox gioithieu;
    }
}