using Chat.Contracts.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Contracts.Entity
{ 
    public class Chat : BaseEntity
    {
        public ICollection<UserChat> UserChats { get; set; }
        public ICollection<ChatMessage> ChatMessages { get; set; }
        public bool Public { get; set; }
        public string Title { get; set; }
    }
}
