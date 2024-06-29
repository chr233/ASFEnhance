# ASFEnhance

![ASFEnhance](https://socialify.git.ci/chr233/ASFEnhance/image?description=1&forks=1&language=1&name=1&owner=1&pattern=Diagonal%20Stripes&stargazers=1&theme=Auto)

[![Codacy Badge](https://app.codacy.com/project/badge/Grade/3d174e792fd4412bb6b34a77d67e5dea)](https://www.codacy.com/gh/chr233/ASFEnhance/dashboard)
![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/chr233/ASFEnhance/publish.yml?logo=github)
[![License](https://img.shields.io/github/license/chr233/ASFEnhance?logo=apache)](https://github.com/chr233/ASFEnhance/blob/master/license)
[![Crowdin](https://badges.crowdin.net/asfenhance/localized.svg)](https://crowdin.com/project/asfenhance)

[![GitHub Release](https://img.shields.io/github/v/release/chr233/ASFEnhance?logo=github)](https://github.com/chr233/ASFEnhance/releases)
[![GitHub Release](https://img.shields.io/github/v/release/chr233/ASFEnhance?include_prereleases&label=pre-release&logo=github)](https://github.com/chr233/ASFEnhance/releases)
![GitHub last commit](https://img.shields.io/github/last-commit/chr233/ASFEnhance?logo=github)

![GitHub Repo stars](https://img.shields.io/github/stars/chr233/ASFEnhance?logo=github)
[![GitHub Download](https://img.shields.io/github/downloads/chr233/ASFEnhance/total?logo=github)](https://img.shields.io/github/v/release/chr233/ASFEnhance)

[![Bilibili](https://img.shields.io/badge/bilibili-Chr__-00A2D8.svg?logo=bilibili)](https://space.bilibili.com/5805394)
[![Steam](https://img.shields.io/badge/steam-Chr__-1B2838.svg?logo=steam)](https://steamcommunity.com/id/Chr_)

[![Steam](https://img.shields.io/badge/steam-donate-1B2838.svg?logo=steam)](https://steamcommunity.com/tradeoffer/new/?partner=221260487&token=xgqMgL-i)
[![爱发电][afdian_img]][afdian_link]
[![buy me a coffee][bmac_img]][bmac_link]

[中文说明](README.md) | [English Version](README.en.md)

## ЛИЦЕНЗИОННОЕ СОГЛАШЕНИЕ

> Пожалуйста, не используйте этот плагин для отвратительного поведения, включая, но не ограничиваясь: публикацией фейковых отзывов, размещением рекламы и т.д.
>
> Смотри [Конфигурацию Плагина](#конфигурация-плагина)

## КОМАНДЫ СОБЫТИЙ

> Эта группа команд доступна только в течение ограниченного времени и будет удалена при выходе следующей версии плагина, если она потеряет свою актуальность

| Команда            | Сокращение | Доступ     | Описание                                                                                                    |
| ------------------ | ---------- | ---------- | ----------------------------------------------------------------------------------------------------------- |
| `CLAIMITEM [Bots]` | `CI`       | `Operator` | Получить предмет распродажи, например, наклейки или что-то еще                                              |
| `CLAIM20TH [Bots]` | `C20`      | `Operator` | Получить бесплатные предметы 20-ой годовщины Steam в магазине очков                                         |
| `DL2 [Bots]`       |            | `Operator` | Получить вещи `Dying Light 2 Stay Human` [ссылка](https://store.steampowered.com/sale/dyinglight2towerraid) |

> `ASFEnhance` will automatic execute `CLAIMITEM` command for every bot defiend in `AutoClaimItemBotNames` after 1 hour since ASF started and every 23 hours.

## Установка

### Первая установка / Обновление в ручном режиме

1. Загрузите плагин через [GitHub Releases](https://github.com/chr233/ASFEnhance/releases) страницу
2. Распакуйте файл `ASFEnhance.dll` и скопируйте его в папку `plugins` в директории `ArchiSteamFarm`
3. Перезапустить `ArchiSteamFarm` и используйте команду `ASFE` для проверки работоспособности плагина

### Подмодули

> Начиная с версии ASFEnhance 2.0.0.0, была добавлена система подмодулей для управления и обновления поддерживаемых плагинов.

Список поддерживаемых плагинов.

- [ASFBuffBot](https://github.com/chr233/ASFBuffBot) (Bugfix WIP)
- [ASFOAuth](https://github.com/chr233/ASFOAuth)
- [ASFTradeExtension](https://github.com/chr233/ASFTradeExtension) (Bugfix WIP)
- [ASFAchievementManagerEx](https://github.com/chr233/ASFAchievementManagerEx) (Bugfix WIP)
- ...

### Обновления плагинов и подмодулей

> ASFEnhance now supported update using `Update` command, to enable this future, requires set `PluginsUpdateMode` to `blacklist` in `ASF.json`

![blacklist](img/blacklist.png)

> or set `PluginsUpdateMode` to `whitelist`, and add `ASFEnhance` into `PluginsUpdateList`

![whitelist](img/whitelist.png)

| Команда                       | Сокращение | Доступ     | Описание                                                                                                                                               |
| ----------------------------- | ---------- | ---------- | ------------------------------------------------------------------------------------------------------------------------------------------------------ |
| `PLUGINSLIST`                 | `PL`       | `Operator` | Получить список установленных в данный момент плагинов, те из них, которые имеют в конце [], являются подмодулями, которыми может управлять ASFEnhance |
| `PLUGINLIST`                  | -          | `Operator` | То же, что и `PLUGINSLIST`                                                                                                                             |
| `PLUGINSVERSION [PluginName]` | `PV`       | `Master`   | Получение информации о версии указанного модуля, а также проверка информации о версии всех поддерживаемых плагинов, если имя плагина не указано        |
| `PLUGINVERSION`               | -          | `Master`   | То же, что и `PLUGINSVERSION`                                                                                                                          |
| `PLUGINSUPDATE [PluginName]`  | `PU`       | `Master`   | Автоматическое обновление всех поддерживаемых плагинов без указания имени плагина (Require ASF-generic)                                                |
| `PLUGINUPDATE`                | -          | `Master`   | То же, что и `PLUGINSUPDATE`                                                                                                                           |

### Donate

|               ![img][afdian_qr]                |                   ![img][bmac_qr]                   |                       ![img][usdt_qr]                       |
| :--------------------------------------------: | :-------------------------------------------------: | :---------------------------------------------------------: |
| ![爱发电][afdian_img] <br> [链接][afdian_link] | ![buy me a coffee][bmac_img] <br> [链接][bmac_link] | ![USDT][usdt_img] <br> `TW41eecZ199QK6zujgKP4j1cz2bXzRus3c` |

[afdian_qr]: https://raw.chrxw.com/chr233/master/afadian_qr.png
[afdian_img]: https://img.shields.io/badge/爱发电-@chr__-ea4aaa.svg?logo=github-sponsors
[afdian_link]: https://afdian.net/@chr233
[bmac_qr]: https://raw.chrxw.com/chr233/master/bmc_qr.png
[bmac_img]: https://img.shields.io/badge/buy%20me%20a%20coffee-@chr233-yellow?logo=buymeacoffee
[bmac_link]: https://www.buymeacoffee.com/chr233
[usdt_qr]: https://raw.chrxw.com/chr233/master/usdt_qr.png
[usdt_img]: https://img.shields.io/badge/USDT-TRC20-2354e6.svg?logo=bitcoin

### ChangeLog

| Версия ASFEnhance                                                      | Совместимая версия ASF | Описание                                                                 |
| ---------------------------------------------------------------------- | :--------------------: | ------------------------------------------------------------------------ |
| [2.1.9.2](https://github.com/chr233/ASFEnhance/releases/tag/2.1.9.2)   |        6.0.3.4         | 新增 `DL2` 命令, 移除失效命令                                            |
| [2.1.8.3](https://github.com/chr233/ASFEnhance/releases/tag/2.1.8.3)   |        6.0.3.4         | 新增 `STACKINVENTORY`, `UNSTACKINVENTORY` 命令                           |
| [2.1.7.1](https://github.com/chr233/ASFEnhance/releases/tag/2.1.7.1)   |        6.0.3.4         | 新增 `CHECKMARKLIMIT` 命令                                               |
| [2.1.6.0](https://github.com/chr233/ASFEnhance/releases/tag/2.1.6.0)   |        6.0.3.4         | ASF -> 6.0.3.4                                                           |
| [2.1.5.0](https://github.com/chr233/ASFEnhance/releases/tag/2.1.5.0)   |        6.0.2.6         | 修复 `GETACCCOUNTBAN` 命令, 新增 `DELETECART` 命令                       |
| [2.1.4.0](https://github.com/chr233/ASFEnhance/releases/tag/2.1.4.0)   |        6.0.2.6         | ASF -> 6.0.2.6, 修复 `ADDWISHLIST` 命令                                  |
| [2.1.3.3](https://github.com/chr233/ASFEnhance/releases/tag/2.1.3.3)   |        6.0.1.24        | ASF -> 6.0.1.24, 修复 `ADDWISHLIST` 命令, 新增 `GETPRIVACYAPP` 等命令    |
| [2.1.2.3](https://github.com/chr233/ASFEnhance/releases/tag/2.1.2.3)   |        6.0.0.3         | 修复 `PURCHASE` 命令, 新增 `RECOMMENT` 命令                              |
| [2.1.1.1](https://github.com/chr233/ASFEnhance/releases/tag/2.1.1.1)   |        6.0.0.3         | ASF -> 6.0.0.3                                                           |
| [2.0.16.2](https://github.com/chr233/ASFEnhance/releases/tag/2.0.16.2) |        5.5.3.4         | 新增 `EDITCART` `ADDCARTGIFT` 命令, 改进 `APPDETAIL` 命令, 改进 IPC 接口 |

<details>
  <summary>История версий</summary>

| Версия ASFEnhance                                                      | Совместимая версия ASF | Описание                                                                     |
| ---------------------------------------------------------------------- | :--------------------: | ---------------------------------------------------------------------------- |
| [2.0.16.2](https://github.com/chr233/ASFEnhance/releases/tag/2.0.16.2) |        5.5.3.4         | 新增 `EDITCART` `ADDCARTGIFT` 命令, 改进 `APPDETAIL` 命令, 改进 IPC 接口     |
| [2.0.15.0](https://github.com/chr233/ASFEnhance/releases/tag/2.0.15.0) |        5.5.3.4         | 适配新的购物车接口, 移除 `PURCHASEGIFT` 命令                                 |
| [2.0.14.2](https://github.com/chr233/ASFEnhance/releases/tag/2.0.14.2) |        5.5.3.4         | ASF -> 5.5.3.4                                                               |
| [2.0.13.1](https://github.com/chr233/ASFEnhance/releases/tag/2.0.13.1) |        5.5.2.3         | 新增 `BALANCEINFO` 命令, 用来查看待处理余额到账时间, 改进 `PROFILELINK` 命令 |
| [2.0.12.1](https://github.com/chr233/ASFEnhance/releases/tag/2.0.12.1) |        5.5.2.3         | 新增 `EDITCUSTOMURL`, `DELETECUSTOMURL` 命令                                 |
| [2.0.11.1](https://github.com/chr233/ASFEnhance/releases/tag/2.0.11.1) |        5.5.2.3         | ASF -> 5.5.2.3, 旧版本不兼容                                                 |
| [2.0.10.1](https://github.com/chr233/ASFEnhance/releases/tag/2.0.10.1) |        5.5.1.4         | 新增 `REDEEMPOINTSITEM`, `REDEEMPOINTSBADGE` 命令                            |
| [2.0.9.3](https://github.com/chr233/ASFEnhance/releases/tag/2.0.9.3)   |        5.5.1.4         | ASF -> 5.5.1.4 , 自动领取增加配置项, 修复 `CRAFTBADGE` 命令                  |

| Версия ASFEnhance                                                    | Зависит от ASF | 5.5.0.x | 5.5.1.4 | 5.5.2.3 |
| -------------------------------------------------------------------- | :------------: | :-----: | :-----: | :-----: |
| [2.0.8.0](https://github.com/chr233/ASFEnhance/releases/tag/2.0.8.0) |    5.5.0.11    |   ✔️    |   ✔️    |   ❌    |
| [2.0.7.0](https://github.com/chr233/ASFEnhance/releases/tag/2.0.7.0) |    5.5.0.11    |   ✔️    |   ✔️    |   ❌    |
| [2.0.6.0](https://github.com/chr233/ASFEnhance/releases/tag/2.0.6.0) |    5.5.0.11    |   ✔️    |   ✔️    |   ❌    |
| [2.0.5.1](https://github.com/chr233/ASFEnhance/releases/tag/2.0.5.1) |    5.5.0.11    |   ✔️    |   ✔️    |   ❌    |
| [2.0.4.0](https://github.com/chr233/ASFEnhance/releases/tag/2.0.4.0) |    5.5.0.10    |   ✔️    |   ✔️    |   ❌    |
| [2.0.3.2](https://github.com/chr233/ASFEnhance/releases/tag/2.0.3.2) |    5.5.0.10    |   ✔️    |   ✔️    |   ❌    |

> ASF 5.5.0.11 开始使用 .Net8.0, 旧版本插件无法适配新版本 ASF

| Версия ASFEnhance                                                    | Зависит от ASF | 5.4.10.3 | 5.4.12.5 | 5.4.13.4 | 5.5.0.x |
| -------------------------------------------------------------------- | :------------: | :------: | :------: | :------: | :-----: |
| [2.0.2.0](https://github.com/chr233/ASFEnhance/releases/tag/2.0.2.0) |    5.4.13.4    |    ❌    |    ❌    |    ✔️    |   ❌    |
| [2.0.1.3](https://github.com/chr233/ASFEnhance/releases/tag/2.0.1.3) |    5.4.12.5    |    ❌    |    ✔️    |    ✔️    |   ❌    |
| [2.0.0.0](https://github.com/chr233/ASFEnhance/releases/tag/2.0.0.0) |    5.4.12.5    |    ❌    |    ✔️    |    ✔️    |   ❌    |

> ASF 5.4.10.3 以及之前的版本因为 Steam 的改动已经无法使用, 请使用新版本的 ASF 和 ASFEnhance

| Версия ASFEnhance                                                      | Зависит от ASF | 5.4.7.3 | 5.4.8.3 | 5.4.9.3 |
| ---------------------------------------------------------------------- | :------------: | :-----: | :-----: | :-----: |
| [1.8.13.0](https://github.com/chr233/ASFEnhance/releases/tag/1.8.13.0) |    5.4.10.3    |   ❌    |   ✔️    |   ✔️    |
| [1.8.12.2](https://github.com/chr233/ASFEnhance/releases/tag/1.8.12.2) |    5.4.9.3     |         |   ✔️    |   ✔️    |
| [1.8.11.1](https://github.com/chr233/ASFEnhance/releases/tag/1.8.11.1) |    5.4.9.3     |         |   ✔️    |   ✔️    |
| [1.8.10.0](https://github.com/chr233/ASFEnhance/releases/tag/1.8.10.0) |    5.4.9.3     |         |   ✔️    |   ✔️    |
| [1.8.9.0](https://github.com/chr233/ASFEnhance/releases/tag/1.8.9.0)   |    5.4.9.3     |         |   ✔️    |   ✔️    |
| [1.8.8.0](https://github.com/chr233/ASFEnhance/releases/tag/1.8.8.0)   |    5.4.8.3     |   ❌    |   ✔️    |   ✔️    |

| Версия ASFEnhance                                                    | Зависит от ASF | 5.4.5.2 | 5.4.6.3 | 5.4.7.3 | 5.4.8.3 |
| -------------------------------------------------------------------- | :------------: | :-----: | :-----: | :-----: | :-----: |
| [1.8.7.1](https://github.com/chr233/ASFEnhance/releases/tag/1.8.7.1) |    5.4.7.3     |         |         |   ✔️    |   ❌    |
| [1.8.6.2](https://github.com/chr233/ASFEnhance/releases/tag/1.8.6.2) |    5.4.7.3     |         |         |   ✔️    |   ❌    |
| [1.8.5.0](https://github.com/chr233/ASFEnhance/releases/tag/1.8.5.0) |    5.4.7.3     |         |         |   ✔️    |   ❌    |
| [1.8.4.0](https://github.com/chr233/ASFEnhance/releases/tag/1.8.4.0) |    5.4.7.2     |         |         |   ✔️    |   ❌    |
| [1.8.3.0](https://github.com/chr233/ASFEnhance/releases/tag/1.8.3.0) |    5.4.6.3     |         |   ✔️    |   ✔️    |   ❌    |
| [1.8.2.0](https://github.com/chr233/ASFEnhance/releases/tag/1.8.2.0) |    5.4.6.3     |         |   ✔️    |   ✔️    |   ❌    |
| [1.8.1.3](https://github.com/chr233/ASFEnhance/releases/tag/1.8.1.3) |    5.4.5.2     |   ✔️    |   ✔️    |   ✔️    |   ❌    |

| Версия ASFEnhance                                                      | Зависит от ASF | 5.4.1.11 | 5.4.2.13 | 5.4.3.2 | 5.4.4.x |
| ---------------------------------------------------------------------- | :------------: | :------: | :------: | :-----: | :-----: |
| [1.7.25.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.25.0) |    5.4.4.5     |          |    ❌    |   ❌    |   ✔️    |
| [1.7.24.1](https://github.com/chr233/ASFEnhance/releases/tag/1.7.24.1) |    5.4.4.5     |          |    ❌    |   ❌    |   ✔️    |
| [1.7.23.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.23.0) |    5.4.4.5     |          |    ❌    |   ❌    |   ✔️    |
| [1.7.22.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.22.0) |    5.4.4.5     |          |    ❌    |   ❌    |   ✔️    |
| [1.7.21.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.21.0) |    5.4.4.4     |          |    ❌    |   ❌    |   ✔️    |
| [1.7.20.1](https://github.com/chr233/ASFEnhance/releases/tag/1.7.20.1) |    5.4.4.3     |          |    ❌    |   ❌    |   ✔️    |
| [1.7.19.1](https://github.com/chr233/ASFEnhance/releases/tag/1.7.19.1) |    5.4.3.2     |          |    ❌    |   ❌    |   ✔️    |
| [1.7.18.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.18.0) |    5.4.2.13    |          |    ❌    |   ❌    |   ✔️    |
| [1.7.17.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.17.0) |    5.4.2.13    |    ❌    |    ✔️    |   ✔️    |         |
| [1.7.16.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.16.0) |    5.4.2.13    |    ❌    |    ✔️    |   ✔️    |         |
| [1.7.15.2](https://github.com/chr233/ASFEnhance/releases/tag/1.7.15.2) |    5.4.2.13    |    ❌    |    ✔️    |   ✔️    |         |

| Версия ASFEnhance                                                      | Зависит от ASF | 5.3.1.2 | 5.3.2.4 | 5.4.0.3 | 5.4.1.11 |
| ---------------------------------------------------------------------- | :------------: | :-----: | :-----: | :-----: | :------: |
| [1.7.12.1](https://github.com/chr233/ASFEnhance/releases/tag/1.7.12.1) |    5.4.1.11    |   ❌    |   ❌    |   ❌    |    ✔️    |
| [1.7.11.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.11.0) |    5.4.1.11    |   ❌    |   ❌    |   ❌    |    ✔️    |
| [1.7.10.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.10.0) |    5.4.1.11    |   ❌    |   ❌    |   ❌    |    ✔️    |
| [1.7.9.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.9.0)   |    5.4.1.11    |   ❌    |   ❌    |   ❌    |    ✔️    |
| [1.7.8.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.8.0)   |    5.4.1.11    |   ❌    |   ❌    |   ❌    |    ✔️    |
| [1.7.7.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.7.0)   |    5.4.1.11    |   ❌    |   ❌    |   ❌    |    ✔️    |
| [1.7.6.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.6.0)   |    5.4.0.3     |   ❌    |   ❌    |   ✔️    |    ✔️    |
| [1.7.5.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.5.0)   |    5.4.0.3     |   ❌    |   ❌    |   ✔️    |    ✔️    |
| [1.7.4.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.4.0)   |    5.4.0.3     |   ❌    |   ❌    |   ✔️    |    ✔️    |
| [1.7.3.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.3.0)   |    5.4.0.3     |   ❌    |   ❌    |   ✔️    |    ✔️    |
| [1.7.2.1](https://github.com/chr233/ASFEnhance/releases/tag/1.7.2.1)   |    5.4.0.3     |   ❌    |   ❌    |   ✔️    |    ✔️    |
| [1.7.1.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.1.0)   |    5.4.0.3     |   ❌    |   ❌    |   ✔️    |    ✔️    |
| [1.7.0.1](https://github.com/chr233/ASFEnhance/releases/tag/1.7.0.1)   |    5.4.0.3     |   ❌    |   ❌    |   ✔️    |    ✔️    |
| [1.6.23.0](https://github.com/chr233/ASFEnhance/releases/tag/1.6.23.0) |    5.3.2.4     |   ❌    |   ✔️    |   ✔️    |          |
| [1.6.22.1](https://github.com/chr233/ASFEnhance/releases/tag/1.6.22.1) |    5.3.2.4     |   ❌    |   ✔️    |   ✔️    |          |
| [1.6.21.6](https://github.com/chr233/ASFEnhance/releases/tag/1.6.21.6) |    5.3.2.4     |   ❌    |   ✔️    |   ✔️    |          |
| [1.6.20.1](https://github.com/chr233/ASFEnhance/releases/tag/1.6.20.1) |    5.3.2.4     |   ❌    |   ✔️    |   ✔️    |          |
| [1.6.19.4](https://github.com/chr233/ASFEnhance/releases/tag/1.6.19.4) |    5.3.2.4     |   ❌    |   ✔️    |   ✔️    |          |
| [1.6.18.1](https://github.com/chr233/ASFEnhance/releases/tag/1.6.18.1) |    5.3.2.4     |   ❌    |   ✔️    |   ✔️    |          |

| Версия ASFEnhance                                                          | Зависит от ASF | 5.2.6.3 | 5.2.7.7 | 5.2.8.4 | 5.3.0.3 | 5.3.1.2 |
| -------------------------------------------------------------------------- | :------------: | :-----: | :-----: | :-----: | :-----: | :-----: |
| [1.6.18.0](https://github.com/chr233/ASFEnhance/releases/tag/1.6.18.0)     |    5.3.1.2     |   ❌    |   ❌    |   ❌    |   ✔️    |   ✔️    |
| [1.6.16.0](https://github.com/chr233/ASFEnhance/releases/tag/1.6.16.0)     |    5.3.1.2     |   ❌    |   ❌    |   ❌    |   ✔️    |   ✔️    |
| [1.6.15.0](https://github.com/chr233/ASFEnhance/releases/tag/1.6.15.0)     |    5.3.1.2     |   ❌    |   ❌    |   ❌    |   ✔️    |   ✔️    |
| [1.6.14.0](https://github.com/chr233/ASFEnhance/releases/tag/1.6.14.0)     |    5.3.1.2     |   ❌    |   ❌    |   ❌    |   ✔️    |   ✔️    |
| [1.6.12.717](https://github.com/chr233/ASFEnhance/releases/tag/1.6.12.717) |    5.3.1.2     |   ❌    |   ❌    |   ❌    |   ✔️    |   ✔️    |
| [1.6.11.670](https://github.com/chr233/ASFEnhance/releases/tag/1.6.11.670) |    5.3.1.2     |   ❌    |   ❌    |   ❌    |   ✔️    |   ✔️    |
| [1.6.10.666](https://github.com/chr233/ASFEnhance/releases/tag/1.6.10.666) |    5.3.0.3     |   ❌    |   ❌    |   ❌    |   ✔️    |  ✔️\*   |
| [1.6.9.663](https://github.com/chr233/ASFEnhance/releases/tag/1.6.9.663)   |    5.2.8.4     |   ❌    |   ❌    |   ✔️    |   ❌    |         |
| [1.6.8.661](https://github.com/chr233/ASFEnhance/releases/tag/1.6.8.661)   |    5.2.7.7     |   ❌    |   ✔️    |         |         |         |

</details>

## Конфигурация плагина

> Настройка этого плагина не требуется, большинство функций доступно в настройках по умолчанию

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
    "DevFeature": false,
    "DisabledCmds": ["foo", "bar"],
    "AutoClaimItemBotNames": "",
    "AutoClaimItemPeriod": 23,
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

| Конфигурация            | Тип      | По умолчанию | Описание                                                                                                                                                                       |
| ----------------------- | -------- | ------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| `EULA`                  | `bool`   | `true`       | Если согласны с [лицензионным соглашением](#лицензионное-соглашение)\*                                                                                                         |
| `Statistic`             | `bool`   | `true`       | Разрешить отправку данных для статистики. Она используется для подсчета количества пользователей, при этом никакой другой информации отправляться не будет                     |
| `DevFeature`            | `bool`   | `false`      | Включить функции [разработчика (3 команды)](#для-разработчика) `Может представить угрозу безопасности, включайте на свой страх и риск`                                         |
| `DisabledCmds`          | `list`   | `null`       | Команды в списке будет отключены , **`DisabledCmds` нечувствительна к командам ASF**, данная конфигурация влияет только на команды `ASFEnhance`                                |
| `Address`\*\*\*         | `dict`   | `null`       | При наличии одного расчетного адреса используйте команду `REDEEMWALLET` для активации кода пополнения кошелька, который будет использоваться автоматически при запросе адреса. |
| `Addresses`\*\*\*       | `list`   | `null`       | Если у вас несколько расчетных адресов , произвольно используйте один из списка, когда вам нужен расчетный адрес                                                               |
| `AutoClaimItemBotNames` | `string` | `null`       | **Optional**, 自动领取物品的机器人名称, 用" "或者","分隔多个机器人, 例如 `bot1 bot2,bot3`, 也支持 `ASF` 指代所有机器人                                                         |
| `AutoClaimItemPeriod`   | `uint`   | `23`         | **Optional**, 自动领取物品的周期, 单位小时                                                                                                                                     |

> \* Если Вы согласны с [лицензионным соглашением](#лицензионное-соглашение), то в ASFEnhance будут доступны все команды, в обмен на это, при использовании команд `GROUPLIST` и `CURATORLIST`, ASFEnhance подпишется на [Куратора](https://store.steampowered.com/curator/39487086/) и [Группу](https://steamcommunity.com/groups/11012580) (если бот не подписался или не присоединился)
>
> \* Если Вы не согласны с [лицензионным соглашением](#лицензионное-соглашение), то ASFEnhance ограничит команды куратора/подписок на игры/групп/обзоров, и ASFEnhance не подпишется на [Куратора](https://store.steampowered.com/curator/39487086/) и [Группу](https://steamcommunity.com/groups/11012580)
>
> \*\* Описание `DisabledCmds`: каждый элемент в этой конфигурации является **нечувствительным к командам ASF**, и это влияет только на команды `ASFEnhance`.
> Например, если настроить `["foo", "BAR"]`, то это означает, что `FOO` и `BAR` будут отключены,
> если вы не хотите отключать какие-либо команды, настройте их как `null` или `[]`.
> Если некоторые команды отключены, то все равно можно вызвать команду в виде `ASFE.xxx`, например `ASFE.EXPLORER`.
>
> \*\*\* `Address` и `Addresses` это одна и та же конфигурация

## Использование Команд

### Команды Обновления

| Команда      | Сокращение | Доступ          | Описание                   |
| ------------ | ---------- | --------------- | -------------------------- |
| `ASFENHANCE` | `ASFE`     | `FamilySharing` | Получить версию ASFEnhance |

### Команды Аккаунта

| Команда                                   | Сокращение | Доступ     | Описание                                                                                                 |
| ----------------------------------------- | ---------- | ---------- | -------------------------------------------------------------------------------------------------------- |
| `PURCHASEHISTORY [Bots]`                  | `PH`       | `Operator` | Выводит историю покупок бота                                                                             |
| `FREELICENSES [Bots]`                     | `FL`       | `Operator` | Выводит список всех бесплатных лицензий бота                                                             |
| `FREELICENSE [Bots]`                      |            |            | То же, что и `FREELICENSES`                                                                              |
| `LICENSES [Bots]`                         | `L`        | `Operator` | Выводит список всех SUB (лицензий) бота                                                                  |
| `LICENSE [Bots]`                          |            |            | То же, что и `LICENSES`                                                                                  |
| `REMOVEDEMOS [Bots]`                      | `RD`       | `Master`   | Удаляет все демо-лицензии бота                                                                           |
| `REMOVEDEMO [Bots]`                       |            |            | То же, что и `REMOVEDEMOS`                                                                               |
| `REMOVELICENSES [Bots] <SubIDs>`          | `RL`       | `Master`   | Удаляет определённую лицензию бота по subIDs                                                             |
| `REMOVELICENSE [Bots] <SubIDs>`           |            |            | То же, что и `REMOVELICENSES`                                                                            |
| `EMAILIOPTIONS [Bots]`                    | `EO`       | `Operator` | Выводит настройки рассылки бота [ссылка](https://store.steampowered.com/account/emailoptout)             |
| `EMAILIOPTION [Bots]`                     |            |            | То же, что и `EMAILIOPTIONS`                                                                             |
| `SETEMAILOPTIONS [Bots] <Options>`        | `SEO`      | `Master`   | Изменяет настройки рассылки                                                                              |
| `SETEMAILOPTION [Bots] <Options>`         |            |            | То же, что и `SETEMAILOPTIONS`                                                                           |
| `NOTIFICATIONOPTIONS [Bots]`              | `NOO`      | `Operator` | Выводит параметры уведомлений бота [ссылка](https://store.steampowered.com/account/notificationsettings) |
| `NOTIFICATIONOPTION [Bots]`               |            |            | То же, что и `NOTIFICATIONOPTIONS`                                                                       |
| `SETNOTIFICATIONOPTIONS [Bots] <Options>` | `SNOO`     | `Master`   | Изменяет параметры уведомлений бота                                                                      |
| `SETNOTIFICATIONOPTION [Bots] <Options>`  |            |            | То же, что и `SETNOTIFICATIONOPTIONS`                                                                    |
| `GETBOTBANNED [Bots]`                     | `GBB`      | `Operator` | Выводит информацию о блокировках бота                                                                    |
| `GETBOTBANN [Bots]`                       |            |            | То же, что и `GETBOTBANNED`                                                                              |
| `GETACCOUNTBANNED <SteamIds>`             | `GBB`      | `Operator` | Выводит информацию о блокировках аккаунта стим, поддерживает SteamId 64 / SteamId 32                     |
| `GETACCOUNTBAN <SteamIds>`                |            |            | То же, что и `GETACCOUNTBANNED`                                                                          |
| `EMAIL [Bots]`                            | `EM`       | `Operator` | Выводит информацию о электронной почте бота                                                              |
| `CHECKAPIKEY [Bots]`                      |            | `Operator` | Check if ApiKey exists                                                                                   |
| `REVOKEAPIKEY [Bots]`                     |            | `Master`   | Revoke current ApiKey                                                                                    |
| `GETPRIVACYAPP [Bots]`                    | `GPA`      | `Operator` | 获取私密 APP 列表                                                                                        |
| `SETAPPPRIVATE [Bots] <AppIds>`           | `SAPRI`    | `Master`   | 将指定 APP 设置为私密                                                                                    |
| `SETAPPPUBLIC [Bots] <AppIds>`            | `SAPUB`    | `Master`   | 将指定 APP 设置为公开                                                                                    |
| `CHECKMARKETLIMIT [Bots]`                 | `CML`      | `Operator` | 检查机器人的市场交易权限是否被限制                                                                       |

- значения аргументов команды `SETEMAILOPTION`

  `<Options>` имеет не более 9 аргументов. Используйте пробел или `,` для разделения. Порядок аргументов такой же, как [на этой странице](https://store.steampowered.com/account/emailoptout).
  Для каждого аргументов, такие значения, как `on`, `yes`, `true`, `1`, `y`, являются включением уведомлений на почту, в противном случае выключением (по умолчанию).

| Порядковый номер | Описание                                                                                                                                                     | Примечания                                                              |
| ---------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------ | ----------------------------------------------------------------------- |
| 1                | Включить уведомления по почте                                                                                                                                | Если уведомления отключены, то следующие аргументы будут игнорироваться |
| 2                | Товар из вашего списка желаемого получил скидку.                                                                                                             |                                                                         |
| 3                | Товар из моего списка желаемого стал доступен в раннем доступе или полноценно, либо вышел из раннего доступа.                                                |                                                                         |
| 4                | Продукт из Greenlight, на который вы были подписаны или добавляли в избранное, стал доступен в раннем доступе или полноценно, либо вышел из раннего доступа. |                                                                         |
| 5                | Продукт от издателя или разработчика из моих подписок вышел полностью / в раннем доступе / из раннего доступа.                                               |                                                                         |
| 6                | Начинается ежегодная акция.                                                                                                                                  |                                                                         |
| 7                | Ваша группа в Steam получила на обзор копию игры или программы.                                                                                              |                                                                         |
| 8                | Я получаю награду сообщества Steam.                                                                                                                          |                                                                         |
| 9                | Уведомления о событиях отдельных игр.                                                                                                                        |                                                                         |

- значения аргументов команды `NOTIFICATIONOPTIONS`

  `<Options>` имеет не более 9 аргументов, разделенных пробелами или `,`, в порядке, указанной [на этой странице](https://store.steampowered.com/account/notificationsettings).
  Значение индекса и необязательный диапазон значений приведены в таблице ниже.

| Порядковый номер | Описание                                   |
| ---------------- | ------------------------------------------ |
| 1                | Получении подарка                          |
| 2                | Новом ответе в обсуждении из моих подписок |
| 3                | Получении нового предмета                  |
| 4                | Приглашении в друзья                       |
| 5                | Крупной распродаже                         |
| 6                | Скидке на товар из моего списка желаемого  |
| 7                | Новом предложении обмена                   |
| 8                | Получении ответа от службы поддержки       |
| 9                | Моей очереди делать ход                    |

| Параметр | Описание                                                                                               |
| -------- | ------------------------------------------------------------------------------------------------------ |
| 0        | Отключить уведомления                                                                                  |
| 1        | Включить уведомления                                                                                   |
| 2        | Включить уведомления, Всплывающее уведомление в клиенте Steam                                          |
| 3        | Включить уведомления, Push-уведомление в мобильном приложении                                          |
| 4        | Включить уведомления, Всплывающее уведомление в клиенте Steam, Push-уведомление в мобильном приложении |

### Остальные Команды

| Команда          | Сокращение | Доступ          | Описание                          |
| ---------------- | ---------- | --------------- | --------------------------------- |
| `KEY <Text>`     | `K`        | `Any`           | Извлечь ключи из текста           |
| `ASFEHELP`       | `EHELP`    | `FamilySharing` | Получить все использования команд |
| `HELP <Command>` | -          | `FamilySharing` | Получить использование команды    |

## Команды Группы

| Команда                       | Сокращение | Доступ          | Описание                             |
| ----------------------------- | ---------- | --------------- | ------------------------------------ |
| `GROUPLIST [Bots]`            | `GL`       | `FamilySharing` | Выводит список групп бота            |
| `JOINGROUP [Bots] <GroupUrl>` | `JG`       | `Master`        | Присоединиться к определённой группе |
| `LEAVEGROUP [Bots] <GroupID>` | `LG`       | `Master`        | Покинуть определённую группу         |

> `GroupID` можно узнать при помощи команды `GROUPLIST`

## Команды Профиля

| Команда                                | Сокращение | Доступ          | Описание                                                                                                                                                                                                                            |
| -------------------------------------- | ---------- | --------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `PROFILE [Bots]`                       | `PF`       | `FamilySharing` | Информация о профиле бота                                                                                                                                                                                                           |
| `PROFILELINK [Bots]`                   | `PFL`      | `FamilySharing` | Ссылка на Steam профиль бота                                                                                                                                                                                                        |
| `STEAMID [Bots]`                       | `SID`      | `FamilySharing` | steamID64 бота                                                                                                                                                                                                                      |
| `FRIENDCODE [Bots]`                    | `FC`       | `FamilySharing` | «Код для друга» бота                                                                                                                                                                                                                |
| `TRADELINK [Bots]`                     | `TL`       | `Operator`      | «Ссылка на обмен» бота                                                                                                                                                                                                              |
| `REPLAY [Year] [Bots]`                 | `RP`       | `Operator`      | Получить ссылку на баннер `Steam Replay 2022` (позволяет разблокировать значок), if given 2 or more args, the first will be treat as Year(2022/2023)                                                                                |
| `REPLAYPRIVACY [Year] [Bots] Privacy`  | `RPP`      | `Operator`      | Установить настройки приватности баннера `Steam Replay 2022`. Параметр `Privacy` может иметь следующие значения: `1=Скрыто` `2=Только для друзей` `3=Для всех`, if given 3 or more args, the first will be treat as Year(2022/2023) |
| `CLEARALIAS [Bots]`                    |            | `Opetator`      | Очистить историю имён                                                                                                                                                                                                               |
| `GAMEAVATAR [Bots] <AppID> [AvatarID]` | `GA`       | `Opetator`      | Установить аватар бота в соответствии с заданными `AppID` и `AvatarID`, если не задать `AvatarId`, то плагин будет использовать случайный аватар                                                                                    |
| `RANDOMGAMEAVATAR [Bots]`              | `RGA`      | `Opetator`      | Установка случайного аватара бота                                                                                                                                                                                                   |
| `ADVNICKNAME [Bots] Query`             | `ANN`      | `Master`        | Установить использование псевдонима бота в соответствии с `Placeholder`, доступны значения: `%dn%` `%ln%` `%un%` `%botn%`                                                                                                           |
| `SETAVATAR [Bots] ImageUrl` 🐞         | `GA`       | `Opetator`      | Установить аватар бота в соответствии c cсылкойна указанное изображение в Интернете                                                                                                                                                 |
| `DELETEAVATAR [Bots]` 🐞               |            | `Master`        | Удалить аватар бота (сбросить на стандартный)                                                                                                                                                                                       |
| `CRAFTBADGE [Bots]`                    | `CB`       | `Master`        | Автоматическое изготовление крафтовых значков (изготовление всех крафтовых значков за один раз)                                                                                                                                     |
| `EDITCUSTOMURL [Bot] CustomUrl`        | `ECU`      | `Master`        | Edit bot's custom profile url                                                                                                                                                                                                       |
| `DELETECUSTOMURL [Bots]`               | `DCU`      | `Master`        | Delete bot's custom profile url                                                                                                                                                                                                     |

\*🐞: Необходимая релизная версия ASF (**Не** предварительно выпущенная)

- Описание команды `GAMEAVATAR`

  Все аватары беруться с [страницы игровых аватаров](https://steamcommunity.com/actions/GameAvatars/)

---

- Описание параметров команды `ADVNICKNAME`

> "n" означает любое число

| Параметры | Описание                           | Пример                     |
| --------- | ---------------------------------- | -------------------------- |
| `%d%`     | Случайная цифра                    | `5`                        |
| `%dn%`    | n Случайных цифр                   | `%d6%` -> `114514`         |
| `%l%`     | Случайная буква нижнего регистра   | `x`                        |
| `%ln%`    | n Случайных букв нижнего регистра  | `%d7%` -> `asfeadf`        |
| `%u%`     | Случайная буква верхнего регистра  | `C`                        |
| `%un%`    | n Случайных букв верхнего регистра | `%d8%` -> `ASXCGDFA`       |
| `%bot%`   | ник бота                           | `ASFE`                     |
| `%bot3%`  | повторит ник бота 3 раза           | `%bot3%` -> `ASFEASFEASFE` |

### Команды Куратора

| Команда                          | Сокращение | Доступ   | Описание                             |
| -------------------------------- | ---------- | -------- | ------------------------------------ |
| `CURATORLIST [Bots]`             | `CL`       | `Master` | Выводит список кураторов в подписках |
| `FOLLOWCURATOR [Bots] <ClanIDs>` | `FCU`      | `Master` | Подписаться на куратора              |
| `UNFOLLOWCURATOR [Bots]`         | `UFC`      | `Master` | Описаться от куратора                |
| `UNFOLLOWALLCURATORS [Bots]`     | `UFACU`    | `Master` | Отписаться от всех кураторов         |
| `UNFOLLOWALLCURATOR [Bots]`      |            |          | То же, что и `UNFOLLOWALLCURATORS`   |

> `ClanID` можно найти по веб-ссылке куратора или с помощью команды `CURATORLIST`

### Команды Списка Желаний

| Команда                          | Сокращение | Доступ   | Описание                                                       |
| -------------------------------- | ---------- | -------- | -------------------------------------------------------------- |
| `ADDWISHLIST [Bots] <AppIDs>`    | `AW`       | `Master` | Добавить боту игру в список желаемого                          |
| `REMOVEWISHLIST [Bots] <AppIDs>` | `RW`       | `Master` | Убрать у бота игру из списка желаемого                         |
| `FOLLOWGAME [Bots] <AppIDs>`     | `FG`       | `Master` | Подписаться на определённую игру                               |
| `UNFOLLOWGAME [Bots] <AppIDs>`   | `UFG`      | `Master` | Отписаться от определённой игры                                |
| `CHECK [Bots] <AppIDs>`          | `CK`       | `Master` | Проверить наличие игры в библиотеке/списке желаемого/подписках |

### Команды Магазина

| Команда                                    | Сокращение | Доступ     | Описание                                                                          |
| ------------------------------------------ | ---------- | ---------- | --------------------------------------------------------------------------------- |
| `APPDETAIL [Bots] <AppIDs>`                | `AD`       | `Operator` | Информация об игре от Steam API                                                   |
| `SUBS`                                     | `S`        | `Operator` | Same as `APPDETAIL`                                                               |
| `SEARCH [Bots] Keywords`                   | `SS`       | `Operator` | Поиск по магазину Steam                                                           |
| `PUBLISHRECOMMENT [Bots] <AppIDs> COMMENT` | `PREC`     | `Master`   | Опубликовать обзор на игру, `appID` или `+appId` позитивный , `-appId` негативный |
| `DELETERECOMMENT [Bots] <AppIDs>`          | `DREC`     | `Master`   | Удалить обзор на игру                                                             |
| `RECOMMENT [Bots] <AppIDs>`                | `REC`      | `Master`   | 获取评测内容                                                                      |
| `REQUESTACCESS [Bots] <AppIDs>`            | `RA`       | `Operator` | Отправить заявку на playtest игры, равноценно нажатию кнопки `Запросить доступ`   |
| `VIEWPAGE [Bots] Url`                      | `VP`       | `Operator` | Посетить указанную страницу                                                       |
| `REDEEMPOINTSITEM [Bots] <defIds>`         | `RPI`      | `Master`   | Redeem item in the points shop                                                    |
| `REDEEMPOINTITEM [Bots] <defIds>`          |            | `Master`   | Same as `REDEEMPOINTSITEM`                                                        |
| `REDEEMPOINTSBADGE [Bots] defId level`     | `RPB`      | `Master`   | Redeem session badge in the points shop                                           |
| `REDEEMPOINTBADGE  [Bots] defId level`     |            | `Master`   | Same as `REDEEMPOINTSBADGE`                                                       |

> defId can be found in SteamDB, for example, the `Winter Collection - 2023`'s defId is `258511`, see [here](https://steamdb.info/app/2750340/communityitems/#item-class-1-data)

### Команды Корзины

> Steam сохраняет информацию о корзине покупок с помощью файлов cookie, перезапуск экземпляра бота приведет к очистке корзины

| Команда                                          | Сокращение | Доступ     | Описание                                                                                      |
| ------------------------------------------------ | ---------- | ---------- | --------------------------------------------------------------------------------------------- |
| `CART [Bots]`                                    | `C`        | `Operator` | Информация о товарах в корзине магазина Steam                                                 |
| `ADDCART [Bots] <SubIDs\|BundleIDs>`             | `AC`       | `Operator` | Добавить игру в корзину, поддерживает только `SUB/BUNDLE`                                     |
| `ADDCARTPRIVATE [Bots] <SubIDs\|BundleIDs>`      | `ACP`      | `Operator` | 添加购物车, 设置为私密购买                                                                    |
| `ADDCARTGIFT [Bots] <SubIDs\|BundleIDs> SteamId` | `ACG`      | `Operator` | 添加购物车, 设置为礼物赠送, SteamId 支持 botName 或者 SteamID32 或者 SteamId64                |
| `EDITCART [Bots] <lineItemIds>`                  | `EC`       | `Operator` | 编辑购物车项目, 设置为为自己购买                                                              |
| `EDITCARTPRIVATE [Bots] <lineItemIds>`           | `ECP`      | `Operator` | 编辑购物车项目, 设置为私密购买                                                                |
| `EDITCARTGIFT [Bots] <lineItemIds> SteamId`      | `ECG`      | `Operator` | 编辑购物车项目, 设置为礼物赠送, SteamId 支持 botName 或者 SteamID32 或者 SteamId64            |
| `DELETECART [Bots] <lineItemIds>`                | `DC`       | `Operator` | 删除购物车项目                                                                                |
| `CARTRESET [Bots]`                               | `CR`       | `Operator` | Очистить корзину                                                                              |
| `CARTCOUNTRY [Bots]`                             | `CC`       | `Operator` | Информация о доступной валюте (Зависит от IP адреса и страны кошелька)                        |
| `FAKEPURCHASE [Bots]`                            | `FPC`      | `Master`   | Имитация корзины бота-покупателя и создание записи о неудаче без реального оформления покупки |
| `PURCHASE [Bots]`                                | `PC`       | `Master`   | Купить товары из корзины бота «для себя» (оплата через Steam кошелёк)                         |

> Steam позволяет дублировать покупки, пожалуйста, проверьте корзину перед использованием команды `PURCHASE`.

### Команды Сообщества

| Команда                    | Сокращение | Доступ     | Описание                                                               |
| -------------------------- | ---------- | ---------- | ---------------------------------------------------------------------- |
| `CLEARNOTIFICATION [Bots]` | `CN`       | `Operator` | Очистить уведомления о новых предметах инвентаря и новых комментариях. |

### Команды друзей

| Команда                        | Сокращение | Доступ     | Описание                                                                                                                   |
| ------------------------------ | ---------- | ---------- | -------------------------------------------------------------------------------------------------------------------------- |
| `ADDBOTFRIEND <Bots>`          | `ABF`      | `Master`   | `Bots` добавляют друг друга в друзья                                                                                       |
| `ADDBOTFRIEND <BotAs>+<BotBs>` |            | `Master`   | `BotAs` добавляют друг друга в друзья, тогда пусть `BotAs` добавляет `BotBs` в друзья                                      |
| `ADDFRIEND [Bots] <Text>`      | `AF`       | `Master`   | `Bots` отправляют запросы на дружбу другим пользователям, с помощью `custom Url`, `steamId`, `Friend code`, `Invlite link` |
| `DELETEFRIEND [Bots] <Text>`   | `DF`       | `Master`   | `Bots` удаляют друга, `Text` с помощью `custom Url`, `steamId`, `Friend code`                                              |
| `DELETEALLFRIEND [Bots]`       |            | `Master`   | `Bots` удаляют всех своих друзей                                                                                           |
| `INVITELINK [Bots]`            | `IL`       | `Operator` | `Bots` создают ссылки для быстрое приглашение друга                                                                        |

- Пример использования команды `ADDBOTFRIEND`
  - `ADDBOTFRIEND a,b c`: Пусть `a`,`b`,`c` добавятся в друзья друг к другу
  - `ADDBOTFRIEND a,b,c + d,e`: Пусть `a`,`b`,`c` добавятся в друзья друг другу, то пусть `a`, `b`, `c` добавят `d` и `e` в друзья, но `d` не будет добавлять `e` в друзья
  - `ADDBOTFRIEND ASF`: Разрешено использование значения `ASF` вместо ников ботов
  - `ADDBOTFRIEND a b c + ASF`: Разрешено использование значения `ASF` вместо ников ботов
  - `ADDBOTFRIEND ASF + ASF`: Разрешено, но бессмысленно

### Команды Списка Рекомендаций

| Команда                           | Сокращение | Доступ   | Описание                                                            |
| --------------------------------- | ---------- | -------- | ------------------------------------------------------------------- |
| `EXPLORER [Bots]`                 | `EX`       | `Master` | Вызвать задачу ASF "Просмотреть список рекомендаций" через 5 секунд |
| `ENABLEAUTOSTEAMSALEEVENT [Bots]` | `EASSE`    | `Master` | 为指定机器人开启 `AutoSteamSaleEvent` 设置                          |

> Пожалуйста, по возможности, позвольте ASF просматривать список рекомендаций самому, эта команда используется для просмотра списка рекомендаций как можно скорее

### Команды кошелька

| Команда                          | Сокращение | Доступ   | Описание                                                                                                   |
| -------------------------------- | ---------- | -------- | ---------------------------------------------------------------------------------------------------------- |
| `REDEEMWALLET [Bots] <keys>`     | `RWA`      | `Master` | Код кошелька, если требуется адрес для выставления счета, будет использоваться адрес, указанный в ASF.json |
| `REDEEMWALLETMULT [Bots] <keys>` | `RWAM`     | `Master` | Использование кода кошелька, но каждый бот будет использовать только один данный код                       |

## Inventory Commands

> 物品堆叠和取消堆叠会发送大量请求, 请不要对大量机器人同时使用这些命令, 有可能会因为网络请求过多导致临时封禁

| Command                                   | Shorthand | Access     | Description                                                                         |
| ----------------------------------------- | --------- | ---------- | ----------------------------------------------------------------------------------- |
| `STACKINVENTORY [Bots] AppId ContextId`   | `STI`     | `Operator` | 将指定 AppId 的物品库存中同类物品堆叠在一起, 对于大部分 App 来说, `ContextId` = 2   |
| `UNSTACKINVENTORY [Bots] AppId ContextId` | `USTI`    | `Operator` | 将指定 AppId 的物品库存中堆叠后的物品解除堆叠, 对于大部分 App 来说, `ContextId` = 2 |

### Сокращения Команд ASF

| Сокращение             | Команда                        | Описание                             |
| ---------------------- | ------------------------------ | ------------------------------------ |
| `AL [Bots] <Licenses>` | `ADDLICENSE [Bots] <Licenses>` | Добавить бесплатную `SUB` (лицензию) |
| `LA`                   | `LEVEL ASF`                    | Уровни профиля Steam всех ботов      |
| `BA`                   | `BALANCE ASF`                  | Баланс Steam кошелька всех ботов     |
| `PA`                   | `POINTS ASF`                   | Баланс очков у всех ботов            |
| `P [Bots]`             | `POINTS`                       | Баланс очков у бота                  |
| `CA`                   | `CART ASF`                     | Информация о корзине всех ботов      |

### Для Разработчика

> Эта группа команд по умолчанию отключена.
> Вам необходимо добавить `"DevFeature": true` в `ASF.json` для активации.

| Команда              | Доступ   | Описание                          |
| -------------------- | -------- | --------------------------------- |
| `COOKIES [Bots]`     | `Master` | Получить `Cookies` магазина Steam |
| `APIKEY [Bots]`      | `Master` | Получить `APIKey` бота            |
| `ACCESSTOKEN [Bots]` | `Master` | Получить `ACCESSTOKEN` бота       |

## IPC Интерфейс

> Вы должны принять [лицензионное соглашение](#лицензионное-соглашение) перед использованием интерфейса IPC. Смотри [Конфигурацию Плагина](#конфигурация-плагина)

| API                                         | 方法 | 说明                     |
| ------------------------------------------- | ---- | ------------------------ |
| `/Api/Curator/FollowCurator/{botNames}`     | POST | 关注鉴赏家               |
| `/Api/Curator/UnFollowCurator/{botNames}`   | POST | 取消关注鉴赏家           |
| `/Api/Curator/FollowingCurators/{botNames}` | POST | 获取已关注的鉴赏家列表   |
| `/Api/Purchase/GetAppDetail/{botNames}`     | POST | 获取游戏详情             |
| `/Api/Purchase/ClearCart/{botNames}`        | POST | 清空购物车内容           |
| `/Api/Purchase/GetCart/{botNames}`          | POST | 获取购物车内容           |
| `/Api/Purchase/AddCart/{botNames}`          | POST | 添加购物车项目           |
| `/Api/Purchase/Purchase/{botNames}`         | POST | 结算购物车               |
| `/Api/Recommend/PublishReview/{botNames}`   | POST | 发布游戏评测             |
| `/Api/Recommend/DeleteReview/{botNames}`    | POST | 删除游戏评测             |
| `/Api/Wishlist/AddWishlist/{botNames}`      | POST | 添加愿望单               |
| `/Api/Wishlist/RemoveWishlist/{botNames}`   | POST | 移除愿望单               |
| `/Api/Wishlist/FollowGame/{botNames}`       | POST | 关注游戏                 |
| `/Api/Wishlist/UnFollowGame/{botNames}`     | POST | 取消关注游戏             |
| `/Api/Wishlist/CheckGame/{botNames}`        | POST | 检查游戏关注和愿望单情况 |

<details>
  <summary>ASFEnhance 2.0.14.2 Or earlier version's IPC interfaces</summary>

| API                                            | Метод  | Параметры                                          | Описание                                       |
| ---------------------------------------------- | ------ | -------------------------------------------------- | ---------------------------------------------- |
| `/Api/ASFEnhance/{botNames}/FollowCurator`     | `POST` | ClanIDs                                            | Подписаться на куратора                        |
| `/Api/ASFEnhance/{botNames}/UnFollowCurator`   | `POST` | ClanIDs                                            | Отписаться от куратора                         |
| `/Api/ASFEnhance/{botNames}/FollowingCurators` | `POST` | Start, Count                                       | Получить подписки на кураторов                 |
| `/Api/ASFEnhance/{botNames}/GetAppDetail`      | `POST` | AppIDs                                             | Получить информацию о приложении               |
| `/Api/ASFEnhance/{botNames}/Purchase`          | `POST` | SubIDs, BundleIDs, SkipOwned                       | Купить                                         |
| `/Api/ASFEnhance/{botNames}/PublishReview`     | `POST` | AppIDs, RateUp, AllowReply, ForFree,Public,Comment | Опубликовать отзыв                             |
| `/Api/ASFEnhance/{botNames}/DeleteReview`      | `POST` | AppIDs                                             | Удалить отзыв                                  |
| `/Api/ASFEnhance/{botNames}/AddWishlist`       | `POST` | AppIDs                                             | Добавить в Список желаний                      |
| `/Api/ASFEnhance/{botNames}/RemoveWishlist`    | `POST` | AppIDs                                             | Удалить из Списка желаний                      |
| `/Api/ASFEnhance/{botNames}/FollowGame`        | `POST` | AppIDs                                             | Подписаться на игру                            |
| `/Api/ASFEnhance/{botNames}/UnFollowGame`      | `POST` | AppIDs                                             | Отписаться от игры                             |
| `/Api/ASFEnhance/{botNames}/CheckGame`         | `POST` | AppIDs                                             | Проверить наличие Подписки/Списка желания игры |

</details>

---

[![Repobeats analytics image](https://repobeats.axiom.co/api/embed/df6309642cc2a447195c816473e7e54e8ae849f9.svg "Repobeats analytics image")](https://github.com/chr233/ASFEnhance/pulse)

---

[![Stargazers over time](https://starchart.cc/chr233/ASFEnhance.svg)](https://github.com/chr233/ASFEnhance/stargazers)

---
