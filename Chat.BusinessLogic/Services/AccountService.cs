using AutoMapper;
using Chat.Contracts.ConfigurationObjects;
using Chat.Contracts.Constats;
using Chat.Contracts.Constats.ServicesConstants;
using Chat.Contracts.Dtos;
using Chat.Contracts.Dtos.User;
using Chat.Contracts.Entity;
using Chat.Contracts.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.BusinessLogic.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtSettings _jwtSettings;
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;

        public AccountService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, JwtSettings jwtSettings, IAuthenticationService authenticationService, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings;
            _authenticationService = authenticationService;
            _mapper = mapper;
        }

        public async Task<UserRegisteredDto> RegisterUserAsync(UserRegisterDto userRegisterDto)
        {
            var existingUser = await this._userManager.FindByEmailAsync(userRegisterDto.Email);
            if (existingUser != null)
            {
                return new UserRegisteredDto
                {
                   Succeeded = false,
                   Error = AccountServiceConstants.UserWithMailExist,
                };
            }

            var newUser = _mapper.Map<User>(userRegisterDto);
            var createdUser = await this._userManager.CreateAsync(newUser, userRegisterDto.Password);
            var returnedUser = new UserRegisteredDto
            {
                Succeeded = createdUser.Succeeded
            };

            if (!createdUser.Succeeded)
            {
                returnedUser.Error = string.Join(" ", createdUser.Errors.Select(x => x.Description));
                returnedUser.User = _mapper.Map<UserDto>(newUser);
                return returnedUser;
            }

            await _userManager.AddToRoleAsync(newUser, Roles.User);

            returnedUser.User = _mapper.Map<UserDto>(newUser);
            returnedUser.User.Roles = await _userManager.GetRolesAsync(newUser);

            return returnedUser;
        }

        public async Task<UserLogedDto> LoginUserAsync(UserLoginDto userLoginDto)
        {
            var user = await _userManager.FindByEmailAsync(userLoginDto.Email);
            if(user == null || !(await _userManager.CheckPasswordAsync(user, userLoginDto.Password)))
            {
                throw new ArgumentException(AccountServiceConstants.WrongCredentials);
            }
            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = await _authenticationService.GetAccessTokenAsync(user, _jwtSettings);
            var userDto = _mapper.Map<UserDto>(user);
            userDto.Roles = roles;

            return new UserLogedDto
            {
                User = userDto,
                AccessToken = accessToken
            };
        }
    }
}
