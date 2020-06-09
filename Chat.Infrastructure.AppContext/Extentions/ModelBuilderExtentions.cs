using Chat.Contracts.Constats;
using Chat.Contracts.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Infrastructure.AppContext.Extentions
{
    public static class ModelBuilderExtentions
    {
        public static void SeedData(this ModelBuilder builder)
        {
            #region rolesAndUsers
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "2c087181-7837-4f6e-b233-7d0bb9a517fa",
                    Name = Roles.User,
                    NormalizedName = Roles.User.ToUpper()
                });

            var hasher = new PasswordHasher<User>();
            builder.Entity<User>().HasData(
               new User
               {
                   Id = "1",
                   UserName = "User1",
                   NormalizedUserName = "User1".ToUpper(),
                   Email = "user1@gmail.fake",
                   NormalizedEmail = "user1@gmail.fake".ToUpper(),
                   PasswordHash = hasher.HashPassword(null, "User1"),
                   SecurityStamp = string.Empty
               },
               new User
               {
                   Id = "2",
                   UserName = "User2",
                   NormalizedUserName = "User2".ToUpper(),
                   Email = "user2@gmail.fake",
                   NormalizedEmail = "user2@gmail.fake".ToUpper(),
                   PasswordHash = hasher.HashPassword(null, "User2"),
                   SecurityStamp = string.Empty,
               },
               new User
               {
                   Id = "3",
                   UserName = "User3",
                   NormalizedUserName = "User3".ToUpper(),
                   Email = "user3@gmail.fake",
                   NormalizedEmail = "user3@gmail.fake".ToUpper(),
                   PasswordHash = hasher.HashPassword(null, "User3"),
                   SecurityStamp = string.Empty,
               },
               new User
               {
                   Id = "4",
                   UserName = "User4",
                   NormalizedUserName = "User4".ToUpper(),
                   Email = "user4@gmail.fake",
                   NormalizedEmail = "user4@gmail.fake".ToUpper(),
                   PasswordHash = hasher.HashPassword(null, "User4"),
                   SecurityStamp = string.Empty,
               });
            builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                RoleId = "2c087181-7837-4f6e-b233-7d0bb9a517fa",
                UserId = "1"
            },
            new IdentityUserRole<string>
            {
                RoleId = "2c087181-7837-4f6e-b233-7d0bb9a517fa",
                UserId = "2"
            },
             new IdentityUserRole<string>
             {
                 RoleId = "2c087181-7837-4f6e-b233-7d0bb9a517fa",
                 UserId = "3"
             },
            new IdentityUserRole<string>
            {
                RoleId = "2c087181-7837-4f6e-b233-7d0bb9a517fa",
                UserId = "4"
            });
            #endregion rolesAndRoles

            #region chats 
            builder.Entity<Chat.Contracts.Entity.Chat>().HasData(
                new Chat.Contracts.Entity.Chat
                {
                    Id = 1,
                    Title = "Chat1",
                    Public = true,
                    DefaultChat = false
                },
                new Chat.Contracts.Entity.Chat
                {
                    Id = 2,
                    Title = "Chat2",
                    Public = false,
                    DefaultChat = true
                },
                new Chat.Contracts.Entity.Chat
                {
                    Id = 3,
                    Title = "Chat3",
                    Public = true,
                    DefaultChat = false
                });
            builder.Entity<UserChat>().HasData(
                new UserChat
                {
                    Id = 1,
                    ChatId = 1,
                    UserId = "1"

                },
                new UserChat
                {
                    Id = 2,
                    ChatId = 1,
                    UserId = "2"

                },
                new UserChat
                {
                    Id = 3,
                    ChatId = 1,
                    UserId = "3"

                },
                new UserChat
                {
                    Id = 4,
                    ChatId = 2,
                    UserId = "1"

                },
                new UserChat
                {
                    Id = 5,
                    ChatId = 2,
                    UserId = "2"

                });
            #endregion chats
            #region messages
            builder.Entity<Message>().HasData(
                new Message {
                    Id = 1,
                    MessageData = "For chat1 user1",
                    SenderId ="1"
                    
                },
                new Message
                {
                    Id = 2,
                    MessageData = "For chat1 user2",
                    SenderId = "2"
                },
                new Message
                {
                    Id = 3,
                    MessageData = "For chat1 user3",
                    SenderId = "3",
                    ParrentMessageId = 2,
                    DeletedForSender = true
                },
                new Message
                {
                    Id = 4,
                    MessageData = "For chat2 user1",
                    SenderId = "1",
                },
                new Message
                {
                    Id = 5,
                    MessageData = "For chat2 user2",
                    SenderId = "2"
                },
                new Message
                {
                    Id = 6,
                    MessageData = "For chat2 user1",
                    SenderId = "2",
                    ParrentMessageId = 4
                },
                new Message
                {
                    Id = 7,
                    MessageData = "For chat2 user1",
                    SenderId = "1",
                    ParrentMessageId = 6
                });

            builder.Entity<ChatMessage>().HasData(
                new ChatMessage
                {
                    Id = 1,
                    ChatId = 1,
                    MessageId = 1,
                    DateSend = DateTime.UtcNow
                },
                new ChatMessage
                {
                    Id = 2,
                    ChatId = 1,
                    MessageId = 2,
                    DateSend = DateTime.UtcNow.AddDays(1)
                },
                new ChatMessage
                {
                    Id = 3,
                    ChatId = 1,
                    MessageId = 3,
                    DateSend = DateTime.UtcNow.AddMinutes(10)
                },
                new ChatMessage
                {
                    Id = 4,
                    ChatId = 2,
                    MessageId = 4,
                    DateSend = DateTime.UtcNow.AddMinutes(10)
                },
                new ChatMessage
                {
                    Id = 5,
                    ChatId = 2,
                    MessageId = 5,
                    DateSend = DateTime.UtcNow.AddMinutes(12)
                },
                new ChatMessage
                {
                    Id = 6,
                    ChatId = 2,
                    MessageId = 6,
                    DateSend = DateTime.UtcNow.AddMinutes(15)
                },
                new ChatMessage
                {
                    Id = 7,
                    ChatId = 2,
                    MessageId = 7,
                    DateSend = DateTime.UtcNow.AddMinutes(20)
                });
            #endregion messages
        }
    }
}
