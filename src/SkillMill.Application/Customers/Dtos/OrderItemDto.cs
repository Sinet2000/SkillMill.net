using AutoMapper;
using SkillMill.Domain.Entities;

namespace SkillMill.Application.Customers.Dtos;

[AutoMap(typeof(OrderItem))]
public record OrderItemDto
{
    public int Id { get; init; }

    public int Quantity { get; init; }

    public decimal UnitPrice { get; init; }

    public int ProductId { get; init; }

    public ProductDto? Product { get; init; }
}