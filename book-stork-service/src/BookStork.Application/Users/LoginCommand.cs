using AutoMapper;
using BookStork.Application.DTOs;
using BookStork.Application.Ports;
using BookStork.Domain.Exceptions;
using BookStork.Domain.Repositories;
using BookStork.Domain.ValueObjects.User;
using FluentValidation;
using MediatR;

namespace BookStork.Application.Users;

public sealed record LoginCommand(string Email, string Password) : IRequest<AuthResponse>;
 
public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}
 
public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
{
    private readonly IUserRepository _userRepo;
    private readonly IPasswordHasher _hasher;
    private readonly IJwtService _jwtService;
    private readonly IMapper _mapper;
 
    public LoginCommandHandler(IUserRepository userRepo, IPasswordHasher hasher,
        IJwtService jwtService, IMapper mapper)
    { _userRepo = userRepo; _hasher = hasher; _jwtService = jwtService; _mapper = mapper; }
 
    public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken ct)
    {
        var email = Email.Create(request.Email);
        var user = await _userRepo.GetByEmailAsync(email, ct)
                   ?? throw new UnauthorizedException("Credenciales inválidas.");
 
        if (!_hasher.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedException("Credenciales inválidas.");
 
        if (user.Status == UserStatus.Suspended)
            throw new UnauthorizedException("La cuenta está suspendida.");
 
        if (user.Status == UserStatus.Expired)
            throw new UnauthorizedException("La cuenta no existe.");
 
        var token = _jwtService.GenerateToken(user.Id.Value, user.Email.Value, user.FullName);
 
        return new AuthResponse(
            AccessToken: token,
            TokenType: "Bearer",
            ExpiresAt: DateTime.UtcNow.AddHours(8),
            User: _mapper.Map<UserDto>(user));
    }
}