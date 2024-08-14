using System.ComponentModel.DataAnnotations;

namespace SkillMill.Common.Attributes;

/// <summary>
/// Has 5..254 Length
/// </summary>
public class EmailLengthAttribute : StringLengthAttribute
{
    public EmailLengthAttribute()
        : base(254)
    {
        MinimumLength = 5;
    }
}