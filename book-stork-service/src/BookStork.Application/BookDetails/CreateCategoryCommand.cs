using AutoMapper;
using BookStork.Application.DTOs;
using BookStork.Domain.Entities;
using BookStork.Domain.Repositories;
using MediatR;

namespace BookStork.Application.BookDetails;

public sealed record CreateCategoryCommand(string Name, string? Description) : IRequest<CategoryDto>;
public sealed class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryDto>
{
    private readonly ICategoryRepository _repo; private readonly IMapper _mapper;
    public CreateCategoryCommandHandler(ICategoryRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
    public async Task<CategoryDto> Handle(CreateCategoryCommand r, CancellationToken ct)
    {
        var cat = Category.Create(r.Name, r.Description);
        await _repo.AddAsync(cat, ct); await _repo.SaveChangesAsync(ct);
        return _mapper.Map<CategoryDto>(cat);
    }
}
