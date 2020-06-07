using Chat.Contracts.Entity;
using Chat.Contracts.Interfaces;
using Chat.Contracts.Interfaces.Repositories;
using Chat.Infrastructure.AppContext.Persistence.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Infrastructure.AppContext.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool _disposed = false;
        private readonly AppDbContext _context;

        public IChatRepository ChatRepository { get; }
        public IUserChatRepository UserChatRepository { get; }
        public IChatMessageRepository ChatMessageRepository { get; }
        public IMessageRepository MessageRepository { get; }


        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            ChatRepository = new ChatRepository(_context);
            UserChatRepository = new UserChatRepository(_context);
            ChatMessageRepository = new ChatMessageRepository(_context);
            MessageRepository = new MessageRepository(_context);
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _context.Dispose();
            }

            _disposed = true;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}
