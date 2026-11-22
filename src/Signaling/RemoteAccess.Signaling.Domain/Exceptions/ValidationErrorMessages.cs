namespace RemoteAccess.Signaling.Domain.Exceptions;

public static class ValidationErrorMessages
{
    public const string UserRequired = "Не удалось определить пользователя";
    public const string CallerRequired = "Отсутствуют данные отправителя";
    public const string ConnectionRequestRequired = "Отсутствуют данные запроса подключения";
    public const string DeviceRequired = "Требуется идентификатор устройства";
    public const string SessionRequired = "Требуется идентификатор сессии";
    public const string ConnectionRequired = "Не удалось определить соединение";
    public const string TokenRequired = "Токен доступа недоступен";
    public const string DeviceNotFound = "Устройство не найдено";
    public const string OwnerRequired = "Устройство не принадлежит пользователю";
    public const string DeviceOffline = "Устройство не в сети";
    public const string DeviceBusy = "Устройство уже участвует в запросе или сессии";
    public const string HostAlreadyRegistered = "Устройство уже зарегистрировано другим соединением";
    public const string ConnectionAlreadyRegistered = "Соединение уже зарегистрировано для другого устройства";
    public const string SelfConnection = "Нельзя подключиться к тому же устройству";
    public const string RequestUnavailable = "Запрос не найден, истёк или уже обработан";
    public const string SessionUnavailable = "Сессия не найдена или недоступна";
    public const string CoreUnavailable = "Сервис устройств временно недоступен";
    public const string TokenRejected = "Токен недействителен, войдите заново";
    public const string JwtSecretRequired = "Не задан Jwt:Secret, совпадающий с секретом Core";
    public const string CoreAddressRequired = "Не задан корректный адрес Core:BaseUrl";
    public const string PendingTimeoutInvalid = "Время ожидания ответа на запрос должно быть больше нуля";
    public const string UnexpectedError = "Не удалось выполнить операцию сигнализации";
    public const string SessionLeaseOptionsInvalid = "Некорректно настроены сроки аренды и период обслуживания сессий";
    public const string InfrastructureSettingsRequired = "Отсутствуют настройки инфраструктуры";
    public const string DatabaseNameRequired = "Не задано имя базы данных истории сессий";
    public const string CoreTimeoutInvalid = "Таймаут обращения к Core должен быть больше нуля";
    public const string RedisScriptMissing = "Не найден скрипт Redis: {0}";
    public const string RedisUnavailable = "Хранилище сессий временно недоступно";
}
