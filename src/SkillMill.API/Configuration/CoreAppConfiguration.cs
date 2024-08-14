namespace SkillMill.API.Configuration;

public record CoreAppConfiguration
{
    public const string SectionName = "CoreAppConfig";

    public string DbConnectionString { get; init; } = null!;
}