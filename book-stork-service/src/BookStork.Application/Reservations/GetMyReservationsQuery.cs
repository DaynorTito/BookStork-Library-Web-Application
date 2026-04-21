using AutoMapper;
using BookStork.Application.DTOs;
using BookStork.Application.Ports;
using BookStork.Domain.Repositories;
using BookStork.Domain.ValueObjects.User;
using MediatR;

namespace BookStork.Application.Reservations;

public sealed record GetMyReservationsQuery(int Page = 1, int PageSize = 10)
    : IRequest<PagedResult<ReservationDto>>;
 
public sealed class GetMyReservationsQueryHandler : IRequestHandler<GetMyReservationsQuery, PagedResult<ReservationDto>>
{
    private readonly IReservationQueryService _query;
    private readonly ICurrentUserService _currentUser;
    public GetMyReservationsQueryHandler(IReservationQueryService query, ICurrentUserService currentUser)
        => (_query, _currentUser) = (query, currentUser);

    public async Task<PagedResult<ReservationDto>> Handle(GetMyReservationsQuery r, CancellationToken ct)
    {
        var userId = _currentUser.GetCurrentUserId();
        var (items, total) = await _query.GetByUserAsync(userId, r.Page, r.PageSize, ct);
        return new PagedResult<ReservationDto>(items, r.Page, r.PageSize, total);
    }
}