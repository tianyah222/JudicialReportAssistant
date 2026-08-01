from TextAnalyzer import score_name,score_sample_no


texts=[
    "刘宁",
    "26846",
    "102602100034",
    "360321199111236042",
    "30014322245/26021000995",
    "女22岁"
]


for t in texts:

    print(
        t,
        "姓名分:",
        score_name(t),
        "编号分:",
        score_sample_no(t)
    )