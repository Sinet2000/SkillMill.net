using Bogus;
using SkillMill.Domain.Entities;

namespace SkillMill.Data.Common.Fakers;

public class CustomerFaker(OrderFaker orderFaker) : BaseBogusFaker<Customer, CustomerFaker>
{

    protected override void DefaultRuleSet(IRuleSet<Customer> ruleSet)
    {
        ruleSet.Ignore(p => p.Id);
        ruleSet.RuleFor(p => p.Name, f => f.Name.FullName());
        ruleSet.RuleFor(p => p.Email, f => f.Internet.Email());
        ruleSet.FinishWith((f, c) =>
        {
            c.AddOrders(orderFaker.GenerateBetween(1, 3).ToList());
        });
    }
}