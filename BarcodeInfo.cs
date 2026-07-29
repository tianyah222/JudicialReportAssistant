using System.Drawing;

namespace 初筛更名助手
{
    public class BarcodeInfo
    {
        public string Code { get; set; }

        //条码位置
        public Rectangle Location { get; set; }

        //白色标签区域
        public Rectangle LabelArea { get; set; }

        //旋转角度
        public int Rotation { get; set; }
    }
}