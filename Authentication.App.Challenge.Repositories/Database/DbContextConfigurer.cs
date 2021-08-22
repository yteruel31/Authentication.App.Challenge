using System;
using Microsoft.EntityFrameworkCore;

namespace Authentication.App.Challenge.Repositories.Database
{
    public static class DbContextConfigurer
    {
        private static readonly string ConnectionString =
            $"Server={Environment.GetEnvironmentVariable("DB_SERVER")}; Database={Environment.GetEnvironmentVariable("DB_DATABASE")}; User={Environment.GetEnvironmentVariable("DB_USERNAME")}; Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};";

        public static void Configure(
            DbContextOptionsBuilder<CoreContext> builder)
        {
            string connectionString = ConnectionString;
            builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
                option => option.EnableRetryOnFailure()).EnableDetailedErrors();
        }

        public static void Configure(
            DbContextOptionsBuilder builder)
        {
            string connectionString = ConnectionString;
            builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
                option => option.EnableRetryOnFailure()).EnableDetailedErrors();
        }
    }
}