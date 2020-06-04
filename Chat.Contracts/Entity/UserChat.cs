using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Contracts.Entity
{
    public class UserChat : BaseEntity
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public int ChatId { get; set; }
        public Chat Chat { get; set; }
    }
}
