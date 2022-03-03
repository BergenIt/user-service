<details><summary>Grpc порт сервиса (Http2).</summary>

- Порт записан в переменной среды окружения `GRPC_PORT` (80)
- Реализован HealthCheck

</details>

<details><summary>Http порт сервиса (Ws).</summary>

- Порт записан в переменной среды окружения `HTTP_PORT` (443)
- Ендпоинд ws - `/health` (Используется SignalR - SSE) 
- Реализован HealthCheck - `/health`

</details>

<details><summary>Деплой.</summary>

- Сборка + развертывание - docker-compose/docker-compose.yml или UserService.Main/UserService.Main/Dockerfile
- Стандартные переменные среды в директории - docker-compose/env

</details>

**Написание конфигов (типы аудита, оповещений, системные ресурсы)**
<details><summary>Action аудита</summary>

- Стандартный файл для записи действий аудита в /UserService.Main/UserService.Main/UserServiceConfig/Audit.yaml
- Путь к файлу прописывается в переменной среды `AUDIT_ROUTE`
- Так же поддерживаются json файлы с аналогичной структурой

</details>

<details><summary>Системные ресурсы</summary>

- Стандартный файл для записи системных ресурсов /UserService.Main/UserService.Main/UserServiceConfig/Resources.yaml
- Путь к файлу прописывается в переменной среды `PERMISSION_ROUTE`
- Так же поддерживаются json файлы с аналогичной структурой

</details>

<details><summary>Настройки типов уведомлений</summary>

- Стандартный файл для записи типов уведомлений /UserService.Main/UserService.Main/UserServiceConfig/NotifySettings.yaml)
- Путь к файлу прописывается в переменной среды `NOTIFY_EVENT_TYPE_SETTING_ROUTE`
- Так же поддерживаются json файлы с аналогичной структурой

</details>
