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

> Эта группа команд доступна только в течение ограниченного времени и будет удалена при выходе следующей версии плагина, если она потеряет свою актуальность

| Команда                    | Доступ     | Описание                                                                                                                                                                                      |
| -------------------------- | ---------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `CLAIM20TH [Bots]`         | `Operator` | Получить бесплатные предметы 20-ой годовщины Steam в магазине очков                                                                                                                           |
| `C20 [Bots]`               | `Operator` | То же, что и `CLAIM20TH`                                                                                                                                                                      |
| `CLAIMITEM [Bots]`         | `Operator` | Получить предмет распродажи, например, наклейки или что-то еще                                                                                                                                |
| `CI [Bots]`                | `Operator` | То же, что и `CLAIMITEM`                                                                                                                                                                      |
| `SIM4 [Bots]`              | `Operator` | Получить стикеры `The Sims™ 4` [ссылка](https://store.steampowered.com/sale/simscelebrationsale)                                                                                              |
| `DL2 [Bots]`               | `Operator` | Получить вещи `Dying Light 2 Stay Human` [ссылка](https://store.steampowered.com/sale/dyinglight)                                                                                             |
| `DL22 [Bots] [Sticker Id]` | `Operator` | Получить предметы `Dying Light 2 Stay Human` , `Sticker Id` не обязательно, значение может быть от 1 до 4 [ссылка](https://store.steampowered.com/developer/Techland/sale/techlandsummer2023) |
| `RLE [Bots] [Sticker Id]`  | `Operator` | Получить предметы `Redfall Launch Event` , `Sticker Id` не обязательно, значение может быть от 1 до 4 [ссылка](https://store.steampowered.com/sale/redfall_launch)                            |

## Установка

### Первая установка / Обновление в ручном режиме

1.  Загрузите плагин через [GitHub Releases](https://github.com/chr233/ASFEnhance/releases) страницу
2.  Распакуйте файл `ASFEnhance.dll` и скопируйте его в папку `plugins` в директории `ArchiSteamFarm`
3.  Перезапустить `ArchiSteamFarm` и используйте команду `ASFE` для проверки работоспособности плагина

### Команды для обновления

> Для обновления плагина можно использовать собственную команду плагина.
> Обновление версии ASF может быть несовместимым, если вы обнаружили, что плагин не может быть загружен, попробуйте обновить ASF.

-`ASFEVERSION` / `AV` проверить последнюю версию ASFEhance  
 -`ASFEUPDATE` / `AU` автоматическое обновление ASFEhance (возможно, потребуется обновить ASF вручную)

### Журнал изменений

| Версия ASFEnhance                                                      | Совместимая версия ASF | Описание                                              |
| ---------------------------------------------------------------------- | :--------------------: | ----------------------------------------------------- |
| [1.8.13.0](https://github.com/chr233/ASFEnhance/releases/tag/1.8.13.0) |        5.4.10.3        | ASF -> 5.4.10.3                                       |
| [1.8.12.2](https://github.com/chr233/ASFEnhance/releases/tag/1.8.12.2) |        5.4.9.3         | добавлена команда `RECEIVEGIFT`                       |
| [1.8.11.1](https://github.com/chr233/ASFEnhance/releases/tag/1.8.11.1) |        5.4.9.3         | добавлена команда `CLAIM20TH`                         |
| [1.8.10.0](https://github.com/chr233/ASFEnhance/releases/tag/1.8.10.0) |        5.4.9.3         | добавлена команда `GETACCOUNTBANNED` `GETBOTBANNED`   |
| [1.8.9.0](https://github.com/chr233/ASFEnhance/releases/tag/1.8.9.0)   |        5.4.9.3         | ASF -> 5.4.9.3                                        |

<details>
  <summary>История версий</summary>

| Версия ASFEnhance                                                    | Зависит от ASF | 5.4.7.3 | 5.4.8.3 | 5.4.9.3 |
| -------------------------------------------------------------------- | :------------: | :-----: | :-----: | :-----: |
| [1.8.8.0](https://github.com/chr233/ASFEnhance/releases/tag/1.8.8.0) |    5.4.8.3     |   ❌    |   ✔️    |   ✔️    |

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

| Конфигурация      | Тип    | По умолчанию | Описание                                                                                                                                                                       |
| ----------------- | ------ | ------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| `EULA`            | `bool` | `true`       | Если согласны с [лицензионным соглашением](#лицензионное-соглашение)\*                                                                                                         |
| `Statistic`       | `bool` | `true`       | Разрешить отправку данных для статистики. Она используется для подсчета количества пользователей, при этом никакой другой информации отправляться не будет                     |
| `DevFeature`      | `bool` | `false`      | Включить функции [разработчика (3 команды)](#для-разработчика) `Может представить угрозу безопасности, включайте на свой страх и риск`                                         |
| `DisabledCmds`    | `list` | `null`       | Команды в списке будет отключены , **`DisabledCmds` нечувствительна к командам ASF**, данная конфигурация влияет только на команды `ASFEnhance`                                |
| `Address`\*\*\*   | `dict` | `null`       | При наличии одного расчетного адреса используйте команду `REDEEMWALLET` для активации кода пополнения кошелька, который будет использоваться автоматически при запросе адреса. |
| `Addresses`\*\*\* | `list` | `null`       | Если у вас несколько расчетных адресов , произвольно используйте один из списка, когда вам нужен расчетный адрес                                                               |

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

| Команда       | Сокращение | Доступ          | Описание                                                                  |
| ------------- | ---------- | --------------- | ------------------------------------------------------------------------- |
| `ASFENHANCE`  | `ASFE`     | `FamilySharing` | Получить версию ASFEnhance                                                |
| `ASFEVERSION` | `AV`       | `Owner`         | Проверить последнюю версию ASFEnhance                                     |
| `ASFEUPDATE`  | `AU`       | `Owner`         | Обновить ASFEnhance до последней версии (необходим ручной перезапуск ASF) |

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
| `GAMEAVATAR [Bots] <AppID> [AvatarID]` | `GA`       | `Opetator`      | Установить аватар бота в соответствии с заданными `AppID` и `AvatarID`, если не задать `AvatarId`, то плагин будет использовать случайный аватар               |
| `RANDOMGAMEAVATAR [Bots]`              | `RGA`      | `Opetator`      | Установка случайного аватара бота                                                                                                                              |
| `ADVNICKNAME [Bots] Query`             | `ANN`      | `Master`        | Установить использование псевдонима бота в соответствии с `Placeholder`, доступны значения: `%dn%` `%ln%` `%un%` `%botn%`                                      |
| `SETAVATAR [Bots] ImageUrl` 🐞         | `GA`       | `Opetator`      | Установить аватар бота в соответствии c cсылкойна указанное изображение в Интернете                                                                            |
| `DELETEAVATAR [Bots]` 🐞               |            | `Master`        | Удалить аватар бота (сбросить на стандартный)                                                                                                                  |
| `CRAFTBADGE [Bots]`                    | `CB`       | `Master`        | Автоматическое изготовление крафтовых значков (изготовление всех крафтовых значков за один раз)                                                                |

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

| Команда                                    | Сокращение | Доступ     | Описание                                                                                      |
| ------------------------------------------ | ---------- | ---------- | --------------------------------------------------------------------------------------------- |
| `APPDETAIL [Bots] <AppIDs>`                | `AD`       | `Operator` | Информация об игре от Steam API, поддерживает `APP`                                           |
| `SEARCH [Bots] Keywords`                   | `SS`       | `Operator` | Поиск по магазину Steam                                                                       |
| `SUBS [Bots] <AppIDs\|SubIDs\|BundleIDs>`  | `S`        | `Operator` | Показать доступные «SUB» (лицензии) со страницы магазина Steam, поддерживает `APP/SUB/BUNDLE` |
| `PUBLISHRECOMMENT [Bots] <AppIDs> COMMENT` | `PREC`     | `Operator` | Опубликовать обзор на игру, `appID` или `+appId` позитивный , `-appId` негативный             |
| `DELETERECOMMENT [Bots] <AppIDs>`          | `DREC`     | `Operator` | Удалить обзор на игру                                                                         |
| `REQUESTACCESS [Bots] <AppIDs>`            | `RA`       | `Operator` | Отправить заявку на playtest игры, равноценно нажатию кнопки `Запросить доступ`               |
| `VIEWPAGE [Bots] Url`                      | `VP`       | `Operator` | Посетить указанную страницу                                                                   |

### Команды Корзины

> Steam сохраняет информацию о корзине покупок с помощью файлов cookie, перезапуск экземпляра бота приведет к очистке корзины

| Команда                              | Сокращение | Доступ     | Описание                                                                                      |
| ------------------------------------ | ---------- | ---------- | --------------------------------------------------------------------------------------------- |
| `CART [Bots]`                        | `C`        | `Operator` | Информация о товарах в корзине магазина Steam                                                 |
| `ADDCART [Bots] <SubIDs\|BundleIDs>` | `AC`       | `Operator` | Добавить игру в корзину, поддерживает только `SUB/BUNDLE`                                     |
| `CARTRESET [Bots]`                   | `CR`       | `Operator` | Очистить корзину                                                                              |
| `CARTCOUNTRY [Bots]`                 | `CC`       | `Operator` | Информация о доступной валюте (Зависит от IP адреса и страны кошелька)                        |
| `FAKEPURCHASE [Bots]`                | `FPC`      | `Master`   | Имитация корзины бота-покупателя и создание записи о неудаче без реального оформления покупки |
| `PURCHASE [Bots]`                    | `PC`       | `Master`   | Купить товары из корзины бота «для себя» (оплата через Steam кошелёк)                         |
| `PURCHASEGIFT [BotA] BotB`           | `PCG`      | `Master`   | Купить товары из корзины `BotA` в подарок для `BotB` (оплата через Steam кошелёк)             |

> Steam позволяет дублировать покупки, пожалуйста, проверьте корзину перед использованием команды `PURCHASE`.

### Команды Сообщества

| Команда                    | Сокращение | Доступ     | Описание                                                               |
| -------------------------- | ---------- | ---------- | ---------------------------------------------------------------------- |
| `CLEARNOTIFICATION [Bots]` | `CN`       | `Operator` | Очистить уведомления о новых предметах инвентаря и новых комментариях. |

### Команды друзей

| Команда                        | Сокращение | Доступ     | Описание                                                                                                   |
| ------------------------------ | ---------- | ---------- | ---------------------------------------------------------------------------------------------------------- |
| `ADDBOTFRIEND <Bots>`          | `ABF`      | `Master`   | `Bots` добавляют друг друга в друзья                                                                       |
| `ADDBOTFRIEND <BotAs>+<BotBs>` |            | `Master`   | `BotAs` добавляют друг друга в друзья, тогда пусть `BotAs` добавляет `BotBs` в друзья                      |
| `ADDFRIEND [Bots] <Text>`      | `AF`       | `Master`   | `Bots` отправляют запросы на дружбу другим пользователям, с помощью `custom Url`, `steamId`, `Friend code` |
| `DELETEFRIEND [Bots] <Text>`   | `DF`       | `Master`   | `Bots` удаляют друга, `Text` с помощью `custom Url`, `steamId`, `Friend code`                              |
| `DELETEALLFRIEND [Bots]`       |            | `Master`   | `Bots` удаляют всех своих друзей                                                                           |
| `INVITELINK [Bots]`            | `IL`       | `Operator` | `Bots` создают ссылки для быстрое приглашение друга                                                        |

- Пример использования команды `ADDBOTFRIEND`
  - `ADDBOTFRIEND a,b c`: Пусть `a`,`b`,`c` добавятся в друзья друг к другу
  - `ADDBOTFRIEND a,b,c + d,e`: Пусть `a`,`b`,`c` добавятся в друзья друг другу, то пусть `a`, `b`, `c` добавят `d` и `e` в друзья, но `d` не будет добавлять `e` в друзья
  - `ADDBOTFRIEND ASF`: Разрешено использование значения `ASF` вместо ников ботов
  - `ADDBOTFRIEND a b c + ASF`: Разрешено использование значения `ASF` вместо ников ботов
  - `ADDBOTFRIEND ASF + ASF`: Разрешено, но бессмысленно

### Команды Списка Рекомендаций

| Команда           | Сокращение | Доступ   | Описание                                                            |
| ----------------- | ---------- | -------- | ------------------------------------------------------------------- |
| `EXPLORER [Bots]` | `EX`       | `Master` | Вызвать задачу ASF "Просмотреть список рекомендаций" через 5 секунд |

> Пожалуйста, по возможности, позвольте ASF просматривать список рекомендаций самому, эта команда используется для просмотра списка рекомендаций как можно скорее

### Команды кошелька

| Команда                          | Сокращение | Доступ   | Описание                                                                                                   |
| -------------------------------- | ---------- | -------- | ---------------------------------------------------------------------------------------------------------- |
| `REDEEMWALLET [Bots] <keys>`     | `RWA`      | `Master` | Код кошелька, если требуется адрес для выставления счета, будет использоваться адрес, указанный в ASF.json |
| `REDEEMWALLETMULT [Bots] <keys>` | `RWAM`     | `Master` | Использование кода кошелька, но каждый бот будет использовать только один данный код                       |

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
