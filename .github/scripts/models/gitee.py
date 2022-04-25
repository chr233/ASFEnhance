'''
# @Author       : Chr_
# @Date         : 2022-04-25 13:23:27
# @LastEditors  : Chr_
# @LastEditTime : 2022-04-25 16:41:17
# @Description  : Gitee发行版对象
'''

from typing import List, Optional
from .base import Base_Response


class Gitee_Release_User(Base_Response):
    id: int
    login: str
    name: str
    avatar_url: str
    url: str
    html_url: str
    remark: str
    followers_url: str
    following_url: str
    gists_url: str
    starred_url: str
    subscriptions_url: str
    organizations_url: str
    repos_url: str
    events_url: str
    received_events_url: str
    type: str


class Gitee_Release_Asset(Base_Response):
    browser_download_url: str
    name: Optional[str]


class Gitee_Release(Base_Response):
    id: int
    tag_name: str
    target_commitish: str
    prerelease: bool
    name: str
    body: str
    author: Gitee_Release_User
    created_at: str
    assets: List[Gitee_Release_Asset]
    body: str
