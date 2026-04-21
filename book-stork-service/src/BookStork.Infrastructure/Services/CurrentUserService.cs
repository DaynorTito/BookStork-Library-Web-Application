using System.Security.Claims;
using BookStork.Application.Ports;
using BookStork.Domain.Exceptions;
using Microsoft.AspNetCore.Http;

namespace BookStork.Infrastructure.Services;

public sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        => _httpContextAccessor = httpContextAccessor;

    public bool IsAuthenticated
        => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    public Guid GetCurrentUserId()
    {
        var id = TryGetCurrentUserId();
        if (id is null)
            throw new UnauthorizedException("The current user is not authenticated.");
        return id.Value;
    }

    public Guid? TryGetCurrentUserId()
    {
        var sub = _httpContextAccessor.HttpContext?
                      .User?
                      .FindFirstValue(ClaimTypes.NameIdentifier)
                  ?? _httpContextAccessor.HttpContext?
                      .User?
                      .FindFirstValue("sub");

        return Guid.TryParse(sub, out var userId) ? userId : null;
    }
}
