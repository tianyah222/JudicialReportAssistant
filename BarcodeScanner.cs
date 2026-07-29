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
       AutoRotate = true,

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

                        if (result != null)
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


                            if (points == null || points.Length == 0)
                            {
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
                 
                            Rectangle rect =
new Rectangle(
    (int)minX - 30,
    (int)minY - 20,
    (int)(maxX - minX) + 60,
    (int)(maxY - minY) + 40
);
                            //根据条码位置估算白色标签区域
                            Rectangle labelRect = new Rectangle(
    (int)minX - 300,
    (int)minY - 150,
    700,
    350
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
                         
                            resizeImage.Dispose();
                            testImage.Dispose();
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