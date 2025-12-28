using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moodle.Domain.Entities;

namespace Moodle.Infrastructure.Persistence.Configuration
{
    public class AnnouncementConfiguration : IEntityTypeConfiguration<Announcement>
    {
        private const int maxLengthTitle = 150;
        private const int maxLengthText = 1500;
        public void Configure(EntityTypeBuilder<Announcement> builder) 
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Title).IsRequired().HasMaxLength(maxLengthTitle);
            builder.Property(a => a.Text).IsRequired().HasMaxLength(maxLengthText);

            builder.HasOne(a => a.Course)
                .WithMany(c => c.Announcements)
                .HasForeignKey(c => c.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.Professor)
                .WithMany(u => u.Announcements)
                .HasForeignKey(u => u.ProfessorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(a => a.CourseId);
            builder.HasIndex(a => a.CreatedAt);
        }
    }
}
