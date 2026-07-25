namespace 初筛更名助手
{
    partial class HairOCRForm
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
            label1 = new Label();
            txtPhotoFolder = new TextBox();
            btnSelectPhoto = new Button();
            btnStartOCR = new Button();
            dgvResult = new DataGridView();
            colImage = new DataGridViewTextBoxColumn();
            colName = new DataGridViewTextBoxColumn();
            colID = new DataGridViewTextBoxColumn();
            colTag = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            btnExport = new Button();
            picPreview = new PictureBox();
            pbPhoto = new PictureBox();
            btnDrawArea = new Button();
            btnRotateLeft = new Button();
            btnRotateRight = new Button();
            btnSaveArea = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvResult).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picPreview).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbPhoto).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 27);
            label1.Name = "label1";
            label1.Size = new Size(80, 17);
            label1.TabIndex = 0;
            label1.Text = "照片文件夹：";
            // 
            // txtPhotoFolder
            // 
            txtPhotoFolder.Location = new Point(12, 47);
            txtPhotoFolder.Name = "txtPhotoFolder";
            txtPhotoFolder.Size = new Size(293, 23);
            txtPhotoFolder.TabIndex = 1;
            // 
            // btnSelectPhoto
            // 
            btnSelectPhoto.Location = new Point(332, 47);
            btnSelectPhoto.Name = "btnSelectPhoto";
            btnSelectPhoto.Size = new Size(92, 23);
            btnSelectPhoto.TabIndex = 2;
            btnSelectPhoto.Text = "选择文件夹";
            btnSelectPhoto.UseVisualStyleBackColor = true;
            btnSelectPhoto.Click += btnSelectPhoto_Click;
            // 
            // btnStartOCR
            // 
            btnStartOCR.Location = new Point(249, 107);
            btnStartOCR.Name = "btnStartOCR";
            btnStartOCR.Size = new Size(98, 23);
            btnStartOCR.TabIndex = 3;
            btnStartOCR.Text = "开始识别";
            btnStartOCR.UseVisualStyleBackColor = true;
            btnStartOCR.Click += btnStartOCR_Click;
            // 
            // dgvResult
            // 
            dgvResult.AllowUserToAddRows = false;
            dgvResult.AllowUserToDeleteRows = false;
            dgvResult.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvResult.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvResult.Columns.AddRange(new DataGridViewColumn[] { colImage, colName, colID, colTag, colStatus });
            dgvResult.Location = new Point(12, 642);
            dgvResult.Name = "dgvResult";
            dgvResult.ReadOnly = true;
            dgvResult.Size = new Size(950, 300);
            dgvResult.TabIndex = 4;
            dgvResult.CellClick += dgvResult_CellClick;
            dgvResult.CellContentClick += dgvResult_CellContentClick;
            // 
            // colImage
            // 
            colImage.HeaderText = "图片名称";
            colImage.Name = "colImage";
            colImage.ReadOnly = true;
            // 
            // colName
            // 
            colName.HeaderText = "姓名";
            colName.Name = "colName";
            colName.ReadOnly = true;
            // 
            // colID
            // 
            colID.HeaderText = "体检号";
            colID.Name = "colID";
            colID.ReadOnly = true;
            // 
            // colTag
            // 
            colTag.HeaderText = "样本标签";
            colTag.Name = "colTag";
            colTag.ReadOnly = true;
            // 
            // colStatus
            // 
            colStatus.HeaderText = "识别状态";
            colStatus.Name = "colStatus";
            colStatus.ReadOnly = true;
            // 
            // btnExport
            // 
            btnExport.Location = new Point(1072, 712);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(75, 23);
            btnExport.TabIndex = 5;
            btnExport.Text = "导出Excel";
            btnExport.UseVisualStyleBackColor = true;
            // 
            // picPreview
            // 
            picPreview.Location = new Point(668, 80);
            picPreview.Name = "picPreview";
            picPreview.Size = new Size(100, 50);
            picPreview.SizeMode = PictureBoxSizeMode.Zoom;
            picPreview.TabIndex = 6;
            picPreview.TabStop = false;
            // 
            // pbPhoto
            // 
            pbPhoto.Location = new Point(12, 136);
            pbPhoto.Name = "pbPhoto";
            pbPhoto.Size = new Size(950, 500);
            pbPhoto.SizeMode = PictureBoxSizeMode.Zoom;
            pbPhoto.TabIndex = 7;
            pbPhoto.TabStop = false;
            pbPhoto.Paint += pbPhoto_Paint;
            pbPhoto.MouseDown += pbPhoto_MouseDown;
            pbPhoto.MouseMove += pbPhoto_MouseMove;
            pbPhoto.MouseUp += pbPhoto_MouseUp;
            // 
            // btnDrawArea
            // 
            btnDrawArea.Location = new Point(383, 107);
            btnDrawArea.Name = "btnDrawArea";
            btnDrawArea.Size = new Size(89, 23);
            btnDrawArea.TabIndex = 8;
            btnDrawArea.Text = "绘制识别区域";
            btnDrawArea.UseVisualStyleBackColor = true;
            btnDrawArea.Click += btnDrawArea_Click;
            // 
            // btnRotateLeft
            // 
            btnRotateLeft.Location = new Point(40, 107);
            btnRotateLeft.Name = "btnRotateLeft";
            btnRotateLeft.Size = new Size(75, 23);
            btnRotateLeft.TabIndex = 9;
            btnRotateLeft.Text = "左旋90°";
            btnRotateLeft.UseVisualStyleBackColor = true;
            btnRotateLeft.Click += btnRotateLeft_Click;
            // 
            // btnRotateRight
            // 
            btnRotateRight.Location = new Point(148, 107);
            btnRotateRight.Name = "btnRotateRight";
            btnRotateRight.Size = new Size(75, 23);
            btnRotateRight.TabIndex = 10;
            btnRotateRight.Text = "右旋90°";
            btnRotateRight.UseVisualStyleBackColor = true;
            btnRotateRight.Click += btnRotateRight_Click;
            // 
            // btnSaveArea
            // 
            btnSaveArea.Location = new Point(501, 107);
            btnSaveArea.Name = "btnSaveArea";
            btnSaveArea.Size = new Size(94, 23);
            btnSaveArea.TabIndex = 11;
            btnSaveArea.Text = "保存识别区域";
            btnSaveArea.UseVisualStyleBackColor = true;
            btnSaveArea.Click += btnSaveArea_Click;
            // 
            // HairOCRForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1565, 775);
            Controls.Add(btnSaveArea);
            Controls.Add(btnRotateRight);
            Controls.Add(btnRotateLeft);
            Controls.Add(btnDrawArea);
            Controls.Add(pbPhoto);
            Controls.Add(picPreview);
            Controls.Add(btnExport);
            Controls.Add(dgvResult);
            Controls.Add(btnStartOCR);
            Controls.Add(btnSelectPhoto);
            Controls.Add(txtPhotoFolder);
            Controls.Add(label1);
            Name = "HairOCRForm";
            Text = "毛发初筛样本识别";
            WindowState = FormWindowState.Maximized;
            Load += HairOCRForm_Load;
            ((System.ComponentModel.ISupportInitialize)dgvResult).EndInit();
            ((System.ComponentModel.ISupportInitialize)picPreview).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbPhoto).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtPhotoFolder;
        private Button btnSelectPhoto;
        private Button btnStartOCR;
        private DataGridView dgvResult;
        private Button btnExport;
        private PictureBox picPreview;
        private PictureBox pbPhoto;
        private Button btnDrawArea;
        private Button btnRotateLeft;
        private Button btnRotateRight;
        private DataGridViewTextBoxColumn colImage;
        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colID;
        private DataGridViewTextBoxColumn colTag;
        private DataGridViewTextBoxColumn colStatus;
        private Button btnSaveArea;
    }
}