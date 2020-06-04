using Chat.Contracts.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Infrastructure.AppContext.Persistence.Configuration
{
    class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
    {
        public void Configure(EntityTypeBuilder<ChatMessage> builder)
        {
            builder.HasKey(ctm => ctm.Id);

            builder.HasOne(ctm => ctm.Chat)
                .WithMany(ct => ct.ChatMessages)
                .HasForeignKey(ctm => ctm.ChatId);

            builder.HasOne(ctm => ctm.Message)
                .WithMany(ms => ms.ChatMessages)
                .HasForeignKey(ctm => ctm.MessageId);
        }
    }
}
