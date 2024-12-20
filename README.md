# Currency WPF
## Данное приложение позволяет просмотреть курс белорусского рубля по соотношению к другим иностранным валютам, которые доступны в Национальном Банке РБ.
## Функционал
- Описание проекта
- Навигация между страницами
- Загрузка данных о соотношения белорусского рубля ко всем доступным иностранным валютам на конкретный день, сохранение этих данный в JSon-файл и последующий просмотр его в программе при необходимости.
- Загрузка данных о изменении соотношения белорусского рубля к конкретной иностранной валюте в интервале (дата начала и конца интервала анализа), сохранение этих данный в JSon-файл и последующий просмотр его в программе при необходимости.
# Проблемы приложения (моё мнение)
## 1. DI
Внедрение зависимостей является основной концепцией работы WEB-приложений, десктоп и т.д. По логике, в данном приложении DI так же должен присутствовать, реализованный вручную, чтобы была возможность использовать интерфейсы на полную мощность. Так же DI позволяет определить стили работы сервисов (AddSingleton, AddScoped or AddTransient), что делает наши сервисы более универсальными при разработке приложения.
## 2. DataBase vs API-запросы
В данном приложении есть два варианта работы - получание данных из API НБРБ и хранение их в базе данных, либо постоянное взаимодействие с данным API для получения каких-либо данных. Я выбрал второй вариант.
### Плюсы использования DB.
1. База данных позволила бы нам загружать данные единожды, после храниить их в БД. Для получения данных каких-либо уже API не вызывалось бы, т.к. мы работаем с БД.
2. Мы можем организовать структуру данных в БД по своему желанию, оптимизировать ее и использовать любые SQL-запросы.
3. БД отказоустойчива, если API перестанет работать, то БД - наш спаситель, т.к. данные будут в ней и наше приложение продолжит работать (при первом подходе мы полностью полагаемся на API).
### Минусы использования БД
1. Обслуживание базы данных. Необходимо иметь довольно мощный сервер, обслуживая его, т.к. с течением времени в нашей БД начнет хранится слишком много данных, которые необходимо будет обрабатывать и структурировать.
2. Из-за большого количества данных, возможно распределение их на различные кластеры БД, которые пасположены на разных серверах, что уменьшает отказоустойчивость.
3. Работа может превратиться в BigData, т.к. из-за большого количества данных будет необходимо максимально оптимизировать SQL-запросы (индексы, хранимые процедуры).
### P.S. Вопрос: хранить данные в БД или получвть их из API?
## 3. Обработка ошибок и архитектура.
Т.к. я не сильно знаком с WPF, вопрос обработки ошибок и архитектуры актуален. Хотелось бы иметь что-то наподобие ExceptionMiddleware, которая обрабатывала бы ошибки и доносила информацию пользователю. Так же архитектура MVVM - впервые использовал данный паттерн проектирования, и конечно есть вопросы, как правильно его использовать, т.к. 1000 программистов - 1000 мнений по поводу этого паттерна и логики проектирования\реализации его.
