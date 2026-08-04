namespace 初筛工具箱
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            label2 = new Label();
            txtFolder = new TextBox();
            btnSelect = new Button();
            btnRun = new Button();
            lblResult = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(47, 53);
            label1.Name = "label1";
            label1.Size = new Size(80, 17);
            label1.TabIndex = 0;
            label1.Text = "报告文件夹：";
            label1.Click += label1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(59, 233);
            label2.Name = "label2";
            label2.Size = new Size(68, 17);
            label2.TabIndex = 1;
            label2.Text = "处理结果：";
            // 
            // txtFolder
            // 
            txtFolder.Location = new Point(27, 99);
            txtFolder.Name = "txtFolder";
            txtFolder.Size = new Size(357, 23);
            txtFolder.TabIndex = 2;
            // 
            // btnSelect
            // 
            btnSelect.Location = new Point(401, 99);
            btnSelect.Name = "btnSelect";
            btnSelect.Size = new Size(96, 23);
            btnSelect.TabIndex = 3;
            btnSelect.Text = "选择文件夹";
            btnSelect.UseVisualStyleBackColor = true;
            btnSelect.Click += btnSelect_Click;
            // 
            // btnRun
            // 
            btnRun.Location = new Point(133, 166);
            btnRun.Name = "btnRun";
            btnRun.Size = new Size(75, 23);
            btnRun.TabIndex = 4;
            btnRun.Text = "开始处理";
            btnRun.UseVisualStyleBackColor = true;
            btnRun.Click += btnRun_Click;
            // 
            // lblResult
            // 
            lblResult.AutoSize = true;
            lblResult.Location = new Point(133, 233);
            lblResult.Name = "lblResult";
            lblResult.Size = new Size(56, 17);
            lblResult.TabIndex = 5;
            lblResult.Text = "等待处理";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(584, 361);
            Controls.Add(lblResult);
            Controls.Add(btnRun);
            Controls.Add(btnSelect);
            Controls.Add(txtFolder);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "Form1";
            Text = "初筛工具箱";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private TextBox txtFolder;
        private Button btnSelect;
        private Button btnRun;
        private Label lblResult;
    }
}
