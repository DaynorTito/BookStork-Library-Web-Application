using AutoMapper;
using BookStork.Application.DTOs;
using BookStork.Domain.Entities;

namespace BookStork.Application.Common.Mappings;

public sealed class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForCtorParam("Id", o => o.MapFrom(s => s.Id.Value))
            .ForCtorParam("Email", o => o.MapFrom(s => s.Email.Value))
            .ForCtorParam("FirstName", o => o.MapFrom(s => s.FirstName))
            .ForCtorParam("LastName", o => o.MapFrom(s => s.LastName))
            .ForCtorParam("FullName", o => o.MapFrom(s => s.FullName))
            .ForCtorParam("Status", o => o.MapFrom(s => s.Status.Value))
            .ForCtorParam("LoanLimit", o => o.MapFrom(s => s.LoanLimit.Value))
            .ForCtorParam("ActiveLoansCount", o => o.MapFrom(s => s.ActiveLoans.Count))
            .ForCtorParam("NotificationPreference", o => o.MapFrom(s => s.NotificationPreference.Value))
            .ForCtorParam("CreatedAt", o => o.MapFrom(s => s.CreatedAt))
            .ForCtorParam("UpdatedAt", o => o.MapFrom(s => s.UpdatedAt));
    }
}