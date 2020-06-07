using Chat.Contracts.Entity;
using Chat.Contracts.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Infrastructure.AppContext.Persistence.Repositories
{
    public class ChatMessageRepository : Repository<ChatMessage>, IChatMessageRepository
    {
        public ChatMessageRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
