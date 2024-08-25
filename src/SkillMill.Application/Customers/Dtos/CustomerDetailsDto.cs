using AutoMapper;
using SkillMill.Domain.Entities;

namespace SkillMill.Application.Customers.Dtos;

[AutoMap(typeof(Customer))]
public record CustomerDetailsDto
{
    public int Id { get; init; }

    public string Name { get; init; } = null!;

    public string Email { get; init; } = null!;

    public IEnumerable<OrderDto>? Orders { get; init; }
}