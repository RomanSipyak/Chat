using AutoMapper;
using Chat.Contracts.Dtos;
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
                //await _unitOfWork.CommitAsync();
                await transaction.CommitAsync();
                return;
            }
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

                if (user == null && string.IsNullOrEmpty(sendMessageToUserDto.SendToEmail))
                {
                    throw new ArgumentException("We can't find your user for message");
                }

                if (user == null && !string.IsNullOrEmpty(sendMessageToUserDto.SendToEmail))
                {
                    user = await _userManager.FindByEmailAsync(sendMessageToUserDto.SendToEmail);
                    if (user == null)
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
            var chatForSend = sendMessageToUserDto.ChatId == null ? null : await _unitOfWork.ChatRepository.GetChatByIdWithAllIncludes(sendMessageToUserDto.ChatId.Value);

            if (chatForSend != null && chatForSend.Public && ChatContainUser(user, chatForSend).Value)
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

            if (chatForSend != null && !chatForSend.Public && ChatContainUser(SenderId, chatForSend).Value && ChatContainUser(user, chatForSend).Value)
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
            chatForSend = _unitOfWork.ChatRepository.Find(ch => ch.DefaultChat && ch.Public == sendMessageToUserDto.PublicSend && (ch.UserChats.Any(uch => uch.UserId == SenderId)) && (ch.UserChats.Any(uch => uch.UserId == user.Id))).FirstOrDefault();

            if (chatForSend != null)
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
                chatForSend = await CreateDefaultChat(sendMessageToUserDto.PublicSend, SenderId, user);

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
            if (deleteMessageDto.DeleteForAll == false && deleteMessageDto.DeleteForSender == false)
            {
                throw new ArgumentException("You need to chose at least one option");
            }
            var message = await _unitOfWork.MessageRepository.GetByIdAsync(deleteMessageDto.MessageID);

            if (message == null)
            {
                throw new Exception("Message doesn't exist");
            }

            if (message.SenderId != userId || message.DeletedForAll || message.DeletedForSender)
            {
                throw new Exception("You can't delete this message");
            }

            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                message.DeletedForAll = deleteMessageDto.DeleteForAll;
                message.DeletedForSender = deleteMessageDto.DeleteForSender;
                await _unitOfWork.CommitAsync();
                await transaction.CommitAsync();
            }
        }

        public async Task UpdateMessage(UpdateMessageDto updateMessageDto, string userId)
        {
            var message = await _unitOfWork.MessageRepository.GetByIdAsync(updateMessageDto.MessageID);

            if (message == null)
            {
                throw new Exception("Message doesn't exist");
            }

            if (message.SenderId != userId || message.DeletedForAll || message.DeletedForSender)
            {
                throw new Exception("You can't update this message");
            }

            if (string.IsNullOrEmpty(updateMessageDto.Message))
            {
                throw new ArgumentException("You message text is incorrect");
            }

            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                message.MessageData = updateMessageDto.Message;
                await _unitOfWork.CommitAsync();
                await transaction.CommitAsync();
            }
        }


        /// <summary>
        /// Reply for message chat.
        /// If you set ReplyForSenderInPrivateChat = true;
        /// We try to find this chat by ReplyChatId. 
        /// If this chat exists and it is not public and you are not attached for this chat or reciverUser 
        /// is not attached we will be try to find default private chat for you and your reciever and send reply there. 
        /// Another way we will be create this chat attache thre you and your reciever and send message there. 
        /// If Chat not exist we will try find default chat and make the same action that describe above.
        /// If you set ReplyForSenderInPrivateChat = false;
        /// We try to find this chat by ReplyChatId. 
        /// If this chat exists we send a reply message for here if this message that we want to be replied attached for this chat and you are attached also for this chat.
        /// </summary>
        public async Task ReplyMessage(ReplyMessageDto replyMessageDto, string userId)
        {
            if (string.IsNullOrEmpty(replyMessageDto.MessageData))
            {
                throw new ArgumentException("Please Write your reply message");
            }

            var parrentMessage = await _unitOfWork.MessageRepository.GetByIdAsync(replyMessageDto.MessageID);
            if (parrentMessage == null || parrentMessage.DeletedForAll)
            {
                throw new ArgumentException("Your message doesn't exist");
            }

            var recieverUser = await _userManager.FindByIdAsync(parrentMessage.SenderId);
            if(recieverUser == null)
            {
                throw new Exception("we can't send reply message");
            }

            if (replyMessageDto.ReplyForSenderInPrivateChat)
            {
                var chatWhereWeSendMessage = await _unitOfWork.ChatRepository?.GetChatByIdWithAllIncludes(replyMessageDto.ReplyChatId);

                if (chatWhereWeSendMessage != null && !chatWhereWeSendMessage.Public 
                                                   && ChatContainUser(userId, chatWhereWeSendMessage).Value 
                                                   && ChatContainUser(recieverUser, chatWhereWeSendMessage).Value)
                {
                    await SendMessageToChatAsync(
                           chatWhereWeSendMessage,
                           replyMessageDto,
                           userId,
                           parrentMessage
                      );
                    return;
                }

                chatWhereWeSendMessage = _unitOfWork.ChatRepository.Find(ch => ch.DefaultChat && ch.Public == false
                                                                                              && (ChatContainUser(userId, ch).Value) 
                                                                                              && (ChatContainUser(userId, ch).Value)).FirstOrDefault();

                if (chatWhereWeSendMessage != null)
                {
                    await SendMessageToChatAsync(
                           chatWhereWeSendMessage,
                           replyMessageDto,
                           userId,
                           parrentMessage
                      );
                    return;
                }
                else
                {
                    chatWhereWeSendMessage = await CreateDefaultChat(false, userId, recieverUser);
                    await SendMessageToChatAsync(
                            chatWhereWeSendMessage,
                            replyMessageDto,
                            userId,
                            parrentMessage
                       );
                    return;
                }
            }
            else
            {
                var chatWhereWeSendMessage = await _unitOfWork.ChatRepository?.GetChatByIdWithAllIncludes(replyMessageDto.ReplyChatId);

                if (chatWhereWeSendMessage != null && chatWhereWeSendMessage.Public
                                                   && chatWhereWeSendMessage.ChatMessages.Any(chm => chm.MessageId == replyMessageDto.MessageID))
                {
                    await SendMessageToChatAsync(
                           chatWhereWeSendMessage,
                           replyMessageDto,
                           userId,
                           parrentMessage
                      );
                    return;
                }
                else
                {
                    throw new ArgumentException("We cnan't send reply");
                }
            }
        }

        public async Task SendMessageToChatAsync(Chat.Contracts.Entity.Chat chatWhereWeSendMessage, ReplyMessageDto replyMessageDto, string senderId, Message messageParent)
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                 if (chatWhereWeSendMessage.Public)
                {
                    var senderInChat = chatWhereWeSendMessage.UserChats?.Where(uch => uch.UserId == senderId).FirstOrDefault();
                    if (senderInChat == null)
                    {
                        await _unitOfWork.UserChatRepository.AddAsync(
                              new UserChat
                              {
                                  ChatId = chatWhereWeSendMessage.Id,
                                  UserId = senderId
                              });
                        await _unitOfWork.CommitAsync();
                    }
                    Message message = await CreateReplyMessage(replyMessageDto, senderId, messageParent);
                    await _unitOfWork.CommitAsync();
                    await AttachMessageToChat(message, chatWhereWeSendMessage);
                }
                if (!chatWhereWeSendMessage.Public)
                {
                    var senderInChat = chatWhereWeSendMessage.UserChats?.Where(uch => uch.UserId == senderId).FirstOrDefault();
                    if (senderInChat == null)
                    {
                        throw new ArgumentException("You are not in this chat");
                    }

                    var message = await CreateReplyMessage(replyMessageDto, senderId, messageParent);
                    await _unitOfWork.CommitAsync();
                    await AttachMessageToChat(message, chatWhereWeSendMessage);
                }
                await _unitOfWork.CommitAsync();
                await transaction.CommitAsync();
                return;
            }
        }

        public async Task<ICollection<GetMessageDto>> GetAllMessagesForChat(int chatId, string userId, PaginationFilterDto pagination)
        {   
            var chat = await _unitOfWork.ChatRepository.GetChatByIdWithAllIncludes(chatId);
            if(chat == null)
            {
                throw new ArgumentException("This chat doesn't exist");
            }

            if (ChatContainUser(userId, chat).Value)
            {
                var chatMessageList = chat.ChatMessages.OrderBy(chm => chm.DateSend).ToList();
                List<GetMessageDto> getMessagesDto = new List<GetMessageDto>();
                foreach (var chatMessage in chatMessageList)
                {
                    var messageDTO = _mapper.Map<GetMessageDto>(chatMessage.Message);
                    messageDTO.SendDate = chatMessage.DateSend;
                    getMessagesDto.Add(messageDTO);
                }
                getMessagesDto = getMessagesDto.OrderBy(m => m.SendDate)
                                               .Skip((pagination.PageNo - 1) * pagination.Limit)
                                               .Take(pagination.Limit)
                                               .ToList(); 
                return getMessagesDto;
            }
            else
            {
                throw new ArgumentException("You can't do this action");
            }
        }


        private async Task<Message> CreateReplyMessage(ReplyMessageDto replyMessageDto, string senderId, Message messageParent)
        {
            return await _unitOfWork.MessageRepository?.AddAsync(
                                      new Message
                                      {
                                          SenderId = senderId,
                                          MessageData = replyMessageDto.MessageData,
                                          ParrentMessageId = messageParent.Id,
                                          DeletedForAll = false,
                                          DeletedForSender = false,
                                      });
        }

        private async Task<Contracts.Entity.Chat> CreateDefaultChat(bool chatPublic, string SenderId, User user)
        {
            Contracts.Entity.Chat chatForSend = null;
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                chatForSend = await _unitOfWork.ChatRepository.AddAsync(new Contracts.Entity.Chat { DefaultChat = true, Public = chatPublic });
                await _unitOfWork.CommitAsync();
                var userChatSneder = await _unitOfWork.UserChatRepository.AddAsync(new UserChat { ChatId = chatForSend.Id, UserId = SenderId });
                var userChatReciever = await _unitOfWork.UserChatRepository.AddAsync(new UserChat { ChatId = chatForSend.Id, UserId = user.Id });
                chatForSend.UserChats.Add(userChatSneder);
                chatForSend.UserChats.Add(userChatReciever);
                await _unitOfWork.CommitAsync();
                await transaction.CommitAsync();
            }

            return chatForSend;
        }

        private static bool? ChatContainUser(User user, Contracts.Entity.Chat chatForSend)
        {
            return chatForSend.UserChats?.Any(uch => uch.UserId == user.Id);
        }

        private static bool? ChatContainUser(string userId, Contracts.Entity.Chat chatForSend)
        {
            return chatForSend.UserChats?.Any(uch => uch.UserId == userId);
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

        private async Task AttachMessageToChat(Message messageForAttach, Contracts.Entity.Chat chatWhereWeSendMessage)
        {
            await _unitOfWork.ChatMessageRepository?.AddAsync(
                            new ChatMessage
                            {
                                ChatId = chatWhereWeSendMessage.Id,
                                MessageId = messageForAttach.Id,
                                DateSend = DateTime.UtcNow
                            });
            await _unitOfWork.CommitAsync();
        }
    }
}
