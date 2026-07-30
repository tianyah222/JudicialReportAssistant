using System.Drawing;
using ZXing;
using ZXing.QrCode;
using ZXing.Windows.Compatibility;
using System.Collections.Generic;
using System.IO;
namespace 初筛更名助手
{
    public class BarcodeScanner
    {
        private Rectangle ConvertRectangle(
       Rectangle rect,
       int rotation,
       int width,
       int height)
        {
            switch (rotation)
            {
                case 0:
                    return rect;


                case 1: //90度
                    return new Rectangle(
                        rect.Y,
                        width - rect.Right,
                        rect.Height,
                        rect.Width
                    );


                case 2: //180度
                    return new Rectangle(
                        width - rect.Right,
                        height - rect.Bottom,
                        rect.Width,
                        rect.Height
                    );


                case 3: //270度
                    return new Rectangle(
                        height - rect.Bottom,
                        rect.X,
                        rect.Height,
                        rect.Width
                    );


                default:
                    return rect;
            }
        }
        private Rectangle ConvertLabelRectangle(
    Rectangle rect,
    int rotation,
    int width,
    int height)
        {
            switch (rotation)
            {
                case 0:
                    return rect;


                case 1: //90°
                    return new Rectangle(
                        height - rect.Bottom,
                        rect.X,
                        rect.Height,
                        rect.Width
                    );


                case 2: //180°
                    return new Rectangle(
                        width - rect.Right,
                        height - rect.Bottom,
                        rect.Width,
                        rect.Height
                    );


                case 3: //270°
                    return new Rectangle(
                        rect.Y,
                        width - rect.Right,
                        rect.Height,
                        rect.Width
                    );


                default:
                    return rect;
            }
        }
        private Rectangle FindWhiteLabel(
      Bitmap image,
      Rectangle barcodeRect)
        {
            Rectangle label;


            //横向条码
            if (barcodeRect.Width > barcodeRect.Height)
            {
                label = new Rectangle(
                    barcodeRect.X - 100,
                    barcodeRect.Y - 120,
                    450,
                    280
                );
            }
            else
            {
                //纵向条码
                label = new Rectangle(
                    barcodeRect.X - 180,
                    barcodeRect.Y - 120,
                    350,
                    500
                );
            }


            label = Rectangle.Intersect(
                label,
                new Rectangle(
                    0,
                    0,
                    image.Width,
                    image.Height
                )
            );


            // MessageBox.Show(
            // "生成标签区域：" + label.ToString()
            //  );


            return label;
        }
        private List<Bitmap> CreateRotateLabels(Bitmap image)
        {
            List<Bitmap> list = new List<Bitmap>();

            //0°
            list.Add(new Bitmap(image));


            //90°
            Bitmap img90 = new Bitmap(image);
            img90.RotateFlip(
                RotateFlipType.Rotate90FlipNone);
            list.Add(img90);


            //180°
            Bitmap img180 = new Bitmap(image);
            img180.RotateFlip(
                RotateFlipType.Rotate180FlipNone);
            list.Add(img180);


            //270°
            Bitmap img270 = new Bitmap(image);
            img270.RotateFlip(
                RotateFlipType.Rotate270FlipNone);
            list.Add(img270);


            return list;
        }
        private Bitmap RotateLabel(
    Bitmap image,
    int rotation)
        {
            Bitmap result = new Bitmap(image);

            switch (rotation)
            {
                case 0:
                    break;

                case 1:
                    result.RotateFlip(
                        RotateFlipType.Rotate90FlipNone);
                    break;

                case 2:
                    result.RotateFlip(
                        RotateFlipType.Rotate180FlipNone);
                    break;

                case 3:
                    result.RotateFlip(
                        RotateFlipType.Rotate270FlipNone);
                    break;
            }

            return result;
        }
        public BarcodeInfo ReadBarcode(string imagePath)
        {
            try
            {
                using (Bitmap original = new Bitmap(imagePath))
                {
                    Console.WriteLine(
       "图片：" + Path.GetFileName(imagePath) +
       " 尺寸：" +
       original.Width +
       "×" +
       original.Height
   );
                    for (int i = 0; i < 4; i++)
                    {

                        Bitmap testImage = new Bitmap(original);


                        if (i == 1)
                            testImage.RotateFlip(
                                RotateFlipType.Rotate90FlipNone);


                        if (i == 2)
                            testImage.RotateFlip(
                                RotateFlipType.Rotate180FlipNone);


                        if (i == 3)
                            testImage.RotateFlip(
                                RotateFlipType.Rotate270FlipNone);



                        BarcodeReader reader =
   new BarcodeReader
   {
       AutoRotate = false,

       Options = new ZXing.Common.DecodingOptions
       {
           TryHarder = true,
           TryInverted = true,

           PossibleFormats = new List<BarcodeFormat>
           {
            BarcodeFormat.CODE_128
           }
       }
   };

                        Bitmap resizeImage = new Bitmap(
        testImage.Width * 2,
        testImage.Height * 2
    );
                        using (Graphics g = Graphics.FromImage(resizeImage))
                        {
                            g.DrawImage(
                                testImage,
                                0,
                                0,
                                resizeImage.Width,
                                resizeImage.Height
                            );
                        }
                        var result =
                            reader.Decode(resizeImage);
                        //第一次失败，尝试灰度增强
                        if (result == null)
                        {
                            Bitmap gray = new Bitmap(resizeImage.Width, resizeImage.Height);

                            using (Graphics g = Graphics.FromImage(gray))
                            {
                                System.Drawing.Imaging.ColorMatrix cm =
                                    new System.Drawing.Imaging.ColorMatrix(
                                    new float[][]
                                    {
                new float[]{0.3f,0.3f,0.3f,0,0},
                new float[]{0.59f,0.59f,0.59f,0,0},
                new float[]{0.11f,0.11f,0.11f,0,0},
                new float[]{0,0,0,1,0},
                new float[]{0,0,0,0,1}
                                    });

                                using (var attr =
                                    new System.Drawing.Imaging.ImageAttributes())
                                {
                                    attr.SetColorMatrix(cm);

                                    g.DrawImage(
                                        resizeImage,
                                        new Rectangle(0, 0, gray.Width, gray.Height),
                                        0,
                                        0,
                                        resizeImage.Width,
                                        resizeImage.Height,
                                        GraphicsUnit.Pixel,
                                        attr);
                                }
                            }


                            result = reader.Decode(gray);

                            gray.Dispose();
                        }

                        if (result == null)
                        {
                            Console.WriteLine(
                                "第" + i + "次旋转失败"
                            );
                        }

                        if (result != null)
                        {
                            Console.WriteLine("ZXing成功：" + result.Text);

                            var points =
                                result.ResultPoints;

                            if (points == null || points.Length == 0)
                            {
                                Console.WriteLine("没有定位点，直接返回条码");

                                resizeImage.Dispose();
                                testImage.Dispose();

                                return new BarcodeInfo
                                {
                                    Code = result.Text,
                                    Location = Rectangle.Empty,
                                    LabelArea = Rectangle.Empty,
                                    Rotation = i * 90
                                };
                            }

                            float minX = float.MaxValue;
                            float minY = float.MaxValue;
                            float maxX = 0;
                            float maxY = 0;


                            foreach (var p in points)
                            {
                                if (p.X < minX)
                                    minX = p.X;

                                if (p.Y < minY)
                                    minY = p.Y;

                                if (p.X > maxX)
                                    maxX = p.X;

                                if (p.Y > maxY)
                                    maxY = p.Y;
                            }
                            // ZXing是在2倍图片上识别，需要换算回原图坐标
                            minX /= 2;
                            minY /= 2;
                            maxX /= 2;
                            maxY /= 2;
                            //加入这里
                            // MessageBox.Show(
                            // "ZXing定位点：" +
                            //  "\nminX=" + minX +
                            // "\nminY=" + minY +
                            // "\nmaxX=" + maxX +
                            // "\nmaxY=" + maxY
                            // );
                            Rectangle rectRotated =
     new Rectangle(
         (int)minX - 30,
         (int)minY - 20,
         (int)(maxX - minX) + 60,
         (int)(maxY - minY) + 40
     );


                            //转换回原图坐标
                            Rectangle rect =
                                ConvertRectangle(
                                    rectRotated,
                                    i,
                                    original.Width,
                                    original.Height
                                );
                            //根据条码位置寻找白色标签区域
                            Rectangle labelRectRotated =
     FindWhiteLabel(
         testImage,
         rectRotated
     );

                            Rectangle labelRect =
     ConvertLabelRectangle(
         labelRectRotated,
         i,
         original.Width,
         original.Height
     );
                            labelRect = Rectangle.Intersect(
                                labelRect,
                                new Rectangle(
                                    0,
                                    0,
                                    original.Width,
                                    original.Height
                                )
                            );
                            //==============================
                            // 测试标签区域截图
                            //==============================
                            if (labelRect.Width > 0 &&
                                labelRect.Height > 0)
                            {
                                string testPath = Path.Combine(
                                    Application.StartupPath,
                                    "测试标签区域_" +
                                    Path.GetFileName(imagePath)
                                );

                                using (Bitmap cropLabel = original.Clone(
    labelRect,
    original.PixelFormat))
                                {

                                    Bitmap correctLabel =
                                        RotateLabel(
                                            cropLabel,
                                            i
                                        );


                                    string correctPath = Path.Combine(
                                        Application.StartupPath,
                                        "校正标签区域_" +
                                        Path.GetFileName(imagePath)
                                    );


                                    List<Bitmap> rotateLabels =
    CreateRotateLabels(cropLabel);


                                    OCRHelper ocr = new OCRHelper();

                                    int index = 0;

                                    foreach (Bitmap img in rotateLabels)
                                    {

                                        //保存调试图片
                                        string path = Path.Combine(
                                            Application.StartupPath,
                                            "方向" + index + "_" +
                                            Path.GetFileName(imagePath)
                                        );

                                        img.Save(path);


                                        //调用OCR（目前返回空）
                                        string text = ocr.ReadText(img);


                                        Console.WriteLine(
    "方向" + index +
    " OCR结果：" +
    text
);


                                        img.Dispose();

                                        index++;
                                    }


                                    correctLabel.Dispose();
                                }
                            }

                            //释放图片
                            resizeImage.Dispose();
                            testImage.Dispose();
                            //返回识别结果
                            return new BarcodeInfo
                            {
                                Code = result.Text,
                                Location = rect,
                                LabelArea = labelRect,
                                Rotation = i * 90
                            };
                        }


                        testImage.Dispose();
                    }
                }

                Console.WriteLine(
                          "最终失败：" + Path.GetFileName(imagePath)
                      );
                return null;

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}