namespace RemoteAccess.Common.Redis;

public static class ValidationErrorMessages
{
    public const string ConfigurationRequired = "Не задана строка подключения Redis";
    public const string InstanceNameRequired = "Не задан префикс экземпляра Redis";
    public const string OperationFailed = "Не удалось выполнить операцию Redis";
    public const string InvalidResponse = "Redis вернул данные в некорректном формате";
}
