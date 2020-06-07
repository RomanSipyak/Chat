using Chat.Contracts.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Contracts.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IChatRepository ChatRepository { get; }
        IUserChatRepository UserChatRepository { get; }
        IChatMessageRepository ChatMessageRepository { get; }
        IMessageRepository MessageRepository { get; }

        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitAsync();
    }
}
