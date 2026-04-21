using AutoMapper;
using BookStork.Application.DTOs;
using BookStork.Application.Ports;
using BookStork.Domain.Repositories;
using BookStork.Domain.ValueObjects.User;
using MediatR;

namespace BookStork.Application.Loans;

public sealed record GetMyLoansQuery(int Page = 1, int PageSize = 10)
    : IRequest<PagedResult<LoanDto>>;

public sealed class GetMyLoansQueryHandler : IRequestHandler<GetMyLoansQuery, PagedResult<LoanDto>>
{
    private readonly ILoanQueryService _query;
    private readonly ICurrentUserService _currentUser;
    public GetMyLoansQueryHandler(ILoanQueryService query, ICurrentUserService currentUser)
        => (_query, _currentUser) = (query, currentUser);

    public async Task<PagedResult<LoanDto>> Handle(GetMyLoansQuery r, CancellationToken ct)
    {
        var userId = _currentUser.GetCurrentUserId();
        var (items, total) = await _query.GetByUserAsync(userId, r.Page, r.PageSize, ct);
        return new PagedResult<LoanDto>(items, r.Page, r.PageSize, total);
    }
}
