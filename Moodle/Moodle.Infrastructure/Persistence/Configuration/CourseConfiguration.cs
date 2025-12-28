using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moodle.Domain.Entities;

namespace Moodle.Infrastructure.Persistence.Configuration
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        private const int maxLengthName = 150;
        private const int maxLengthDescription = 1500;
        public void Configure(EntityTypeBuilder<Course> builder) 
        {
            builder.HasKey(c  => c.Id);

            builder.Property(c => c.Name).IsRequired().HasMaxLength(maxLengthName);
            builder.Property(c => c.Description).IsRequired().HasMaxLength(maxLengthDescription);

            builder.HasOne(c => c.Professor)
                .WithMany(u => u.TaughtCourses)
                .HasForeignKey(c => c.ProfessorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Enrollments)
                .WithOne(ce => ce.Course)
                .HasForeignKey(ce => ce.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Announcements)
                .WithOne(a => a.Course)
                .HasForeignKey(a => a.CourseId)
                .OnDelete(DeleteBehavior.Cascade);  

            builder.HasMany(c => c.Materials)
                .WithOne(m => m.Course)
                .HasForeignKey(m => m.CourseID)
                .OnDelete(DeleteBehavior.Cascade);  

            builder.HasIndex(c => c.ProfessorId);
            builder.HasIndex(c => c.Name);
        }
    }
}
