using AutoMapper;
using BookStork.Application.DTOs;
using BookStork.Domain.Entities;
using BookStork.Domain.Exceptions;
using BookStork.Domain.Repositories;
using BookStork.Domain.ValueObjects.User;
using FluentValidation;
using MediatR;

namespace BookStork.Application.Users;

public sealed record GetUserByIdQuery(Guid UserId) : IRequest<UserDto>;
 
public sealed class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdQueryValidator() { RuleFor(x => x.UserId).NotEmpty(); }
}
 
public sealed class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
{
    private readonly IUserRepository _repo;
    private readonly IMapper _mapper;
 
    public GetUserByIdQueryHandler(IUserRepository repo, IMapper mapper)
    { _repo = repo; _mapper = mapper; }
 
    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken ct)
    {
        var user = await _repo.GetByIdAsync(UserId.Create(request.UserId), ct)
                   ?? throw new NotFoundException(nameof(User), request.UserId);
        return _mapper.Map<UserDto>(user);
    }
}
