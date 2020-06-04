using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Contracts.Entity
{
    public class Message : BaseEntity
    {   
        public string SenderId { get; set; }
        public User Sender { get; set; }

        public string MessageData { get; set; }
        public ICollection<ChatMessage> ChatMessages { get; set; }

        public int? ParrentMessageId { get; set; }
        public Message ParrentMessage { get; set; }

        public ICollection<Message> Replies { get; set; }

        public bool DeletedForAll { get; set; }

        public bool DeletedForSender { get; set; }
    }
}
