using AutoMapper;
using Chat.Contracts.Dtos.Message;
using Chat.Contracts.Entity;
using Chat.Contracts.Interfaces;
using Chat.Contracts.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.BusinessLogic.Services
{
    public class MessagesService : IMessagesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public MessagesService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }
        //ManualTested
        public async Task SendMessageToChatAsync(SendMessageToChatDto sendMessageToChat, string SenderId)
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                var chatWhereWeSendMessage = await _unitOfWork.ChatRepository?.GetChatByIdWithAllIncludes(sendMessageToChat.ChatId);
                if (chatWhereWeSendMessage == null)
                {
                    throw new ArgumentException("Chat doesn't exist");
                }
                if (chatWhereWeSendMessage.Public)
                {
                    var senderInChat = chatWhereWeSendMessage.UserChats?.Where(uch => uch.UserId == SenderId).FirstOrDefault();
                    if (senderInChat == null)
                    {
                        await _unitOfWork.UserChatRepository.AddAsync(
                              new UserChat
                              {
                                  ChatId = chatWhereWeSendMessage.Id,
                                  UserId = SenderId
                              });
                        await _unitOfWork.CommitAsync();
                    }
                    await AttachMessageToChat(sendMessageToChat, SenderId, chatWhereWeSendMessage);
                }
                if (!chatWhereWeSendMessage.Public)
                {
                    var senderInChat = chatWhereWeSendMessage.UserChats?.Where(uch => uch.UserId == SenderId).FirstOrDefault();
                    if (senderInChat == null)
                    {
                        throw new ArgumentException("You are not in this chat");
                    }
                    await AttachMessageToChat(sendMessageToChat, SenderId, chatWhereWeSendMessage);
                }
                await _unitOfWork.CommitAsync();
                await transaction.CommitAsync();
                return;
            }
        }

        private async Task AttachMessageToChat(SendMessageToChatDto sendMessageToChat, string SenderId, Contracts.Entity.Chat chatWhereWeSendMessage)
        {
            var message = await _unitOfWork.MessageRepository?.AddAsync(
                                                new Message
                                                {
                                                    SenderId = SenderId,
                                                    MessageData = sendMessageToChat.Message,
                                                    DeletedForAll = false,
                                                    DeletedForSender = false,
                                                });
            await _unitOfWork.CommitAsync();
            await _unitOfWork.ChatMessageRepository?.AddAsync(
                            new ChatMessage
                            {
                                ChatId = chatWhereWeSendMessage.Id,
                                MessageId = message.Id,
                                DateSend = DateTime.UtcNow
                            });
            await _unitOfWork.CommitAsync();
        }

        public async Task SendMessageToUserAsync(SendMessageToUserDto sendMessageToUserDto, string SenderId)
        {
            User user = null;
            #region find user to send 
            if (string.IsNullOrEmpty(sendMessageToUserDto.SendToUserId) && string.IsNullOrEmpty(sendMessageToUserDto.SendToEmail))
            {
                throw new ArgumentException("You need to write userId or email");
            }

            if (!string.IsNullOrEmpty(sendMessageToUserDto.SendToUserId))
            {
                user = await _userManager.FindByIdAsync(sendMessageToUserDto.SendToUserId);
                
                if(user == null && string.IsNullOrEmpty(sendMessageToUserDto.SendToEmail))
                {
                    throw new ArgumentException("We can't find your user for message");
                }

                if(user == null && !string.IsNullOrEmpty(sendMessageToUserDto.SendToEmail))
                {
                    user = await _userManager.FindByEmailAsync(sendMessageToUserDto.SendToEmail);
                    if(user == null)
                    {
                        throw new ArgumentException("We can't find your user for message");
                    }
                }
            }
            else
            {
                user = await _userManager.FindByEmailAsync(sendMessageToUserDto.SendToEmail);
                if (user == null)
                {
                    throw new ArgumentException("We can't find your user for message");
                }
            }
            #endregion find user to send
            var chatForSend = sendMessageToUserDto.ChatId == null ? null : await _unitOfWork.ChatRepository.GetByIdAsync(sendMessageToUserDto.ChatId.Value);

            if(chatForSend != null && chatForSend.Public && chatForSend.UserChats.Any(uch => uch.UserId == user.Id))
            {
                await SendMessageToChatAsync(
                           new SendMessageToChatDto
                           {
                               ChatId = chatForSend.Id,
                               Message = sendMessageToUserDto.Message
                           },
                           SenderId
                     );
                return;
            }

            if(chatForSend != null && !chatForSend.Public)
            {
                if(chatForSend.UserChats.Any(uch => uch.UserId == SenderId) && chatForSend.UserChats.Any(uch => uch.UserId == user.Id))
                {
                    await SendMessageToChatAsync(
                            new SendMessageToChatDto
                            {
                                ChatId = chatForSend.Id,
                                Message = sendMessageToUserDto.Message
                            },
                            SenderId
                      );
                    return;
                }
            }
            chatForSend = _unitOfWork.ChatRepository.Find(ch => ch.DefaultChat && ch.Public == sendMessageToUserDto.PublicSend && (ch.UserChats.Any(uch => uch.UserId == SenderId)) && (ch.UserChats.Any(uch => uch.UserId == user.Id))).FirstOrDefault();

            if(chatForSend != null)
            {
                await SendMessageToChatAsync(
                           new SendMessageToChatDto
                           {
                               ChatId = chatForSend.Id,
                               Message = sendMessageToUserDto.Message
                           },
                           SenderId
                     );
                return;
            }
            else
            {
                //create new default  chat with this users 
                //add users and send message
                using (var transaction = await _unitOfWork.BeginTransactionAsync())
                {
                    chatForSend = await _unitOfWork.ChatRepository.AddAsync(new Contracts.Entity.Chat { DefaultChat = true, Public = sendMessageToUserDto.PublicSend });
                    var userChatSneder = await _unitOfWork.UserChatRepository.AddAsync(new UserChat { ChatId = chatForSend.Id, UserId = SenderId });
                    var userChatReciever = await _unitOfWork.UserChatRepository.AddAsync(new UserChat { ChatId = chatForSend.Id, UserId = user.Id });
                    chatForSend.UserChats.Add(userChatSneder);
                    chatForSend.UserChats.Add(userChatReciever);
                    await transaction.CommitAsync();
                }

                await SendMessageToChatAsync(
                            new SendMessageToChatDto
                            {
                                ChatId = chatForSend.Id,
                                Message = sendMessageToUserDto.Message
                            },
                            SenderId
                      );
                return;
            }
        }
        public async Task DeleteMessage(DeleteMessageDto deleteMessageDto, string userId)
        {
            if(deleteMessageDto.DeleteForAll == false && deleteMessageDto.DeleteForSender == false)
            {
                throw new ArgumentException("You need to chose at least one option");
            }
            var message = await _unitOfWork.MessageRepository.GetByIdAsync(deleteMessageDto.MessageID);

            if(message == null)
            {
                throw new Exception("Message doesn't exist");
            }

            if(message.SenderId != userId || message.DeletedForAll || message.DeletedForSender)
            {
                throw new Exception("You can't delete this message");
            }

            using(var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                message.DeletedForAll = deleteMessageDto.DeleteForAll;
                message.DeletedForSender = deleteMessageDto.DeleteForSender;
                await _unitOfWork.CommitAsync();
                await transaction.CommitAsync();
            }
        }
    }
}
