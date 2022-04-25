'''
# @Author       : Chr_
# @Date         : 2022-04-25 13:24:22
# @LastEditors  : Chr_
# @LastEditTime : 2022-04-25 16:41:05
# @Description  : 基础模块,使用ujson解码json
'''
import ujson
from pydantic import BaseModel


class Base_Response(BaseModel):
    class Config:
        json_loads = ujson.loads
