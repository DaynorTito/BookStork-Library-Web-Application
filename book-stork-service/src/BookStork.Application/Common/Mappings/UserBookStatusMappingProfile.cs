using AutoMapper;
using BookStork.Application.DTOs;
using BookStork.Domain.Entities;

namespace BookStork.Application.Common.Mappings;


public sealed class UserBookStatusMappingProfile : Profile
{
    public UserBookStatusMappingProfile()
    {
        CreateMap<UserBookStatus, UserBookStatusDto>()
            .ForMember(d => d.UserId,    o => o.MapFrom(s => s.UserId.Value))
            .ForMember(d => d.BookId,    o => o.MapFrom(s => s.BookId.Value))
            .ForMember(d => d.Status,    o => o.MapFrom(s => s.Status.Value))
            .ForMember(d => d.BookTitle, o => o.Ignore());
    }
}