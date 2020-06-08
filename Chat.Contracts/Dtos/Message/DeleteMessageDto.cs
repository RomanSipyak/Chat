using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Contracts.Dtos.Message
{
    public class DeleteMessageDto
    {
        public int MessageID { get; set; }
        public bool DeleteForAll { get; set; }
        public bool DeleteForSender { get; set; }
    }
}
