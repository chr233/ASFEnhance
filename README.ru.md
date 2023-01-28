# ASFEnhance

<!-- [![Join the chat](https://img.shields.io/gitter/room/chr233/ASFEnhance?color=%234FB999&logo=Google%20Chat&logoColor=white)](https://gitter.im/chr233/ASFEnhance) -->

[![Codacy Badge](https://app.codacy.com/project/badge/Grade/3d174e792fd4412bb6b34a77d67e5dea)](https://www.codacy.com/gh/chr233/ASFEnhance/dashboard)
![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/chr233/ASFEnhance/autobuild.yml?logo=github)
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

| Команда                               | Доступ     | Описание                                 |
| ------------------------------------- | ---------- | ---------------------------------------- |
| `SIM4 [Bots]`                         | `Operator` | Получить стикеры `The Sims™ 4`           |
| `DL2 [Bots]`                          | `Operator` | Получить вещи `Dying Light 2 Stay Human` |
| `UNLOCKPOINTBADGE [Bots] defId level` | `Operator` | Redeem points badge                      |
| `UPB`                                 |            | Same as `UNLOCKPOINTBADGE`               |

- `UNLOCKPOINTBADGE` Explanation

  | defId  | Badge Name                                                                                          |
  | ------ | --------------------------------------------------------------------------------------------------- |
  | 70499  | 2019: Lunar New Year Sale / 2019 农历新年特卖                                                       |
  | 78777  | 2020: The Debut Collection / 首发系列                                                               |
  | 102593 | 2020: Winter Collection / 冬季收藏 - 2020                                                           |
  | 114395 | 2021: Spring Collection / 2021 年春季收藏品                                                         |
  | 121091 | 2021: Summer Collection / 2021 年夏季系列                                                           |
  | 150087 | 2021: Winter Sale / 2021 年冬季特卖                                                                 |
  | 174001 | 2022: Summer Collection / 2022 年夏季系列（这个可以在点数商店直接兑换，不出意外的话下个月冬促换新） |

  > level: the target level minus current level, if current level is 10 and want to redeem to 40, than specify 30

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

| Версия ASFEnhance                                                          | Совместимая версия ASF | Описание                                                 |
| -------------------------------------------------------------------------- | ---------------------- | -------------------------------------------------------- |
| [1.7.11.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.11.0) | 5.4.1.11               | Add `ADDFRIEND` Command                                  |
| [1.7.10.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.10.0)     | 5.4.1.11               | Add `UNLOCKPOINTBADGE` Commands                          |
| [1.7.9.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.9.0)       | 5.4.1.11               | Add `SETAVATAR` Command                                  |
| [1.7.8.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.8.0)       | 5.4.1.11               | Bug fix                                                  |
| [1.7.7.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.7.0)       | 5.4.1.11               | ASF upgrade to `5.4.1.11`, remove expired event commands |
| [1.7.6.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.6.0)       | 5.4.0.3                | Add `GAMEAVATAR` `ADVNICKNAME` Command                   |

<details>
  <summary>История версий</summary>

| Версия ASFEnhance                                                      | Зависит от ASF | 5.3.1.2 | 5.3.2.4 | 5.4.0.3 | 5.4.1.11 |
| ---------------------------------------------------------------------- | -------------- | ------- | ------- | ------- | -------- |
| [1.7.5.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.5.0)   | 5.4.0.3        | ❌      | ❌      | ✔️      | ✔️       |
| [1.7.4.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.4.0)   | 5.4.0.3        | ❌      | ❌      | ✔️      | ✔️       |
| [1.7.3.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.3.0)   | 5.4.0.3        | ❌      | ❌      | ✔️      | ✔️       |
| [1.7.2.1](https://github.com/chr233/ASFEnhance/releases/tag/1.7.2.1)   | 5.4.0.3        | ❌      | ❌      | ✔️      | ✔️       |
| [1.7.1.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.1.0)   | 5.4.0.3        | ❌      | ❌      | ✔️      | ✔️       |
| [1.7.0.1](https://github.com/chr233/ASFEnhance/releases/tag/1.7.0.1)   | 5.4.0.3        | ❌      | ❌      | ✔️      | ✔️       |
| [1.6.23.0](https://github.com/chr233/ASFEnhance/releases/tag/1.6.23.0) | 5.3.2.4        | ❌      | ✔️      | ✔️      |          |
| [1.6.22.1](https://github.com/chr233/ASFEnhance/releases/tag/1.6.22.1) | 5.3.2.4        | ❌      | ✔️      | ✔️      |          |
| [1.6.21.6](https://github.com/chr233/ASFEnhance/releases/tag/1.6.21.6) | 5.3.2.4        | ❌      | ✔️      | ✔️      |          |
| [1.6.20.1](https://github.com/chr233/ASFEnhance/releases/tag/1.6.20.1) | 5.3.2.4        | ❌      | ✔️      | ✔️      |          |
| [1.6.19.4](https://github.com/chr233/ASFEnhance/releases/tag/1.6.19.4) | 5.3.2.4        | ❌      | ✔️      | ✔️      |          |
| [1.6.18.1](https://github.com/chr233/ASFEnhance/releases/tag/1.6.18.1) | 5.3.2.4        | ❌      | ✔️      | ✔️      |          |

| Версия ASFEnhance                                                          | Зависит от ASF | 5.2.6.3 | 5.2.7.7 | 5.2.8.4 | 5.3.0.3 | 5.3.1.2 |
| -------------------------------------------------------------------------- | -------------- | ------- | ------- | ------- | ------- | ------- |
| [1.6.18.0](https://github.com/chr233/ASFEnhance/releases/tag/1.6.18.0)     | 5.3.1.2        | ❌      | ❌      | ❌      | ✔️      | ✔️      |
| [1.6.16.0](https://github.com/chr233/ASFEnhance/releases/tag/1.6.16.0)     | 5.3.1.2        | ❌      | ❌      | ❌      | ✔️      | ✔️      |
| [1.6.15.0](https://github.com/chr233/ASFEnhance/releases/tag/1.6.15.0)     | 5.3.1.2        | ❌      | ❌      | ❌      | ✔️      | ✔️      |
| [1.6.14.0](https://github.com/chr233/ASFEnhance/releases/tag/1.6.14.0)     | 5.3.1.2        | ❌      | ❌      | ❌      | ✔️      | ✔️      |
| [1.6.12.717](https://github.com/chr233/ASFEnhance/releases/tag/1.6.12.717) | 5.3.1.2        | ❌      | ❌      | ❌      | ✔️      | ✔️      |
| [1.6.11.670](https://github.com/chr233/ASFEnhance/releases/tag/1.6.11.670) | 5.3.1.2        | ❌      | ❌      | ❌      | ✔️      | ✔️      |
| [1.6.10.666](https://github.com/chr233/ASFEnhance/releases/tag/1.6.10.666) | 5.3.0.3        | ❌      | ❌      | ❌      | ✔️      | ✔️\*    |
| [1.6.9.663](https://github.com/chr233/ASFEnhance/releases/tag/1.6.9.663)   | 5.2.8.4        | ❌      | ❌      | ✔️      | ❌      |         |
| [1.6.8.661](https://github.com/chr233/ASFEnhance/releases/tag/1.6.8.661)   | 5.2.7.7        | ❌      | ✔️      |         |         |         |

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
    "DevFeature": false
  }
}
```

| Конфигурация | Тип    | По умолчанию | Описание                                                                                                                                                   |
| ------------ | ------ | ------------ | ---------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `EULA`       | `bool` | `true`       | Если согласны с [лицензионным соглашением](#лицензионное-соглашение)\*                                                                                     |
| `Statistic`  | `bool` | `true`       | Разрешить отправку данных для статистики. Она используется для подсчета количества пользователей, при этом никакой другой информации отправляться не будет |
| `DevFeature` | `bool` | `false`      | Включить функции [разработчика (3 команды)](#для-разработчика) `Может представить угрозу безопасности, включайте на свой страх и риск`                     |

> \* Если Вы согласны с [лицензионным соглашением](#лицензионное-соглашение), то в ASFEnhance будут доступны все команды, в обмен на это, при использовании команд `GROUPLIST` и `CURATORLIST`, ASFEnhance подпишется на [Куратора](https://store.steampowered.com/curator/39487086/) и [Группу](https://steamcommunity.com/groups/11012580) (если бот не подписался или не присоединился)
>
> \* Если Вы не согласны с [лицензионным соглашением](#лицензионное-соглашение), то ASFEnhance ограничит команды куратора/подписок на игры/групп/обзоров, и ASFEnhance не подпишется на [Куратора](https://store.steampowered.com/curator/39487086/) и [Группу](https://steamcommunity.com/groups/11012580)

## Использование Команд

### Команды Обновления

| Команда       | Сокращение | Доступ          | Описание                                                                  |
| ------------- | ---------- | --------------- | ------------------------------------------------------------------------- |
| `ASFENHANCE`  | `ASFE`     | `FamilySharing` | Получить версию ASFEnhance                                                |
| `ASFEVERSION` | `AV`       | `Owner`         | Проверить последнюю версию ASFEnhance                                     |
| `ASFEUPDATE`  | `AU`       | `Owner`         | Обновить ASFEnhance до последней версии (необходим ручной перезапуск ASF) |

### Команды Аккаунта

| Команда                            | Сокращение | Доступ     | Описание                                     |
| ---------------------------------- | ---------- | ---------- | -------------------------------------------- |
| `PURCHASEHISTORY [Bots]`           | `PH`       | `Operator` | Выводит историю покупок бота                 |
| `FREELICENSES [Bots]`              | `FL`       | `Operator` | Выводит список всех бесплатных лицензий бота |
| `FREELICENSE [Bots]`               |            |            | То же, что и `FREELICENSES`                  |
| `LICENSES [Bots]`                  | `L`        | `Operator` | Выводит список всех SUB (лицензий) бота      |
| `LICENSE [Bots]`                   |            |            | То же, что и `LICENSES`                      |
| `REMOVEDEMOS [Bots]`               | `RD`       | `Master`   | Удаляет все демо-лицензии бота               |
| `REMOVEDEMO [Bots]`                |            |            | То же, что и `REMOVEDEMOS`                   |
| `REMOVELICENSES [Bots] <SubIDs>`   | `RL`       | `Master`   | Удаляет определённую лицензию бота по subIDs |
| `REMOVELICENSE [Bots] <SubIDs>`    |            |            | То же, что и `REMOVELICENSES`                |
| `EMAILIOPTIONS [Bots]`             | `EO`       | `Operator` | Выводит настройки рассылки бота              |
| `EMAILIOPTION [Bots]`              |            |            | То же, что и `EMAILIOPTIONS`                 |
| `SETEMAILOPTIONS [Bots] <Options>` | `SEO`      | `Master`   | Изменяет настройки рассылки                  |
| `SETEMAILOPTION [Bots] <Options>`  |            |            | То же, что и `SETEMAILOPTIONS`               |

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
| `SETAVATAR [Bots] ImageUrl`            | `GA`       | `Opetator`      | Set bot's avatar to specified online image                                                                                                                     |

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

| Команда                                    | Сокращение | Доступ     | Описание                                                                                                |
| ------------------------------------------ | ---------- | ---------- | ------------------------------------------------------------------------------------------------------- |
| `APPDETAIL [Bots] <AppIDs>`                | `AD`       | `Operator` | Информация об игре от Steam API, поддерживает `APP`                                                     |
| `SEARCH [Bots] Keywords`                   | `SS`       | `Operator` | Поиск по магазину Steam                                                                                 |
| `SUBS [Bots] <AppIDs\|SubIDs\|BundleIDs>`  | `S`        | `Operator` | Показать доступные «SUB» (лицензии) со страницы магазина Steam, поддерживает `APP/SUB/BUNDLE`           |
| `PUBLISHRECOMMENT [Bots] <AppIDs> COMMENT` | `PREC`     | `Operator` | Опубликовать обзор на игру, если appID > 0 отзыв будет положительным, а если appId < 0 то отрицательным |
| `DELETERECOMMENT [Bots] <AppIDs>`          | `DREC`     | `Operator` | Удалить обзор на игру                                                                                   |
| `REQUESTACCESS [Bots] <AppIDs>`            | `RA`       | `Operator` | Отправить заявку на playtest игры, равноценно нажатию кнопки `Запросить доступ`                         |
| `VIEWPAGE [Bots] Url`                      | `VP`       | `Operator` | Посетить указанную страницу                                                                             |

### Команды Корзины

> Steam сохраняет информацию о корзине покупок с помощью файлов cookie, перезапуск экземпляра бота приведет к очистке корзины

| Команда                              | Сокращение | Доступ     | Описание                                                                          |
| ------------------------------------ | ---------- | ---------- | --------------------------------------------------------------------------------- |
| `CART [Bots]`                        | `C`        | `Operator` | Информация о товарах в корзине магазина Steam                                     |
| `ADDCART [Bots] <SubIDs\|BundleIDs>` | `AC`       | `Operator` | Добавить игру в корзину, поддерживает только `SUB/BUNDLE`                         |
| `CARTRESET [Bots]`                   | `CR`       | `Operator` | Очистить корзину                                                                  |
| `CARTCOUNTRY [Bots]`                 | `CC`       | `Operator` | Информация о доступной валюте (Зависит от IP адреса и страны кошелька)            |
| `SETCOUNTRY [Bots] <CountryCode>`    | `SC`       | `Operator` | Сменить валюту (В процессе разработки)                                            |
| `PURCHASE [Bots]`                    | `PC`       | `Master`   | Купить товары из корзины бота «для себя» (оплата через Steam кошелёк)             |
| `PURCHASEGIFT [BotA] BotB`           | `PCG`      | `Master`   | Купить товары из корзины `BotA` в подарок для `BotB` (оплата через Steam кошелёк) |

> Steam позволяет дублировать покупки, пожалуйста, проверьте корзину перед использованием команды `PURCHASE`.

### Команды Сообщества

| Команда                    | Сокращение | Доступ     | Описание                                                               |
| -------------------------- | ---------- | ---------- | ---------------------------------------------------------------------- |
| `CLEARNOTIFICATION [Bots]` | `CN`       | `Operator` | Очистить уведомления о новых предметах инвентаря и новых комментариях. |

### Friend Commands

| Команда                        | Сокращение | Доступ   | Описание                                                                               |
| ------------------------------ | ---------- | -------- | -------------------------------------------------------------------------------------- |
| `ADDFRIEND [Bots] <Text>`      | `AF`       | `Master` | Let bots send friend request to others, support `custom Url`, `steamId`, `Friend code` |
| `ADDBOTFRIEND [BotAs] <BotBs>` | `ABF`      | `Master` | Пусть `BotA` добавит `BotB` в друзья.                                                  |

### Команды Списка Рекомендаций

| Команда           | Сокращение | Доступ   | Описание                                                            |
| ----------------- | ---------- | -------- | ------------------------------------------------------------------- |
| `EXPLORER [Bots]` | `EX`       | `Master` | Вызвать задачу ASF "Просмотреть список рекомендаций" через 5 секунд |

> Пожалуйста, по возможности, позвольте ASF просматривать список рекомендаций самому, эта команда используется для просмотра списка рекомендаций как можно скорее

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
