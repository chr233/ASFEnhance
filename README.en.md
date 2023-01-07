# ASFEnhance

[![Join the chat](https://img.shields.io/gitter/room/chr233/ASFEnhance?color=%234FB999&logo=Google%20Chat&logoColor=white)](https://gitter.im/chr233/ASFEnhance)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/3d174e792fd4412bb6b34a77d67e5dea)](https://www.codacy.com/gh/chr233/ASFEnhance/dashboard)
![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/chr233/ASFEnhance/autobuild.yml?logo=github)
[![License](https://img.shields.io/github/license/chr233/ASFEnhance?logo=apache)](https://github.com/chr233/ASFEnhance/blob/master/license)


[![GitHub Release](https://img.shields.io/github/v/release/chr233/ASFEnhance?logo=github)](https://github.com/chr233/ASFEnhance/releases)
[![GitHub Release](https://img.shields.io/github/v/release/chr233/ASFEnhance?include_prereleases&label=pre-release&logo=github)](https://github.com/chr233/ASFEnhance/releases)
[![GitHub Download](https://img.shields.io/github/downloads/chr233/ASFEnhance/total?logo=github)](https://img.shields.io/github/v/release/chr233/ASFEnhance)

![GitHub Repo stars](https://img.shields.io/github/stars/chr233/ASFEnhance?logo=github)
![GitHub last commit](https://img.shields.io/github/last-commit/chr233/ASFEnhance?logo=github)

[![爱发电](https://img.shields.io/badge/爱发电-chr__-ea4aaa.svg?logo=github-sponsors)](https://afdian.net/@chr233)
[![Bilibili](https://img.shields.io/badge/bilibili-Chr__-00A2D8.svg?logo=bilibili)](https://space.bilibili.com/5805394)
[![Steam](https://img.shields.io/badge/steam-Chr__-1B2838.svg?logo=steam)](https://steamcommunity.com/id/Chr_)

[![Steam](https://img.shields.io/badge/steam-donate-1B2838.svg?logo=steam)](https://steamcommunity.com/tradeoffer/new/?partner=221260487&token=xgqMgL-i)

[中文说明](README.md) | [Русская Версия](README.ru.md)

## EULA

> Please don't use this plugin to conduct repulsive behaviors, including but not limited to: post fake reviews, posting advertisements, etc.
>
> See [Plugin Configuration](#plugin-configuration)

## EVENT COMMAND

| Command       | Access     | Description                                |
| ------------- | ---------- | ------------------------------------------ |
| `SIM4 [Bots]` | `Operator` | Claim the `The Sims™ 4` stickers           |
| `DL2 [Bots]`  | `Operator` | Claim the `Dying Light 2 Stay Human` items |

## Installation

### First-Time Install / Manually Update

1. Download the plugin via [GitHub Releases](https://github.com/chr233/ASFEnhance/releases) page
2. Unzip the `ASFEnhance.dll` and copy it into the `plugins` folder in the `ArchiSteamFarm`'s directory
3. Restart the `ArchiSteamFarm` and use `ASFE` command to check if the plugin is working

### Use Command to Update

- Update Command

  - `ASFEVERSION` / `AV` check the latest version of ASFEhance
  - `ASFEUPDATE` / `AU` auto update ASFEhance (Maybe need to update ASF manually)

### ChangeLog

| ASFEnhance Version                                                   | Compat ASF Version | Description                                              |
| -------------------------------------------------------------------- | :----------------: | -------------------------------------------------------- |
| [1.7.7.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.7.0) |      5.4.1.11      | ASF upgrade to `5.4.1.11`, remove expired event commands |
| [1.7.6.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.6.0) |      5.4.0.3       | Add `GAMEAVATAR` `ADVNICKNAME` Command                   |
| [1.7.5.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.5.0) |      5.4.0.3       | Add commands related with `Steam Replay`                 |
| [1.7.4.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.4.0) |      5.4.0.3       | Add commands to claim `Steam Awards 2022` stickers       |
| [1.7.3.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.3.0) |      5.4.0.3       | Add commands for `Steam Awards 2022`                     |

<details>
  <summary>History Version</summary>

| ASFEnhance Version                                                     | Depended ASF | 5.3.1.2 | 5.3.2.4 | 5.4.0.3 | 5.4.1.11 |
| ---------------------------------------------------------------------- | :----------: | :-----: | :-----: | :-----: | :------: |
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

ASF.json

```json
{
  //ASF Configuration
  "CurrentCulture": "...",
  "IPCPassword": "...",

  //ASFEnhance Configuration
  "ASFEnhance": {
    "EULA": true,
    "Statistic": true,
    "DevFeature": false
  }
}
```

| Configuration | Type | Default | Description                                                                                              |
| ------------- | ---- | ------- | -------------------------------------------------------------------------------------------------------- |
| `EULA`        | bool | `true`  | If agree the [EULA](#EULA)\*                                                                             |
| `Statistic`   | bool | `true`  | Allow send statistics data, it's used to count number of users, this will not send any other information |
| `DevFeature`  | bool | `false` | Enabled developer feature (3 Commands) `May causing security risk, turn on with caution`                 |

> \* When Agree [EULA](#EULA), ASFEnhance will let all commands avilable, in exchange, ASFEnhance will follow the author's [Curator](https://store.steampowered.com/curator/39487086/) and [Group](https://steamcommunity.com/groups/11012580) when execute `GROUPLIST` and `CURATORLIST` commands (if bot not following or joined)
>
> \* When Disagree [EULA](#EULA), ASFEnhance will limit features of curator/follow game/group/reviews, and ASFEnhance will not follow [Curator](https://store.steampowered.com/curator/39487086/) and [Group](https://steamcommunity.com/groups/11012580)

## Commands Usage

### Update Commands

| Command       | Shorthand | Access          | Description                                                         |
| ------------- | --------- | --------------- | ------------------------------------------------------------------- |
| `ASFENHANCE`  | `ASFE`    | `FamilySharing` | Get the version of the ASFEnhance                                   |
| `ASFEVERSION` | `AV`      | `Owner`         | Check ASFEnhance's latest version                                   |
| `ASFEUPDATE`  | `AU`      | `Owner`         | Update ASFEnhance to the latest version (need restart ASF manually) |

### Account Commands

| Command                            | Shorthand | Access     | Description                                     |
| ---------------------------------- | --------- | ---------- | ----------------------------------------------- |
| `PURCHASEHISTORY [Bots]`           | `PH`      | `Operator` | Get bot's purchase history.                     |
| `FREELICENSES [Bots]`              | `FL`      | `Operator` | Get bot's all free sub licenses list            |
| `FREELICENSE [Bots]`               |           |            | Same as `FREELICENSES`                          |
| `LICENSES [Bots]`                  | `L`       | `Operator` | Get bot's all licenses list                     |
| `LICENSE [Bots]`                   |           |            | Same as `LICENSES`                              |
| `REMOVEDEMOS [Bots]`               | `RD`      | `Master`   | Remove bot's all demo licenses                  |
| `REMOVEDEMO [Bots]`                |           |            | Same as `REMOVEDEMOS`                           |
| `REMOVELICENSES [Bots] <SubIDs>`   | `RL`      | `Master`   | Remove bot's licenses with the specified subIDs |
| `REMOVELICENSE [Bots] <SubIDs>`    |           |            | Same as `REMOVELICENSES`                        |
| `EMAILOPTIONS [Bots]`              | `EO`      | `Operator` | Get bot's email preferences                     |
| `EMAILOPTION [Bots]`               |           |            | Same as `EMAILOPTIONS`                          |
| `SETEMAILOPTIONS [Bots] <Options>` | `SEO`     | `Master`   | Set bot's email preferences                     |
| `SETEMAILOPTION [Bots] <Options>`  |           |            | Same as `SETEMAILOPTIONS`                       |

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
| `GAMEAVATAR [Bots] <AppID> [AvatarID]` | `GA`      | `Opetator`      | Set bot's avatar as given `AppID` and `AvatarID`, if not set `AvatarId`, plugin will use random one |
| `RANDOMGAMEAVATAR [Bots]`              | `RGA`     | `Opetator`      | Set bot's avatar randomly                                                                           |
| `ADVNICKNAME [Bots] Query`             | `ANN`     | `Master`        | Set bot's nickname use `Placeholder`, avilable: `%dn%` `%ln%` `%un%` `%botn%`, case insensitive     |

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
| `PUBLISHRECOMMENT [Bots] <AppIDs> COMMENT` | `PREC`    | `Operator` | Publish a recomment for game, if appID > 0 it will rateUp, or if appId < 0 rateDown  |
| `DELETERECOMMENT [Bots] <AppIDs>`          | `DREC`    | `Operator` | Delete a recomment for game                                                          |
| `REQUESTACCESS [Bots] <AppIDs>`            | `RA`      | `Operator` | Send join playtest request to specified appIDs, equivalent to click `Request Access` |
| `VIEWPAGE [Bots] Url`                      | `VP`      | `Operator` | Visit the specified page                                                             |

### Cart Commands

> Steam saves cart information via cookies, restart bot instance will let shopping cart being emptied

| Command                              | Shorthand | Access     | Description                                                                    |
| ------------------------------------ | --------- | ---------- | ------------------------------------------------------------------------------ |
| `CART [Bots]`                        | `C`       | `Operator` | Get bot's cart information                                                     |
| `ADDCART [Bots] <SubIDs\|BundleIDs>` | `AC`      | `Operator` | Add game to bot's cart, only support `SUB/BUNDLE`                              |
| `CARTRESET [Bots]`                   | `CR`      | `Operator` | Clear bot's cart                                                               |
| `CARTCOUNTRY [Bots]`                 | `CC`      | `Operator` | Get bot's available currency area (Depends to wallet area and the IP location) |
| `SETCOUNTRY [Bots] <CountryCode>`    | `SC`      | `Operator` | Set bot's currency area (NOT WORKING, WIP)                                     |
| `PURCHASE [Bots]`                    | `PC`      | `Master`   | Purchase bot's cart items for it self (paid via steam wallet)                  |
| `PURCHASEGIFT [BotA] BotB`           | `PCG`     | `Master`   | Purchase botA's cart items for botB as gift (paid via steam wallet)            |

> Steam allows duplicate purchases, please check cart before using PURCHASE command.

### Community Commands

| Command                        | Shorthand | Access     | Description                                |
| ------------------------------ | --------- | ---------- | ------------------------------------------ |
| `CLEARNOTIFICATION [Bots]`     | `CN`      | `Operator` | Clear new item and new commit notification |
| `ADDBOTFRIEND [BotAs] <BotBs>` | `ABF`     | `Master`   | Let `BotA` add `BotB` as friend            |

### Discovery Queue Commands

| Command           | Shorthand | Access   | Description                                            |
| ----------------- | --------- | -------- | ------------------------------------------------------ |
| `EXPLORER [Bots]` | `EX`      | `Master` | Invoke ASF's Explore Discovery Queue Task in 5 seconds |

> Please try to let ASF explore discovery queue itself, this command is used to invoke ASF's Explore Discovery Queue Task as soon as possible

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
