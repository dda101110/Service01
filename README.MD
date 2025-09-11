# Service01
## Приложение эмулирующее работу с брокером

[![N|Solid](https://cldup.com/dTxpPi9lDf.thumb.png)](https://nodesource.com/products/nsolid)

[![Build Status](https://travis-ci.org/dda101110/service01.svg?branch=main)](https://travis-ci.org/dda101110/service01)

## Features

- Получение данных по API endpoint / TCP port
- Отправка данных в брокер сообщений (директория в файловой системе)
- Группировка запросов
- Валидация данных
- Обработка ошибок (формат, таймаут)

## Structure

Структура проекта состоит:
* API Service - проект Service01.API
    * сервис имеет endpoint /api/[version]/rate/[currency1]/[currency2]/[bank]
    возвращает курс валют в банке, где:
    [version] - версия API
        * версия v1 - работает напрямую через BrokerService
        * версия v2 - работает через BufferService ("схлопывание" запросов по времени)
        * версия v3 - работает через BrokerService и вызывается через mediatr
        
      [currency1] - строковое значение валюты, например USD/EUR/CAD/RUB

      [currency2] - строковое значение валюты
      
      [bank] - строковое значение банка (SWIFT)
      
      example: `/api/v3/rate/USD/EUR/TBC`
* TCP Service - проект Service01.Host
    * сервис прослушивает запросы на порту 7777
    формат сообщения `[METHOD] [PATH]`

## Tech scheme
Схема обработки данных представлена на рисунке

![This is a scheme image.](/images/scheme.png)


## Metrics

Для Prometheuse развернуты метрики доступные по url `/metrics`

![metrics](/images/metrics.png)

## HealthCheck

Для получения состояния о работе компонентов приложения, добавлены HealthChecks, а также HealthCheck UI
HealthCheck UI доступен по url `/healthchecks-ui`

![healthchecks-ui](/images/healthchecks-ui.png)

Общее состояние приложения доступно по url `/health`

Более подробное состояние приложения доступно по url `/healthAPI`

![healthAPI](/images/healthAPI.png)

## Links

По ссылке развернуто данное приложение и можно ознакомится с данными [service01.mathete.ru](https://service01.matchete.ru/)

## License

MIT

**Free Software**
