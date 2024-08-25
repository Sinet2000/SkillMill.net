using Microsoft.EntityFrameworkCore;

namespace SkillMill.Data.EF.Interfaces;

public interface IUpdateableDbContext<TContext> : IDataSearchQuery<TContext>
    where TContext : DbContext
{

}