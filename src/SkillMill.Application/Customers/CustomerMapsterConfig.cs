using Mapster;
using SkillMill.Application.Customers.Dtos;
using SkillMill.Domain.Entities;

namespace SkillMill.Application.Customers;

public class CustomerMapsterConfig
{
    public static void Configure()
    {
        TypeAdapterConfig<Customer, CustomerDetailsDto>.NewConfig();
        TypeAdapterConfig<Customer, CustomerDto>.NewConfig();

        TypeAdapterConfig<Order, OrderDto>.NewConfig()
            .Map(dest => dest.OrderSum, src => src.OrderItems.Sum(item => item.UnitPrice * item.Quantity));

        TypeAdapterConfig<OrderItem, OrderItemDto>.NewConfig();
        TypeAdapterConfig<Product, ProductDto>.NewConfig();
    }
}