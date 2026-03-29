namespace BookStork.Domain.Exceptions;

public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
    
    public DomainException(string message, Exception innerException) : base(message, innerException) { }
    
}

public class NotFoundException : DomainException
{
    public NotFoundException(string bookName, Guid idBook) 
        : base($"{bookName} with ID {idBook} not found.") { }
}

public class ConflictException : DomainException
{
    public ConflictException(string message) : base(message) { }
}
