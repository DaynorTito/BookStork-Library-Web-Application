using AutoMapper;
using BookStork.Application.DTOs;
using BookStork.Domain.Entities;

namespace BookStork.Application.Common.Mappings;

public sealed class ReservationMappingProfile : Profile
{
    public ReservationMappingProfile()
    {
        CreateMap<Reservation, ReservationDto>()
            .ForMember(d => d.Id, o => o.MapFrom(s => s.Id.Value))
            .ForMember(d => d.UserId, o => o.MapFrom(s => s.UserId.Value))
            .ForMember(d => d.Book, o => o.Ignore())
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.Value))
            .ForMember(d => d.UserFullName, o => o.Ignore());
    }
}
