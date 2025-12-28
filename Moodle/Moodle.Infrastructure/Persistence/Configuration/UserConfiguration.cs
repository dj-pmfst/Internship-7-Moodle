using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moodle.Domain.Entities;

namespace Moodle.Infrastructure.Persistence.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        private const int maxLength = 150;
        public void Configure(EntityTypeBuilder<User> builder) 
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Email).IsRequired().HasMaxLength(maxLength);

            builder.HasIndex(u => u.Email).IsUnique();

            builder.Property(u => u.Name).IsRequired().HasMaxLength(maxLength);

            builder.Property(u => u.Password).IsRequired();


            builder.HasMany(u => u.TaughtCourses)
                .WithOne(c => c.Professor)
                .HasForeignKey(c => c.ProfessorId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasMany(u => u.SentMessages)
                .WithOne(m => m.Sender)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.ReceivedMessages)
                .WithOne(m => m.Receiver)
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
