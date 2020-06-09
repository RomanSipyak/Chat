using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Chat.Contracts.Constats.ControllersConstants;
using Chat.Contracts.Dtos;
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

        /// <summary>
        /// Send Message to chat 
        /// </summary>
        /// <param name="sendMessageDto">Contains all information about chat where we want to send our message</param>
        /// <returns>Return status about condition of sendToChatAction</returns>
        [HttpPost("SendMessageToChat")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> SendMessageToChat(SendMessageToChatDto sendMessageDto)
        {
            try
            {
                var userId = this.User.Claims.First(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                await _messagesService.SendMessageToChatAsync(sendMessageDto, userId);
                return Ok(MessageControllerConstants.YouMessageSenddedWithSuccess);
            } catch (Exception ex)
            {
                _logger.Error(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Send Message to user 
        /// </summary>
        /// <param name="sendMessageToUserDto">Contains all information about where we want to send and hom we want to send our message</param>
        /// <returns>Return status about condition of sendToUserAction</returns>
        [HttpPost("SendMessageToUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> SendMessageToUser(SendMessageToUserDto sendMessageToUserDto)
        {
            try
            {
                var userId = this.User.Claims.First(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                await _messagesService.SendMessageToUserAsync(sendMessageToUserDto, userId);
                return Ok(MessageControllerConstants.YouMessageSenddedWithSuccess);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Delete message If It is possible
        /// </summary>
        /// <param name="deleteMessageDto">Contains Id and options for deleteing our message</param>
        /// <returns>Return status about condition of deleteAction.</returns>
        [HttpDelete("DeleteMessage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> DeleteMessage(DeleteMessageDto deleteMessageDto)
        {
            try
            {
                var userId = this.User.Claims.First(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                await _messagesService.DeleteMessage(deleteMessageDto, userId);
                return Ok(MessageControllerConstants.YouMessageIsDeleted);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Update message if it is possible
        /// </summary>
        /// <param name="updateMessageDto">This Dto contains All Information about messsage that we want to update</param>
        /// <returns>Return status 200 if your message is updated</returns>
        [HttpPatch("UpdateMessage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> UpdateMessage(UpdateMessageDto updateMessageDto)
        {
            try
            {
                var userId = this.User.Claims.First(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                await _messagesService.UpdateMessage(updateMessageDto, userId);
                return Ok(MessageControllerConstants.YouMessageIsUpdated);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Reply for message chat.
        /// </summary>
        /// <param name="replyMessageDto">Dto for reply message</param>
        /// <returns>ActionResult with staus 200 if all good</returns>
        [HttpPost("ReplyOnMessage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> ReplyForMessage(ReplyMessageDto replyMessageDto)
        {
            try
            {
                var userId = this.User.Claims.First(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                await _messagesService.ReplyMessage(replyMessageDto, userId);
                return Ok(MessageControllerConstants.YourReplySended);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Get all messages for chat 
        /// </summary>
        /// <param name="ChatId">This is Id Chat with messages</param>
        /// <param name="pagination">This is filter with count of elements and number of page</param>
        /// <returns>GetMessageDto that contain information about our messages</returns>
        [HttpGet("GetMessagesForChat")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<ICollection<GetMessageDto>>> GetMessagesForChat(int ChatId, [FromQuery] PaginationFilterDto pagination)
        {
            try
            {
                var userId = this.User.Claims.First(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;

                return Ok(await _messagesService.GetAllMessagesForChat(ChatId,userId, pagination));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}