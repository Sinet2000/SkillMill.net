using System.ComponentModel.DataAnnotations;

namespace SkillMill.Common.Attributes;

/// <summary>
/// Has 1..256 Length
/// </summary>
public class NameLengthAttribute : StringLengthAttribute
{
    public const int MaxLength = 256;

    public NameLengthAttribute()
        : base(MaxLength)
    {
        MinimumLength = 1;
    }
}