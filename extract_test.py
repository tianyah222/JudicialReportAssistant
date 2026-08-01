from FieldExtractor import extract_fields


texts=[
    {
        "text":"刘宁"
    },
    {
        "text":"102602100034"
    },
    {
        "text":"女22岁"
    },
    {
        "text":"毒检毛发检测"
    }
]


print(
    extract_fields(texts)
)