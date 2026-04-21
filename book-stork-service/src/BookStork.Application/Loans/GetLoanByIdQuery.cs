using AutoMapper;
using BookStork.Application.DTOs;
using BookStork.Domain.Entities;
using BookStork.Domain.Exceptions;
using BookStork.Domain.Repositories;
using BookStork.Domain.ValueObjects.Loan;
using MediatR;

namespace BookStork.Application.Loans;

public sealed record GetLoanByIdQuery(Guid LoanId) : IRequest<LoanDto>;
 
public sealed class GetLoanByIdQueryHandler : IRequestHandler<GetLoanByIdQuery, LoanDto>
{
    private readonly ILoanRepository _repo;
    private readonly IMapper _mapper;
 
    public GetLoanByIdQueryHandler(ILoanRepository repo, IMapper mapper)
    { _repo = repo; _mapper = mapper; }
 
    public async Task<LoanDto> Handle(GetLoanByIdQuery request, CancellationToken ct)
    {
        var loan = await _repo.GetByIdAsync(LoanId.Create(request.LoanId), ct)
                   ?? throw new NotFoundException(nameof(Loan), request.LoanId);
        return _mapper.Map<LoanDto>(loan);
    }
}
