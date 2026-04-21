using AutoMapper;
using BookStork.Application.DTOs;
using BookStork.Domain.Entities;
using BookStork.Domain.Repositories;
using MediatR;

namespace BookStork.Application.BookDetails;

public sealed record CreateAuthorCommand(string Name, string? Biography) : IRequest<AuthorDto>;
public sealed class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, AuthorDto>
{
    private readonly IAuthorRepository _repo; private readonly IMapper _mapper;
    public CreateAuthorCommandHandler(IAuthorRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
    public async Task<AuthorDto> Handle(CreateAuthorCommand r, CancellationToken ct)
    {
        var author = Author.Create(r.Name, r.Biography);
        await _repo.AddAsync(author, ct); await _repo.SaveChangesAsync(ct);
        return _mapper.Map<AuthorDto>(author);
    }
}
