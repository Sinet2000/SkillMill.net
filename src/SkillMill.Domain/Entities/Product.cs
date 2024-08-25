using System.ComponentModel.DataAnnotations.Schema;
using Sieve.Attributes;
using SkillMill.Common.Attributes;

namespace SkillMill.Domain.Entities;

public class Product : BaseEntity, ICloneable
{
    /// <summary>
    /// CTOR to init Bogus.Faker.
    /// </summary>
    public Product()
    {
    }

    private readonly List<OrderItem> _orderItems = [];

    [Sieve(CanFilter = true, CanSort = true)]
    [NameLength]
    public string Name { get; private set; } = null!;

    [Sieve(CanFilter = true, CanSort = true)]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; private set; }

    public IEnumerable<OrderItem> OrderItems => _orderItems.AsReadOnly();
    
    public object Clone() => (Product)MemberwiseClone();
}