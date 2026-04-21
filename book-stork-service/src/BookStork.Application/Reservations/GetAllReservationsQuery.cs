using AutoMapper;
using BookStork.Application.DTOs;
using BookStork.Domain.Repositories;
using MediatR;

namespace BookStork.Application.Reservations;

public sealed record GetAllReservationsQuery(int Page = 1, int PageSize = 10) : IRequest<PagedResult<ReservationDto>>;
 
public sealed class GetAllReservationsQueryHandler : IRequestHandler<GetAllReservationsQuery, PagedResult<ReservationDto>>
{
    private readonly IReservationRepository _repo;
    private readonly IMapper _mapper;
 
    public GetAllReservationsQueryHandler(IReservationRepository repo, IMapper mapper)
    { _repo = repo; _mapper = mapper; }
 
    public async Task<PagedResult<ReservationDto>> Handle(GetAllReservationsQuery request, CancellationToken ct)
    {
        var (items, total) = await _repo.GetAllAsync(request.Page, request.PageSize, ct);
        return new PagedResult<ReservationDto>(_mapper.Map<IReadOnlyList<ReservationDto>>(items), request.Page, request.PageSize, total);
    }
}