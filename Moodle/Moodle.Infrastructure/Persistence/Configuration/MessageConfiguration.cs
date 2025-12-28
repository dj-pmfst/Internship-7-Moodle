using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moodle.Domain.Entities;

namespace Moodle.Infrastructure.Persistence.Configuration
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        private const int maxLength = 1500;
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(m => m.Id);
            
            builder.Property(m => m.Text).IsRequired().HasMaxLength(maxLength);

            builder.HasOne(m => m.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(m => m.Receiver)
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(m => m.SenderId);
            builder.HasIndex(m => m.ReceiverId);
            builder.HasIndex(m => m.SentAt);
        }
    }
}
