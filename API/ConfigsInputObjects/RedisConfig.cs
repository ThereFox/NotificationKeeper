namespace Notification.ConfigsInputObjects
{
    public record RedisConfig
    (
        string Host,
        int Port,
        string CommonPassword,
        string UserName,
        string UserPassword
    );
}
