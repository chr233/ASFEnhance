# ASFEnhance

[![AutoBuild][workflow_b]][workflow] [![Codacy Badge][codacy_b]][codacy] [![release][release_b]][release] [![Download][download_b]][release] [![License][license_b]][license]

[English](README.md)

---

[![Repobeats analytics image](https://repobeats.axiom.co/api/embed/df6309642cc2a447195c816473e7e54e8ae849f9.svg "Repobeats analytics image")](https://github.com/chr233/ASFEnhance/pulse)

---

> 扩展 ASF 的功能, 增加几条实用命令

发布帖：[https://keylol.com/t716051-1-1](https://keylol.com/t716051-1-1)

## 适配说明

> 因为 ASF 的接口更改, 新版插件可能无法兼容旧版本 ASF
> 兼容性更改情况请参考下表

| ASFEnhance 版本                                                            | 编译依赖的 ASF 版本 | 最低支持的 ASF 版本 | 备注                             |
| -------------------------------------------------------------------------- | ------------------- | ------------------- | -------------------------------- |
| [1.5.14.233](https://github.com/chr233/ASFEnhance/releases/tag/1.5.14.233) | 5.2.2.5             | 5.2.2.5             | 插件 API 更改, 无法兼容旧版 ASF  |
| [1.5.13.231](https://github.com/chr233/ASFEnhance/releases/tag/1.5.13.231) | 5.1.2.5             | 5.1.2.5             | -                                |
| [1.5.12.230](https://github.com/chr233/ASFEnhance/releases/tag/1.5.12.230) | 5.1.2.5             | 5.1.2.5             | 切换到 .net6.0, 无法兼容旧版 ASF |

## TODO

- [ ] 修复 `JOINGROUP` 命令
- [ ] 支持获取加入的组列表，以及退出指定的组
- [ ] 修复 `SETCOUNTRY` 命令
- [ ] 添加发送消息命令
- [ ] 添加制作补充包命令
- [ ] 添加清除通知命令

## 新增命令

### 实用功能

| 命令         | 缩写   | 权限            | 说明                   |
| ------------ | ------ | --------------- | ---------------------- |
| `KEY <Text>` | `K`    | `FamilySharing` | 从文本提取 key         |
| `ASFENHANCE` | `ASFE` | `FamilySharing` | 查看 ASFEnhance 的版本 |

### 社区相关

| 命令                          | 缩写  | 权限            | 说明         |
| ----------------------------- | ----- | --------------- | ------------ |
| `PROFILE [Bots]`              | `PF`  | `FamilySharing` | 查看个人资料 |
| `STEAMID [Bots]`              | `SID` | `FamilySharing` | 查看 steamID |
| `FRIENDCODE [Bots]`           | `FC`  | `FamilySharing` | 查看好友代码 |
| `JOINGROUP [Bots] <GroupIDs>` | `JG`  | `Master`        | 加入指定群组 |

### 愿望单相关

| 命令                             | 缩写 | 权限     | 说明       |
| -------------------------------- | ---- | -------- | ---------- |
| `ADDWISHLIST [Bots] <AppIDs>`    | `AW` | `Master` | 添加愿望单 |
| `REMOVEWISHLIST [Bots] <AppIDs>` | `RW` | `Master` | 移除愿望单 |

### 商店相关

| 命令                                       | 缩写   | 权限       | 说明                                         |
| ------------------------------------------ | ------ | ---------- | -------------------------------------------- |
| `SUBS [Bots] <AppIDS\|SubIDS\|BundleIDS>`  | `S`    | `Operator` | 查询商店 SUB, 支持`APP/SUB/BUNDLE`           |
| `PUBLISHRECOMMENT [Bots] <AppIDS> COMMENT` | `PREC` | `Master`   | 发布评测, APPID > 0 给好评, AppID < 0 给差评 |
| `DELETERECOMMENT [Bots] <AppIDS>`          | `DREC` | `Master`   | 删除评测 (有 BUG,暂不能正常工作)             |

### 购物车相关

> STEAM 的购物车储存在 Cookies 里,重启 ASF 将会导致购物车清空

| 命令                                 | 缩写 | 权限       | 说明                                                                      |
| ------------------------------------ | ---- | ---------- | ------------------------------------------------------------------------- |
| `CART [Bots]`                        | `C`  | `Operator` | 查看机器人购物车                                                          |
| `ADDCART [Bots] <SubIDs\|BundleIDs>` | `AC` | `Operator` | 添加购物车, 仅能使用`SubID`和`BundleID`                                   |
| `CARTRESET [Bots]`                   | `CR` | `Operator` | 清空购物车                                                                |
| `CARTCOUNTRY [Bots]`                 | `CC` | `Operator` | 获取购物车可用结算区域(跟账号钱包和当前 IP 所在地有关)                    |
| `SETCOUNTRY [Bots] <CountryCode>`    | `SC` | `Master`   | 购物车改区,可以用`CARTCOUNTRY`命令获取当前可选的`CountryCode`(仍然有 Bug) |
| `PURCHASE [Bots]`                    | `PC` | `Master`   | 结算机器人的购物车, 只能为机器人自己购买                                  |

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

## 下载链接

[Releases](https://github.com/chr233/ASFEnhance/releases)

[workflow_b]: https://github.com/chr233/ASFEnhance/actions/workflows/dotnet.yml/badge.svg
[workflow]: https://github.com/chr233/ASFEnhance/actions/workflows/dotnet.yml
[codacy_b]: https://app.codacy.com/project/badge/Grade/3d174e792fd4412bb6b34a77d67e5dea
[codacy]: https://www.codacy.com/gh/chr233/ASFEnhance/dashboard
[download_b]: https://img.shields.io/github/downloads/chr233/ASFEnhance/total
[release]: https://github.com/chr233/ASFEnhance/releases
[release_b]: https://img.shields.io/github/v/release/chr233/ASFEnhance
[license]: https://github.com/chr233/ASFEnhance/blob/master/license
[license_b]: https://img.shields.io/github/license/chr233/ASFEnhance
