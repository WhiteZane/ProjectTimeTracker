using Microsoft.EntityFrameworkCore;
using ProjectTimeTrackerAuth.Models;

namespace ProjectTimeTrackerAuth.Data {
    public class TimerContext : DbContext {

        public TimerContext(DbContextOptions<TimerContext> options) : base(options) { }

        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityType> ActivityTypes { get; set; }
        public DbSet<TimeLog> TimeLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Activity>().ToTable("Activity");
            modelBuilder.Entity<ActivityType>().ToTable("ActivityType");
            modelBuilder.Entity<TimeLog>().ToTable("TimeLog");
        }
    }
}