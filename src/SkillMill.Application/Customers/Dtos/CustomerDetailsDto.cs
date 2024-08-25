namespace SkillMill.Application.Customers.Dtos;

public record CustomerDetailsDto
{
    public int Id { get; init; }

    public string Name { get; init; } = null!;

    public string Email { get; init; } = null!;

    public IEnumerable<OrderDto>? Orders { get; init; }
}