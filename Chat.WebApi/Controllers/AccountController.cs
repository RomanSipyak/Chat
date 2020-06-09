using Chat.Contracts.Constats.ControllersConstants;
using Chat.Contracts.Dtos.User;
using Chat.Contracts.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly Logger _logger;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
            _logger = LogManager.GetCurrentClassLogger();
        }

        /// <summary>
        /// Registers new user
        /// </summary>
        /// <param name="registerDto">Instance of UserRegisterDto type with user`s info</param>
        /// <returns>An ActionResult of type UserDto</returns>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDto>> RegisterUser([FromForm] UserRegisterDto registerDto)
        {
            try
            {
                var registeredUser = await _accountService.RegisterUserAsync(registerDto);

                if (!registeredUser.Succeeded)
                {
                    return BadRequest($"{AccountControllerConstants.RegisterUser_ErrorMessage}: {registeredUser.Error}");
                }

                return Ok(registeredUser.User);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    AccountControllerConstants.RegisterUser_ErrorMessage);
            }
        }

        /// <summary>
        /// Login method for user
        /// </summary>
        /// <param name="userLoginDto">Instance of UserLoginDto type with user`s credentials</param>
        /// <returns>An ActionResult of type UserLogedDto with access token and user`s info</returns>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserLogedDto>> LoginUser([FromForm] UserLoginDto userLoginDto)
        {
            try
            {
                return Ok(await _accountService.LoginUserAsync(userLoginDto));
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, AccountControllerConstants.LoginUser_ErrorMessage);
            }
        }
    }
}
