using Bogus;
using SkillMill.Domain.Entities;

namespace SkillMill.Data.Common.Fakers;

public class OrderItemFaker(ProductFaker productFaker) : BaseBogusFaker<OrderItem, OrderItemFaker>
{

    protected override void DefaultRuleSet(IRuleSet<OrderItem> ruleSet)
    {
        ruleSet.Ignore(p => p.Id);
        ruleSet.RuleFor(oi => oi.Quantity, f => f.Random.Int(1, 10));
        ruleSet.RuleFor(oi => oi.UnitPrice, f => decimal.Parse(f.Commerce.Price()));
        ruleSet.FinishWith((f, oi) =>
        {
            oi.SetProduct(productFaker.Generate());
        });
    }
}