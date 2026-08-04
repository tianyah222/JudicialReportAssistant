using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Windows.Forms;

namespace 初筛工具箱
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtFolder.Text = dialog.SelectedPath;
            }
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFolder.Text))
            {
                MessageBox.Show("请先选择文件夹！");
                return;
            }

            string folder = txtFolder.Text;

            List<string> oldFiles = new List<string>();
            List<string> newFiles = new List<string>();

            foreach (string file in Directory.GetFiles(folder))
            {
                string name = Path.GetFileNameWithoutExtension(file);
                string ext = Path.GetExtension(file);

                Match match = Regex.Match(name, @"^(.*)_\d+$");

                if (match.Success)
                {
                    string newName = match.Groups[1].Value + ext;

                    string newPath = Path.Combine(folder, newName);

                    if (!File.Exists(newPath))
                    {
                        oldFiles.Add(file);
                        newFiles.Add(newPath);
                    }
                }
            }

            if (oldFiles.Count == 0)
            {
                MessageBox.Show("没有发现需要修改的文件！");
                return;
            }

            // 修改前预览
            string preview = "";

            for (int i = 0; i < oldFiles.Count && i < 10; i++)
            {
                preview +=
                Path.GetFileName(oldFiles[i])
                + "\r\n↓\r\n"
                + Path.GetFileName(newFiles[i])
                + "\r\n\r\n";
            }

            preview += "共 " + oldFiles.Count + " 个文件需要修改。\r\n是否继续？";

            DialogResult result = MessageBox.Show(
                preview,
                "确认修改",
                MessageBoxButtons.YesNo
            );

            if (result != DialogResult.Yes)
            {
                return;
            }

            int count = 0;

            string log =
                "初筛工具箱修改记录\r\n"
                + DateTime.Now
                + "\r\n\r\n";

            for (int i = 0; i < oldFiles.Count; i++)
            {
                File.Move(oldFiles[i], newFiles[i]);

                log +=
                Path.GetFileName(oldFiles[i])
                + "\r\n↓\r\n"
                + Path.GetFileName(newFiles[i])
                + "\r\n\r\n";

                count++;
            }

            File.WriteAllText(
                Path.Combine(folder,
                "改名记录_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt"),
                log
            );

            lblResult.Text =
                "处理完成，共修改：" + count + " 个文件";

            MessageBox.Show("完成！");
        }
    }
}