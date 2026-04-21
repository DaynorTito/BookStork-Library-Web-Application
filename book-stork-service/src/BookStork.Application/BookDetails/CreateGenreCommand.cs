using AutoMapper;
using BookStork.Application.DTOs;
using BookStork.Domain.Entities;
using BookStork.Domain.Repositories;
using MediatR;

namespace BookStork.Application.BookDetails;

public sealed record CreateGenreCommand(string Name) : IRequest<GenreDto>;
public sealed class CreateGenreCommandHandler : IRequestHandler<CreateGenreCommand, GenreDto>
{
    private readonly IGenreRepository _repo; private readonly IMapper _mapper;
    public CreateGenreCommandHandler(IGenreRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
    public async Task<GenreDto> Handle(CreateGenreCommand r, CancellationToken ct)
    {
        var genre = Genre.Create(r.Name);
        await _repo.AddAsync(genre, ct); await _repo.SaveChangesAsync(ct);
        return _mapper.Map<GenreDto>(genre);
    }
}
