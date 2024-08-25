using Bogus;
using SkillMill.Domain.Entities;

namespace SkillMill.Data.Common.Fakers;

public class OrderFaker(OrderItemFaker orderItemFaker) : BaseBogusFaker<Order, OrderFaker>
{

    protected override void DefaultRuleSet(IRuleSet<Order> ruleSet)
    {
        ruleSet.Ignore(p => p.Id);
        ruleSet.RuleFor(o => o.OrderDate, f => f.Date.Between(DateTime.Now.AddYears(-10), DateTime.Now));
        ruleSet.FinishWith((f, oi) =>
        {
            oi.AddOrderItems(orderItemFaker.GenerateBetween(1, 3).ToList());
        });
    }
}