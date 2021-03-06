﻿using Chat.Contracts.Dtos;
using Chat.Contracts.Dtos.Message;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Contracts.Interfaces.Services
{
    public interface IMessagesService
    {
        Task SendMessageToChatAsync(SendMessageToChatDto sendMessageDto, string SenderId);
        Task SendMessageToUserAsync(SendMessageToUserDto sendMessageToUserDto, string SenderId);
        Task DeleteMessage(DeleteMessageDto deleteMessageDto, string userId);
        Task UpdateMessage(UpdateMessageDto updateMessageDto, string userId);
        Task ReplyMessage(ReplyMessageDto replyMessageDto, string userId);
        Task<ICollection<GetMessageDto>> GetAllMessagesForChat(int chatId, string userId, PaginationFilterDto pagination);
    }
}
