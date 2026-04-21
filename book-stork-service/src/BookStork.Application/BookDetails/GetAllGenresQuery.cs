using AutoMapper;
using BookStork.Application.DTOs;
using BookStork.Domain.Repositories;
using MediatR;

namespace BookStork.Application.BookDetails;

public sealed record GetAllGenresQuery : IRequest<IReadOnlyList<GenreDto>>;
public sealed class GetAllGenresQueryHandler : IRequestHandler<GetAllGenresQuery, IReadOnlyList<GenreDto>>
{
    private readonly IGenreRepository _repo; private readonly IMapper _mapper;
    public GetAllGenresQueryHandler(IGenreRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
    public async Task<IReadOnlyList<GenreDto>> Handle(GetAllGenresQuery r, CancellationToken ct)
        => _mapper.Map<IReadOnlyList<GenreDto>>(await _repo.GetAllAsync(ct));
}
