import subprocess
import json
import os


base_dir = os.path.dirname(
    os.path.abspath(__file__)
)


rapidocr_py = os.path.join(
    base_dir,
    "rapidocr.py"
)


image = os.path.join(
    base_dir,
    "测试标签区域_4.jpg"
)


env = os.environ.copy()

# 强制Python UTF-8环境
env["PYTHONIOENCODING"] = "utf-8"
env["PYTHONUTF8"] = "1"


result = subprocess.run(
    [
        r"C:\Users\Administrator\AppData\Local\Programs\Python\Python310\python.exe",
        rapidocr_py,
        image
    ],
    cwd=base_dir,
    capture_output=True,
    env=env
)


print("返回码:", result.returncode)


stdout = result.stdout.decode(
    "utf-8",
    errors="replace"
)


stderr = result.stderr.decode(
    "utf-8",
    errors="replace"
)


print("\nstdout:")
print(stdout)


print("\nstderr:")
print(stderr)


if stdout.strip():

    data=json.loads(stdout)

    print("\n结果:")
    print("方向:",data["direction"])
    print("姓名:",data["name"])
    print("体检号:",data["sampleNo"])
    print("评分:",data["score"])