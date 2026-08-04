import os
import json
import subprocess
import sys


# ==============================
# 配置区域
# ==============================

# 图片目录
IMAGE_DIR = r"C:\Users\Administrator\source\repos\初筛工具箱\初筛工具箱\测试图片"


# rapidocr.py路径
BASE_DIR = os.path.dirname(
    os.path.abspath(__file__)
)


RAPIDOCR = os.path.join(
    BASE_DIR,
    "rapidocr.py"
)


# 当前Python解释器
PYTHON_EXE = sys.executable


# 支持图片格式
IMAGE_EXT = [
    ".jpg",
    ".jpeg",
    ".png"
]


# ==============================
# 调用OCR
# ==============================

def ocr_image(image_path):


    # 关键：
    # 强制子Python使用UTF-8环境
    env = os.environ.copy()

    env["PYTHONIOENCODING"] = "utf-8"

    env["PYTHONUTF8"] = "1"



    result = subprocess.run(

        [
            PYTHON_EXE,
            RAPIDOCR,
            image_path
        ],

        stdout=subprocess.PIPE,

        stderr=subprocess.PIPE,

        env=env

    )



    if result.returncode != 0:


        print("OCR运行失败:")

        print(
            result.stderr.decode(
                "utf-8",
                errors="ignore"
            )
        )


        return None



    try:


        output = result.stdout.decode(
            "utf-8"
        )


        data = json.loads(
            output
        )


        return data



    except Exception as e:


        print(
            "JSON解析失败:",
            image_path
        )


        print(
            "原始输出:"
        )


        print(
            result.stdout.decode(
                "utf-8",
                errors="ignore"
            )
        )


        print(e)


        return None



# ==============================
# 文件名处理
# ==============================

def safe_filename(name):


    invalid_chars = [
        "\\",
        "/",
        ":",
        "*",
        "?",
        "\"",
        "<",
        ">",
        "|"
    ]


    for c in invalid_chars:

        name = name.replace(
            c,
            "_"
        )


    return name



# ==============================
# 防止重名
# ==============================

def get_unique_name(path):


    if not os.path.exists(path):

        return path



    base, ext = os.path.splitext(path)


    index = 1


    while True:


        new_path = (
            f"{base}_{index}{ext}"
        )


        if not os.path.exists(new_path):

            return new_path


        index += 1



# ==============================
# 主程序
# ==============================

def main():


    print("=" * 60)

    print(
        "开始批量OCR重命名"
    )

    print("=" * 60)



    success = 0

    failed = 0



    files = sorted(
        os.listdir(IMAGE_DIR)
    )



    for file in files:



        ext = os.path.splitext(file)[1].lower()



        if ext not in IMAGE_EXT:

            continue



        old_path = os.path.join(
            IMAGE_DIR,
            file
        )



        print()

        print(
            "处理:",
            file
        )



        data = ocr_image(
            old_path
        )



        if not data:


            print(
                "❌ OCR失败"
            )


            failed += 1


            continue




        name = data.get(
            "name",
            ""
        )


        sample = data.get(
            "sampleNo",
            ""
        )



        if not name or not sample:


            print(
                "❌ 信息缺失:",
                name,
                sample
            )


            failed += 1


            continue




        name = safe_filename(
            name
        )



        new_name = (
            f"{name}_{sample}{ext}"
        )



        new_path = os.path.join(
            IMAGE_DIR,
            new_name
        )



        new_path = get_unique_name(
            new_path
        )



        os.rename(
            old_path,
            new_path
        )



        print(
            "✔",
            os.path.basename(new_path)
        )



        success += 1




    print()

    print("=" * 60)

    print(
        "测试完成"
    )

    print(
        "成功:",
        success
    )

    print(
        "失败:",
        failed
    )

    print("=" * 60)




if __name__ == "__main__":

    main()