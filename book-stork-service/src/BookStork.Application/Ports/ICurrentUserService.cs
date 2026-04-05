namespace BookStork.Application.Ports;

public interface ICurrentUserService
{

    Guid GetCurrentUserId();
    
    Guid? TryGetCurrentUserId();
 
    bool IsAuthenticated { get; }
}