using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace 初筛工具箱
{
    public class OCRHelper
    {

        //=========================
        // OCR服务器进程（只启动一次）
        //=========================

        private static Process ocrProcess;

        private static StreamWriter ocrInput;

        private static StreamReader ocrOutput;



        //=========================
        // 启动OCR服务器
        //=========================

        private static void StartOCRServer()
        {
            if (ocrProcess != null &&
                !ocrProcess.HasExited)
            {
                return;
            }


            string pythonPath =
                @"C:\Users\Administrator\AppData\Local\Programs\Python\Python310\python.exe";


            string projectPath =
                Directory.GetParent(Application.StartupPath)
                .Parent
                .Parent
                .Parent
                .FullName;


            string serverPath =
                Path.Combine(
                    projectPath,
                    "rapidocr_server.py"
                );



            ProcessStartInfo psi =
                new ProcessStartInfo();


            psi.FileName = pythonPath;


            psi.Arguments =
                "\"" +
                serverPath +
                "\"";


            psi.WorkingDirectory =
                projectPath;


            psi.UseShellExecute = false;


            psi.RedirectStandardInput = true;

            psi.RedirectStandardOutput = true;

            psi.RedirectStandardError = true;


            psi.CreateNoWindow = true;



            ocrProcess =
                Process.Start(psi);



            ocrInput =
                ocrProcess.StandardInput;


            ocrOutput =
                ocrProcess.StandardOutput;



            //等待Python初始化完成
            string ready =
                ocrOutput.ReadLine();

       

            if (ready != "READY")
            {
                throw new Exception(
                    "OCR服务器启动失败：" + ready
                );
            }

        }




        //=========================
        // OCR识别
        //=========================
        public OCRResult ReadAreaText(string file, Rectangle area)
        {
            using (Bitmap img = new Bitmap(file))
            {
                using (Bitmap crop = img.Clone(
                    area,
                    img.PixelFormat))
                {

                    string temp =
                        Path.Combine(
                            Application.StartupPath,
                            "OCR临时",
                            "OCR区域_" +
                            Path.GetFileNameWithoutExtension(file)
                            + "_" +
                            Guid.NewGuid().ToString("N")
                            + ".jpg"
                        );


                    //如果文件夹不存在则创建
                    string folder =
                        Path.GetDirectoryName(temp);

                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }


                    crop.Save(temp);


                    OCRResult result = ReadText(temp);


                    //识别完成删除临时图片
                    try
                    {
                        File.Delete(temp);
                    }
                    catch
                    {

                    }


                    return result;
                }
            }
        }
        public OCRResult ReadText(string imagePath)

        {

            try
            {

                //启动一次即可
                StartOCRServer();



                //发送图片路径
                ocrInput.WriteLine(imagePath);

                ocrInput.Flush();



                //读取返回JSON
                string output =
                    ocrOutput.ReadLine();
              


                if (string.IsNullOrEmpty(output))
                {
                    return null;
                }



                try
                {

                    OCRResult result =
                        JsonSerializer.Deserialize<OCRResult>(
                            output
                        );


                    return result;

                }

                catch (Exception ex)
                {

                    Console.WriteLine(
                        "JSON解析失败:"
                        + ex.Message
                    );


                    Console.WriteLine(
                        output
                    );


                    return null;
                }


            }


            catch (Exception ex)
            {

                Console.WriteLine(
                    "OCR错误:"
                    + ex.Message
                );


                return null;

            }

        }



    }

}