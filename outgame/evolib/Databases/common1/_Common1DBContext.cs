using Microsoft.EntityFrameworkCore;

namespace evolib.Databases.common1
{
    public class Common1DBContext : DbContext
    {
		public Common1DBContext(DbContextOptions<Common1DBContext> options)
		   : base(options)
		{
		}

		public DbSet<PlayerId> PlayerIds { get; set; }
		public DbSet<Account> Accounts { get; set; }
		public DbSet<EnabledVersion> EnabledVersions { get; set; }
		public DbSet<OpsNotice> OpsNotices { get; set; }
		public DbSet<LoginReject> LoginRejects { get; set; }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Account>()
				.HasKey(e => new { e.account, e.type });

			modelBuilder.Entity<EnabledVersion>()
				.HasKey(e => new { e.checkTarget, e.referenceSrc });

			
			modelBuilder.Entity<LoginReject>()
				.HasKey(e => new { e.target, e.value });
		}
	}
}
