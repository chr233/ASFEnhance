# ASFEnhance

[![Codacy Badge][codacy_b]][Codacy] [![release][release_b]][Release] [![Download][download_b]][Release] [![License][license_b]][License]

[中文说明](README.zh-CN.md)

> Extend the function of ASF, add several useful commands
>
> Require minimal version of ASF: 5.1.2.4

Post link: [https://keylol.com/t716051-1-1](https://keylol.com/t716051-1-1)

## New Commands

### 秋季促销相关

> 本组命令将在秋促结束后删除, 自动投票命令会发送10个请求, 批量使用请注意间隔时间

| Command                 | Shorthand | Access     | Description                                                                                                                                    |
| ----------------------- | --------- | ---------- | ---------------------------------------------------------------------------------------------------------------------------------------------- |
| `EVENT [Bots] [AppIDs]` | `E`       | `Operator` | Vote Steam Awards, gameIDs use "," to split, totaly 10 games needed, if you don't give enough games, it will use built-in random games instead |
| `EVENTCHECK [Bots]`     | `EC`      | `Operator` | Check summer sale badge tasks status                                                                                                           |

EVENT Usage:

```txt
> EVENT ASF 1091500,1523370,578080,1426210,1506980,1195290,1369630,1639930,1111460,1366540
< <bot> Nominate at least 1 game: √ Nominate a game in each category: √

> EVENTCHECK ASF
< <bot> Nominate at least 1 game: √ Nominate a game in each category: √ Play a game you nominated: × Review or update your review of a game you nominated: √
```

### Common Commands

| Command             | Shorthand | Access          | Description                                                      |
| ------------------- | --------- | --------------- | ---------------------------------------------------------------- |
| `KEY <Text>`        | `K`       | `Any`           | Extract keys from plain text                                     |
| `PROFILE [Bots]`    | `PF`      | `FamilySharing` | Get bot's profile infomation                                     |
| `STEAMID [Bots]`    | `SID`     | `FamilySharing` | Get bot's steamID                                                |
| `FRIENDCODE [Bots]` | `FC`      | `FamilySharing` | Get bot's friend code                                            |
| `COOKIES [Bots]`    | -         | `Master`        | Get bot's steam cookies(only for debug, don't leak your cookies) |
| `ASFENHANCE`        | `ASFE`    | `Any`           | Get the version of the ASFEnhance                                |

### Wishlist Commands

| Command                          | Shorthand | Access   | Description                     |
| -------------------------------- | --------- | -------- | ------------------------------- |
| `ADDWISHLIST [Bots] <AppIDs>`    | `AW`      | `Master` | Add game to bot's wishlist      |
| `REMOVEWISHLIST [Bots] <AppIDs>` | `RW`      | `Master` | Delete game from bot's wishlist |

### Store Commands

| Command                                   | Shorthand | Access     | Description                                                  |
| ----------------------------------------- | --------- | ---------- | ------------------------------------------------------------ |
| `SUBS [Bots] <AppIDS\|SubIDS\|BundleIDS>` | `S`       | `Operator` | Get available subs from store page, support `APP/SUB/BUNDLE` |

### Cart Commands

> Steam saves cart information via cookies, restart bot instance will let shopping cart being emptied

| Command                              | Shorthand | Access     | Description                                                                    |
| ------------------------------------ | --------- | ---------- | ------------------------------------------------------------------------------ |
| `CART [Bots]`                        | `C`       | `Operator` | Get bot's cart information                                                     |
| `ADDCART [Bots] <SubIDs\|BundleIDs>` | `AC`      | `Operator` | Add game to bot's cart, only support `SUB/BUNDLE`                              |
| `CARTRESET [Bots]`                   | `CR`      | `Operator` | Clear bot's cart                                                               |
| `CARTCOUNTRY [Bots]`                 | `CC`      | `Operator` | Get bot's available currency area (Depends to wallet area and the IP location) |
| `SETCOUNTRY [Bots] <CountryCode>`    | `SC`      | `Operator` | Set bot's currency area (NOT WORKING, WIP)                                     |
| `PURCHASE [Bots]`                    | `PC`      | `Master`   | Purchase bot's items for it self                                               |

> Steam allows duplicate purchases, please check cart before using PURCHASE command.

## Shorthand Commands

| Shorthand              | Equivalent Command             | Description                    |
| ---------------------- | ------------------------------ | ------------------------------ |
| `AL [Bots] <Licenses>` | `ADDLICENSE [Bots] <Licenses>` | Add free `SUB`                 |
| `LA`                   | `LEVEL ASF`                    | Get All bot's level            |
| `BA`                   | `BALANCE ASF`                  | Get All bot's wallet balance   |
| `PA`                   | `POINTS ASF`                   | Get All bot's points balance   |
| `P [Bots]`             | `POINTS`                       | Get bot's points balance       |
| `CA`                   | `CART ASF`                     | Get All bot's cart information |

## Download Link

[Releases](https://github.com/chr233/ASFEnhance/releases)

[codacy_b]: https://app.codacy.com/project/badge/Grade/3d174e792fd4412bb6b34a77d67e5dea
[codacy]: https://www.codacy.com/gh/chr233/ASFEnhance/dashboard
[download_b]: https://img.shields.io/github/downloads/chr233/ASFEnhance/total
[release]: https://github.com/chr233/ASFEnhance/releases
[release_b]: https://img.shields.io/github/v/release/chr233/ASFEnhance
[license]: https://github.com/chr233/ASFEnhance/blob/master/license
[license_b]: https://img.shields.io/github/license/chr233/ASFEnhance
