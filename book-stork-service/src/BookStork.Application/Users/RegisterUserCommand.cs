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

public sealed record RegisterUserCommand(
    string Email, string FirstName, string LastName,
    string Password, int LoanLimit = 3) : IRequest<UserDto>;
 
public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8)
            .WithMessage("La contraseña debe tener al menos 8 caracteres.");
        RuleFor(x => x.LoanLimit).InclusiveBetween(1, 10);
    }
}
 
public sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserDto>
{
    private readonly IUserRepository _userRepo;
    private readonly IPasswordHasher _hasher;
    private readonly IDomainEventDispatcher _dispatcher;
    private readonly IMapper _mapper;
 
    public RegisterUserCommandHandler(IUserRepository userRepo, IPasswordHasher hasher,
        IDomainEventDispatcher dispatcher, IMapper mapper)
    { _userRepo = userRepo; _hasher = hasher; _dispatcher = dispatcher; _mapper = mapper; }
 
    public async Task<UserDto> Handle(RegisterUserCommand request, CancellationToken ct)
    {
        var email = Email.Create(request.Email);
 
        if (await _userRepo.ExistsByEmailAsync(email, ct))
            throw new ConflictException($"Ya existe un usuario con el email '{email}'.");
 
        var passwordHash = _hasher.Hash(request.Password);
        var user = User.Create(request.Email, request.FirstName, request.LastName, passwordHash, request.LoanLimit);
 
        await _userRepo.AddAsync(user, ct);
        await _userRepo.SaveChangesAsync(ct);
        await _dispatcher.DispatchAsync([user], ct);
 
        return _mapper.Map<UserDto>(user);
    }
}