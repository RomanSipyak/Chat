using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Contracts.Dtos.User
{
    public class UserRegisteredDto
    {
        public bool Succeeded { get; set; }
        public string Error { get; set; }
        public UserDto User { get; set; }
    }
}
