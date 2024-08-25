using System.Linq.Expressions;
using SkillMill.Domain.Entities;

namespace SkillMill.Application.Customers;

public static class CustomerFilter
{
    public static Expression<Func<Customer, bool>> HasMultipleOrders(bool value)
    {
        return x => value && x.Orders.Count() > 1;
    }

    public static Expression<Func<Customer, bool>> AllOrderDateFrom(DateTime dateFrom)
    {
        return x => x.Orders.All(o => o.OrderDate.Date >= dateFrom.Date);
    }

    // & OrderProductNameContains == TV | Oven
    public static Expression<Func<Customer, bool>> OrderProductNameContains(string value)
    {
        var values = value.Split('|', StringSplitOptions.RemoveEmptyEntries);

        return customer => customer.Orders
            .Any(order => order.OrderItems
                .Any(orderItem => values
                    .Any(v => orderItem.Product.Name.Contains(v))));
    }
}