using AutoMapper;
using BookStork.Application.DTOs;
using BookStork.Application.DTOs.Book;
using BookStork.Domain.Entities;

namespace BookStork.Application.Common.Mappings;

public sealed class BookMappingProfile : Profile
{
    public BookMappingProfile()
    {
        CreateMap<Author, AuthorDto>()
            .ForMember(d => d.Id, o => o.MapFrom(s => s.Id.Value));

        CreateMap<Genre, GenreDto>()
            .ForMember(d => d.Id, o => o.MapFrom(s => s.Id.Value));

        CreateMap<Category, CategoryDto>()
            .ForMember(d => d.Id, o => o.MapFrom(s => s.Id.Value));

        CreateMap<Book, BookListDto>()
            .ForMember(d => d.Id,            o => o.MapFrom(s => s.Id.Value))
            .ForMember(d => d.AuthorName,    o => o.Ignore())
            .ForMember(d => d.CategoryName,  o => o.Ignore())
            .ForMember(d => d.Genres,        o => o.Ignore())
            .ForMember(d => d.Status,        o => o.MapFrom(s => s.Status.Value))
            .ForMember(d => d.Language,      o => o.MapFrom(s => s.Metadata.Language))
            .ForMember(d => d.AverageRating, o => o.MapFrom(s => s.Metadata.AverageRating))
            .ForMember(d => d.CoverImageUrl, o => o.MapFrom(s =>
                s.Images.Count > 0 ? s.Images[0].Url : null));


    }
}
