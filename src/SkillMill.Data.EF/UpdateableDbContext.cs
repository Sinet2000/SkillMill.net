using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Sieve.Services;
using SkillMill.Data.EF.Interfaces;

namespace SkillMill.Data.EF;

public class UpdateableDbContext<TContext>(TContext context, IMapper mapper, ISieveProcessor sieveProcessor)
    : DataSearchableDbContext<TContext>(context, mapper, sieveProcessor), IUpdateableDbContext<TContext>
    where TContext : DbContext
{

}