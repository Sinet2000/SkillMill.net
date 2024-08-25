using AutoMapper;
using SkillMill.Application.Customers.Dtos;
using SkillMill.Domain.Entities;

namespace SkillMill.Application.Customers;

public class CustomerMappingProfile : Profile
{
    public CustomerMappingProfile()
    {
        CreateMap<Customer, CustomerDetailsDto>();
        CreateMap<Customer, CustomerDto>();

        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.OrderSum, opt => opt.MapFrom<OrderSumResolver>());

        CreateMap<OrderItem, OrderItemDto>();
        CreateMap<Product, ProductDto>();
    }

    private class OrderSumResolver : IValueResolver<Order, OrderDto, decimal>
    {
        public decimal Resolve(Order source, OrderDto destination, decimal destMember, ResolutionContext context)
        {
            return source.OrderItems.Sum(item => item.UnitPrice * item.Quantity);
        }
    }
}