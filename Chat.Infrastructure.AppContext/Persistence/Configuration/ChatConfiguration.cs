using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Chat.Contracts.Entity;


namespace Chat.Infrastructure.AppContext.Persistence.Configuration
{
    class ChatConfiguration : IEntityTypeConfiguration<Chat.Contracts.Entity.Chat>
    {
        public void Configure(EntityTypeBuilder<Chat.Contracts.Entity.Chat> builder)
        {
            builder.HasKey(ct => ct.Id);
        }
    }
}
