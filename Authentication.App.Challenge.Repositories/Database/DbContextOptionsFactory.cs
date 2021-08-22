using Microsoft.EntityFrameworkCore;
using Portfolio.Back.Repositories.Database;

namespace Authentication.App.Challenge.Repositories.Database
{
    public static class DbContextOptionsFactory
    {
        public static DbContextOptions<CoreContext> Get()
        {
            var builder = new DbContextOptionsBuilder<CoreContext>();
            DbContextConfigurer.Configure(builder);

            return builder.Options;
        }
    }
}