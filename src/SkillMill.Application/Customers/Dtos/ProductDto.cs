using AutoMapper;
using SkillMill.Domain.Entities;

namespace SkillMill.Application.Customers.Dtos;

[AutoMap(typeof(Product))]
public class ProductDto
{
    public int Id { get; init; }

    public string Name { get; init; } = null!;

    public decimal Price { get; init; }
}