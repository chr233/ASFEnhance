'''
# @Author       : Chr_
# @Date         : 2022-04-25 12:46:01
# @LastEditors  : Chr_
# @LastEditTime : 2022-04-25 16:41:37
# @Description  : 
'''

import asyncio
import pydantic
from aiohttp import ClientSession
from os import path, makedirs, listdir, rmdir
from typing import List
from models.gitee import Gitee_Release
from models.github import GitHub_Release, GitHub_Release_Asset

GITHUB_RELEASE_API = 'https://api.github.com/repos/chr233/ASFEnhance/releases'
GITEE_RELEASE_API = 'https://gitee.com/api/v5/repos/chr_a1/ASFEnhance/releases'

BASE_DIR = path.split(path.realpath(__file__))[0]

DIST_DIR = path.join(BASE_DIR, 'dist')


async def get_github_release() -> List[GitHub_Release]:
    '''获取GitHub发行版信息'''
    async with ClientSession() as http:
        resp = await http.get(GITHUB_RELEASE_API)
        if resp.status == 200:
            raw = await resp.read()
            result = pydantic.parse_raw_as(List[GitHub_Release], raw)
            return result


async def get_gitee_release() -> List[Gitee_Release]:
    '''获取Gitee发行版信息'''
    async with ClientSession() as http:
        resp = await http.get(GITEE_RELEASE_API)
        if resp.status == 200:
            raw = await resp.read()
            result = pydantic.parse_raw_as(List[Gitee_Release], raw)
            return result


def compare_releases(github_releases: List[GitHub_Release], gitee_releases: List[Gitee_Release]) -> List[GitHub_Release]:
    '''比较缺失的发行版'''
    diff = []

    for github in github_releases:
        tag = github.tag_name

        if not tag or len(github.assets) == 0:
            continue

        exists = False

        for gitee in gitee_releases:
            if gitee.tag_name == tag:
                exists = True
                break

        if not exists:
            diff.append(github)

    return diff


async def download_release_assets(github_release: GitHub_Release):
    '''下载发行版附件'''

    async def download_asset(client: ClientSession, asset: GitHub_Release_Asset, folder: str, tag: str):
        url = asset.browser_download_url
        name = asset.name
        size = asset.size
        file_path = path.join(folder, name)

        resp = await client.get(url)
        if resp.status == 200:
            content = await resp.read()
            if size == len(content):
                with open(file_path, 'wb') as f:
                    f.write(content)
                print(f'{tag} {name} 下载完成')
            else:
                print(f'{tag} {name} 校验失败')
        else:
            print(f'{tag} {name} 下载失败')

    async with ClientSession() as http:
        tag_name = github_release.tag_name

        folder = path.join(DIST_DIR, tag_name)

        print(f'开始下载 {tag_name} -> {folder}')

        if not path.exists(folder):
            makedirs(folder)

        tasks = []

        for asset in github_release.assets:
            tasks.append(
                asyncio.create_task(
                    download_asset(http, asset, folder, tag_name)
                )
            )

        await asyncio.gather(*tasks)

        if len(listdir(folder)) > 0:
            file_path = path.join(folder, 'README.md')
            readme = github_release.body
            with open(file_path, 'w', encoding='utf-8') as f:
                f.write(readme or "created by chr233")
        else:
            rmdir(folder)


async def main():
    # github
    print('获取GitHub发行版信息')
    github_releases = await get_github_release()
    # gitee
    print('获取Gitee发行版信息')
    gitee_releases = await get_gitee_release()

    print('比较发行版差异')
    diff_releases = compare_releases(github_releases, gitee_releases)

    tasks = []

    slice_size = 3

    for i in range(0, len(diff_releases), slice_size):
        for release in diff_releases[i:i+slice_size]:
            tasks.append(
                asyncio.create_task(
                    download_release_assets(release)
                )
            )

        await asyncio.gather(*tasks)

        print(1)

if __name__ == '__main__':
    asyncio.run(main(), debug=True)
