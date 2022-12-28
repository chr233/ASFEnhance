# ASFEnhance

[![Codacy Badge](https://app.codacy.com/project/badge/Grade/3d174e792fd4412bb6b34a77d67e5dea)](https://www.codacy.com/gh/chr233/ASFEnhance/dashboard)
![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/chr233/ASFEnhance/workflows/autobuild.yml?branch=master&logo=github)
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

[中文说明](README.md) | [English Version](README.en.md)

## ЛИЦЕНЗИОННОЕ СОГЛАШЕНИЕ

> Пожалуйста, не используйте этот плагин для отвратительного поведения, включая, но не ограничиваясь: публикацией фейковых отзывов, размещением рекламы и т.д.
>
> Смотри [Конфигурацию Плагина](#конфигурация-плагина)

## КОМАНДЫ СОБЫТИЙ

| Команда                 | Доступ     | Описание                                                                                                |
| ----------------------- | ---------- |---------------------------------------------------------------------------------------------------------|
| `SIM4 [Bots]`           | `Operator` | Получить стикеры `The Sims™ 4`                                                                          |
| `DL2 [Bots]`            | `Operator` | Получить вещи `Dying Light 2 Stay Human`                                                                |
| `DECK [Bots]`           | `Operator` | Получить стикер `Steam Deck`                                                                            |
| `VOTE [Bots] <gameIds>` | `Operator` | Проголосовать в `Steam Awards 2022`, если не указаны `gameIds`, то игры будут выбраны случайным образом |
| `CHECKVOTE [Bots]`      | `Operator` | Получить статус голосования в `Steam Awards 2022`                                                       |
| `EVENT [Bots]`          | `Operator` | Получить «дневные» стикеры `Steam Awards 2022`                                                          |

- Steam Award 2022 (12.23 ~ 1.3)

  ```txt
  VOTE     # Голосование ботом по умолчанию, игры будут выбраны случайным образом
  VOTE ASF # Голосование всеми ботам, игры будут выбраны случайным образом
  VOTE ASF 534380,1592190,570,648800... # Голосование всеми ботам, в голосовании будут использованы указанные игры
  ```

## Установка

Скачать нужный архив в [GitHub Releases](https://github.com/chr233/ASFEnhance/releases).
Для русского языка скачать файл `ASFEnhance-ru-RU.zip`

> Распакуйте файл `ASFEnhance.dll` и скопируйте его в папку `plugins` в дирректории с установленным ASF

## Поддержка Версий

- Команда Обновления

  - Используйте команду `ASFEVERSION` или `AV`, чтобы проверить последнюю версию ASFEnhance
  - Используйте команду `ASFEUPDATE` или `AU` для автоматического обновления ASFEnhance (возможно, потребуется ручной перезапуск ASF)

- Примечание об изменении версии .NET

  > ASF 5.4.0.3 использует .NET 7.0 вместо .NET 6.0, поэтому ASFEnhance 1.7.0.0 и более поздние версии также будут использовать .NET 7.0
  > Старый плагин совместим с новым ASF, но новый плагин **не** совместим со старым ASF

- Скачать

| Версия ASFEnhance                                                      | Совместимая версия ASF | Описание                                                          |
| ---------------------------------------------------------------------- | ---------------------- |-------------------------------------------------------------------|
| [1.7.5.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.5.0)   | 5.4.0.3                | Добавлены команды связанные с `Steam Replay`                      |
| [1.7.4.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.4.0)   | 5.4.0.3                | Добавлена команда для получения `Steam Awards 2022` стикеров      |
| [1.7.3.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.3.0)   | 5.4.0.3                | Добавлены команды для `Steam Awards 2022`                         |
| [1.7.2.1](https://github.com/chr233/ASFEnhance/releases/tag/1.7.2.1)   | 5.4.0.3                | Добавлена команда `ADDBOTFRIEND`                                  |
| [1.7.1.0](https://github.com/chr233/ASFEnhance/releases/tag/1.7.1.0)   | 5.4.0.3                | Добавлена команда `EMAILOPTIONS`, `SETEMAILOPTIONS`               |
| [1.7.0.1](https://github.com/chr233/ASFEnhance/releases/tag/1.7.0.1)   | 5.4.0.3                | ASF -> `5.4.0.3`, переход на .NET 7                               |
| [1.6.23.0](https://github.com/chr233/ASFEnhance/releases/tag/1.6.23.0) | 5.3.2.4                | Добавлена команда `DECK`, обновление до последней версии .NET 6.0 |

<details>
  <summary>История версий</summary>

| Версия ASFEnhance                                                          | Зависит от ASF | 5.2.6.3 | 5.2.7.7 | 5.2.8.4 | 5.3.0.3 | 5.3.1.2 | 5.3.2.4 | 5.4.0.3 |
| -------------------------------------------------------------------------- | -------------- | ------- | ------- | ------- | ------- | ------- | ------- |---------|
| [1.6.22.1](https://github.com/chr233/ASFEnhance/releases/tag/1.6.22.1)     | 5.3.2.4        | ❌      | ❌      | ❌      | ❌      | ❌      | ✔️      | ✔️      |
| [1.6.21.6](https://github.com/chr233/ASFEnhance/releases/tag/1.6.21.6)     | 5.3.2.4        | ❌      | ❌      | ❌      | ❌      | ❌      | ✔️      | ✔️      |
| [1.6.20.1](https://github.com/chr233/ASFEnhance/releases/tag/1.6.20.1)     | 5.3.2.4        | ❌      | ❌      | ❌      | ❌      | ❌      | ✔️      | ✔️      |
| [1.6.19.4](https://github.com/chr233/ASFEnhance/releases/tag/1.6.19.4)     | 5.3.2.4        | ❌      | ❌      | ❌      | ❌      | ❌      | ✔️      | ✔️      |
| [1.6.18.1](https://github.com/chr233/ASFEnhance/releases/tag/1.6.18.1)     | 5.3.2.4        | ❌      | ❌      | ❌      | ❌      | ❌      | ✔️      | ✔️      |
| [1.6.18.0](https://github.com/chr233/ASFEnhance/releases/tag/1.6.18.0)     | 5.3.1.2        | ❌      | ❌      | ❌      | ✔️      | ✔️      |         |         |
| [1.6.16.0](https://github.com/chr233/ASFEnhance/releases/tag/1.6.16.0)     | 5.3.1.2        | ❌      | ❌      | ❌      | ✔️      | ✔️      |         |         |
| [1.6.15.0](https://github.com/chr233/ASFEnhance/releases/tag/1.6.15.0)     | 5.3.1.2        | ❌      | ❌      | ❌      | ✔️      | ✔️      |         |         |
| [1.6.14.0](https://github.com/chr233/ASFEnhance/releases/tag/1.6.14.0)     | 5.3.1.2        | ❌      | ❌      | ❌      | ✔️      | ✔️      |         |         |
| [1.6.12.717](https://github.com/chr233/ASFEnhance/releases/tag/1.6.12.717) | 5.3.1.2        | ❌      | ❌      | ❌      | ✔️      | ✔️      |         |         |
| [1.6.11.670](https://github.com/chr233/ASFEnhance/releases/tag/1.6.11.670) | 5.3.1.2        | ❌      | ❌      | ❌      | ✔️      | ✔️      |         |         |
| [1.6.10.666](https://github.com/chr233/ASFEnhance/releases/tag/1.6.10.666) | 5.3.0.3        | ❌      | ❌      | ❌      | ✔️      | ✔️\*    |         |         |
| [1.6.9.663](https://github.com/chr233/ASFEnhance/releases/tag/1.6.9.663)   | 5.2.8.4        | ❌      | ❌      | ✔️      | ❌      |         |         |         |
| [1.6.8.661](https://github.com/chr233/ASFEnhance/releases/tag/1.6.8.661)   | 5.2.7.7        | ❌      | ✔️      |         |         |         |         |         |

</details>

## Конфигурация плагина

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

| Порядочный номер | Описание                                                                                                                                                     | Примечания                                                               |
| ---------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------ |--------------------------------------------------------------------------|
| 1                | Включить уведомления по почте                                                                                                                                | Если уведомления отключены, то следующие аргументы будут игнорироваться  |
| 2                | Товар из вашего списка желаемого получил скидку.                                                                                                             |                                                                          |
| 3                | Товар из моего списка желаемого стал доступен в раннем доступе или полноценно, либо вышел из раннего доступа.                                                |                                                                          |
| 4                | Продукт из Greenlight, на который вы были подписаны или добавляли в избранное, стал доступен в раннем доступе или полноценно, либо вышел из раннего доступа. |                                                                          |
| 5                | Продукт от издателя или разработчика из моих подписок вышел полностью / в раннем доступе / из раннего доступа.                                               |                                                                          |
| 6                | Начинается ежегодная акция.                                                                                                                                  |                                                                          |
| 7                | Ваша группа в Steam получила на обзор копию игры или программы.                                                                                              |                                                                          |
| 8                | Я получаю награду сообщества Steam.                                                                                                                          |                                                                          |
| 9                | Уведомления о событиях отдельных игр.                                                                                                                        |                                                                          |

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

| Команда                                | Сокращение| Доступ          | Описание                                                                                                                                                                                                                                                                   |
|----------------------------------------| --------- |-----------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `PROFILE [Bots]`                       | `PF`      | `FamilySharing` | Информация о профиле бота                                                                                                                                                                                                                                                  |
| `PROFILELINK [Bots]`                   | `PFL`     | `FamilySharing` | Ссылка на Steam профиль бота                                                                                                                                                                                                                                               |
| `STEAMID [Bots]`                       | `SID`     | `FamilySharing` | steamID64 бота                                                                                                                                                                                                                                                             |
| `FRIENDCODE [Bots]`                    | `FC`      | `FamilySharing` | «Код для друга» бота                                                                                                                                                                                                                                                       |
| `TRADELINK [Bots]`                     | `TL`      | `Operator`      | «Ссылка на обмен» бота                                                                                                                                                                                                                                                     |
| `RENAME [Bots] <New nickname>`         |           | `Owner`         | Установить ник бота в Steam (поддерживаются пробелы и плейсхолдеры).<br/>Доступные плейсхолдеры: `%RANDOM1%`..`%RANDOM9%`, `%BOTNAME%`. <br/><br/> Пример:<br/> Команда => `rename bot1 Sam-%RANDOM3% aka %BOTNAME%`<br/> Установленный боту никнейм => `Sam-581 aka bot1` |
| `REPLAY [Bots]`                        | `RP`      | `Operator`      | Получить ссылку на баннер «Итоги STEAM 2022 года» (позволяет разблокировать значок)                                                                                                                                                                                        |
| `REPLAYPRIVACY [Bots] Privacy`         | `RPP`     | `Operator`      | Установить настройки приватности баннера «Итоги STEAM 2022 года». <br/>Параметр `Privacy` может иметь следующие значения: <br/>`1=Скрыто` <br/>`2=Только для друзей` <br/>`3=Для всех`                                                                                     |
| `CLEARALIAS [Bots]`                    |           | `Opetator`      | Очистить историю имён                                                                                                                                                                                                                                                      |
| `GAMEAVATAR [Bots] <AppID> <AvatarID>` | `GA`      | `Opetator`      | Установить аватар из игры `AppID` с индексом `AvatarID`.                                                                                                                                                                                                                   |
| `RANDOMGAMEAVATAR [Bots]`              | `RGA`     | `Opetator`      | Установить случайный аватар, выбирается случайным образом со страницы [Game Avatars Page](https://steamcommunity.com/actions/GameAvatars/)                                                                                                                                 |

> `AvatarID` - Порядковый номер аватара на странице `https://steamcommunity.com/games/<AppID>/Avatar/List`

> `%RANDOM1%`..`%RANDOM9%` - Плейсхолдеры для генерации случайного числа (максимум 9-значное число)
>
> `%BOTNAME%` - Плейсхолдер с внутренним именем бота ASF
### Команды Куратора

| Команда                          | Сокращение | Доступ   | Описание                             |
| -------------------------------- | ---------- | -------- | ------------------------------------ |
| `CURATORLIST [Bots]`             | `CL`       | `Master` | Выводит список кураторов в подписках |
| `FOLLOWCURATOR [Bots] <ClanIDs>` | `FC`       | `Master` | Подписаться на куратора              |
| `UNFOLLOWCURATOR [Bots]`         | `UFC`      | `Master` | Описаться от куратора                |

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
|--------------------------------------------| ---------- | ---------- |---------------------------------------------------------------------------------------------------------|
| `APPDETAIL [Bots] <AppIDs>`                | `AD`       | `Operator` | Информация об игре от Steam API, поддерживает `APP`                                                     |
| `SEARCH [Bots] Keywords`                   | `SS`       | `Operator` | Поиск по магазину Steam                                                                                 |
| `SUBS [Bots] <AppIDs\|SubIDs\|BundleIDs> ` | `S`        | `Operator` | Показать доступные «SUB» (лицензии) со страницы магазина Steam, поддерживает `APP/SUB/BUNDLE`           |
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

| Команда                        | Сокращение | Доступ     | Описание                                                               |
| ------------------------------ | ---------- | ---------- | ---------------------------------------------------------------------- |
| `CLEARNOTIFICATION [Bots]`     | `CN`       | `Operator` | Очистить уведомления о новых предметах инвентаря и новых комментариях. |
| `ADDBOTFRIEND [BotAs] <BotBs>` | `ABF`      | `Master`   | Пусть `BotA` добавит `BotB` в друзья.                                  |

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
