using AutoMapper;
using BookStork.Application.DTOs;
using BookStork.Domain.Entities;
using BookStork.Domain.Exceptions;
using BookStork.Domain.Repositories;
using BookStork.Domain.ValueObjects.Book;
using FluentValidation;
using MediatR;

namespace BookStork.Application.Books.Commands;

public sealed record SetUserBookStatusCommand(Guid UserId, Guid BookId, string Status) : IRequest<UserBookStatusDto>;

public sealed class SetUserBookStatusCommandValidator : AbstractValidator<SetUserBookStatusCommand>
{
    public SetUserBookStatusCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.BookId).NotEmpty();
        RuleFor(x => x.Status).NotEmpty().Must(s =>
                new[] { "WISHLIST", "READING", "COMPLETED" }.Contains(s.ToUpper()))
            .WithMessage("Status must be: WISHLIST, READING o COMPLETED.");
    }
}

public sealed class SetUserBookStatusCommandHandler : IRequestHandler<SetUserBookStatusCommand, UserBookStatusDto>
{
    private readonly IUserBookStatusRepository _repo;
    private readonly IBookRepository _bookRepo;
    private readonly IMapper _mapper;

    public SetUserBookStatusCommandHandler(IUserBookStatusRepository repo, IBookRepository bookRepo, IMapper mapper)
    { _repo = repo; _bookRepo = bookRepo; _mapper = mapper; }

    public async Task<UserBookStatusDto> Handle(SetUserBookStatusCommand r, CancellationToken ct)
    {
        var userId = Domain.ValueObjects.User.UserId.Create(r.UserId);
        var bookId = BookId.From(r.BookId);

        var book = await _bookRepo.GetByIdAsync(bookId, ct)
                   ?? throw new NotFoundException(nameof(Book), r.BookId);

        var existing = await _repo.GetAsync(userId, bookId, ct);

        if (existing is null)
        {
            var status = UserBookStatus.Create(userId, bookId, r.Status);
            await _repo.AddAsync(status, ct);
            await _repo.SaveChangesAsync(ct);
            return _mapper.Map<UserBookStatusDto>(status);
        }

        existing.ChangeStatus(r.Status);
        _repo.Update(existing);
        await _repo.SaveChangesAsync(ct);
        return _mapper.Map<UserBookStatusDto>(existing);
    }
}