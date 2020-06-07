using AutoMapper;
using Chat.Contracts.Dtos.Chat;
using Chat.Contracts.Entity;
using Chat.Contracts.Interfaces;
using Chat.Contracts.Interfaces.Services;
using Chat.Infrastructure.AppContext.Persistence;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Chat.BusinessLogic.Services
{
    public class ChatService : IChatService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public ChatService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task AddUserToChatAsync(ConnectChatDto connectChatDto, string userId)
        {
            var chat = await _unitOfWork.ChatRepository.GetByIdAsync(connectChatDto.Id);
            if (chat == null || !chat.Public)
            {
                throw new ArgumentException("Chat with this is not exist or you can't connect to this chat");
            }

            await  _unitOfWork.UserChatRepository.AddAsync(new UserChat
            {
                ChatId = chat.Id,
                UserId = userId
            });

            await _unitOfWork.CommitAsync();
        }
    }
}
