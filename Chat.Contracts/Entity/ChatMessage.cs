using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Contracts.Entity
{
    public class ChatMessage : BaseEntity
    {
        public int ChatId { get; set; }
        public Chat Chat { get; set; }
        public int MessageId { get; set; }
        public Message Message { get; set; }
    }
}
