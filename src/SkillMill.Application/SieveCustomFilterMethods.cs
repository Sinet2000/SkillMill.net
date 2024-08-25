using Sieve.Services;
using SkillMill.Application.Customers;
using SkillMill.Domain.Entities;

namespace SkillMill.Application;

public class SieveCustomFilterMethods : ISieveCustomFilterMethods
{
    public IQueryable<Customer> HasMultipleOrders(IQueryable<Customer> source, string op, string[] values)
    {
        var filterValue = bool.Parse(values[0]);

        return source.Where(CustomerFilter.HasMultipleOrders(filterValue));
    }

    public IQueryable<Customer> AllOrderDateFrom(IQueryable<Customer> source, string op, string[] values)
    {
        var dateFrom = DateTime.Parse(values[0]);

        return source.Where(CustomerFilter.AllOrderDateFrom(dateFrom));
    }

    public IQueryable<Customer> OrderProductNameContains(IQueryable<Customer> source, string op, string[] values)
    {
        return source.Where(CustomerFilter.OrderProductNameContains(values[0]));
    }
}