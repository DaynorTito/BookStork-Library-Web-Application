using AutoMapper;
using BookStork.Application.DTOs;
using BookStork.Domain.Repositories;
using MediatR;

namespace BookStork.Application.Users;

public sealed record GetAllUsersQuery(int Page = 1, int PageSize = 10) : IRequest<PagedResult<UserDto>>;
 
public sealed class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, PagedResult<UserDto>>
{
    private readonly IUserRepository _repo;
    private readonly IMapper _mapper;
 
    public GetAllUsersQueryHandler(IUserRepository repo, IMapper mapper)
    { _repo = repo; _mapper = mapper; }
 
    public async Task<PagedResult<UserDto>> Handle(GetAllUsersQuery request, CancellationToken ct)
    {
        var (users, total) = await _repo.GetAllAsync(request.Page, request.PageSize, ct);
        return new PagedResult<UserDto>(_mapper.Map<IReadOnlyList<UserDto>>(users), request.Page, request.PageSize, total);
    }
}