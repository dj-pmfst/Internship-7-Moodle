using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moodle.Domain.Entities;

namespace Moodle.Infrastructure.Persistence.Configuration
{
    public class MaterialConfiguration : IEntityTypeConfiguration<Material>
    {
        private const int maxLength = 150;
        public void Configure(EntityTypeBuilder<Material> builder)
        {
            builder.HasKey(m  => m.Id);

            builder.Property(m => m.Name).IsRequired().HasMaxLength(maxLength);
            builder.Property(m => m.Url).IsRequired();

            builder.HasOne(m => m.Course)
                .WithMany(c => c.Materials)
                .HasForeignKey(m => m.CourseID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(m => m.CourseID);
        }
    }
}
