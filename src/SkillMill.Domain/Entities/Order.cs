using System.ComponentModel.DataAnnotations;
using Sieve.Attributes;

namespace SkillMill.Domain.Entities;

public class Order : BaseEntity, ICloneable
{
    /// <summary>
    /// CTOR to init Bogus.Faker.
    /// </summary>
    public Order()
    {
    }

    private readonly List<OrderItem> _orderItems = [];

    [Sieve(CanFilter = true, CanSort = true)]
    public DateTime OrderDate { get; private set; }

    [Range(1, int.MaxValue)]
    public int CustomerId { get; private set; }

    public Customer Customer { get; private set; } = null!;

    public IEnumerable<OrderItem> OrderItems => _orderItems.AsReadOnly();

    public void AddOrderItems(IEnumerable<OrderItem> orderItems)
    {
        _orderItems.AddRange(orderItems);
    }

    public void SetOrderItems(IEnumerable<OrderItem> orderItems)
    {
        _orderItems.Clear();
        _orderItems.AddRange(orderItems);
    }

    public void UpdateDate(DateTime newOrderDate)
    {
        OrderDate = newOrderDate;
    }

    public object Clone()
    {
        var clone = (Order)MemberwiseClone();
        clone.SetOrderItems(clone.OrderItems.Select(p => (OrderItem)p.Clone()).ToList());

        return clone;
    }
}