namespace SkillMill.Application.Customers.Dtos;

public record CustomerDto
{
    public int Id { get; init; }

    public string Name { get; init; } = null!;

    public string Email { get; init; } = null!;
}