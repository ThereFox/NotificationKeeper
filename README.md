# Сервис распределения уведомлений

Данный микросервис ответственен за
- получение запроса на отправку уведомления о предложении конкретному пользователю
- обработку входного запроса
- отправку команды на уведомление другим микросервисам
- обработку отчётов об отправке.

## Особенности предметной области

Как ограничения предметной области, помимо ограничений валидности, выделено ограничение в 5 сообщений в день конкретному пользователю.

## Использование технологий

### Redis

Используется для кэширования следующих данных:
- Шаблона уведомления, т.к. полагается что возможно уведомление нескольких пользователей одним шаблоном с некоторым интервалом
- Информации об не отосланном уведомлении, т.к. позволяет быстрее обработать отчёт об отправке
- Количестве уведомлений пользователя за текущий день

### InfluxDB

Используется для хранения логов

### Apache Kafka

Используется как брокер сообщений при асинхронном взаимодействии.
Используется для обеспечения большей отказоустойчивости (если упадёт этот или другой микросервис, другой продолжит работать с оставшимися сообщениями)

Данный микросервис содержит:
- Producer-а комманд на отправку уведомлений
- Consumer-а репортов об отправке уведомлений

### PostgreSQL

Используется как основная БД
