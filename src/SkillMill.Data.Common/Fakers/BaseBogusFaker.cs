using System.Reflection;
using Bogus;

namespace SkillMill.Data.Common.Fakers;

public abstract class BaseBogusFaker<T, TFaker> : Faker<T>
    where T : class
    where TFaker : BaseBogusFaker<T, TFaker>
{
    private const string DefaultLocale = "en";
    
    protected BaseBogusFaker()
    {
        FakerHub = new Faker
        {
            Lorem = { Locale = DefaultLocale }
        };
    }

    public virtual T Generate()
    {
        RuleSet(Default, DefaultRuleSet);

        var currentInstance = BuildInstanceFromType();
        PopulateInternal(currentInstance, [Default]);

        CleanUp();

        return currentInstance;
    }

    public override List<T> Generate(int count, string? ruleSets = null)
    {
        return GenerateRange(count).ToList();
    }


    public IEnumerable<T> GenerateRange(int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            yield return Generate();
        }
    }

    public IEnumerable<T> GenerateItemsInRange(int minCount, int maxCount)
    {
        if (minCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(minCount), "Minimum count cannot be negative.");
        }

        if (maxCount < minCount)
        {
            throw new ArgumentException("Maximum count cannot be less than minimum count.");
        }

        int rangeSize = FakerHub.Random.Int(minCount, maxCount);
        for (int i = 0; i < rangeSize; i++)
        {
            yield return Generate();
        }
    }

    protected abstract void DefaultRuleSet(IRuleSet<T> ruleSet);

    protected virtual void CleanUp()
    {
    }

    private static T BuildInstanceFromType()
    {
        var type = typeof(T);
        var instanceCtor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
        if (instanceCtor == null)
        {
            throw new BogusException($"Type {type.FullName} does not have a parameterless constructor.");
        }

        try
        {
            return (T)instanceCtor.Invoke(null);
        }
        catch (Exception ex)
        {
            throw new BogusException($"Failed to create an instance of type {type.FullName}.", ex);
        }
    }
}