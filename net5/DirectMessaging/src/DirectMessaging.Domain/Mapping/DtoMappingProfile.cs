using AutoMapper;
using DirectMessaging.Domain.Dtos;
using DirectMessaging.Domain.Models;

namespace DirectMessaging.Domain.Mapping;

public class DtoMappingProfile : Profile
{
    public DtoMappingProfile()
    {
        CreateMap<Message, MessageDto>();
    }
}