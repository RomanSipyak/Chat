using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Contracts.Interfaces.Repositories
{
    public interface IChatRepository : IRepository<Chat.Contracts.Entity.Chat>
    {
        Task<Chat.Contracts.Entity.Chat> GetChatByIdWithAllIncludes(int Id);
    }
}
