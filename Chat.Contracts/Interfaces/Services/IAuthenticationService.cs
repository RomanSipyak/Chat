using Chat.Contracts.ConfigurationObjects;
using Chat.Contracts.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Contracts.Interfaces.Services
{
    public interface IAuthenticationService
    {
        Task<string> GetAccessTokenAsync(User user, JwtSettings jwtSettings);
    }
}
