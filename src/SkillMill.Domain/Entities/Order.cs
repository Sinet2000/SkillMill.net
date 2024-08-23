using System.ComponentModel.DataAnnotations;

namespace SkillMill.Domain.Entities;

public class Order : BaseEntity
{
    /// <summary>
    /// CTOR to init Bogus.Faker.
    /// </summary>
    public Order()
    {
    }

    private readonly List<OrderItem> _orderItems = [];

    public DateTime OrderDate { get; private set; }

    [Range(1, int.MaxValue)]
    public int CustomerId { get; private set; }

    public Customer Customer { get; private set; } = null!;

    public IEnumerable<OrderItem> OrderItems => _orderItems.AsReadOnly();

    public void AddOrderItems(IEnumerable<OrderItem> orderItems)
    {
        _orderItems.AddRange(orderItems);
    }

    public void UpdateDate(DateTime newOrderDate)
    {
        OrderDate = newOrderDate;
    }
}