using Authentication.App.Challenge.Repositories.Database.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Authentication.App.Challenge.Repositories.Database
{
    public class CoreContext : DbContext
    {
        public CoreContext()
        {
        }
        
        public CoreContext(DbContextOptions<CoreContext> options) : base(options)
        {
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                DbContextConfigurer.Configure(optionsBuilder);
            }
        }

        public DbSet<UserDto> Users { get; set; }
    }
}