using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Contracts.Dtos.Chat
{
    public class GetChatDto : BaseDto<int>
    {
        public bool Public { get; set; }
        public bool DefaultChat { get; set; }
        public string Title { get; set; }
    }
}
