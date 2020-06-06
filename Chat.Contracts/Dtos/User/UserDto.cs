﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Contracts.Dtos.User
{
    public class UserDto
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public ICollection<string> Roles { get; set; }
    }
}
