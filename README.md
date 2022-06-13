# ASFEnhance

[![Codacy Badge](https://app.codacy.com/project/badge/Grade/3d174e792fd4412bb6b34a77d67e5dea)](https://www.codacy.com/gh/chr233/ASFEnhance/dashboard)
![GitHub Workflow Status](https://img.shields.io/github/workflow/status/chr233/ASFEnhance/AutoBuild?logo=github)
[![GitHub Release](https://img.shields.io/github/v/release/chr233/ASFEnhance?logo=github)](https://github.com/chr233/ASFEnhance/releases)
[![GitHub Download](https://img.shields.io/github/downloads/chr233/ASFEnhance/total?logo=github)](https://img.shields.io/github/v/release/chr233/ASFEnhance)
![GitHub last commit](https://img.shields.io/github/last-commit/chr233/ASFEnhance?logo=github)

[![License](https://img.shields.io/github/license/chr233/ASFEnhance?logo=apache)](https://github.com/chr233/ASFEnhance/blob/master/license)
![GitHub Repo stars](https://img.shields.io/github/stars/chr233/ASFEnhance?logo=github)
[![Gitee Stars](https://gitee.com/chr_a1/ASFEnhance/badge/star.svg?theme=dark)](https://gitee.com/chr_a1/ASFEnhance/stargazers)

[![爱发电](https://img.shields.io/badge/爱发电-chr__-ea4aaa.svg?logo=github-sponsors)](https://afdian.net/@chr233)
[![Bilibili](https://img.shields.io/badge/bilibili-Chr__-00A2D8.svg?logo=bilibili)](https://space.bilibili.com/5805394)
[![Steam](https://img.shields.io/badge/steam-Chr__-1B2838.svg?logo=steam)](https://steamcommunity.com/id/Chr_)

[English Version](README.en.md)

ASFEnhance 介绍 & 使用指南: [https://keylol.com/t804841-1-1](https://keylol.com/t804841-1-1)

## 下载链接

> 解压后将 "ASFEnhance.dll" 丢进 ASF 目录下的 "plugins" 文件夹即可安装

[GitHub Releases](https://github.com/chr233/ASFEnhance/releases)

## 适配说明

> 使用命令 `ASFEVERSION` / `AV` 可以检查插件更新
>
> 使用命令 `ASFEUPDATE` / `AU` 可以自动更新插件到最新版本 (需要手动重启 ASF)
>
> 标 \* 代表理论上兼容但是未经测试, 如果 ASF 没有修改插件 API 理论上可以向后兼容

| ASFEnhance 版本                                          | 依赖 ASF 版本 | 5.2.4.2 | 5.2.5.7 | 5.2.6.3 | 5.2.6.3+ |
| -------------------------------------------------------- | ------------- | ------- | ------- | ------- | -------- |
| [1.6.3.x](https://github.com/chr233/ASFEnhance/releases) | 5.2.6.3       | ❌      | ✔️      | ✔️      | ✔️\*     |

<details>
  <summary>历史版本</summary>

| ASFEnhance 版本                                                            | 依赖 ASF 版本 | 5.1.2.5 | 5.2.2.5 | 5.2.3.7 | 5.2.4.2 | 5.2.5.6 |
| -------------------------------------------------------------------------- | ------------- | ------- | ------- | ------- | ------- | ------- |
| [1.5.20.381](https://github.com/chr233/ASFEnhance/releases/tag/1.5.20.381) | 5.2.5.7       |         |         |         | ❌      | ✔️      |
| [1.5.18.304](https://github.com/chr233/ASFEnhance/releases/tag/1.5.18.304) | 5.2.4.2       | ❌      | ✔️\*    | ✔️\*    | ✔️      | ✔️\*    |
| [1.5.17.289](https://github.com/chr233/ASFEnhance/releases/tag/1.5.17.289) | 5.2.4.2       | ❌      | ✔️\*    | ✔️\*    | ✔️      | ✔️\*    |
| [1.5.16.260](https://github.com/chr233/ASFEnhance/releases/tag/1.5.16.260) | 5.2.4.2       | ❌      | ✔️\*    | ✔️\*    | ✔️      | ✔️\*    |
| [1.5.15.257](https://github.com/chr233/ASFEnhance/releases/tag/1.5.15.257) | 5.2.3.7       |         | ❌      | ✔️\*    | ✔️      | ✔️      |
| [1.5.14.235](https://github.com/chr233/ASFEnhance/releases/tag/1.5.14.235) | 5.2.2.5       | ❌      | ✔️      | ✔️      | ✔️\*    |         |
| [1.5.13.231](https://github.com/chr233/ASFEnhance/releases/tag/1.5.13.231) | 5.1.2.5       | ✔️      | ❌      |         |         |         |
| [1.5.12.230](https://github.com/chr233/ASFEnhance/releases/tag/1.5.12.230) | 5.1.2.5       | ✔️      | ❌      |         |         |         |

</details>

## TODO

- [ ] 修复 `SETCOUNTRY` 命令
- [ ] 添加发送消息命令
- [ ] 添加制作补充包命令
- [ ] 添加清除通知命令

## 插件指令说明

### 实用功能

| 命令             | 缩写    | 权限            | 说明                       |
| ---------------- | ------- | --------------- | -------------------------- |
| `KEY <Text>`     | `K`     | `FamilySharing` | 从文本提取 key             |
| `ASFENHANCE`     | `ASFE`  | `FamilySharing` | 查看 ASFEnhance 的版本     |
| `ASFEHELP`       | `EHELP` | `FamilySharing` | 查看全部指令说明           |
| `HELP <Command>` | -       | `FamilySharing` | 查看指令说明               |
| `ASFEVERSION`    | `AV`    | `Owner`         | 检查 ASFEnhance 的最新版本 |
| `ASFEUPDATE`     | `AU`    | `Owner`         | 更新 ASFEnhance 到最新版本 |

### 社区相关

| 命令                           | 缩写  | 权限            | 说明                 |
| ------------------------------ | ----- | --------------- | -------------------- |
| `PROFILE [Bots]`               | `PF`  | `FamilySharing` | 查看个人资料         |
| `PROFILELINK [Bots]`           | `PFL` | `FamilySharing` | 查看个人资料链接     |
| `STEAMID [Bots]`               | `SID` | `FamilySharing` | 查看 steamID         |
| `FRIENDCODE [Bots]`            | `FC`  | `FamilySharing` | 查看好友代码         |
| `GROUPLIST [Bots]`             | `GL`  | `FamilySharing` | 查看机器人的群组列表 |
| `JOINGROUP [Bots] <GroupName>` | `JG`  | `Master`        | 加入指定群组         |
| `LEAVEGROUP [Bots] <GroupID>`  | `LG`  | `Master`        | 离开指定群组         |

> `GroupID`可以用命令`GROUPLIST`获取

### 愿望单相关

| 命令                             | 缩写 | 权限     | 说明       |
| -------------------------------- | ---- | -------- | ---------- |
| `ADDWISHLIST [Bots] <AppIDs>`    | `AW` | `Master` | 添加愿望单 |
| `REMOVEWISHLIST [Bots] <AppIDs>` | `RW` | `Master` | 移除愿望单 |

### 商店相关

| 命令                                       | 缩写   | 权限       | 说明                                             |
| ------------------------------------------ | ------ | ---------- | ------------------------------------------------ |
| `APPDETAIL [Bots] <AppIDS>`                | `AD`   | `Operator` | 获取 APP 信息, 无法获取锁区游戏信息, 仅支持`APP` |
| `SEARCH [Bots] Keywords`                   | `SS`   | `Operator` | 搜索商店                                         |
| `SUBS [Bots] <AppIDS\|SubIDS\|BundleIDS>`  | `S`    | `Operator` | 查询商店 SUB, 支持`APP/SUB/BUNDLE`               |
| `PURCHASEHISTORY`                          | `PH`   | `Operator` | 读取商店消费历史记录                             |
| `PUBLISHRECOMMENT [Bots] <AppIDS> COMMENT` | `PREC` | `Master`   | 发布评测, APPID > 0 给好评, AppID < 0 给差评     |
| `DELETERECOMMENT [Bots] <AppIDS>`          | `DREC` | `Master`   | 删除评测                                         |

### 购物车相关

> STEAM 的购物车储存在 Cookies 里,重启 ASF 将会导致购物车清空

| 命令                                 | 缩写  | 权限       | 说明                                                                      |
| ------------------------------------ | ----- | ---------- | ------------------------------------------------------------------------- |
| `CART [Bots]`                        | `C`   | `Operator` | 查看机器人购物车                                                          |
| `ADDCART [Bots] <SubIDs\|BundleIDs>` | `AC`  | `Operator` | 添加购物车, 仅能使用`SubID`和`BundleID`                                   |
| `CARTRESET [Bots]`                   | `CR`  | `Operator` | 清空购物车                                                                |
| `CARTCOUNTRY [Bots]`                 | `CC`  | `Operator` | 获取购物车可用结算区域(跟账号钱包和当前 IP 所在地有关)                    |
| `SETCOUNTRY [Bots] <CountryCode>`    | `SC`  | `Master`   | 购物车改区,可以用`CARTCOUNTRY`命令获取当前可选的`CountryCode`(仍然有 Bug) |
| `PURCHASE [Bots]`                    | `PC`  | `Owner`    | 结算机器人的购物车, 只能为机器人自己购买 (使用 Steam 钱包余额结算)        |
| `PURCHASEGIFT [BotA] BotB`           | `PCG` | `Owner`    | 结算机器人 A 的购物车, 发送礼物给机器人 B (使用 Steam 钱包余额结算)       |

> Steam 允许重复购买,使用 `PURCHASE` 命令前请自行确认有无重复内容

## ASF 命令缩写

| 命令缩写               | 等价命令                       | 说明                       |
| ---------------------- | ------------------------------ | -------------------------- |
| `AL [Bots] <Licenses>` | `ADDLICENSE [Bots] <Licenses>` | 添加免费 SUB               |
| `LA`                   | `LEVEL ASF`                    | 获取所有机器人的等级       |
| `BA`                   | `BALANCE ASF`                  | 获取所有机器人的钱包余额   |
| `PA`                   | `POINTS ASF`                   | 获取所有机器人的点数余额   |
| `P [Bots]`             | `POINTS`                       | 获取机器人的点数余额       |
| `CA`                   | `CART ASF`                     | 获取所有机器人的购物车信息 |

## 面向开发者

> 本组命令默认是禁用的.
> 需要在 `ASF.json` 中添加 `"ASFEnhanceDevFuture": true` 才能启用本组命令

| 命令                 | 权限    | 说明                      |
| -------------------- | ------- | ------------------------- |
| `COOKIES [Bots]`     | `Owner` | 查看 Steam 商店的 Cookies |
| `APIKEY [Bots]`      | `Owner` | 查看 Bot 的 APIKey        |
| `ACCESSTOKEN [Bots]` | `Owner` | 查看 Bot 的 ACCESSTOKEN   |

---

[![Repobeats analytics image](https://repobeats.axiom.co/api/embed/df6309642cc2a447195c816473e7e54e8ae849f9.svg "Repobeats analytics image")](https://github.com/chr233/ASFEnhance/pulse)

---
