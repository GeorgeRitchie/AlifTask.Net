using AlifTask.Entities;
using AlifTask.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace AlifTask.Persistence
{
	// add-migration Init -Context AppDbContext -o "Persistence/Migrations/"
	// update-database -Context AppDbContext
	// drop-database -Context AppDbContext
	public class AppDbContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Wallet> Wallets { get; set; }

		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfiguration(new OperationConfigurations());
		}
	}
}
