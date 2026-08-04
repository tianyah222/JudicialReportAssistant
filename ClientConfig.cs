using System;
using System.Collections.Generic;
using System.Drawing;

namespace 初筛工具箱
{
    public class ClientConfig
    {
        // 委托方名称
        public string ClientName { get; set; }


        // 条码类型
        public string BarcodeType { get; set; }


        // 姓名识别区域
        public Rectangle NameArea { get; set; }


        // 体检号区域
        public Rectangle IDArea { get; set; }


        // 标签区域
        public Rectangle LabelArea { get; set; }


        // 条码区域
        public Rectangle BarcodeArea { get; set; }

        // 是否需要体检号
        public bool NeedID { get; set; }

        // OCR过滤关键词
        public List<string> ExcludeWords { get; set; }


        public ClientConfig()
        {
            ExcludeWords = new List<string>();
        }
    }
}