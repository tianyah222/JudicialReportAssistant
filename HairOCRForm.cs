using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.Json;
namespace 初筛工具箱
{
    public partial class HairOCRForm : Form
    {
        //当前委托方配置
        ClientConfig currentConfig;
        //识别区域
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
        string configFolder = Path.Combine(
    Application.StartupPath,
    "Config"
);
        public HairOCRForm()
        {
            InitializeComponent();
            InitResultTable();
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
            if (currentConfig == null)
            {
                MessageBox.Show("请先选择委托方！");
                return;
            }
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
                    string status = "未找到条码";
                    string barcode = "";

                    string name = "";
                    string sampleNo = "";


                    if (info != null)
                    {
                        barcode = info.Code;
                        status = "定位成功";


                        //条码框使用ZXing自动定位
                        barcodeAreaList[file] = info.Location;


                        //委托方配置
                        if (currentConfig != null)
                        {
                            labelAreaList[file] = info.LabelArea;


                            //====================
                            //标签区域OCR
                            //====================

                            OCRHelper helper = new OCRHelper();


                            //截取BarcodeScanner找到的标签区域
                            using (Bitmap img = new Bitmap(file))
                            {
                                using (Bitmap label =
                                    img.Clone(
                                        info.LabelArea,
                                        img.PixelFormat))
                                {

                                    string temp =
                                        Path.Combine(
                                            Application.StartupPath,
                                            "OCR临时",
                                            Path.GetFileName(file)
                                        );


                                    if (!Directory.Exists(
                                        Path.GetDirectoryName(temp)))
                                    {
                                        Directory.CreateDirectory(
                                            Path.GetDirectoryName(temp));
                                    }


                                    label.Save(temp);


                                    OCRResult result =
                                        helper.ReadText(temp);


                                    if (result != null)
                                    {
                                        name = result.name;


                                        if (currentConfig.NeedID)
                                        {
                                            sampleNo = result.sampleNo;
                                        }
                                    }
                                }
                            }
                        }
                    }


                    //所有图片加入结果表
                    dgvResult.Rows.Add(
                        Path.GetFileName(file),
                        name,
                        barcode,
                        sampleNo,
                        status
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


            //手动区域仅绘制时显示
            if (currentBox == "name")
            {
                e.Graphics.DrawRectangle(
                    pen,
                    DrawRect(nameArea));
            }


            if (currentBox == "id")
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
            if (barcodeArea.Width > 0 &&
   barcodeArea.Height > 0)
            {
                e.Graphics.DrawRectangle(
                    pen,
                    DrawRect(barcodeArea));
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
            cmbClient.Items.Clear();

            cmbClient.Items.Add("长沙医检");
            cmbClient.Items.Add("浏阳公安");

            cmbClient.SelectedIndex = 0;
        }

        private void LoadClientList()
        {
            cmbClient.Items.Clear();


            if (!Directory.Exists(configFolder))
            {
                MessageBox.Show(
                    "配置文件夹不存在：\n" + configFolder
                );
                return;
            }


            string[] files = Directory.GetFiles(
                configFolder,
                "*.json"
            );


            foreach (string file in files)
            {
                cmbClient.Items.Add(
                    Path.GetFileNameWithoutExtension(file)
                );
            }


            if (cmbClient.Items.Count > 0)
            {
                cmbClient.SelectedIndex = 0;
            }
        }
        private void LoadClientConfig()
        {
            string file = "";

            if (cmbClient.Text == "长沙医检")
            {
                file = "Config\\长沙医检.json";
            }
            else if (cmbClient.Text == "浏阳公安")
            {
                file = "Config\\浏阳.json";
            }


            if (File.Exists(file))
            {
                string json = File.ReadAllText(file);

                currentConfig =
                    System.Text.Json.JsonSerializer.Deserialize<ClientConfig>(json);

            }
        }
        private void cmbClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbClient.SelectedItem == null)
                return;


            string client =
                cmbClient.SelectedItem.ToString();

            string file = Path.Combine(
                configFolder,
                client + ".json"
            );


            if (!File.Exists(file))
            {
                MessageBox.Show("找不到配置：" + file);
                return;
            }


            string json = File.ReadAllText(file);


            currentConfig =
                JsonSerializer.Deserialize<ClientConfig>(json);

            if (currentConfig != null)
            {
                pbPhoto.Invalidate();
            }
        }

        private void dgvResult_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void btnExport_Click(object sender, EventArgs e)
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