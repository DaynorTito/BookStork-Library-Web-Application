using AutoMapper;
using BookStork.Application.DTOs.Book;
using BookStork.Domain.Entities;

namespace BookStork.Application.Common.Mappings;

public sealed class BookMappingProfile : Profile
{
    public BookMappingProfile()
    {
        CreateMap<Book, ListBookDTO>()
            .ForMember(dest => dest.Id,            opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Height,        opt => opt.MapFrom(src => src.Dimensions.Height))
            .ForMember(dest => dest.Weight,        opt => opt.MapFrom(src => src.Dimensions.Weight))
            .ForMember(dest => dest.Thickness,     opt => opt.MapFrom(src => src.Dimensions.Thickness))
            .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src => src.Metadata.AverageRating))
            .ForMember(dest => dest.Language,      opt => opt.MapFrom(src => src.Metadata.Language))
            .ForMember(dest => dest.Images,        opt => opt.MapFrom(src => src.Images.Select(i => i.Url).ToList()));
 
        CreateMap<Book, BookDetailDTO>()
            .ForMember(dest => dest.Id,            opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Height,        opt => opt.MapFrom(src => src.Dimensions.Height))
            .ForMember(dest => dest.Weight,        opt => opt.MapFrom(src => src.Dimensions.Weight))
            .ForMember(dest => dest.Thickness,     opt => opt.MapFrom(src => src.Dimensions.Thickness))
            .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src => src.Metadata.AverageRating))
            .ForMember(dest => dest.Language,      opt => opt.MapFrom(src => src.Metadata.Language))
            .ForMember(dest => dest.Images,        opt => opt.MapFrom(src => src.Images.Select(i => i.Url).ToList()));
    }
}
