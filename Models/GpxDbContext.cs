using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace RunPlanner.Models
{
    public class GpxDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public GpxDbContext(DbContextOptions<GpxDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public DbSet<GpxTrack> GpxTracks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
            }
        }
    }
}
