import os
import sys
import json
import subprocess
from datetime import datetime


# =============================
# 软件信息
# =============================

PROGRAM = "初筛工具箱"
VERSION = "5.1"
MODULE = "批量重命名"


# =============================
# 路径
# =============================

BASE_DIR = os.path.dirname(
    os.path.abspath(__file__)
)


RAPIDOCR = os.path.join(
    BASE_DIR,
    "rapidocr.py"
)


PYTHON_EXE = sys.executable


IMAGE_EXT = [
    ".jpg",
    ".jpeg",
    ".png"
]



# =============================
# OCR调用
# =============================

def ocr_image(path):

    env = os.environ.copy()

    env["PYTHONIOENCODING"] = "utf-8"
    env["PYTHONUTF8"] = "1"


    result = subprocess.run(

        [
            PYTHON_EXE,
            RAPIDOCR,
            path
        ],

        stdout=subprocess.PIPE,
        stderr=subprocess.PIPE,
        env=env

    )


    if result.returncode != 0:

        return None


    try:

        return json.loads(

            result.stdout.decode(
                "utf-8"
            )

        )

    except:

        return None




# =============================
# 文件名处理
# =============================

def safe_name(name):

    for c in '\\/:*?"<>|':

        name = name.replace(
            c,
            "_"
        )

    return name



def name_type(name):

    if not name:

        return "未知"


    if name.isdigit():

        return "数字编码"


    return "中文姓名"




# =============================
# 单张处理
# =============================

def process_image(path):


    old_name = os.path.basename(path)



    data = ocr_image(path)



    if not data:

        return {

            "oldName": old_name,

            "status": "OCR失败"

        }



    name = data.get(
        "name",
        ""
    )


    sample = data.get(
        "sampleNo",
        ""
    )


    score = data.get(
        "score",
        0
    )


    direction = data.get(
        "direction",
        ""
    )



    if not name:

        return {

            "oldName":old_name,

            "status":"信息缺失",

            "message":"姓名为空"

        }



    if not sample:

        return {

            "oldName":old_name,

            "status":"信息缺失",

            "message":"体检号为空"

        }




    if score < 150:

        return {

            "oldName":old_name,

            "status":"人工确认",

            "message":"评分不足",

            "score":score

        }




    name = safe_name(name)



    ext = os.path.splitext(
        old_name
    )[1]


    new_name = (

        f"{name}_{sample}{ext}"

    )



    new_path = os.path.join(

        os.path.dirname(path),

        new_name

    )



    # 重复文件

    if os.path.exists(new_path):


        return {

            "oldName":old_name,

            "newName":new_name,

            "name":name,

            "sampleNo":sample,

            "status":"重复文件"

        }




    os.rename(

        path,

        new_path

    )



    return {


        "oldName":old_name,

        "newName":new_name,

        "name":name,

        "nameType":name_type(name),

        "sampleNo":sample,

        "direction":direction,

        "score":score,

        "status":"成功"

    }




# =============================
# 主程序
# =============================

def main():


    if len(sys.argv)<2:


        print(json.dumps({

            "success":False,

            "message":"未指定目录"

        },ensure_ascii=False))


        return



    folder=sys.argv[1]



    results=[]



    for file in sorted(
        os.listdir(folder)
    ):


        ext=os.path.splitext(file)[1].lower()


        if ext not in IMAGE_EXT:

            continue



        path=os.path.join(
            folder,
            file
        )


        results.append(

            process_image(path)

        )



    success=sum(
        1 for x in results
        if x["status"]=="成功"
    )


    duplicate=sum(
        1 for x in results
        if x["status"]=="重复文件"
    )


    manual=sum(
        1 for x in results
        if x["status"]=="人工确认"
    )


    failed=len(results)-success-duplicate-manual



    output={


        "program":PROGRAM,

        "version":VERSION,

        "module":MODULE,

        "time":datetime.now().strftime(

            "%Y-%m-%d %H:%M:%S"

        ),


        "success":True,


        "total":len(results),

        "successCount":success,

        "duplicateCount":duplicate,

        "manualCount":manual,

        "failedCount":failed,


        "results":results

    }



    # 保存日志

    log=os.path.join(

        folder,

        "批量重命名记录_"

        +

        datetime.now().strftime(

            "%Y%m%d_%H%M%S"

        )

        +

        ".json"

    )


    with open(

        log,

        "w",

        encoding="utf-8"

    ) as f:


        json.dump(

            output,

            f,

            ensure_ascii=False,

            indent=2

        )



    print(

        json.dumps(

            output,

            ensure_ascii=False

        )

    )




if __name__=="__main__":

    main()