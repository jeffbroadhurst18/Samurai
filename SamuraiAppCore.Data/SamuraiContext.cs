using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using SamuraiAppCore.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SamuraiAppCore.Data
{
	public class SamuraiContext : DbContext
	{
		private IConfiguration _config;

		public DbSet<Samurai> Samurais { get; set; }
		public DbSet<Battle> Battles { get; set; }
		public DbSet<Quote> Quotes { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<SamuraiBattle>().HasKey(s => new { s.BattleId, s.SamuraiId });
			base.OnModelCreating(builder);

		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
			_config = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", true, true)
				.Build();

			optionsBuilder.UseSqlServer("Server=BROADHURST_WIN8\\BROADHURST;Database=SamuraiDataCore;Trusted_Connection=True;MultipleActiveResultSets=true;");
		}
	}
}
