using Chat.Contracts.Entity;
using Chat.Contracts.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Infrastructure.AppContext.Persistence.Repositories
{
    public class UserChatRepository : Repository<UserChat>, IUserChatRepository
    {
        public UserChatRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
