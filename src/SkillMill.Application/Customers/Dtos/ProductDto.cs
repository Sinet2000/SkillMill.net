namespace SkillMill.Application.Customers.Dtos;

public class ProductDto
{
    public int Id { get; init; }

    public string Name { get; init; } = null!;

    public decimal Price { get; init; }
}