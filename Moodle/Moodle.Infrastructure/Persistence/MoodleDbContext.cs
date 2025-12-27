using Microsoft.EntityFrameworkCore;
using Moodle.Domain.Entities;

namespace Moodle.Infrastructure.Persistence
{
    public class MoodleDbContext : DbContext
    {
        public MoodleDbContext(DbContextOptions<MoodleDbContext> options) 
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<CourseEnrollment> Enrollments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MoodleDbContext).Assembly);
        }
    }
}
