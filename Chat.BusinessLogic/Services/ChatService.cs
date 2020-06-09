using AutoMapper;
using Chat.Contracts.Dtos.Chat;
using Chat.Contracts.Entity;
using Chat.Contracts.Interfaces;
using Chat.Contracts.Interfaces.Services;
using Chat.Infrastructure.AppContext.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<ICollection<GetChatDto>> GetAllChatsForUserAsync(string userId)
        {
            var chats = _unitOfWork.ChatRepository.Find(ch => ch.UserChats.Any(uch => uch.UserId == userId)).ToList(); ;
            ICollection<GetChatDto> listChatsDto = new List<GetChatDto>();
            foreach (var chat in chats)
            {
                listChatsDto.Add(_mapper.Map<GetChatDto>(chat));
            }
            return listChatsDto;
        }
    }
}
