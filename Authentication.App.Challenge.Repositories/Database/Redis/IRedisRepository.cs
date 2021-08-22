using StackExchange.Redis;

namespace Authentication.App.Challenge.Repositories.Database.Redis
{
    public interface IRedisRepository
    {
        IDatabase Database { get; }
    }
}