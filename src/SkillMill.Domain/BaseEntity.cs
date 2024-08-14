namespace SkillMill.Domain;

public abstract class BaseEntity : IEntity
{
    public virtual int Id { get; init; }

    public static bool operator ==(BaseEntity? a, BaseEntity? b)
    {
        if (a is null && b is null)
        {
            return true;
        }

        if (a is null || b is null)
        {
            return false;
        }

        return a.Equals(b);
    }

    public static bool operator !=(BaseEntity? a, BaseEntity? b)
    {
        if (a is null && b is not null)
        {
            return false;
        }

        if (b is null && a is not null)
        {
            return false;
        }

        return !(a! == b!);
    }

    public override bool Equals(object? obj)
    {
        if (obj is not BaseEntity other)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (Id.Equals(default) || other.Id.Equals(default))
        {
            return false;
        }

        return Id.Equals(other.Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}

public interface IEntity
{
    int Id { get; init; }
}