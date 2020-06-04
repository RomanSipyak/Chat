using Chat.Contracts.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Infrastructure.AppContext.Persistence.Configuration
{
    class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(me => me.Id);

            builder.HasOne(me => me.Sender)
                .WithMany(sr => sr.Messages)
                .HasForeignKey(me => me.SenderId);

            builder.HasMany(me => me.Replies)
                .WithOne(rs => rs.ParrentMessage)
                .HasForeignKey(pe => pe.ParrentMessageId);
        }
    }
}
