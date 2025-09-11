# Service01
## Приложение эмулирующее работу с брокером

[![N|Solid](https://cldup.com/dTxpPi9lDf.thumb.png)](https://nodesource.com/products/nsolid)

[![Build Status](https://travis-ci.org/dda101110/service01.svg?branch=main)](https://travis-ci.org/dda101110/service01)

## Features

- Получение данных по API endpoint
- Отправка данных в брокер сообщений (директория в файловой системе)
- Группировка запросов
- Валидация данных
- Обработка ошибок (формат, таймаут)

## Tructure

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

[//]: # (These are reference links used in the body of this note and get stripped out when the markdown processor does its job. There is no need to format nicely because it shouldn't be seen. Thanks SO - http://stackoverflow.com/questions/4823468/store-comments-in-markdown-syntax)

   [dill]: <https://github.com/joemccann/dillinger>
   [git-repo-url]: <https://github.com/joemccann/dillinger.git>
   [john gruber]: <http://daringfireball.net>
   [df1]: <http://daringfireball.net/projects/markdown/>
   [markdown-it]: <https://github.com/markdown-it/markdown-it>
   [Ace Editor]: <http://ace.ajax.org>
   [node.js]: <http://nodejs.org>
   [Twitter Bootstrap]: <http://twitter.github.com/bootstrap/>
   [jQuery]: <http://jquery.com>
   [@tjholowaychuk]: <http://twitter.com/tjholowaychuk>
   [express]: <http://expressjs.com>
   [AngularJS]: <http://angularjs.org>
   [Gulp]: <http://gulpjs.com>

   [PlDb]: <https://github.com/joemccann/dillinger/tree/master/plugins/dropbox/README.md>
   [PlGh]: <https://github.com/joemccann/dillinger/tree/master/plugins/github/README.md>
   [PlGd]: <https://github.com/joemccann/dillinger/tree/master/plugins/googledrive/README.md>
   [PlOd]: <https://github.com/joemccann/dillinger/tree/master/plugins/onedrive/README.md>
   [PlMe]: <https://github.com/joemccann/dillinger/tree/master/plugins/medium/README.md>
   [PlGa]: <https://github.com/RahulHP/dillinger/blob/master/plugins/googleanalytics/README.md>
