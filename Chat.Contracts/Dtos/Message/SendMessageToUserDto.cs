using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Chat.Contracts.Dtos.Message
{
    public class SendMessageToUserDto
    {
        public int? ChatId { get; set; }
        [EmailAddress]
        public string SendToEmail { get; set; }
        public string SendToUserId { get; set; }
        public bool PublicSend { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
