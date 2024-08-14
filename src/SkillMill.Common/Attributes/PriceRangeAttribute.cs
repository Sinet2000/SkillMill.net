using System.ComponentModel.DataAnnotations;

namespace SkillMill.Common.Attributes;

/// <summary>
/// sets Range to 100_000_000
/// </summary>
public class PriceRangeAttribute() : RangeAttribute(0, MaximumPrice)
{
    private const int MaximumPrice = 100_000_000;
}