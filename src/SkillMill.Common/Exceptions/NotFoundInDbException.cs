namespace SkillMill.Common.Exceptions;

public class NotFoundInDbException(Type entityType, string propertyName, object? value)
    : Exception(BuildErrorMessage(entityType, propertyName, value))
{
    private static string BuildErrorMessage(Type entityType, string propertyName, object? value)
    {
        return $"{entityType.Name} with {propertyName} '{value}' was not found.";
    }
}