using Chat.Contracts.Entity;
using Chat.Contracts.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Contracts.Interfaces.Repositories
{
    public interface IUserChatRepository : IRepository<UserChat>
    {
    }
}
