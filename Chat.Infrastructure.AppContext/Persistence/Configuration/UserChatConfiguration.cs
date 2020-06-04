using Chat.Contracts.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Infrastructure.AppContext.Persistence.Configuration
{
    class UserChatConfiguration : IEntityTypeConfiguration<UserChat>
    {
        public void Configure(EntityTypeBuilder<UserChat> builder)
        {
            builder.HasKey(uct => uct.Id);

            builder.HasOne(uct => uct.User)
                .WithMany(u => u.UserChats)
                .HasForeignKey(uct => uct.UserId);

            builder.HasOne(uct => uct.Chat)
                .WithMany(ct => ct.UserChats)
                .HasForeignKey(uct => uct.ChatId);
        }
    }
}
