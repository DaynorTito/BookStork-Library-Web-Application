using AutoMapper;
using BookStork.Application.DTOs;
using BookStork.Domain.Repositories;
using MediatR;

namespace BookStork.Application.Loans;

public sealed record GetAllLoansQuery(int Page = 1, int PageSize = 10) : IRequest<PagedResult<LoanDto>>;
 
public sealed class GetAllLoansQueryHandler : IRequestHandler<GetAllLoansQuery, PagedResult<LoanDto>>
{
    private readonly ILoanQueryService _query;
    public GetAllLoansQueryHandler(ILoanQueryService query) => _query = query;

    public async Task<PagedResult<LoanDto>> Handle(GetAllLoansQuery r, CancellationToken ct)
    {
        var (items, total) = await _query.GetAllAsync(r.Page, r.PageSize, ct);
        return new PagedResult<LoanDto>(items, r.Page, r.PageSize, total);
    }
}
