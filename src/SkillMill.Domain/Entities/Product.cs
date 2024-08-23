using System.ComponentModel.DataAnnotations.Schema;
using SkillMill.Common.Attributes;

namespace SkillMill.Domain.Entities;

public class Product : BaseEntity
{
    /// <summary>
    /// CTOR to init Bogus.Faker.
    /// </summary>
    public Product()
    {
    }

    private readonly List<OrderItem> _orderItems = [];

    [NameLength]
    public string Name { get; private set; } = null!;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; private set; }

    public IEnumerable<OrderItem> OrderItems => _orderItems.AsReadOnly();
}