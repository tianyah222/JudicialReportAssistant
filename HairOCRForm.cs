using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace 初筛更名助手
{
    public partial class HairOCRForm : Form
    {//识别区域
        Rectangle nameArea;
        Rectangle idArea;
        //条码识别区域
        Rectangle barcodeArea;

        //白色标签区域
        Rectangle labelArea;
        //自动体检号区域（暂时保留）
        string currentBox = "";
        Point mouseDownPoint;

        bool dragging = false;
        string resizeDirection = "";
        bool resizing = false;
        List<string> imageFiles = new List<string>();
        Dictionary<string, Rectangle> labelAreaList =
    new Dictionary<string, Rectangle>();

        Dictionary<string, Rectangle> barcodeAreaList =
            new Dictionary<string, Rectangle>();
        string areaFile = Path.Combine(
      Application.StartupPath,
      "area.config.txt"
  );
        public HairOCRForm()
        {
            InitializeComponent();
            InitResultTable();
            LoadAreaConfig();
        }

        private void InitResultTable()
        {
            dgvResult.Columns.Clear();

            dgvResult.Columns.Add("colImage", "图片名称");
            dgvResult.Columns.Add("colName", "姓名");
            dgvResult.Columns.Add("colBarcode", "条码号");
            dgvResult.Columns.Add("colID", "体检号");
            dgvResult.Columns.Add("colStatus", "识别状态");
            dgvResult.Columns["colImage"].Width = 180;
            dgvResult.Columns["colName"].Width = 80;
            dgvResult.Columns["colBarcode"].Width = 120;
            dgvResult.Columns["colID"].Width = 120;
            dgvResult.Columns["colStatus"].Width = 80;
            dgvResult.AllowUserToAddRows = false;
            dgvResult.ReadOnly = true;
            dgvResult.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            dgvResult.RowTemplate.Height = 25;
            dgvResult.AutoSizeRowsMode =
                DataGridViewAutoSizeRowsMode.None;
        }

        private Point PictureBoxToImage(Point p)
        {
            if (pbPhoto.Image == null)
                return p;


            float scaleX = (float)pbPhoto.Image.Width / pbPhoto.Width;
            float scaleY = (float)pbPhoto.Image.Height / pbPhoto.Height;


            return new Point(
                (int)(p.X * scaleX),
                (int)(p.Y * scaleY)
            );
        }
        private void btnStartOCR_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPhotoFolder.Text))
            {
                MessageBox.Show("请先选择照片文件夹！");
                return;
            }


            string folder = txtPhotoFolder.Text;


            List<string> files = new List<string>();

            files.AddRange(Directory.GetFiles(folder, "*.jpg"));
            files.AddRange(Directory.GetFiles(folder, "*.jpeg"));
            files.AddRange(Directory.GetFiles(folder, "*.png"));
            MessageBox.Show("实际读取图片数量：" + files.Count);
            imageFiles.Clear();
            imageFiles.AddRange(files);

            if (files.Count == 0)
            {
                MessageBox.Show("没有找到图片！");
                return;
            }


            //清空结果
            dgvResult.Rows.Clear();


            //批量加入结果表
            foreach (string file in files)
            {
                try
                {
                    BarcodeScanner scanner = new BarcodeScanner();

                    BarcodeInfo info =
                        scanner.ReadBarcode(file);

                    //识别结果
                    string status;
                    string barcode = "";


                    if (info != null)
                    {
                        //保存区域
                        barcodeArea = info.Location;
                        labelArea = info.LabelArea;

                        barcodeAreaList[file] = info.Location;
                        labelAreaList[file] = info.LabelArea;


                        barcode = info.Code;
                        status = "定位成功";
                    }
                    else
                    {
                        status = "未找到条码";
                    }


                    //所有图片都加入表格
                    dgvResult.Rows.Add(
                        Path.GetFileName(file),   //图片名称
                        "",                       //姓名
                        barcode,                  //条码号
                        "",                       //体检号
                        status                    //识别状态
                    );
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        file + "\r\n" + ex.Message
                    );
                }
            }


            //循环结束后再显示第一张图片
            if (files.Count > 0)
            {
                pbPhoto.Image = Image.FromFile(files[0]);
                pbPhoto.Refresh();
            }
        }
        private void btnSelectPhoto_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();

            dialog.Description = "请选择照片文件夹";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtPhotoFolder.Text = dialog.SelectedPath;
            }
        }

        private void btnDrawArea_Click(object sender, EventArgs e)
        {
            if (pbPhoto.Image == null)
            {
                MessageBox.Show("请先加载照片");
                return;
            }


            //根据原图尺寸设置区域
            if (nameArea.Width == 0)
            {
                nameArea = new Rectangle(
                    80,
                    120,
                    120,
                    60
                );
            }


            if (idArea.Width == 0)
            {
                idArea = new Rectangle(
                    300,
                    80,
                    220,
                    60
                );
            }


            pbPhoto.Refresh();
        }

        private void pbPhoto_Paint(object sender, PaintEventArgs e)
        {
            if (pbPhoto.Image == null)
                return;

            float scaleX = (float)pbPhoto.Width / pbPhoto.Image.Width;
            float scaleY = (float)pbPhoto.Height / pbPhoto.Image.Height;

            Rectangle DrawRect(Rectangle rect)
            {
                if (rect.Width <= 0 || rect.Height <= 0)
                {
                    return new Rectangle(0, 0, 1, 1);
                }


                int x = (int)(rect.X * scaleX);
                int y = (int)(rect.Y * scaleY);
                int w = (int)(rect.Width * scaleX);
                int h = (int)(rect.Height * scaleY);


                if (w <= 0)
                    w = 1;

                if (h <= 0)
                    h = 1;


                return new Rectangle(
                    x,
                    y,
                    w,
                    h
                );
            }

            Pen pen = new Pen(Color.Red, 3);


            //姓名区域
            if (nameArea.Width > 0 &&
                nameArea.Height > 0)
            {
                e.Graphics.DrawRectangle(
                    pen,
                    DrawRect(nameArea));
            }


            //体检号区域
            if (idArea.Width > 0 &&
                idArea.Height > 0)
            {
                e.Graphics.DrawRectangle(
                    pen,
                    DrawRect(idArea));
            }
            //根据当前图片绘制条码和标签区域
          

            if (labelArea.Width > 0 &&
                labelArea.Height > 0)
            {
                e.Graphics.DrawRectangle(
                    pen,
                    DrawRect(labelArea));
            }
        }
        private void pbPhoto_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDownPoint = PictureBoxToImage(e.Location);

            if (nameArea.Contains(mouseDownPoint))
            {
                currentBox = "name";

                resizeDirection = CheckResize(nameArea, mouseDownPoint);

                if (resizeDirection != "")
                    resizing = true;
                else
                    dragging = true;
            }


            else if (idArea.Contains(mouseDownPoint))
            {
                currentBox = "id";

                resizeDirection = CheckResize(idArea, mouseDownPoint);

                if (resizeDirection != "")
                    resizing = true;
                else
                    dragging = true;
            }
        }
        private string CheckResize(Rectangle rect, Point p)
        {
            int size = Math.Max(30, rect.Width / 10);


            //左上
            if (Math.Abs(p.X - rect.Left) < size &&
                Math.Abs(p.Y - rect.Top) < size)
                return "LT";


            //右上
            if (Math.Abs(p.X - rect.Right) < size &&
                Math.Abs(p.Y - rect.Top) < size)
                return "RT";


            //左下
            if (Math.Abs(p.X - rect.Left) < size &&
                Math.Abs(p.Y - rect.Bottom) < size)
                return "LB";


            //右下
            if (Math.Abs(p.X - rect.Right) < size &&
                Math.Abs(p.Y - rect.Bottom) < size)
                return "RB";

            return "";
        }
        private void pbPhoto_MouseMove(object sender, MouseEventArgs e)
        {
            if (!dragging && !resizing)
                return;


            Point now = PictureBoxToImage(e.Location);


            int dx = now.X - mouseDownPoint.X;
            int dy = now.Y - mouseDownPoint.Y;
            if (resizing)
            {
                ResizeRectangle(dx, dy);

                mouseDownPoint = now;

                pbPhoto.Refresh();

                return;
            }

            if (currentBox == "name")
            {
                nameArea.X += dx;
                nameArea.Y += dy;
            }


            if (currentBox == "id")
            {
                idArea.X += dx;
                idArea.Y += dy;
            }




            mouseDownPoint = now;

            pbPhoto.Refresh();

        }
        private void ResizeRectangle(int dx, int dy)
        {

            if (currentBox == "name")
            {
                Resize(ref nameArea, dx, dy);
            }

            if (currentBox == "id")
            {
                Resize(ref idArea, dx, dy);
            }
        }
        private void Resize(ref Rectangle rect, int dx, int dy)
        {
            switch (resizeDirection)
            {
                case "RB":
                    rect.Width += dx;
                    rect.Height += dy;
                    break;


                case "LB":
                    rect.X += dx;
                    rect.Width -= dx;
                    rect.Height += dy;
                    break;


                case "RT":
                    rect.Y += dy;
                    rect.Width += dx;
                    rect.Height -= dy;
                    break;


                case "LT":
                    rect.X += dx;
                    rect.Y += dy;
                    rect.Width -= dx;
                    rect.Height -= dy;
                    break;
            }


            if (rect.Width < 30)
                rect.Width = 30;

            if (rect.Height < 20)
                rect.Height = 20;
        }
        private void pbPhoto_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
            resizing = false;
            resizeDirection = "";
            currentBox = "";
        }

        private void btnRotateLeft_Click(object sender, EventArgs e)
        {
            if (pbPhoto.Image == null)
            {
                MessageBox.Show("请先加载图片");
                return;
            }

            pbPhoto.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
            pbPhoto.Refresh();
        }

        private void btnRotateRight_Click(object sender, EventArgs e)
        {
            if (pbPhoto.Image == null)
            {
                MessageBox.Show("请先加载图片");
                return;
            }


            pbPhoto.Image.RotateFlip(
                RotateFlipType.Rotate90FlipNone
            );

            pbPhoto.Refresh();
        }

        private void HairOCRForm_Load(object sender, EventArgs e)
        {

        }

        private void dgvResult_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvResult_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //防止点击表头
            if (e.RowIndex < 0)
                return;


            //防止越界
            if (e.RowIndex >= imageFiles.Count)
                return;


            string file = imageFiles[e.RowIndex];


            //释放之前图片
            if (pbPhoto.Image != null)
            {
                pbPhoto.Image.Dispose();
            }


            //加载新图片
            pbPhoto.Image = Image.FromFile(file);
            //恢复条码框
            if (barcodeAreaList.ContainsKey(file))
            {
                barcodeArea = barcodeAreaList[file];
            }
            else
            {
                barcodeArea = Rectangle.Empty;
            }


            //恢复标签框
            if (labelAreaList.ContainsKey(file))
            {
                labelArea = labelAreaList[file];
            }
            else
            {
                labelArea = Rectangle.Empty;
            }
            //刷新红框
            pbPhoto.Refresh();
        }

        private void btnSaveArea_Click(object sender, EventArgs e)
        {
            if (nameArea.Width == 0 ||
               idArea.Width == 0)
            {
                MessageBox.Show("请先绘制识别区域！");
                return;
            }


            using (StreamWriter sw = new StreamWriter(areaFile))
            {
                sw.WriteLine(
                    $"{nameArea.X},{nameArea.Y},{nameArea.Width},{nameArea.Height}"
                );

                sw.WriteLine(
                    $"{idArea.X},{idArea.Y},{idArea.Width},{idArea.Height}"
                );

            }

            MessageBox.Show(areaFile);
            MessageBox.Show("识别区域保存成功！");
        }
        private void LoadArea()
        {
            if (!File.Exists(areaFile))
                return;


            string[] lines = File.ReadAllLines(areaFile);


            if (lines.Length >= 2)
            {
                string[] name = lines[0].Split(',');

                nameArea = new Rectangle(
                    int.Parse(name[0]),
                    int.Parse(name[1]),
                    int.Parse(name[2]),
                    int.Parse(name[3])
                );


                string[] id = lines[1].Split(',');

                idArea = new Rectangle(
                    int.Parse(id[0]),
                    int.Parse(id[1]),
                    int.Parse(id[2]),
                    int.Parse(id[3])
                );
            }
        }
        private void LoadAreaConfig()
        {
            if (!File.Exists(areaFile))
                return;


            string[] lines = File.ReadAllLines(areaFile);


            if (lines.Length >= 2)
            {
                string[] a = lines[0].Split(',');

                nameArea = new Rectangle(
                    int.Parse(a[0]),
                    int.Parse(a[1]),
                    int.Parse(a[2]),
                    int.Parse(a[3])
                );


                string[] b = lines[1].Split(',');

                idArea = new Rectangle(
                    int.Parse(b[0]),
                    int.Parse(b[1]),
                    int.Parse(b[2]),
                    int.Parse(b[3])
                );



            }
        }
    }
}