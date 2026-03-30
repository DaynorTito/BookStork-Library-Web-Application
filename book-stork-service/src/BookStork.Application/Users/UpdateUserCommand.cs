using AutoMapper;
using BookStork.Application.DTOs;
using BookStork.Application.Ports;
using BookStork.Domain.Entities;
using BookStork.Domain.Exceptions;
using BookStork.Domain.Repositories;
using BookStork.Domain.ValueObjects.User;
using FluentValidation;
using MediatR;

namespace BookStork.Application.Users;

public sealed record UpdateUserCommand(
    Guid UserId, string Email, string FirstName,
    string LastName, string NotificationPreference) : IRequest<UserDto>;
 
public sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
    }
}
 
public sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
{
    private readonly IUserRepository _userRepo;
    private readonly IDomainEventDispatcher _dispatcher;
    private readonly IMapper _mapper;
 
    public UpdateUserCommandHandler(IUserRepository userRepo, IDomainEventDispatcher dispatcher, IMapper mapper)
    { _userRepo = userRepo; _dispatcher = dispatcher; _mapper = mapper; }
 
    public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken ct)
    {
        var userId = UserId.Create(request.UserId);
        var user = await _userRepo.GetByIdAsync(userId, ct)
                   ?? throw new NotFoundException(nameof(User), request.UserId);
 
        var newEmail = Email.Create(request.Email);
        if (user.Email != newEmail && await _userRepo.ExistsByEmailAsync(newEmail, ct))
            throw new ConflictException($"Ya existe otro usuario con el email '{newEmail}'.");
 
        user.UpdateProfile(request.FirstName, request.LastName, request.Email, request.NotificationPreference);
 
        _userRepo.Update(user);
        await _userRepo.SaveChangesAsync(ct);
        await _dispatcher.DispatchAsync([user], ct);
 
        return _mapper.Map<UserDto>(user);
    }
}
