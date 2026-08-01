from TextAnalyzer import classify_text


texts=[
    "刘宁",
    "102602100034",
    "女22岁",
    "30014322245/26021000995",
    "毒检毛发检测"
]


for t in texts:
    print(
        t,
        classify_text(t)
    )