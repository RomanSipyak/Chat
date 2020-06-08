using AutoMapper;
using Chat.Contracts.Dtos.Message;
using Chat.Contracts.Dtos.User;
using Chat.Contracts.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.BusinessLogic.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserRegisterDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Message, GetMessageDto>().ReverseMap();
        }
    }
}
