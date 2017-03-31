using Microsoft.EntityFrameworkCore;
using ProjectTimeTracker.Models;

namespace ProjectTimeTracker.Data
{
    public class TimerContext : DbContext
    {
        public TimerContext(DbContextOptions<TimerContext> options) : base(options)
        {
        }

        public DbSet<Activities> Activity { get; set; }
        public DbSet<ActivityTypes> ActivityType { get; set; }
        public DbSet<Users> User { get; set; }
        public DbSet<TimeLog> TimeLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Activities>().ToTable("Activities");
            modelBuilder.Entity<ActivityTypes>().ToTable("ActivityTypes");
            modelBuilder.Entity<Users>().ToTable("Users");
            modelBuilder.Entity<TimeLog>().ToTable("TimeLog");
        }
    }
}

