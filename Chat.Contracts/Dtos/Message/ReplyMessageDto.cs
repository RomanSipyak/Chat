using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Contracts.Dtos.Message
{
    public class ReplyMessageDto
    {
        public int MessageID { get; set; }
        public int ReplyChatId { get; set; }
        public string MessageData { get; set; }
        public bool ReplyForSenderInPrivateChat { get; set; }
    }
}
