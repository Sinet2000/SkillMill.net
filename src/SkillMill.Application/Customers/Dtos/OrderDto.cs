namespace SkillMill.Application.Customers.Dtos;

public record OrderDto
{
    public int Id { get; init; }

    public DateTime OrderDate { get; init; }

    public int CustomerId { get; init; }

    public decimal OrderSum { get; init; }

    public IEnumerable<OrderItemDto>? OrderItems { get; init; }
}