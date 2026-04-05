using AutoMapper;
using BookStork.Application.DTOs;
using BookStork.Domain.Repositories;
using MediatR;

namespace BookStork.Application.BookDetails;

public sealed record GetAllCategoriesQuery : IRequest<IReadOnlyList<CategoryDto>>;
public sealed class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, IReadOnlyList<CategoryDto>>
{
    private readonly ICategoryRepository _repo; private readonly IMapper _mapper;
    public GetAllCategoriesQueryHandler(ICategoryRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
    public async Task<IReadOnlyList<CategoryDto>> Handle(GetAllCategoriesQuery r, CancellationToken ct)
        => _mapper.Map<IReadOnlyList<CategoryDto>>(await _repo.GetAllAsync(ct));
}