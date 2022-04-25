'''
# @Author       : Chr_
# @Date         : 2022-04-25 12:57:26
# @LastEditors  : Chr_
# @LastEditTime : 2022-04-25 16:41:23
# @Description  : GitHub发行版对象
'''

from typing import List, Optional
from .base import Base_Response


class GitHub_Release_User(Base_Response):
    login: str
    id: int
    node_id: str
    avatar_url: str
    gravatar_id: str
    url: str
    html_url: str
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
    site_admin: bool


class GitHub_Release_Asset(Base_Response):
    url: str
    id: int
    node_id: str
    name: str
    label: Optional[str]
    uploader: GitHub_Release_User
    content_type: str
    state: str
    size: int
    download_count: int
    created_at: str
    updated_at: str
    browser_download_url: str


class GitHub_Release(Base_Response):
    url: str
    assets_url: str
    upload_url: str
    html_url: str
    id: Optional[int]
    author: Optional[GitHub_Release_User]
    node_id: str
    tag_name: str
    target_commitish: str
    name: str
    draft: bool
    prerelease: bool
    created_at: str
    published_at: str
    assets: List[GitHub_Release_Asset]
    tarball_url: str
    zipball_url: str
    body: str
