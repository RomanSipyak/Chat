using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Contracts.Constats.ControllersConstants;
using Chat.Contracts.Dtos.Chat;
using Chat.Contracts.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace Chat.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : Controller
    {
        private readonly Logger _logger;
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
            _logger = LogManager.GetCurrentClassLogger();
        }

        /// <summary>
        /// This method for add authenticated user to public chat 
        /// </summary>
        /// <param name="connectChatDto">Consist of Title and Id Chat. Id is required parameter</param>
        /// <returns>Status</returns>
        [HttpPost("AddAuthenticatedUserToChat")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ConnectToChat([FromBody] ConnectChatDto connectChatDto)
        {
            var userId = this.User.Claims.First(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            try
            {
                await _chatService.AddUserToChatAsync(connectChatDto, userId);
                return Ok(ChatControllerConstants.YouAreAddedToChat);
            } catch (Exception ex)
            {
                _logger.Error(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.InnerException.Message);
            }
        }

        /// <summary>
        /// Get All chats for User
        /// </summary>
        /// <returns>Collection of GetChatDto</returns>
        [HttpGet("GetAllChatsForUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<ICollection<GetChatDto>>> GetAllChatsForUser()
        {
            var userId = this.User.Claims.First(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            try
            {
                return Ok(await _chatService.GetAllChatsForUserAsync(userId));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.InnerException.Message);
            }
        }
    }
}