using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Entities;
using System.Reflection;

namespace SocialMediaApp.Data
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions options) : base(options)
		{
		}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }



        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Photo> Photos  { get; set; }

    }
}
