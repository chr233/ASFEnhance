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

[中文说明](README.md) | [English Version](README.en.md)

## ЛИЦЕНЗИОННОЕ СОГЛАШЕНИЕ

> Пожалуйста, не используйте этот плагин для отвратительного поведения, включая, но не ограничиваясь: публикацией фейковых отзывов, размещением рекламы и т.д.
>
> Смотри [Конфигурацию Плагина](#конфигурация-плагина)

## КОМАНДЫ СОБЫТИЙ

> This group of commands is only available in a limited time, and will be removed when next version of this plugin published if it lose efficacy

| Команда                    | Доступ     | Описание                                                                                                                                                                       |
| -------------------------- | ---------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| `CLAIM20TH [Bots]`         | `Operator` | Claim 20th free item in the Point shop                                                                                                                                         |
| `C20 [Bots]`               | `Operator` | Same as `CLAIM20TH`                                                                                                                                                            |
| `CLAIMITEM [Bots]`         | `Operator` | Claim sale event's item, such as stickers or something else                                                                                                                    |
| `CI [Bots]`                | `Operator` | Same as `CLAIMITEM`                                                                                                                                                            |
| `SIM4 [Bots]`              | `Operator` | Получить стикеры `The Sims™ 4` [url](https://store.steampowered.com/sale/simscelebrationsale)                                                                                  |
| `DL2 [Bots]`               | `Operator` | Получить вещи `Dying Light 2 Stay Human` [url](https://store.steampowered.com/sale/dyinglight)                                                                                 |
| `DL22 [Bots] [Sticker Id]` | `Operator` | Claim the `Dying Light 2 Stay Human` items, `Sticker Id` is not required, value can be 1 to 8 [url](https://store.steampowered.com/developer/Techland/sale/techlandsummer2023) |
| `RLE [Bots] [Sticker Id]`  | `Operator` | Claim the `Redfall Launch Event` items, `Sticker Id` is not required, value can be 1 to 4 [url](https://store.steampowered.com/sale/redfall_launch)                            |

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

| Версия ASFEnhance                                                      | Совместимая версия ASF | Описание                                    |
| ---------------------------------------------------------------------- | :--------------------: | ------------------------------------------- |
| [1.8.11.1](https://github.com/chr233/ASFEnhance/releases/tag/1.8.11.1) |        5.4.9.3         | 新增 `CLAIM20TH` 命令                       |
| [1.8.10.0](https://github.com/chr233/ASFEnhance/releases/tag/1.8.10.0) |        5.4.9.3         | 新增 `GETACCOUNTBANNED` `GETBOTBANNED` 命令 |
| [1.8.9.0](https://github.com/chr233/ASFEnhance/releases/tag/1.8.9.0)   |        5.4.9.3         | ASF -> 5.4.9.3                              |
| [1.8.7.0](https://github.com/chr233/ASFEnhance/releases/tag/1.8.8.0)   |        5.4.8.3         | ASF -> 5.4.8.3, 不兼容旧版                  |
| [1.8.7.1](https://github.com/chr233/ASFEnhance/releases/tag/1.8.7.1)   |        5.4.7.3         | 新增 `NOTIFICATIONOPTIONS` 命令             |
| [1.8.6.2](https://github.com/chr233/ASFEnhance/releases/tag/1.8.6.2)   |        5.4.7.3         | 新增 `DL22` 命令                            |
| [1.8.5.0](https://github.com/chr233/ASFEnhance/releases/tag/1.8.5.0)   |        5.4.7.3         | ASF -> 5.4.7.3, 修复 `PURCHASE` 命令        |

<details>
  <summary>История версий</summary>

| Версия ASFEnhance                                                    | Зависит от ASF | 5.4.5.2 | 5.4.6.3 | 5.4.7.3 | 5.4.8.3 |
| -------------------------------------------------------------------- | :------------: | :-----: | :-----: | :-----: | :-----: |
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

> The configuration of this plugin is not required, and most functions is avilable in default settings

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

| Конфигурация      | Тип    | По умолчанию | Описание                                                                                                                                                   |
| ----------------- | ------ | ------------ | ---------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `EULA`            | `bool` | `true`       | Если согласны с [лицензионным соглашением](#лицензионное-соглашение)\*                                                                                     |
| `Statistic`       | `bool` | `true`       | Разрешить отправку данных для статистики. Она используется для подсчета количества пользователей, при этом никакой другой информации отправляться не будет |
| `DevFeature`      | `bool` | `false`      | Включить функции [разработчика (3 команды)](#для-разработчика) `Может представить угрозу безопасности, включайте на свой страх и риск`                     |
| `DisabledCmds`    | `list` | `null`       | Cmd in the list will be disabled\*\* , **Case Insensitive**, only effects on `ASFEnhance` cmds                                                             |
| `Address`\*\*\*   | `dict` | `null`       | 单个账单地址, 使用 `REDEEMWALLET` 命令激活钱包兑换码如果要求设置账单地址时自动使用                                                                         |
| `Addresses`\*\*\* | `list` | `null`       | 多个账单地址, 需要账单地址时从列表中随机使用一个                                                                                                           |

> \* Если Вы согласны с [лицензионным соглашением](#лицензионное-соглашение), то в ASFEnhance будут доступны все команды, в обмен на это, при использовании команд `GROUPLIST` и `CURATORLIST`, ASFEnhance подпишется на [Куратора](https://store.steampowered.com/curator/39487086/) и [Группу](https://steamcommunity.com/groups/11012580) (если бот не подписался или не присоединился)
>
> \* Если Вы не согласны с [лицензионным соглашением](#лицензионное-соглашение), то ASFEnhance ограничит команды куратора/подписок на игры/групп/обзоров, и ASFEnhance не подпишется на [Куратора](https://store.steampowered.com/curator/39487086/) и [Группу](https://steamcommunity.com/groups/11012580)
>
> \*\* `DisabledCmds` description: every item in this configuration is **Case Insensitive**, and this only effects on `ASFEnhance` cmds
> For example, configure as `["foo","BAR"]` , it means `FOO` and `BAR` will be disabled
> If don't want to disable any cmds, please configure as `null` or `[]`
> If Some cmd is disabled, it's still avilable to call the command in the form of `ASFE.xxx`, for example `ASFE.EXPLORER`
>
> \*\*\* `Address` and `Addresses` is the same configuration

## Использование Команд

### Команды Обновления

| Команда       | Сокращение | Доступ          | Описание                                                                  |
| ------------- | ---------- | --------------- | ------------------------------------------------------------------------- |
| `ASFENHANCE`  | `ASFE`     | `FamilySharing` | Получить версию ASFEnhance                                                |
| `ASFEVERSION` | `AV`       | `Owner`         | Проверить последнюю версию ASFEnhance                                     |
| `ASFEUPDATE`  | `AU`       | `Owner`         | Обновить ASFEnhance до последней версии (необходим ручной перезапуск ASF) |

### Команды Аккаунта

| Команда                                   | Сокращение | Доступ     | Описание                                                                                  |
| ----------------------------------------- | ---------- | ---------- | ----------------------------------------------------------------------------------------- |
| `PURCHASEHISTORY [Bots]`                  | `PH`       | `Operator` | Выводит историю покупок бота                                                              |
| `FREELICENSES [Bots]`                     | `FL`       | `Operator` | Выводит список всех бесплатных лицензий бота                                              |
| `FREELICENSE [Bots]`                      |            |            | То же, что и `FREELICENSES`                                                               |
| `LICENSES [Bots]`                         | `L`        | `Operator` | Выводит список всех SUB (лицензий) бота                                                   |
| `LICENSE [Bots]`                          |            |            | То же, что и `LICENSES`                                                                   |
| `REMOVEDEMOS [Bots]`                      | `RD`       | `Master`   | Удаляет все демо-лицензии бота                                                            |
| `REMOVEDEMO [Bots]`                       |            |            | То же, что и `REMOVEDEMOS`                                                                |
| `REMOVELICENSES [Bots] <SubIDs>`          | `RL`       | `Master`   | Удаляет определённую лицензию бота по subIDs                                              |
| `REMOVELICENSE [Bots] <SubIDs>`           |            |            | То же, что и `REMOVELICENSES`                                                             |
| `EMAILIOPTIONS [Bots]`                    | `EO`       | `Operator` | Выводит настройки рассылки бота [url](https://store.steampowered.com/account/emailoptout) |
| `EMAILIOPTION [Bots]`                     |            |            | То же, что и `EMAILIOPTIONS`                                                              |
| `SETEMAILOPTIONS [Bots] <Options>`        | `SEO`      | `Master`   | Изменяет настройки рассылки                                                               |
| `SETEMAILOPTION [Bots] <Options>`         |            |            | То же, что и `SETEMAILOPTIONS`                                                            |
| `NOTIFICATIONOPTIONS [Bots]`              | `NOO`      | `Operator` | 读取账户中的通知选项 [url](https://store.steampowered.com/account/notificationsettings)   |
| `NOTIFICATIONOPTION [Bots]`               |            |            | 同 `NOTIFICATIONOPTIONS`                                                                  |
| `SETNOTIFICATIONOPTIONS [Bots] <Options>` | `SNOO`     | `Master`   | 设置账户中的通知选项                                                                      |
| `SETNOTIFICATIONOPTION [Bots] <Options>`  |            |            | 同 `SETNOTIFICATIONOPTIONS`                                                               |
| `GETBOTBANNED [Bots]`                     | `GBB`      | `Operator` | 获取机器人的账户封禁情况                                                                  |
| `GETBOTBANN [Bots]`                       |            |            | 同 `GETBOTBANNED`                                                                         |
| `GETACCOUNTBANNED <SteamIds>`             | `GBB`      | `Operator` | 获取指定账户封禁情况, 支持 SteamId 64 / SteamId 32                                        |
| `GETACCOUNTBAN <SteamIds>`                |            |            | 同 `GETACCOUNTBANNED`                                                                     |

- `SETEMAILOPTION` значения аргументов

  `<Options>` имеет не более 9 аргументов. Используйте пробел или `,` для разделения. Порядок аргументов такой же, как [на этой странице](https://store.steampowered.com/account/emailoptout).
  Для каждого аргумента, такие значения, как `on`, `yes`, `true`, `1`, `y`, являются включением уведомлений на почту, в противном случае выключением (по умолчанию).

| Порядочный номер | Описание                                                                                                                                                     | Примечания                                                              |
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

- `SETNOTIFICATIONS` 参数说明

  `<Options>` 参数接受最多 9 个参数, 使用空格或者 `,` 分隔, 顺序参照 [url](https://store.steampowered.com/account/notificationsettings)
  索引含义和设置值可选的范围见下表

| Порядочный номер | Описание                                   |
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

| Option | Description                                                                                            |
| ------ | ------------------------------------------------------------------------------------------------------ |
| 0      | отключить уведомления                                                                                  |
| 1      | Включить уведомления                                                                                   |
| 2      | Включить уведомления, Всплывающее уведомление в клиенте Steam                                          |
| 3      | Включить уведомления, Push-уведомление в мобильном приложении                                          |
| 4      | Включить уведомления, Всплывающее уведомление в клиенте Steam, Push-уведомление в мобильном приложении |

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

| Команда                                | Сокращение | Доступ          | Описание                                                                                                                                                       |
| -------------------------------------- | ---------- | --------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `PROFILE [Bots]`                       | `PF`       | `FamilySharing` | Информация о профиле бота                                                                                                                                      |
| `PROFILELINK [Bots]`                   | `PFL`      | `FamilySharing` | Ссылка на Steam профиль бота                                                                                                                                   |
| `STEAMID [Bots]`                       | `SID`      | `FamilySharing` | steamID64 бота                                                                                                                                                 |
| `FRIENDCODE [Bots]`                    | `FC`       | `FamilySharing` | «Код для друга» бота                                                                                                                                           |
| `TRADELINK [Bots]`                     | `TL`       | `Operator`      | «Ссылка на обмен» бота                                                                                                                                         |
| `REPLAY [Bots]`                        | `RP`       | `Operator`      | Получить ссылку на баннер `Steam Replay 2022` (позволяет разблокировать значок)                                                                                |
| `REPLAYPRIVACY [Bots] Privacy`         | `RPP`      | `Operator`      | Установить настройки приватности баннера `Steam Replay 2022`. Параметр `Privacy` может иметь следующие значения: `1=Скрыто` `2=Только для друзей` `3=Для всех` |
| `CLEARALIAS [Bots]`                    |            | `Opetator`      | Очистить историю имён                                                                                                                                          |
| `GAMEAVATAR [Bots] <AppID> [AvatarID]` | `GA`       | `Opetator`      | Set bot's avatar as given `AppID` and `AvatarID`, if not set `AvatarId`, plugin will use random one                                                            |
| `RANDOMGAMEAVATAR [Bots]`              | `RGA`      | `Opetator`      | Set bot's avatar randomly                                                                                                                                      |
| `ADVNICKNAME [Bots] Query`             | `ANN`      | `Master`        | Set bot's nickname use `Placeholder`, avilable: `%dn%` `%ln%` `%un%` `%botn%`, case insensitive                                                                |
| `SETAVATAR [Bots] ImageUrl` 🐞         | `GA`       | `Opetator`      | Set bot's avatar to specified online image                                                                                                                     |
| `DELETEAVATAR [Bots]` 🐞               |            | `Master`        | Delete bot's avatar (reset to default)                                                                                                                         |
| `CRAFTBADGE [Bots]`                    | `CB`       | `Master`        | Automatically craft craftable badges (craft every craftable badge once at one time)                                                                            |

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

### Команды Куратора

| Команда                          | Сокращение | Доступ   | Описание                             |
| -------------------------------- | ---------- | -------- | ------------------------------------ |
| `CURATORLIST [Bots]`             | `CL`       | `Master` | Выводит список кураторов в подписках |
| `FOLLOWCURATOR [Bots] <ClanIDs>` | `FCU`      | `Master` | Подписаться на куратора              |
| `UNFOLLOWCURATOR [Bots]`         | `UFC`      | `Master` | Описаться от куратора                |
| `UNFOLLOWALLCURATORS [Bots]`     | `UFACU`    | `Master` | Unfollow all curators                |
| `UNFOLLOWALLCURATOR [Bots]`      |            |          | Same as `UNFOLLOWALLCURATORS`        |

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

| Команда                                    | Сокращение | Доступ     | Описание                                                                                      |
| ------------------------------------------ | ---------- | ---------- | --------------------------------------------------------------------------------------------- |
| `APPDETAIL [Bots] <AppIDs>`                | `AD`       | `Operator` | Информация об игре от Steam API, поддерживает `APP`                                           |
| `SEARCH [Bots] Keywords`                   | `SS`       | `Operator` | Поиск по магазину Steam                                                                       |
| `SUBS [Bots] <AppIDs\|SubIDs\|BundleIDs>`  | `S`        | `Operator` | Показать доступные «SUB» (лицензии) со страницы магазина Steam, поддерживает `APP/SUB/BUNDLE` |
| `PUBLISHRECOMMENT [Bots] <AppIDs> COMMENT` | `PREC`     | `Operator` | Опубликовать обзор на игру, `appID` or `+appId` rateUp, `-appId` reteDown                     |
| `DELETERECOMMENT [Bots] <AppIDs>`          | `DREC`     | `Operator` | Удалить обзор на игру                                                                         |
| `REQUESTACCESS [Bots] <AppIDs>`            | `RA`       | `Operator` | Отправить заявку на playtest игры, равноценно нажатию кнопки `Запросить доступ`               |
| `VIEWPAGE [Bots] Url`                      | `VP`       | `Operator` | Посетить указанную страницу                                                                   |

### Команды Корзины

> Steam сохраняет информацию о корзине покупок с помощью файлов cookie, перезапуск экземпляра бота приведет к очистке корзины

| Команда                              | Сокращение | Доступ     | Описание                                                                                 |
| ------------------------------------ | ---------- | ---------- | ---------------------------------------------------------------------------------------- |
| `CART [Bots]`                        | `C`        | `Operator` | Информация о товарах в корзине магазина Steam                                            |
| `ADDCART [Bots] <SubIDs\|BundleIDs>` | `AC`       | `Operator` | Добавить игру в корзину, поддерживает только `SUB/BUNDLE`                                |
| `CARTRESET [Bots]`                   | `CR`       | `Operator` | Очистить корзину                                                                         |
| `CARTCOUNTRY [Bots]`                 | `CC`       | `Operator` | Информация о доступной валюте (Зависит от IP адреса и страны кошелька)                   |
| `FAKEPURCHASE [Bots]`                | `FPC`      | `Master`   | Simulate purchase bot's cart, and generate a failed record without actually checking out |
| `PURCHASE [Bots]`                    | `PC`       | `Master`   | Купить товары из корзины бота «для себя» (оплата через Steam кошелёк)                    |
| `PURCHASEGIFT [BotA] BotB`           | `PCG`      | `Master`   | Купить товары из корзины `BotA` в подарок для `BotB` (оплата через Steam кошелёк)        |

> Steam позволяет дублировать покупки, пожалуйста, проверьте корзину перед использованием команды `PURCHASE`.

### Команды Сообщества

| Команда                    | Сокращение | Доступ     | Описание                                                               |
| -------------------------- | ---------- | ---------- | ---------------------------------------------------------------------- |
| `CLEARNOTIFICATION [Bots]` | `CN`       | `Operator` | Очистить уведомления о новых предметах инвентаря и новых комментариях. |

### Friend Commands

| Команда                        | Сокращение | Доступ     | Описание                                                                               |
| ------------------------------ | ---------- | ---------- | -------------------------------------------------------------------------------------- |
| `ADDBOTFRIEND <Bots>`          | `ABF`      | `Master`   | Let `Bots` add each other as friend                                                    |
| `ADDBOTFRIEND <BotAs>+<BotBs>` |            | `Master`   | Let `BotAs` add each other as friend, then let `BotAs` add `BotBs` as friend           |
| `ADDFRIEND [Bots] <Text>`      | `AF`       | `Master`   | Let bots send friend request to others, support `custom Url`, `steamId`, `Friend code` |
| `DELETEFRIEND [Bots] <Text>`   | `DF`       | `Master`   | Let bots delete frined, `Text` support `custom Url`, `steamId`, `Friend code`          |
| `DELETEALLFRIEND [Bots]`       |            | `Master`   | Let bots delete all its friends                                                        |
| `INVITELINK [Bots]`            | `IL`       | `Operator` | Let bots generate friend invite link                                                   |

- `ADDBOTFRIEND` Usage Example
  - `ADDBOTFRIEND a,b c`: Let `a`,`b`,`c` add friends with each other
  - `ADDBOTFRIEND a,b,c + d,e`: Let `a`,`b`,`c` add friends with each other, then let `a`,`b`,`c` add `d` and `e` as friend, `d` will not add `e` as friend
  - `ADDBOTFRIEND ASF`: Allow use `ASF` of bots
  - `ADDBOTFRIEND a b c + ASF`: Allow use `ASF` of bots
  - `ADDBOTFRIEND ASF + ASF`: Allow, but meaningless

### Команды Списка Рекомендаций

| Команда           | Сокращение | Доступ   | Описание                                                            |
| ----------------- | ---------- | -------- | ------------------------------------------------------------------- |
| `EXPLORER [Bots]` | `EX`       | `Master` | Вызвать задачу ASF "Просмотреть список рекомендаций" через 5 секунд |

> Пожалуйста, по возможности, позвольте ASF просматривать список рекомендаций самому, эта команда используется для просмотра списка рекомендаций как можно скорее

### Wallet Command

| Команда                          | Сокращение | Доступ   | Описание                                                                                    |
| -------------------------------- | ---------- | -------- | ------------------------------------------------------------------------------------------- |
| `REDEEMWALLET [Bots] <keys>`     | `RWA`      | `Master` | Redeem wallet code, if balance address is required, will use confitgred address in ASF.json |
| `REDEEMWALLETMULT [Bots] <keys>` | `RWAM`     | `Master` | Redeem wallet code, but every bot will only redeem one given code                           |

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

---

[![Repobeats analytics image](https://repobeats.axiom.co/api/embed/df6309642cc2a447195c816473e7e54e8ae849f9.svg "Repobeats analytics image")](https://github.com/chr233/ASFEnhance/pulse)

---

[![Stargazers over time](https://starchart.cc/chr233/ASFEnhance.svg)](https://github.com/chr233/ASFEnhance/stargazers)

---
