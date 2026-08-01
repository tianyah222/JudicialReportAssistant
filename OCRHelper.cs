using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.Json;

namespace 初筛更名助手
{
    public class OCRHelper
    {

        public string ReadText(string imagePath)
        {
            try
            {

                ProcessStartInfo psi =
                    new ProcessStartInfo();

                psi.FileName = "python";

                psi.Arguments =
                    "rapidocr.py \"" +
                    imagePath +
                    "\"";


                psi.WorkingDirectory =
                    Application.StartupPath;


                psi.UseShellExecute = false;
                psi.RedirectStandardOutput = true;
                psi.RedirectStandardError = true;
                psi.CreateNoWindow = true;


                using (Process p = Process.Start(psi))
                {

                    string output =
                        p.StandardOutput.ReadToEnd();


                    string error =
                        p.StandardError.ReadToEnd();


                    p.WaitForExit();


                    if (!string.IsNullOrEmpty(error))
                    {
                        return error;
                    }


                    return output;
                }

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    }
}