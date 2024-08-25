using System.ComponentModel.DataAnnotations;
using Ardalis.GuardClauses;
using Sieve.Attributes;
using SkillMill.Common.Attributes;

namespace SkillMill.Domain.Entities;

public class Customer : BaseEntity, ICloneable
{
    /// <summary>
    /// CTOR to init Bogus.Faker.
    /// </summary>
    public Customer()
    {
    }

    private readonly List<Order> _orders = [];

    [NameLength]
    [Sieve(CanFilter = true, CanSort = true)]
    public string Name { get; private set; } = null!;

    [EmailLength]
    [Sieve(CanFilter = true, CanSort = true)]
    public string Email { get; private set; } = null!;

    [Timestamp]
    public byte[] RowVersion { get; private set; }

    public IEnumerable<Order> Orders => _orders.AsReadOnly();

    public void AddOrders(IEnumerable<Order> orders)
    {
        _orders.AddRange(orders);
    }

    private void SetOrders(IEnumerable<Order> orders)
    {
        _orders.Clear();
        _orders.AddRange(orders);
    }

    public void UpdateEmail(string email)
    {
        Email = Guard.Against.NullOrEmpty(email);
    }
    
    public object Clone()
    {
        var clone = (Customer)MemberwiseClone();
        clone.SetOrders(clone.Orders.Select(p => (Order)p.Clone()).ToList());

        return clone;
    }
}