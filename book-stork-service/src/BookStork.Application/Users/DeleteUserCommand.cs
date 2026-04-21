using BookStork.Application.Ports;
using BookStork.Domain.Entities;
using BookStork.Domain.Exceptions;
using BookStork.Domain.Repositories;
using BookStork.Domain.ValueObjects.User;
using FluentValidation;
using MediatR;

namespace BookStork.Application.Users;

public sealed record DeleteUserCommand(Guid UserId) : IRequest;
 
public sealed class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator() { RuleFor(x => x.UserId).NotEmpty(); }
}
 
public sealed class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly IUserRepository _userRepo;
    private readonly IDomainEventDispatcher _dispatcher;
 
    public DeleteUserCommandHandler(IUserRepository userRepo, IDomainEventDispatcher dispatcher)
    { _userRepo = userRepo; _dispatcher = dispatcher; }
 
    public async Task Handle(DeleteUserCommand request, CancellationToken ct)
    {
        var userId = UserId.Create(request.UserId);
        var user = await _userRepo.GetByIdAsync(userId, ct)
                   ?? throw new NotFoundException(nameof(User), request.UserId);
 
        user.Suspend();
        _userRepo.Update(user);
        await _userRepo.SaveChangesAsync(ct);
        await _dispatcher.DispatchAsync([user], ct);
    }
}
