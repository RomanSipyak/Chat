using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Chat.Contracts.Dtos.Chat
{
    public class ConnectChatDto
    {
        [Required]
        public int Id { get; set; }

        public string Title { get; set; }
    }
}
