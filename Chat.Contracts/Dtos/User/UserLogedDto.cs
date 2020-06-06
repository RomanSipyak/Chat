using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Contracts.Dtos.User
{
    public class UserLogedDto
    {
        public string AccessToken { get; set; }
        public UserDto User { get; set; }
    }
}
