using System;
using StackExchange.Redis;

namespace Authentication.App.Challenge.Repositories.Database.Redis
{
    public class RedisRepository : IRedisRepository
    {
        public IDatabase Database { get; }
        
        public RedisRepository()
        {
            Database = GetConnectionMultiplexer();
        }
        
        private IDatabase GetConnectionMultiplexer()
        {
            return ConnectionMultiplexer.Connect(Environment.GetEnvironmentVariable("REDIS_SERVER")).GetDatabase();
        }
    }
}