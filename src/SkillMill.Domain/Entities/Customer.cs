using System.ComponentModel.DataAnnotations;
using Ardalis.GuardClauses;
using SkillMill.Common.Attributes;

namespace SkillMill.Domain.Entities;

public class Customer : BaseEntity
{
    /// <summary>
    /// CTOR to init Bogus.Faker.
    /// </summary>
    public Customer()
    {
    }

    private readonly List<Order> _orders = [];

    [NameLength]
    public string Name { get; private set; } = null!;

    [EmailLength]
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
}