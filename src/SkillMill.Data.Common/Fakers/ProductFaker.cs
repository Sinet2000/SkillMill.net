using Bogus;
using SkillMill.Domain.Entities;

namespace SkillMill.Data.Common.Fakers;

public class ProductFaker : BaseBogusFaker<Product, ProductFaker>
{

    protected override void DefaultRuleSet(IRuleSet<Product> ruleSet)
    {
        ruleSet.Ignore(p => p.Id);
        ruleSet.RuleFor(p => p.Name, f => f.Commerce.ProductName());
        ruleSet.RuleFor(p => p.Price, f => decimal.Parse(f.Commerce.Price()));
    }
}