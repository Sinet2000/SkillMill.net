using AutoMapper;
using SkillMill.Domain.Entities;

namespace SkillMill.Application.Customers.Dtos;

// [AutoMap(typeof(Order))]
public record OrderDto
{
    public int Id { get; init; }

    public DateTime OrderDate { get; init; }

    public int CustomerId { get; init; }

    public decimal OrderSum { get; init; }

    public IEnumerable<OrderItemDto>? OrderItems { get; init; }
}