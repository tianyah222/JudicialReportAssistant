from FieldExtractor import extract_fields


def get_center(box):

    x = (
        box[0][0]
        + box[1][0]
        + box[2][0]
        + box[3][0]
    ) / 4

    y = (
        box[0][1]
        + box[1][1]
        + box[2][1]
        + box[3][1]
    ) / 4

    return x, y



def choose_direction(results):

    best_result = None
    best_score = -1


    for item in results:

        fields = extract_fields(
            item["texts"]
        )


        score = 0


        # 基础字段
        if fields["name"]:
            score += 50


        if fields["sampleNo"]:
            score += 100



        # 位置评分

        for text_item in item["texts"]:

            text = text_item["text"]

            box = text_item["box"]


            x,y = get_center(box)



            # 姓名靠近左上
            if text == fields["name"]:

                if x < 200 and y < 150:
                    score += 30



            # 体检号靠近右上
            if text == fields["sampleNo"]:

                if x > 200 and y < 150:
                    score += 30



        if score > best_score:

            best_score = score

            best_result = {

                "direction": item["angle"],

                "name": fields["name"],

                "sampleNo": fields["sampleNo"],

                "score": score
            }


    return best_result