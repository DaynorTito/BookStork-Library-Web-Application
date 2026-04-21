using AutoMapper;
using BookStork.Application.DTOs;
using BookStork.Domain.Repositories;
using MediatR;

namespace BookStork.Application.BookDetails;

public sealed record GetAllAuthorsQuery : IRequest<IReadOnlyList<AuthorDto>>;
public sealed class GetAllAuthorsQueryHandler : IRequestHandler<GetAllAuthorsQuery, IReadOnlyList<AuthorDto>>
{
    private readonly IAuthorRepository _repo; private readonly IMapper _mapper;
    public GetAllAuthorsQueryHandler(IAuthorRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
    public async Task<IReadOnlyList<AuthorDto>> Handle(GetAllAuthorsQuery r, CancellationToken ct)
        => _mapper.Map<IReadOnlyList<AuthorDto>>(await _repo.GetAllAsync(ct));
}
