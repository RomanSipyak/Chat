using Chat.Contracts.Entity;
using Chat.Contracts.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Infrastructure.AppContext.Persistence.Repositories
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        public MessageRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
