using System;
using System.Collections.Generic;

namespace 初筛工具箱
{

    public class OCRResult
    {
        private bool IsValidName(string text)
        {
            if (string.IsNullOrEmpty(text))
                return false;


            string[] exclude =
            {
        "毛发",
        "毛发种类",
        "毛发检测",
        "姓名",
        "性别",
        "身份证号",
        "取样地点",
        "提取地点",
        "取样单位",
        "提取时间",
        "取样人",
        "见证人",
        "备注",
        "取样时间",
        "样本编号",
        "毒检毛发检测",
        "毛发取样",
        "编号",
        "体检号",
        "取样地点",
    };


            foreach (string word in exclude)
            {
                if (text.Contains(word))
                    return false;
            }




            //排除明显编号
            if (text.Contains("/") ||
                text.Length > 6)
            {
                return false;
            }



            //中文姓名 2-4字
            if (System.Text.RegularExpressions.Regex.IsMatch(
                text,
                @"^[\u4e00-\u9fa5]{2,4}$"))
            {
                return true;
            }



            //数字姓名 3-6位
            if (System.Text.RegularExpressions.Regex.IsMatch(
                text,
                @"^\d{3,6}$"))
            {
                return true;
            }


            return false;
        }
        public int angle { get; set; }

        public List<OCRText> texts { get; set; }


        //提取姓名
        //提取姓名
        public string name
        {
            get
            {
                if (texts == null)
                    return "";


                //第一优先：
                //找“姓名”关键词后面的内容
                for (int i = 0; i < texts.Count - 1; i++)
                {
                    string current = texts[i].text.Trim();


                    if (current.Contains("姓名"))
                    {
                        string next = texts[i + 1].text.Trim();


                        if (IsValidName(next))
                            return next;
                    }
                }



                //第二优先：
                //找靠近标签右侧的姓名
                //排除身份证、体检号等长数字

                foreach (var t in texts)
                {
                    string value = t.text.Trim();


                    if (IsValidName(value))
                    {
                        return value;
                    }
                }


                return "";
            }
        }


        //提取体检号
        //提取体检号
        public string sampleNo
        {
            get
            {
                if (texts == null)
                    return "";


                foreach (var t in texts)
                {
                    string value = t.text.Trim();


                    //体检号：
                    //12位纯数字
                    if (System.Text.RegularExpressions.Regex.IsMatch(
                        value,
                        @"^\d{12}$"))
                    {
                        return value;
                    }
                }


                return "";
            }
        }


        public int score
        {
            get
            {
                return 0;
            }
        }

    }



    public class OCRText
    {
        public string text { get; set; }

        public double[][] box { get; set; }

        public double score { get; set; }
    }
}