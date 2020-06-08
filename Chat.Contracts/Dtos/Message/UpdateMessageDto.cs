using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Contracts.Dtos.Message
{
    public class UpdateMessageDto
    {
        public int MessageID { get; set; }
        public string Message { get; set; }
    }
}
