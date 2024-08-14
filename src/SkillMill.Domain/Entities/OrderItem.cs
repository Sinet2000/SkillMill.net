using System.ComponentModel.DataAnnotations;

namespace SkillMill.Domain.Entities;

public class OrderItem : BaseEntity
{
    /// <summary>
    /// CTOR to init Bogus.Faker.
    /// </summary>
    public OrderItem()
    {
    }

    public int Quantity { get; private set; }

    public decimal UnitPrice { get; private set; }

    [Range(1, int.MaxValue)]
    public int OrderId { get; private set; }

    public Order Order { get; private set; } = null!;

    [Range(1, int.MaxValue)]
    public int ProductId { get; private set; }

    public Product Product { get; private set; } = null!;

    public void SetProduct(Product product)
    {
        Product = product;
    }
}