from TextAnalyzer import score_name, score_sample_no


def extract_fields(texts):

    name = ""
    sample_no = ""

    max_name_score = -999
    max_sample_score = -999


    for item in texts:

        text = item["text"].strip()


        #姓名评分
        ns = score_name(text)

        if ns > max_name_score:
            max_name_score = ns
            name = text


        #体检号评分
        ss = score_sample_no(text)

        if ss > max_sample_score:
            max_sample_score = ss
            sample_no = text


    return {
        "name": name,
        "sampleNo": sample_no
    }