from rapidocr_onnxruntime import RapidOCR
import sys
import json
from PIL import Image
from DirectionSelector import choose_direction


# 只加载一次
ocr = RapidOCR()


print("READY", flush=True)


while True:

    line = sys.stdin.readline()

    if not line:
        break


    image_path = line.strip()


    if image_path == "":
        continue


    try:

        img = Image.open(image_path)


        all_result = []


        #目前保持单方向
        for angle in [0]:

            temp = img.rotate(
                angle,
                expand=True
            )


            result, elapse = ocr(temp)


            texts = []


            if result:

                for item in result:

                    texts.append(
                        {
                            "text": item[1],
                            "box": item[0],
                            "score": item[2]
                        }
                    )


            all_result.append(
                {
                    "angle": angle,
                    "texts": texts
                }
            )


        final_result = all_result[0]


        print(
            json.dumps(
                final_result,
                ensure_ascii=False
            ),
            flush=True
        )


    except Exception as e:

        print(
            json.dumps(
                {
                    "error":str(e)
                },
                ensure_ascii=False
            ),
            flush=True
        )