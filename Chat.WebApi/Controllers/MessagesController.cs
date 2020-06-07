using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Chat.Contracts.Dtos.Message;
using Chat.Contracts.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.Extensions.Logging;
using NLog;

namespace Chat.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : Controller
    {
        private readonly Logger _logger;
        private readonly IMessagesService _messagesService;
        private readonly IMapper _mapper;

        public MessagesController(IMessagesService messagesService, IMapper mapper)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _messagesService = messagesService;
            _mapper = mapper;
        }

        [HttpPost("SendMessageToChat")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> SendMessageToChat(SendMessageToChatDto sendMessageDto)
        {
            try
            {
                var userId = this.User.Claims.First(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                await _messagesService.SendMessageToChatAsync(sendMessageDto, userId);
                return Ok("Your Message succes send");
            }catch(Exception ex)
            {
                _logger.Error(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("SendMessageToUser")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> SendMessageToUser(SendMessageToUserDto sendMessageToUserDto)
        {
            try
            {
                var userId = this.User.Claims.First(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                await _messagesService.SendMessageToUserAsync(sendMessageToUserDto, userId);
                return Ok("Your Message succes send");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}