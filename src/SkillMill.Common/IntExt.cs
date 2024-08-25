using System.Diagnostics.CodeAnalysis;

namespace SkillMill.Common;

public static class IntExt
{
    public static bool HasValue([NotNullWhen(true)] this int? value)
    {
        return value is > 0;
    }

    public static bool HasValue(this int value)
    {
        return value > 0;
    }
}