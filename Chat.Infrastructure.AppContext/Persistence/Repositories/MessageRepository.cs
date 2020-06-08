using Chat.Contracts.Entity;
using Chat.Contracts.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Infrastructure.AppContext.Persistence.Repositories
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        public MessageRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Chat.Contracts.Entity.Chat> GetMessagesById(int chatId)
        {
            return await _dbContext.Chats.
                Where(ch => ch.Id == chatId)
                .Include(ct => ct.UserChats)
                .ThenInclude(uct => uct.User)
                .Include(ct => ct.ChatMessages)
                .ThenInclude(ctm => ctm.Message)
                .ThenInclude(ms => ms.Replies)
                .Include(ct => ct.ChatMessages)
                .ThenInclude(ctm => ctm.Message)
                .ThenInclude(ms => ms.ParrentMessage)
                .Include(ct => ct.ChatMessages)
                .ThenInclude(ctm => ctm.Message)
                .ThenInclude(ms => ms.Sender)
                .FirstOrDefaultAsync();
        }
    }
}
