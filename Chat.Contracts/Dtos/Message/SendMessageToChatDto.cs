using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Chat.Contracts.Dtos.Message
{
    public class SendMessageToChatDto
    {
        public int ChatId { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
