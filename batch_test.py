import os
import subprocess
import json


# ===============================
# 配置区域
# ===============================

# 测试图片文件夹
image_folder = r"C:\Users\Administrator\source\repos\初筛更名助手\初筛更名助手\测试图片"


# Python路径
python_exe = (
    r"C:\Users\Administrator\AppData\Local\Programs\Python\Python310\python.exe"
)


# rapidocr.py路径
rapidocr_py = (
    r"C:\Users\Administrator\source\repos\初筛更名助手\初筛更名助手\rapidocr.py"
)


# 支持图片格式
exts = [
    ".jpg",
    ".jpeg",
    ".png"
]


# ===============================
# 执行测试
# ===============================

success = 0
failed = 0


print("\n")
print("=" * 70)
print("开始批量OCR测试")
print("=" * 70)


for filename in sorted(os.listdir(image_folder)):

    if not filename.lower().endswith(tuple(exts)):
        continue


    image_path = os.path.join(
        image_folder,
        filename
    )


    print("\n图片:", filename)


    try:

        # 设置UTF-8环境
        env = os.environ.copy()

        env["PYTHONIOENCODING"] = "utf-8"
        env["PYTHONUTF8"] = "1"


        result = subprocess.run(
            [
                python_exe,
                rapidocr_py,
                image_path
            ],
            cwd=os.path.dirname(rapidocr_py),
            capture_output=True,
            env=env
        )


        stdout = result.stdout.decode(
            "utf-8",
            errors="replace"
        )


        if result.returncode != 0:

            print("❌ Python错误")
            print(result.stderr.decode(
                "utf-8",
                errors="replace"
            ))

            failed += 1
            continue


        if not stdout.strip():

            print("❌ 无返回")
            failed += 1
            continue


        data = json.loads(stdout)


        direction = data.get(
            "direction",
            ""
        )

        name = data.get(
            "name",
            ""
        )

        sample_no = data.get(
            "sampleNo",
            ""
        )

        score = data.get(
            "score",
            ""
        )


        print(
            f"方向:{direction} | "
            f"姓名:{name} | "
            f"体检号:{sample_no} | "
            f"评分:{score}"
        )


        if name and sample_no:

            success += 1

        else:

            failed += 1


    except Exception as e:

        print(
            "❌ 异常:",
            e
        )

        failed += 1



# ===============================
# 汇总
# ===============================

print("\n")
print("=" * 70)
print("测试完成")
print("=" * 70)

print(
    "成功:",
    success
)

print(
    "失败:",
    failed
)

print(
    "总数:",
    success + failed
)