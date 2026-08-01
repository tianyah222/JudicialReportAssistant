import re


def classify_text(text):

    text = text.strip()

    result = []


    # 12位数字
    if re.fullmatch(r"\d{12}", text):
        result.append("sample_no")


    # 数字/数字
    if re.match(r"^\d+/\d+$", text):
        result.append("sample_no")


    # 中文姓名
    if re.fullmatch(r"[\u4e00-\u9fa5]{2,4}", text):
        result.append("name")


    # 数字编号姓名
    if re.fullmatch(r"\d{4,6}", text):
        result.append("name")


    # 日期时间
    if re.search(
        r"\d{4}[-/]\d{1,2}[-/]\d{1,2}",
        text
    ):
        result.append("date")


    # 性别年龄
    if "男" in text or "女" in text:
        result.append("gender")


    return result



def score_name(text, box=None):

    score = 0


    exclude_words = [
        "毛发",
        "头发",
        "毒检毛发检测",
        "身份证号",
        "证号",
        "姓名",
        "性别",
        "年龄",
        "男",
        "女",
        "毛发取样袋",
        "毛发取样",
        "取样",
        "检测",
        "检验",
        "名",
        "别"
    ]


    if text in exclude_words:
        return -100


    # 身份证
    if re.fullmatch(r"\d{18}", text):
        return -100


    # 12位体检号
    if re.fullmatch(r"\d{12}", text):
        return -100


    # 编号/编号
    if re.match(r"^\d+/\d+$", text):
        return -100


    # 数字姓名（提高优先级）
    if re.fullmatch(r"\d{4,6}", text):
        score += 80


    # 中文姓名
    if re.fullmatch(r"[\u4e00-\u9fa5]{2,4}", text):
        score += 50


    return score



def score_sample_no(text):

    score = 0


    # 12位数字
    if re.fullmatch(r"\d{12}", text):
        score += 100


    # 数字/数字
    if re.match(r"^\d+/\d+$", text):
        score += 80


    # 身份证
    if re.fullmatch(r"\d{18}", text):
        score -= 100


    return score