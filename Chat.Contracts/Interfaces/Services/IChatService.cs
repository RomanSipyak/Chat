using Chat.Contracts.Dtos.Chat;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Contracts.Interfaces.Services
{
    public interface IChatService
    {
        Task AddUserToChatAsync(ConnectChatDto connectChatDto, string userId);
    }
}
