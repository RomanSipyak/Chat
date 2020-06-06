using Chat.Contracts.Dtos.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Contracts.Interfaces.Services
{
    public interface IAccountService
    {
        Task<UserRegisteredDto> RegisterUserAsync(UserRegisterDto userRegisterDto);
        Task<UserLogedDto> LoginUserAsync(UserLoginDto userLoginDto);
    }
}
