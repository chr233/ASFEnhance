# ASFEnhance

[![Codacy Badge](https://app.codacy.com/project/badge/Grade/3d174e792fd4412bb6b34a77d67e5dea)](https://www.codacy.com/gh/chr233/ASFEnhance/dashboard)
![GitHub Workflow Status](https://img.shields.io/github/workflow/status/chr233/ASFEnhance/AutoBuild?logo=github)
[![License](https://img.shields.io/github/license/chr233/ASFEnhance?logo=apache)](https://github.com/chr233/ASFEnhance/blob/master/license)
![GitHub last commit](https://img.shields.io/github/last-commit/chr233/ASFEnhance?logo=github)

[![GitHub Release](https://img.shields.io/github/v/release/chr233/ASFEnhance?logo=github)](https://github.com/chr233/ASFEnhance/releases)
[![GitHub Release](https://img.shields.io/github/v/release/chr233/ASFEnhance?include_prereleases&label=pre-release&logo=github)](https://github.com/chr233/ASFEnhance/releases)
[![GitHub Download](https://img.shields.io/github/downloads/chr233/ASFEnhance/total?logo=github)](https://img.shields.io/github/v/release/chr233/ASFEnhance)

![GitHub Repo stars](https://img.shields.io/github/stars/chr233/ASFEnhance?logo=github)
[![Gitee Stars](https://gitee.com/chr_a1/ASFEnhance/badge/star.svg?theme=dark)](https://gitee.com/chr_a1/ASFEnhance/stargazers)

[![爱发电](https://img.shields.io/badge/爱发电-chr__-ea4aaa.svg?logo=github-sponsors)](https://afdian.net/@chr233)
[![Bilibili](https://img.shields.io/badge/bilibili-Chr__-00A2D8.svg?logo=bilibili)](https://space.bilibili.com/5805394)
[![Steam](https://img.shields.io/badge/steam-Chr__-1B2838.svg?logo=steam)](https://steamcommunity.com/id/Chr_)

[![Steam](https://img.shields.io/badge/steam-donate-1B2838.svg?logo=steam)](https://steamcommunity.com/tradeoffer/new/?partner=221260487&token=xgqMgL-i)

[中文说明](README.md)

## EULA

> Please don't use this plugin to conduct repulsive behaviors, including but not limited to: post fake reviews, posting advertisements, etc.

[Notice](#Global-Configuration)

## Download

> Unzip the ASFEnhance.dll and copy it into the "plugins" folder in the ASF's directory to install

[GitHub Releases](https://github.com/chr233/ASFEnhance/releases)

## Support Version

> Use command `ASFEVERSION` / `AV` to check the latest version of ASFEhance
>
> Use command `ASFEUPDATE` / `AU` to auto update ASFEhance (Maybe need to update ASF manually)
>
> The \* mark means the ASFEnhance is compatibility with the ASF in theory, but haven't tested.

| ASFEnhance Version                                                         | Compat ASF Version |
| -------------------------------------------------------------------------- | ------------------ |
| [1.6.12.717](https://github.com/chr233/ASFEnhance/releases/tag/1.6.12.717) | 5.3.1.2            |
| [1.6.11.670](https://github.com/chr233/ASFEnhance/releases/tag/1.6.11.670) | 5.3.1.2            |
| [1.6.10.666](https://github.com/chr233/ASFEnhance/releases/tag/1.6.10.666) | 5.3.0.3            |
| [1.6.9.663](https://github.com/chr233/ASFEnhance/releases/tag/1.6.9.663)   | 5.2.8.4            |
| [1.6.8.661](https://github.com/chr233/ASFEnhance/releases/tag/1.6.8.661)   | 5.2.7.7            |

<details>
  <summary>History Version</summary>

| ASFEnhance Version                                                         | Depended ASF | 5.2.2.5 | 5.2.3.7 | 5.2.4.2 | 5.2.5.7 | 5.2.6.3 | 5.2.7.7 | 5.2.8.4 |
| -------------------------------------------------------------------------- | ------------ | ------- | ------- | ------- | ------- | ------- | ------- | ------- |
| [1.6.9.663](https://github.com/chr233/ASFEnhance/releases/tag/1.6.9.663)   | 5.2.8.4      |         |         |         |         | ❌      | ❌      | ✔️      |
| [1.6.8.661](https://github.com/chr233/ASFEnhance/releases/tag/1.6.8.661)   | 5.2.7.7      |         |         |         |         | ❌      | ✔️      |         |
| [1.6.6.622](https://github.com/chr233/ASFEnhance/releases/tag/1.6.6.622)   | 5.2.6.3      | ❌      | ❌      | ❌      | ✔️\*    | ✔️      |         |         |
| [1.5.20.381](https://github.com/chr233/ASFEnhance/releases/tag/1.5.20.381) | 5.2.5.7      | ❌      | ❌      | ❌      | ✔️      |         |         |         |
| [1.5.18.304](https://github.com/chr233/ASFEnhance/releases/tag/1.5.18.304) | 5.2.4.2      | ❌      | ✔️\*    | ✔️      | ✔️\*    |         |         |         |
| [1.5.17.289](https://github.com/chr233/ASFEnhance/releases/tag/1.5.17.289) | 5.2.4.2      | ❌      | ✔️\*    | ✔️      | ✔️\*    |         |         |         |
| [1.5.16.260](https://github.com/chr233/ASFEnhance/releases/tag/1.5.16.260) | 5.2.4.2      | ❌      | ✔️\*    | ✔️      | ✔️\*    |         |         |         |
| [1.5.15.257](https://github.com/chr233/ASFEnhance/releases/tag/1.5.15.257) | 5.2.3.7      | ❌      | ✔️\*    | ✔️      | ✔️      |         |         |         |

</details>

## Global Configuration

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

| Command                          | Shorthand | Access     | Description                                     |
| -------------------------------- | --------- | ---------- | ----------------------------------------------- |
| `PURCHASEHISTORY [Bots]`         | `PH`      | `Operator` | Get bot's purchase history.                     |
| `FREELICENSES [Bots]`            | `FL`      | `Operator` | Get bot's all free sub licenses list            |
| `FREELICENSE [Bots]`             |           |            | Same as `FREELICENSES`                          |
| `LICENSES [Bots]`                | `L`       | `Operator` | Get bot's all licenses list                     |
| `LICENSE [Bots]`                 |           |            | Same as `LICENSES`                              |
| `REMOVEDEMOS [Bots]`             | `RD`      | `Master`   | Remove bot's all demo licenses                  |
| `REMOVEDEMO [Bots]`              |           |            | Same as `REMOVEDEMOS`                           |
| `REMOVELICENSES [Bots] <SubIDs>` | `RL`      | `Master`   | Remove bot's licenses with the specified subIDs |
| `REMOVELICENSE [Bots] <SubIDs>`  |           |            | Same as `REMOVELICENSES`                        |

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

| Command              | Shorthand | Access          | Description                  |
| -------------------- | --------- | --------------- | ---------------------------- |
| `PROFILE [Bots]`     | `PF`      | `FamilySharing` | Get bot's profile infomation |
| `PROFILELINK [Bots]` | `PFL`     | `FamilySharing` | Get bot's profile link       |
| `STEAMID [Bots]`     | `SID`     | `FamilySharing` | Get bot's steamID            |
| `FRIENDCODE [Bots]`  | `FC`      | `FamilySharing` | Get bot's friend code        |
| `TRADELINK [Bots]`   | `TL`      | `Operator`      | Get bot's trade link         |

### Curator Commands

| Command                          | Shorthand | Access   | Description                      |
| -------------------------------- | --------- | -------- | -------------------------------- |
| `CURATORLIST [Bots]`             | `CL`      | `Master` | Get bot's following curator list |
| `FOLLOWCURATOR [Bots] <ClanIDs>` | `FC`      | `Master` | Follow specified curator         |
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

| Command                                    | Shorthand | Access     | Description                                                                         |
| ------------------------------------------ | --------- | ---------- | ----------------------------------------------------------------------------------- |
| `APPDETAIL [Bots] <AppIDs>`                | `AD`      | `Operator` | Get app detail from steam API, support `APP`                                        |
| `SEARCH [Bots] Keywords`                   | `SS`      | `Operator` | Search in Steam store                                                               |
| `SUBS [Bots] <AppIDs\|SubIDs\|BundleIDs>`  | `S`       | `Operator` | Get available subs from store page, support `APP/SUB/BUNDLE`                        |
| `PUBLISHRECOMMENT [Bots] <AppIDs> COMMENT` | `PREC`    | `Operator` | Publish a recomment for game, if appID > 0 it will rateUp, or if appId < 0 rateDown |
| `DELETERECOMMENT [Bots] <AppIDs>`          | `DREC`    | `Operator` | Delete a recomment for game                                                         |

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

| API                                            | 方法 | 参数                                               | 说明                       |
| ---------------------------------------------- | ---- | -------------------------------------------------- | -------------------------- |
| `/Api/ASFEnhance/{botNames}/FollowCurator`     | POST | ClanIDs                                            | Follow Curator             |
| `/Api/ASFEnhance/{botNames}/UnFollowCurator`   | POST | ClanIDs                                            | UnFollow Curator           |
| `/Api/ASFEnhance/{botNames}/FollowingCurators` | POST | Start, Count                                       | Get Following Curators     |
| `/Api/ASFEnhance/{botNames}/GetAppDetail`      | POST | AppIDs                                             | Get AppDetail              |
| `/Api/ASFEnhance/{botNames}/Purchase`          | POST | SubIDs, BundleIDs, SkipOwned                       | Purchase                   |
| `/Api/ASFEnhance/{botNames}/PublishReview`     | POST | AppIDs, RateUp, AllowReply, ForFree,Public,Comment | Publish Review             |
| `/Api/ASFEnhance/{botNames}/DeleteReview`      | POST | AppIDs                                             | Delete Review              |
| `/Api/ASFEnhance/{botNames}/AddWishlist`       | POST | AppIDs                                             | Add Wishlist               |
| `/Api/ASFEnhance/{botNames}/RemoveWishlist`    | POST | AppIDs                                             | Remove Wishlist            |
| `/Api/ASFEnhance/{botNames}/FollowGame`        | POST | AppIDs                                             | Follow Game                |
| `/Api/ASFEnhance/{botNames}/UnFollowGame`      | POST | AppIDs                                             | UnFollow Game              |
| `/Api/ASFEnhance/{botNames}/CheckGame`         | POST | AppIDs                                             | Check Game Follow/Wishlist |

---

[![Repobeats analytics image](https://repobeats.axiom.co/api/embed/df6309642cc2a447195c816473e7e54e8ae849f9.svg "Repobeats analytics image")](https://github.com/chr233/ASFEnhance/pulse)

---
