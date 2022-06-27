"""
# @Author       : Chr_
# @Date         : 2022-06-27 21:22:49
# @LastEditors  : Chr_
# @LastEditTime : 2022-06-27 22:15:25
# @Description  : Set special profile tooles
"""

from collections import OrderedDict
import requests
from urllib3 import encode_multipart_formdata

# Bot"s AccessToken
# (Can be found using ASFEnhance"s ACCESSTOKEN command, requires enable developer ferture)
AccessTokens = [
    "YOUR_ACCESS_TOKEN",
    # "YOUR_ACCESS_TOKEN (multiple tokens can be added)",
]

# Request payload
Argument = "CNqzexCviZ/nThgB"
# Do not use special profile : CNqzexCviZ/nThgA
# Steam 3000 profile         : CNqzexCviZ/nThgB
# 2021 winter profile        : CMzccBChvIm2SBgB
# 2021 autumn profile        : COqzbRCfxLilRhgB
# 2077 game profile          : CKzPQhC8pKOxQhgB
# to be continued...

for token in AccessTokens:
    url = "https://api.steampowered.com/IQuestService/ActivateProfileModifierItem/v1"
    params = {"access_token": token}
    form_data = OrderedDict([
        ("input_protobuf_encoded", Argument),
    ])
    data, content_type = encode_multipart_formdata(form_data)

    headers = {
        "Content-Type": content_type,
        "Referer": "https://steamcommunity.com/",
        "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:101.0) Gecko/20100101 Firefox/101.0",
    }

    resp = requests.post(url, params=params, data=data, headers=headers)
    print('done!')
