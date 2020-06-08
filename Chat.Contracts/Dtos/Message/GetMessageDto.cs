using Chat.Contracts.Dtos.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Contracts.Dtos.Message
{
    public class GetMessageDto
    {
        public int Id { get; set; }

        public string SenderId { get; set; }
        public UserDto Sender { get; set; }

        public string MessageData { get; set; }
      
        public int? ParrentMessageId { get; set; }
        public GetMessageDto ParrentMessage { get; set; }

        public ICollection<GetMessageDto> Replies { get; set; }

        public DateTime? SendDate { get; set; }
    }
}
