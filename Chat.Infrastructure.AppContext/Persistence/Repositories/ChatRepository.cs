using Chat.Contracts.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Infrastructure.AppContext.Persistence.Repositories
{
    public class ChatRepository : Repository<Chat.Contracts.Entity.Chat>, IChatRepository
    {
        public ChatRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Chat.Contracts.Entity.Chat> GetChatByIdWithAllIncludes(int Id)
        {
            return await _dbContext.Chats.
                           Where(ch => ch.Id == Id)
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
