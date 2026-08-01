from DirectionSelector import choose_direction


results = [

{
"angle":0,
"texts":[
    {
    "text":"毛发",
    "box":[[96,0],[132,0],[132,67],[96,67]],
    "score":0.99
    },
    {
    "text":"刘宁",
    "box":[[282,82],[307,82],[307,134],[282,134]],
    "score":0.99
    },
    {
    "text":"102602100034",
    "box":[[282,288],[304,288],[304,447],[282,447]],
    "score":0.99
    }
]
},


{
"angle":90,
"texts":[
    {
    "text":"刘宁",
    "box":[[82,41],[136,43],[135,71],[82,70]],
    "score":0.99
    },
    {
    "text":"102602100034",
    "box":[[290,47],[445,47],[445,66],[290,66]],
    "score":0.99
    }
]
}

]


print(
    choose_direction(results)
)