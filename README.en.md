# ASFEnhance

[![Codacy Badge](https://app.codacy.com/project/badge/Grade/3d174e792fd4412bb6b34a77d67e5dea)](https://www.codacy.com/gh/chr233/ASFEnhance/dashboard)
![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/chr233/ASFEnhance/publish.yml?logo=github)
[![License](https://img.shields.io/github/license/chr233/ASFEnhance?logo=apache)](https://github.com/chr233/ASFEnhance/blob/master/license)

[![GitHub Release](https://img.shields.io/github/v/release/chr233/ASFEnhance?logo=github)](https://github.com/chr233/ASFEnhance/releases)
[![GitHub Release](https://img.shields.io/github/v/release/chr233/ASFEnhance?include_prereleases&label=pre-release&logo=github)](https://github.com/chr233/ASFEnhance/releases)
![GitHub last commit](https://img.shields.io/github/last-commit/chr233/ASFEnhance?logo=github)

![GitHub Repo stars](https://img.shields.io/github/stars/chr233/ASFEnhance?logo=github)
[![GitHub Download](https://img.shields.io/github/downloads/chr233/ASFEnhance/total?logo=github)](https://img.shields.io/github/v/release/chr233/ASFEnhance)

[![Bilibili](https://img.shields.io/badge/bilibili-Chr__-00A2D8.svg?logo=bilibili)](https://space.bilibili.com/5805394)
[![Steam](https://img.shields.io/badge/steam-Chr__-1B2838.svg?logo=steam)](https://steamcommunity.com/id/Chr_)

[![Steam](https://img.shields.io/badge/steam-donate-1B2838.svg?logo=steam)](https://steamcommunity.com/tradeoffer/new/?partner=221260487&token=xgqMgL-i)
[![爱发电](https://img.shields.io/badge/爱发电-chr__-ea4aaa.svg?logo=github-sponsors)](https://afdian.net/@chr233)

[中文说明](README.md) | [Русская Версия](README.ru.md)

## EULA

> Please don't use this plugin to conduct repulsive behaviors, including but not limited to: post fake reviews, posting advertisements, etc.
>
> See [Plugin Configuration](#plugin-configuration)

## EVENT COMMAND

> This group of commands is only available in a limited time, and will be removed when next version of this plugin published if it lose efficacy

| Command                    | Shorthand | Access     | Description                                                                                                                                                                    |
| -------------------------- | --------- | ---------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| `CLAIM20TH [Bots]`         |           | `Operator` | Claim 20th free item in the Point shop                                                                                                                                         |
| `C20 [Bots]`               |           | `Operator` | Same as `CLAIM20TH`                                                                                                                                                            |
| `CLAIMITEM [Bots]`         |           | `Operator` | Claim sale event's item, such as stickers or something else                                                                                                                    |
| `CI [Bots]`                |           | `Operator` | Same as `CLAIMITEM`                                                                                                                                                            |
| `SIM4 [Bots]`              |           | `Operator` | Claim the `The Sims™ 4` stickers [url](https://store.steampowered.com/sale/simscelebrationsale)                                                                                |
| `DL2 [Bots]`               |           | `Operator` | Claim the `Dying Light 2 Stay Human` items [url](https://store.steampowered.com/sale/dyinglight)                                                                               |
| `DL22 [Bots] [Sticker Id]` |           | `Operator` | Claim the `Dying Light 2 Stay Human` items, `Sticker Id` is not required, value can be 1 to 8 [url](https://store.steampowered.com/developer/Techland/sale/techlandsummer2023) |
| `RLE [Bots] [Sticker Id]`  |           | `Operator` | Claim the `Redfall Launch Event` items, `Sticker Id` is not required, value can be 1 to 4 [url](https://store.steampowered.com/sale/redfall_launch)                            |
| `VOTE [Bots] <AppIds>`     | `V`       | `Operator` | 为 `STEAM 大奖` 投票, AppIds 最多指定 10 个游戏, 未指定或 AppIds 不足 11 个时不足部分将使用内置 AppId 进行投票                                                                 |
| `CHECKVOTE [Bots]`         | `CV`      | `Operator` | 获取 `STEAM 大奖` 徽章任务完成情况                                                                                                                                             |

## Installation

### First-Time Install / Manually Update

1. Download the plugin via [GitHub Releases](https://github.com/chr233/ASFEnhance/releases) page
2. Unzip the `ASFEnhance.dll` and copy it into the `plugins` folder in the `ArchiSteamFarm`'s directory
3. Restart the `ArchiSteamFarm` and use `ASFEnhance` or `ASFE` command to check if the plugin is working

### Sub Module

> After ASFEnhance 2.0.0.0, its contains a sub module system, provides command manager and plugin update service

Supported Plugin List:

- [ASFBuffBot](https://github.com/chr233/ASFBuffBot) (Bugfix WIP)
- [ASFOAuth](https://github.com/chr233/ASFOAuth)
- [ASFTradeExtension](https://github.com/chr233/ASFTradeExtension) (Bugfix WIP)
- [ASFAchievementManagerEx](https://github.com/chr233/ASFAchievementManagerEx) (Bugfix WIP)
- ...

> Demo: [ASFEnhanceAdapterDemoPlugin](https://github.com/chr233/ASFEnhanceAdapterDemoPlugin)

### Plugin Update & Sub Module Update

| Command                        | Shorthand | Access     | Description                                                          |
| ------------------------------ | --------- | ---------- | -------------------------------------------------------------------- |
| `PLUGINSLIST`                  | `PL`      | `Operator` | 获取当前安装的插件列表, 末尾带 [] 的为可被 ASFEnhance 管理的子模块   |
| `PLUGINLIST`                   | -         | `Operator` | 同 `PLUGINSLIST`                                                     |
| `PLUGINSVERSION [Plugin Name]` | `PV`      | `Master`   | 获取指定模块的版本信息, 未指定插件名时检查所有受支持的插件的版本信息 |
| `PLUGINVERSION`                | -         | `Master`   | 同 `PLUGINSVERSION`                                                  |
| `PLUGINSUPDATE [Plugin Name]`  | `PU`      | `Master`   | 自动更新指定模块, 未指定插件名时自动更新所有受支持的插件             |
| `PLUGINUPDATE`                 | -         | `Master`   | 同 `PLUGINSUPDATE`                                                   |

### ChangeLog

| ASFEnhance Version                                                   | Depended ASF Version | Description                                          |
| -------------------------------------------------------------------- | :------------------: | ---------------------------------------------------- |
| [2.0.1.3](https://github.com/chr233/ASFEnhance/releases/tag/2.0.1.3) |       5.4.12.5       | 新增 `VOTE` `CHECKVOTE` 命令                         |
| [2.0.0.0](https://github.com/chr233/ASFEnhance/releases/tag/2.0.0.0) |       5.4.12.5       | ASF -> 5.4.12.5, 新的子模块系统, 新增 `EMAIL` 命令等 |

<details>
  <summary>History Version</summary>

> ASF 5.4.10.3 以及之前的版本因为 Steam 的改动已经无法使用, 请使用新版本的 ASF 和 ASFEnhance

| ASFEnhance Version                                                     | Depended ASF Version | 5.4.7.3 | 5.4.8.3 | 5.4.9.3 |
| ---------------------------------------------------------------------- | :------------------: | :-----: | :-----: | :-----: |
| [1.8.13.0](https://github.com/chr233/ASFEnhance/releases/tag/1.8.13.0) |       5.4.10.3       |   ❌    |   ✔️    |   ✔️    |
| [1.8.12.2](https://github.com/chr233/ASFEnhance/releases/tag/1.8.12.2) |       5.4.9.3        |         |   ✔️    |   ✔️    |
| [1.8.11.1](https://github.com/chr233/ASFEnhance/releases/tag/1.8.11.1) |       5.4.9.3        |         |   ✔️    |   ✔️    |
| [1.8.10.0](https://github.com/chr233/ASFEnhance/releases/tag/1.8.10.0) |       5.4.9.3        |         |   ✔️    |   ✔️    |
| [1.8.9.0](https://github.com/chr233/ASFEnhance/releases/tag/1.8.9.0)   |       5.4.9.3        |         |   ✔️    |   ✔️    |
| [1.8.8.0](https://github.com/chr233/ASFEnhance/releases/tag/1.8.8.0)   |       5.4.8.3        |   ❌    |   ✔️    |   ✔️    |

| ASFEnhance Version                                                   | Depended ASF | 5.4.7.3 | 5.4.8.3 | 5.4.9.3 |
| -------------------------------------------------------------------- | :----------: | :-----: | :-----: | :-----: |
| [1.8.8.0](https://github.com/chr233/ASFEnhance/releases/tag/1.8.8.0) |   5.4.8.3    |   ❌    |   ✔️    |   ✔️    |

| ASFEnhance Version                                                   | Depended ASF | 5.4.5.2 | 5.4.6.3 | 5.4.7.3 | 5.4.8.3 |
| -------------------------------------------------------------------- | :----------: | :-----: | :-----: | :-----: | :-----: |
| [1.8.7.1](https://github.com/chr233/ASFEnhance/releases/tag/1.8.7.1) |   5.4.7.3    |         |         |   ✔️    |   ❌    |
| [1.8.6.2](https://github.com/chr233/ASFEnhance/releases/tag/1.8.6.2) |   5.4.7.3    |         |         |   ✔️    |   ❌    |
| [1.8.5.0](https://github.com/chr233/ASFEnhance/releases/tag/1.8.5.0) |   5.4.7.3    |         |         |   ✔️    |   ❌    |
| [1.8.4.0](https://github.com/chr233/ASFEnhance/releases/tag/1.8.4.0) |   5.4.7.2    |         |         |   ✔️    |   ❌    |
| [1.8.3.0](https://github.com/chr233/ASFEnhance/releases/tag/1.8.3.0) |   5.4.6.3    |         |   ✔️    |   ✔️    |   ❌    |
| [1.8.2.0](https://github.com/chr233/ASFEnhance/releases/tag/1.8.2.0) |   5.4.6.3    |         |   ✔️    |   ✔️    |   ❌    |
| [1.8.1.3](https://github.com/chr233/ASFEnhance/releases/tag/1.8.1.3) |   5.4.5.2    |   ✔️    |   ✔️    |   ✔️    |   ❌    |

| ASFEnhance Version                                                     | Depended ASF | 5.4.1.11 | 5.4.2.13 | 5.4.3.2 | 5.4.4.x |
| ---------------------------------------------------------------------- | :----------: | :------: | :------: | :-----: | :-----: |
| [1.7.25.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.25.0) |   5.4.4.5    |          |    ❌    |   ❌    |   ✔️    |
| [1.7.24.1](https://github.com/chr233/ASFEnhance/releases/tag/1.7.24.1) |   5.4.4.5    |          |    ❌    |   ❌    |   ✔️    |
| [1.7.23.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.23.0) |   5.4.4.5    |          |    ❌    |   ❌    |   ✔️    |
| [1.7.22.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.22.0) |   5.4.4.5    |          |    ❌    |   ❌    |   ✔️    |
| [1.7.21.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.21.0) |   5.4.4.4    |          |    ❌    |   ❌    |   ✔️    |
| [1.7.20.1](https://github.com/chr233/ASFEnhance/releases/tag/1.7.20.1) |   5.4.4.3    |          |    ❌    |   ❌    |   ✔️    |
| [1.7.19.1](https://github.com/chr233/ASFEnhance/releases/tag/1.7.19.1) |   5.4.3.2    |          |    ❌    |   ❌    |   ✔️    |
| [1.7.18.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.18.0) |   5.4.2.13   |          |    ❌    |   ❌    |   ✔️    |
| [1.7.17.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.17.0) |   5.4.2.13   |    ❌    |    ✔️    |   ✔️    |         |
| [1.7.16.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.16.0) |   5.4.2.13   |    ❌    |    ✔️    |   ✔️    |         |
| [1.7.15.2](https://github.com/chr233/ASFEnhance/releases/tag/1.7.15.2) |   5.4.2.13   |    ❌    |    ✔️    |   ✔️    |         |

| ASFEnhance Version                                                     | Depended ASF | 5.3.1.2 | 5.3.2.4 | 5.4.0.3 | 5.4.1.11 |
| ---------------------------------------------------------------------- | :----------: | :-----: | :-----: | :-----: | :------: |
| [1.7.12.1](https://github.com/chr233/ASFEnhance/releases/tag/1.7.12.1) |   5.4.1.11   |   ❌    |   ❌    |   ❌    |    ✔️    |
| [1.7.11.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.11.0) |   5.4.1.11   |   ❌    |   ❌    |   ❌    |    ✔️    |
| [1.7.10.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.10.0) |   5.4.1.11   |   ❌    |   ❌    |   ❌    |    ✔️    |
| [1.7.9.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.9.0)   |   5.4.1.11   |   ❌    |   ❌    |   ❌    |    ✔️    |
| [1.7.8.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.8.0)   |   5.4.1.11   |   ❌    |   ❌    |   ❌    |    ✔️    |
| [1.7.7.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.7.0)   |   5.4.1.11   |   ❌    |   ❌    |   ❌    |    ✔️    |
| [1.7.6.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.6.0)   |   5.4.0.3    |   ❌    |   ❌    |   ✔️    |    ✔️    |
| [1.7.5.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.5.0)   |   5.4.0.3    |   ❌    |   ❌    |   ✔️    |    ✔️    |
| [1.7.4.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.4.0)   |   5.4.0.3    |   ❌    |   ❌    |   ✔️    |    ✔️    |
| [1.7.3.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.3.0)   |   5.4.0.3    |   ❌    |   ❌    |   ✔️    |    ✔️    |
| [1.7.2.1](https://github.com/chr233/ASFEnhance/releases/tag/1.7.2.1)   |   5.4.0.3    |   ❌    |   ❌    |   ✔️    |    ✔️    |
| [1.7.1.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.1.0)   |   5.4.0.3    |   ❌    |   ❌    |   ✔️    |    ✔️    |
| [1.7.0.1](https://github.com/chr233/ASFEnhance/releases/tag/1.7.0.1)   |   5.4.0.3    |   ❌    |   ❌    |   ✔️    |    ✔️    |
| [1.6.23.0](https://github.com/chr233/ASFEnhance/releases/tag/1.6.23.0) |   5.3.2.4    |   ❌    |   ✔️    |   ✔️    |          |
| [1.6.22.1](https://github.com/chr233/ASFEnhance/releases/tag/1.6.22.1) |   5.3.2.4    |   ❌    |   ✔️    |   ✔️    |          |
| [1.6.21.6](https://github.com/chr233/ASFEnhance/releases/tag/1.6.21.6) |   5.3.2.4    |   ❌    |   ✔️    |   ✔️    |          |
| [1.6.20.1](https://github.com/chr233/ASFEnhance/releases/tag/1.6.20.1) |   5.3.2.4    |   ❌    |   ✔️    |   ✔️    |          |
| [1.6.19.4](https://github.com/chr233/ASFEnhance/releases/tag/1.6.19.4) |   5.3.2.4    |   ❌    |   ✔️    |   ✔️    |          |
| [1.6.18.1](https://github.com/chr233/ASFEnhance/releases/tag/1.6.18.1) |   5.3.2.4    |   ❌    |   ✔️    |   ✔️    |          |

| ASFEnhance Version                                                         | Depended ASF | 5.2.6.3 | 5.2.7.7 | 5.2.8.4 | 5.3.0.3 | 5.3.1.2 |
| -------------------------------------------------------------------------- | :----------: | :-----: | :-----: | :-----: | :-----: | :-----: |
| [1.6.18.0](https://github.com/chr233/ASFEnhance/releases/tag/1.6.18.0)     |   5.3.1.2    |   ❌    |   ❌    |   ❌    |   ✔️    |   ✔️    |
| [1.6.16.0](https://github.com/chr233/ASFEnhance/releases/tag/1.6.16.0)     |   5.3.1.2    |   ❌    |   ❌    |   ❌    |   ✔️    |   ✔️    |
| [1.6.15.0](https://github.com/chr233/ASFEnhance/releases/tag/1.6.15.0)     |   5.3.1.2    |   ❌    |   ❌    |   ❌    |   ✔️    |   ✔️    |
| [1.6.14.0](https://github.com/chr233/ASFEnhance/releases/tag/1.6.14.0)     |   5.3.1.2    |   ❌    |   ❌    |   ❌    |   ✔️    |   ✔️    |
| [1.6.12.717](https://github.com/chr233/ASFEnhance/releases/tag/1.6.12.717) |   5.3.1.2    |   ❌    |   ❌    |   ❌    |   ✔️    |   ✔️    |
| [1.6.11.670](https://github.com/chr233/ASFEnhance/releases/tag/1.6.11.670) |   5.3.1.2    |   ❌    |   ❌    |   ❌    |   ✔️    |   ✔️    |
| [1.6.10.666](https://github.com/chr233/ASFEnhance/releases/tag/1.6.10.666) |   5.3.0.3    |   ❌    |   ❌    |   ❌    |   ✔️    |  ✔️\*   |
| [1.6.9.663](https://github.com/chr233/ASFEnhance/releases/tag/1.6.9.663)   |   5.2.8.4    |   ❌    |   ❌    |   ✔️    |   ❌    |         |
| [1.6.8.661](https://github.com/chr233/ASFEnhance/releases/tag/1.6.8.661)   |   5.2.7.7    |   ❌    |   ✔️    |         |         |         |

</details>

## Plugin Configuration

> The configuration of this plugin is not required, and most functions is avilable in default settings

ASF.json

```json
{
  //ASF Configuration
  "CurrentCulture": "...",
  "IPCPassword": "...",
  "...": "...",
  //ASFEnhance Configuration
  "ASFEnhance": {
    "EULA": true,
    "Statistic": true,
    "DevFeature": false,
    "DisabledCmds": ["foo", "bar"],
    "Address": {
      "Address": "Address",
      "City": "City",
      "Country": "US",
      "State": "NE",
      "PostCode": "12345"
    },
    "Addresses": [
      {
        "Address": "Address",
        "City": "City",
        "Country": "US",
        "State": "NE",
        "PostCode": "12345"
      }
    ]
  }
}
```

| Configuration     | Type   | Default | Description                                                                                                                  |
| ----------------- | ------ | ------- | ---------------------------------------------------------------------------------------------------------------------------- |
| `EULA`            | `bool` | `true`  | If agree the [EULA](#EULA)\*                                                                                                 |
| `Statistic`       | `bool` | `true`  | Allow send statistics data, it's used to count number of users, this will not send any other information                     |
| `DevFeature`      | `bool` | `false` | Enabled developer feature (3 Commands) `May causing security risk, turn on with caution`                                     |
| `DisabledCmds`    | `list` | `null`  | Optional, Cmd in the list will be disabled\*\* , **Case Insensitive**, only effects on `ASFEnhance` cmds                     |
| `Address`\*\*\*   | `dict` | `null`  | Optional, single billing address, when using `REDEEMWALLET` cmd requires billing address, plugin will use configured address |
| `Addresses`\*\*\* | `list` | `null`  | Optional, multiple billing addresses, the same as `Address`                                                                  |

> \* When Agree [EULA](#EULA), ASFEnhance will let all commands avilable, in exchange, ASFEnhance will follow the author's [Curator](https://store.steampowered.com/curator/39487086/) and [Group](https://steamcommunity.com/groups/11012580) when execute `GROUPLIST` and `CURATORLIST` commands (if bot not following or joined)
>
> \* When Disagree [EULA](#EULA), ASFEnhance will limit features of curator/follow game/group/reviews, and ASFEnhance will not follow [Curator](https://store.steampowered.com/curator/39487086/) and [Group](https://steamcommunity.com/groups/11012580)
>
> \*\* `DisabledCmds` description: every item in this configuration is **Case Insensitive**, and this only effects on `ASFEnhance` cmds
> For example, configure as `["foo","BAR"]` , it means `FOO` and `BAR` will be disabled
> If don't want to disable any cmds, please configure as `null` or `[]`
> If Some cmd is disabled, it's still avilable to call the command in the form of `ASFE.xxx`, for example `ASFE.EXPLORER`
>
> \*\*\* `Address` and `Addresses` is the same configuration

## Commands Usage

### Update Commands

| Command       | Shorthand | Access          | Description                                                         |
| ------------- | --------- | --------------- | ------------------------------------------------------------------- |
| `ASFENHANCE`  | `ASFE`    | `FamilySharing` | Get the version of the ASFEnhance                                   |
| `ASFEVERSION` | `AV`      | `Owner`         | Check ASFEnhance's latest version                                   |
| `ASFEUPDATE`  | `AU`      | `Owner`         | Update ASFEnhance to the latest version (need restart ASF manually) |

### Account Commands

| Command                                   | Shorthand | Access     | Description                                                                             |
| ----------------------------------------- | --------- | ---------- | --------------------------------------------------------------------------------------- |
| `PURCHASEHISTORY [Bots]`                  | `PH`      | `Operator` | Get bot's purchase history.                                                             |
| `FREELICENSES [Bots]`                     | `FL`      | `Operator` | Get bot's all free sub licenses list                                                    |
| `FREELICENSE [Bots]`                      |           |            | Same as `FREELICENSES`                                                                  |
| `LICENSES [Bots]`                         | `L`       | `Operator` | Get bot's all licenses list                                                             |
| `LICENSE [Bots]`                          |           |            | Same as `LICENSES`                                                                      |
| `REMOVEDEMOS [Bots]`                      | `RD`      | `Master`   | Remove bot's all demo licenses                                                          |
| `REMOVEDEMO [Bots]`                       |           |            | Same as `REMOVEDEMOS`                                                                   |
| `REMOVELICENSES [Bots] <SubIDs>`          | `RL`      | `Master`   | Remove bot's licenses with the specified subIDs                                         |
| `REMOVELICENSE [Bots] <SubIDs>`           |           |            | Same as `REMOVELICENSES`                                                                |
| `EMAILOPTIONS [Bots]`                     | `EO`      | `Operator` | Get bot's email preferences [url](https://store.steampowered.com/account/emailoptout)   |
| `EMAILOPTION [Bots]`                      |           |            | Same as `EMAILOPTIONS`                                                                  |
| `SETEMAILOPTIONS [Bots] <Options>`        | `SEO`     | `Master`   | Set bot's email preferences                                                             |
| `SETEMAILOPTION [Bots] <Options>`         |           |            | Same as `SETEMAILOPTIONS`                                                               |
| `NOTIFICATIONOPTIONS [Bots]`              | `NOO`     | `Operator` | 读取账户中的通知选项 [url](https://store.steampowered.com/account/notificationsettings) |
| `NOTIFICATIONOPTION [Bots]`               |           |            | 同 `NOTIFICATIONOPTIONS`                                                                |
| `SETNOTIFICATIONOPTIONS [Bots] <Options>` | `SNOO`    | `Master`   | 设置账户中的通知选项                                                                    |
| `SETNOTIFICATIONOPTION [Bots] <Options>`  |           |            | 同 `SETNOTIFICATIONOPTIONS`                                                             |
| `GETBOTBANNED [Bots]`                     | `GBB`     | `Operator` | 获取机器人的账户封禁情况                                                                |
| `GETBOTBANN [Bots]`                       |           |            | 同 `GETBOTBANNED`                                                                       |
| `GETACCOUNTBANNED <SteamIds>`             | `GBB`     | `Operator` | 获取指定账户封禁情况, 支持 SteamId 64 / SteamId 32                                      |
| `GETACCOUNTBAN <SteamIds>`                |           |            | 同 `GETACCOUNTBANNED`                                                                   |

- `SETEMAILOPTION` arguments explanation

  `<Options>` receives at most 9 arguments, use space or `,` to split, the order is same as [this page](https://store.steampowered.com/account/emailoptout)
  For each argument, if it is among the `on`, `yes`, `true`, `1`, `y`, that means enable, otherwise disable (default).

| Index | Name                                                    | Description                                  |
| ----- | ------------------------------------------------------- | -------------------------------------------- |
| 1     | Enable email notification                               | If disabled, other arguments will be ignored |
| 2     | Send email when a item in wishlist has a discount       |                                              |
| 3     | Send email when a item in wishlist has released         |                                              |
| 4     | Send email when a greenlight item has released          |                                              |
| 5     | Send email when followed publishers has released a item |                                              |
| 6     | Send email when sesonal promotion started               |                                              |
| 7     | Send email when receives a review copy of a curator     |                                              |
| 8     | Send email when receives Steam Community Awards         |                                              |
| 9     | Send email when there has a game-specific event         |                                              |

- `SETNOTIFICATIONS` 参数说明

  `<Options>` 参数接受最多 9 个参数, 使用空格或者 `,` 分隔, 顺序参照 [url](https://store.steampowered.com/account/notificationsettings)
  索引含义和设置值可选的范围见下表

| Index | Name                                     |
| ----- | ---------------------------------------- |
| 1     | I receive a gift                         |
| 2     | A discussion I subscribed to has a reply |
| 3     | I receive a new item in my inventory     |
| 4     | I receive a friend invitation            |
| 5     | There's a major sale                     |
| 6     | An item on my wishlist is on sale        |
| 7     | I receive a new trade offer              |
| 8     | I receive a reply from Steam Support     |
| 9     | I receive a Steam Turn notification      |

| Option | Description                                                                                       |
| ------ | ------------------------------------------------------------------------------------------------- |
| 0      | Disable notifications                                                                             |
| 1      | Enable notifications                                                                              |
| 2      | Enable notifications, Toast notification in the Steam Client                                      |
| 3      | Enable notifications, Push notification in the Mobile App                                         |
| 4      | Enable notifications, Toast notification in the Steam Client, Push notification in the Mobile App |

### Other Commands

| Command          | Shorthand | Access          | Description                  |
| ---------------- | --------- | --------------- | ---------------------------- |
| `KEY <Text>`     | `K`       | `Any`           | Extract keys from plain text |
| `ASFEHELP`       | `EHELP`   | `FamilySharing` | Get all command usage        |
| `HELP <Command>` | -         | `FamilySharing` | Get command usage            |

## Group Commands

| Command                       | Shorthand | Access          | Description                      |
| ----------------------------- | --------- | --------------- | -------------------------------- |
| `GROUPLIST [Bots]`            | `GL`      | `FamilySharing` | Get bot's joined group list      |
| `JOINGROUP [Bots] <GroupUrl>` | `JG`      | `Master`        | Let bot to join specified group  |
| `LEAVEGROUP [Bots] <GroupID>` | `LG`      | `Master`        | Let bot to leave specified group |

> `GroupID` can be found using `GROUPLIST` command

## Profile Commands

| Command                                | Shorthand | Access          | Description                                                                                         |
| -------------------------------------- | --------- | --------------- | --------------------------------------------------------------------------------------------------- |
| `PROFILE [Bots]`                       | `PF`      | `FamilySharing` | Get bot's profile infomation                                                                        |
| `PROFILELINK [Bots]`                   | `PFL`     | `FamilySharing` | Get bot's profile link                                                                              |
| `STEAMID [Bots]`                       | `SID`     | `FamilySharing` | Get bot's steamID                                                                                   |
| `FRIENDCODE [Bots]`                    | `FC`      | `FamilySharing` | Get bot's friend code                                                                               |
| `TRADELINK [Bots]`                     | `TL`      | `Operator`      | Get bot's trade link                                                                                |
| `REPLAY [Bots]`                        | `RP`      | `Operator`      | Get bot's «Steam Awards 2022» banner link (can get badge)                                           |
| `REPLAYPRIVACY [Bots] Privacy`         | `RPP`     | `Operator`      | Set privacy settings for `Steam Replay 2022`. `Privacy`: `1=Private` `2=Only friends` `3=Public`    |
| `CLEARALIAS [Bots]`                    |           | `Opetator`      | Clear history of previous names                                                                     |
| `GAMEAVATAR [Bots] <AppID> [AvatarID]` | `GA`      | `Master`        | Set bot's avatar as given `AppID` and `AvatarID`, if not set `AvatarId`, plugin will use random one |
| `RANDOMGAMEAVATAR [Bots]`              | `RGA`     | `Master`        | Set bot's avatar randomly                                                                           |
| `ADVNICKNAME [Bots] Query`             | `ANN`     | `Master`        | Set bot's nickname use `Placeholder`, avilable: `%dn%` `%ln%` `%un%` `%botn%`, case insensitive     |
| `SETAVATAR [Bots] ImageUrl` 🐞         | `GA`      | `Master`        | Set bot's avatar to specified online image                                                          |
| `DELETEAVATAR [Bots]` 🐞               |           | `Master`        | Delete bot's avatar (reset to default)                                                              |
| `CRAFTBADGE [Bots]`                    | `CB`      | `Master`        | Automatically craft craftable badges (craft every craftable badge once at one time)                 |

\*🐞: Required generic version of ASF (**not** generic-netf)

- GAMEAVATAR Description

All avatars are from [Game Avatars Page](https://steamcommunity.com/actions/GameAvatars/)

---

- ADVNICKNAME Query Description

> "n" means any number

| Placeholder | Description                    | Demo                       |
| ----------- | ------------------------------ | -------------------------- |
| `%d%`       | Random digit                   | `5`                        |
| `%dn%`      | n Random digits                | `%d6%` -> `114514`         |
| `%l%`       | Random lowercase letter        | `x`                        |
| `%ln%`      | n Random lowercase letters     | `%d7%` -> `asfeadf`        |
| `%u%`       | Random uppercase letter        | `C`                        |
| `%un%`      | n Random uppercase letters     | `%d8%` -> `ASXCGDFA`       |
| `%bot%`     | bot's nickname                 | `ASFE`                     |
| `%bot3%`    | bot's nickname, repeat 3 times | `%bot3%` -> `ASFEASFEASFE` |

### Curator Commands

| Command                          | Shorthand | Access   | Description                      |
| -------------------------------- | --------- | -------- | -------------------------------- |
| `CURATORLIST [Bots]`             | `CL`      | `Master` | Get bot's following curator list |
| `FOLLOWCURATOR [Bots] <ClanIDs>` | `FCU`     | `Master` | Follow specified curator         |
| `UNFOLLOWCURATOR [Bots]`         | `UFC`     | `Master` | Unfollow specified curator       |
| `UNFOLLOWALLCURATORS [Bots]`     | `UFACU`   | `Master` | Unfollow all curators            |
| `UNFOLLOWALLCURATOR [Bots]`      |           |          | Same as `UNFOLLOWALLCURATORS`    |

> `ClanID` can be found in curator's web link or using `CURATORLIST` command

### Wishlist Commands

| Command                          | Shorthand | Access   | Description                                    |
| -------------------------------- | --------- | -------- | ---------------------------------------------- |
| `ADDWISHLIST [Bots] <AppIDs>`    | `AW`      | `Master` | Add game to bot's wishlist                     |
| `REMOVEWISHLIST [Bots] <AppIDs>` | `RW`      | `Master` | Delete game from bot's wishlist                |
| `FOLLOWGAME [Bots] <AppIDs>`     | `FG`      | `Master` | Follow specified game                          |
| `UNFOLLOWGAME [Bots] <AppIDs>`   | `UFG`     | `Master` | Unfollow specified game                        |
| `CHECK [Bots] <AppIDs>`          | `CK`      | `Master` | Check if following / wishlisted specified game |

### Store Commands

| Command                                    | Shorthand | Access     | Description                                                                          |
| ------------------------------------------ | --------- | ---------- | ------------------------------------------------------------------------------------ |
| `APPDETAIL [Bots] <AppIDs>`                | `AD`      | `Operator` | Get app detail from steam API, support `APP`                                         |
| `SEARCH [Bots] Keywords`                   | `SS`      | `Operator` | Search in Steam store                                                                |
| `SUBS [Bots] <AppIDs\|SubIDs\|BundleIDs>`  | `S`       | `Operator` | Get available subs from store page, support `APP/SUB/BUNDLE`                         |
| `PUBLISHRECOMMENT [Bots] <AppIDs> COMMENT` | `PREC`    | `Operator` | Publish a recomment for game, `appd` or `+appId` rateUp, `-appId` rateDown           |
| `DELETERECOMMENT [Bots] <AppIDs>`          | `DREC`    | `Operator` | Delete a recomment for game                                                          |
| `REQUESTACCESS [Bots] <AppIDs>`            | `RA`      | `Operator` | Send join playtest request to specified appIDs, equivalent to click `Request Access` |
| `VIEWPAGE [Bots] Url`                      | `VP`      | `Operator` | Visit the specified page                                                             |

### Cart Commands

> Steam saves cart information via cookies, restart bot instance will let shopping cart being emptied

| Command                              | Shorthand | Access     | Description                                                                              |
| ------------------------------------ | --------- | ---------- | ---------------------------------------------------------------------------------------- |
| `CART [Bots]`                        | `C`       | `Operator` | Get bot's cart information                                                               |
| `ADDCART [Bots] <SubIDs\|BundleIDs>` | `AC`      | `Operator` | Add game to bot's cart, only support `SUB/BUNDLE`                                        |
| `CARTRESET [Bots]`                   | `CR`      | `Operator` | Clear bot's cart                                                                         |
| `CARTCOUNTRY [Bots]`                 | `CC`      | `Operator` | Get bot's available currency area (Depends to wallet area and the IP location)           |
| `FAKEPURCHASE [Bots]`                | `FPC`     | `Master`   | Simulate purchase bot's cart, and generate a failed record without actually checking out |
| `PURCHASE [Bots]`                    | `PC`      | `Master`   | Purchase bot's cart items for it self (paid via steam wallet)                            |
| `PURCHASEGIFT [BotA] BotB`           | `PCG`     | `Master`   | Purchase botA's cart items for botB as gift (paid via steam wallet)                      |

> Steam allows duplicate purchases, please check cart before using PURCHASE command.

### Community Commands

| Command                    | Shorthand | Access     | Description                                |
| -------------------------- | --------- | ---------- | ------------------------------------------ |
| `CLEARNOTIFICATION [Bots]` | `CN`      | `Operator` | Clear new item and new commit notification |

### Friend Commands

| Command                        | Shorthand | Access     | Description                                                                                   |
| ------------------------------ | --------- | ---------- | --------------------------------------------------------------------------------------------- |
| `ADDBOTFRIEND <Bots>`          | `ABF`     | `Master`   | Let `Bots` add each other as friend                                                           |
| `ADDBOTFRIEND <BotAs>+<BotBs>` |           | `Master`   | Let `BotAs` add each other as friend, then let `BotAs` add `BotBs` as friend                  |
| `ADDFRIEND [Bots] <Text>`      | `AF`      | `Master`   | Let bots send friend request to others, `Text` support `custom Url`, `steamId`, `Friend code` |
| `DELETEFRIEND [Bots] <Text>`   | `DF`      | `Master`   | Let bots delete frined, `Text` support `custom Url`, `steamId`, `Friend code`                 |
| `DELETEALLFRIEND [Bots]`       |           | `Master`   | Let bots delete all its friends                                                               |
| `INVITELINK [Bots]`            | `IL`      | `Operator` | Let bots generate friend invite link                                                          |

- `ADDBOTFRIEND` Usage Example
  - `ADDBOTFRIEND a,b c`: Let `a`,`b`,`c` add friends with each other
  - `ADDBOTFRIEND a,b,c + d,e`: Let `a`,`b`,`c` add friends with each other, then let `a`,`b`,`c` add `d` and `e` as friend, `d` will not add `e` as friend
  - `ADDBOTFRIEND ASF`: Allow use `ASF` of bots
  - `ADDBOTFRIEND a b c + ASF`: Allow use `ASF` of bots
  - `ADDBOTFRIEND ASF + ASF`: Allow, but meaningless

### Discovery Queue Commands

| Command           | Shorthand | Access   | Description                                            |
| ----------------- | --------- | -------- | ------------------------------------------------------ |
| `EXPLORER [Bots]` | `EX`      | `Master` | Invoke ASF's Explore Discovery Queue Task in 5 seconds |

> Please try to let ASF explore discovery queue itself, this command is used to invoke ASF's Explore Discovery Queue Task as soon as possible

### Wallet Command

| Command                          | Shorthand | Access   | Description                                                                                 |
| -------------------------------- | --------- | -------- | ------------------------------------------------------------------------------------------- |
| `REDEEMWALLET [Bots] <keys>`     | `RWA`     | `Master` | Redeem wallet code, if balance address is required, will use confitgred address in ASF.json |
| `REDEEMWALLETMULT [Bots] <keys>` | `RWAM`    | `Master` | Redeem wallet code, but every bot will only redeem one given code                           |

### Alias of ASF's Commands

| Shorthand              | Equivalent Command             | Description                    |
| ---------------------- | ------------------------------ | ------------------------------ |
| `AL [Bots] <Licenses>` | `ADDLICENSE [Bots] <Licenses>` | Add free `SUB`                 |
| `LA`                   | `LEVEL ASF`                    | Get All bot's level            |
| `BA`                   | `BALANCE ASF`                  | Get All bot's wallet balance   |
| `PA`                   | `POINTS ASF`                   | Get All bot's points balance   |
| `P [Bots]`             | `POINTS`                       | Get bot's points balance       |
| `CA`                   | `CART ASF`                     | Get All bot's cart information |

### For Developer

> This group of commands is disabled by default.
> You need to add `"DevFeature": true` in `ASF.json` to enable it.

| Command              | Access   | Description               |
| -------------------- | -------- | ------------------------- |
| `COOKIES [Bots]`     | `Master` | Get Steam store's Cookies |
| `APIKEY [Bots]`      | `Master` | Get Bot's APIKey          |
| `ACCESSTOKEN [Bots]` | `Master` | Get Bot's ACCESSTOKEN     |

## IPC Interface

> You need to agree EULA before using IPC interface. See [Plugin Configuration](#plugin-configuration)

| API                                            | Method | Params                                             | Description                |
| ---------------------------------------------- | ------ | -------------------------------------------------- | -------------------------- |
| `/Api/ASFEnhance/{botNames}/FollowCurator`     | POST   | ClanIDs                                            | Follow Curator             |
| `/Api/ASFEnhance/{botNames}/UnFollowCurator`   | POST   | ClanIDs                                            | UnFollow Curator           |
| `/Api/ASFEnhance/{botNames}/FollowingCurators` | POST   | Start, Count                                       | Get Following Curators     |
| `/Api/ASFEnhance/{botNames}/GetAppDetail`      | POST   | AppIDs                                             | Get AppDetail              |
| `/Api/ASFEnhance/{botNames}/Purchase`          | POST   | SubIDs, BundleIDs, SkipOwned                       | Purchase                   |
| `/Api/ASFEnhance/{botNames}/PublishReview`     | POST   | AppIDs, RateUp, AllowReply, ForFree,Public,Comment | Publish Review             |
| `/Api/ASFEnhance/{botNames}/DeleteReview`      | POST   | AppIDs                                             | Delete Review              |
| `/Api/ASFEnhance/{botNames}/AddWishlist`       | POST   | AppIDs                                             | Add Wishlist               |
| `/Api/ASFEnhance/{botNames}/RemoveWishlist`    | POST   | AppIDs                                             | Remove Wishlist            |
| `/Api/ASFEnhance/{botNames}/FollowGame`        | POST   | AppIDs                                             | Follow Game                |
| `/Api/ASFEnhance/{botNames}/UnFollowGame`      | POST   | AppIDs                                             | UnFollow Game              |
| `/Api/ASFEnhance/{botNames}/CheckGame`         | POST   | AppIDs                                             | Check Game Follow/Wishlist |

---

[![Repobeats analytics image](https://repobeats.axiom.co/api/embed/df6309642cc2a447195c816473e7e54e8ae849f9.svg "Repobeats analytics image")](https://github.com/chr233/ASFEnhance/pulse)

---

[![Stargazers over time](https://starchart.cc/chr233/ASFEnhance.svg)](https://github.com/chr233/ASFEnhance/stargazers)

---
