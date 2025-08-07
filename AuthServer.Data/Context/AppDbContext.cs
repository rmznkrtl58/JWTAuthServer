using AuthServer.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Data.Context
{
   public class AppDbContext:IdentityDbContext<AppUser,AppRole,string>
    {
        //DbContextOptions->api->appsettingsJson->içinden sqlconnection vereceğim
        public AppDbContext(DbContextOptions<AppDbContext>options):base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Üzerinde çalıştığım libraryimde configuration implemente eden classlarımı yapılandır.
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            base.OnModelCreating(builder);
        }
    }
}
