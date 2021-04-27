using Microsoft.EntityFrameworkCore;

namespace AuthenticationServer.Databases
{
    public class AuthDBContext : DbContext
    {
        public AuthDBContext(DbContextOptions<AuthDBContext> options)
            : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
		

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

		}
	}
}
