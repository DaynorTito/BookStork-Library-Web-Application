using BookStork.Domain.Exceptions;

namespace BookStork.Domain.ValueObjects.Book;

public class BookDimensions : ValueObject
{
    public decimal Height { get; }
    public decimal Weight { get; }
    public decimal Thickness { get; }
    
    public BookDimensions(decimal height, decimal weight, decimal thickness)
    {
        if (height <= 0) throw new DomainException("Height can not be less than zero");
        if (weight <= 0) throw new DomainException("Weight can not be less than zero");
        if (thickness <= 0) throw new DomainException("Thickness can not be less than zero");
        
        Height = height;
        Weight = weight;
        Thickness = thickness;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Height;
        yield return Weight;
        yield return Thickness;
    }
}
