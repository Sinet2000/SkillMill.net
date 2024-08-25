using AutoMapper;
using BenchmarkDotNet.Attributes;
using Mapster;
using SkillMill.Application.Customers.Dtos;
using SkillMill.Domain.Entities;

namespace SkillMill.Application.Customers;

public class CustomerMappingBenchmark
{
    private IMapper _mapper;
    private readonly List<Customer> _customers = [];

    [GlobalSetup]
    public void Init()
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(DataConst.CoreDbConnectionString);
        var dbContext = new AppDbContext(optionsBuilder.Options);

        _mapper = new MapperConfiguration(cfg => cfg.AddProfile<CustomerMappingProfile>()).CreateMapper();
        CustomerMapsterConfig.Configure();

        var queryableDbContext = new QueryableDbContext<AppDbContext>(dbContext, _mapper);

        string[] customerIncludesWithDetailedOrderIncludes =
        [
            nameof(Customer.Orders) + "." + nameof(Order.OrderItems) + "." + nameof(OrderItem.Product)
        ];

        var requiredCustomersCount = 10_000;
        var customersFromDb = queryableDbContext.List<Customer>(false, customerIncludesWithDetailedOrderIncludes).ToList();
        while (_customers.Count < requiredCustomersCount)
        {
            foreach (var clonedCustomer in customersFromDb.TakeWhile(customer => _customers.Count < requiredCustomersCount).Select(customer => (Customer)customer.Clone()))
            {
                _customers.Add(clonedCustomer);
            }
        }
    }

    [Benchmark]
    public void CustomerAutoMapperBenchmark_10000Customers()
    {
        Init();
        var customerDtos = _mapper.Map<List<CustomerDto>>(_customers);
    }

    [Benchmark]
    public void CustomerMapsterBenchmark_10000Customers()
    {
        var customerDtos = _customers.Adapt<List<CustomerDto>>();
    }

    [Benchmark]
    public void CustomerAutoMapperBenchmark_1000Customers()
    {
        var customerDtos = _mapper.Map<List<CustomerDto>>(_customers.Take(1000));
    }

    [Benchmark]
    public void CustomerMapsterBenchmark_1000Customers()
    {
        var customerDtos = _customers.Take(1000).Adapt<List<CustomerDto>>();
    }

    [Benchmark]
    public void CustomerAutoMapperBenchmark_100Customers()
    {
        var customerDtos = _mapper.Map<List<CustomerDto>>(_customers.Take(100));
    }

    [Benchmark]
    public void CustomerMapsterBenchmark_100Customers()
    {
        var customerDtos = _customers.Take(100).Adapt<List<CustomerDto>>();
    }

    [Benchmark]
    public void CustomerWithOrdersAutoMapperBenchmark_1000Customers()
    {
        var customerDtos = _mapper.Map<List<CustomerDetailsDto>>(_customers.Take(1000));
    }

    [Benchmark]
    public void CustomerWithOrdersMapsterBenchmark_1000Customers()
    {
        var customerDtos = _customers.Take(1000).Adapt<List<CustomerDetailsDto>>();
    }

    [Benchmark]
    public void CustomerWithOrdersAutoMapperBenchmark_100Customers()
    {
        var customerDtos = _mapper.Map<List<CustomerDetailsDto>>(_customers.Take(100));
    }

    [Benchmark]
    public void CustomerWithOrdersMapsterBenchmark_100Customers()
    {
        var customerDtos = _customers.Take(100).Adapt<List<CustomerDetailsDto>>();
    }
}