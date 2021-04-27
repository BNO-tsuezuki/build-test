using Microsoft.EntityFrameworkCore;

namespace evolib.Databases.common3
{
    public class Common3DBContext : DbContext
    {
		public Common3DBContext(DbContextOptions<Common3DBContext> options)
		   : base(options)
		{
		}
		public DbSet<MatchMember> MatchMember { get; set; }
		public DbSet<ReplayViewNum> ReplayViewNum { get; set; }
		public DbSet<ReplayInfoRankMatch> ReplayInfoRankMatch { get; set; }
		public DbSet<ReplayInfoAllMatch> ReplayInfoAllMatch { get; set; }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ReplayInfoAllMatch>()
				.HasKey(e => new { e.date, e.matchId });
			modelBuilder.Entity<ReplayInfoAllMatch>()
				.HasIndex(e => e.matchId);

			modelBuilder.Entity<ReplayInfoRankMatch>()
				.HasKey(e => new { e.date, e.matchId });
		}
	}
}
