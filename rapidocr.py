from rapidocr_onnxruntime import RapidOCR
import sys
import json
from PIL import Image
from DirectionSelector import choose_direction


ocr = RapidOCR()


image_path = sys.argv[1]


img = Image.open(image_path)


all_result = []


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


final_result = choose_direction(all_result)


print(
    json.dumps(
        final_result,
        ensure_ascii=False
    )
)