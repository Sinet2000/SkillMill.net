using AutoMapper;
using BenchmarkDotNet.Attributes;
using Gridify;
using Microsoft.Extensions.DependencyInjection;
using Sieve.Models;
using Sieve.Services;
using SkillMill.Application.Customers.Dtos;
using SkillMill.Data.Common.Models;
using SkillMill.Data.EF.Interfaces;
using SkillMill.Domain.Entities;

namespace SkillMill.Application.Customers;

public class CustomerPaginationBenchmark
{
    private readonly string[] _customerIncludesAll =
    [
        nameof(Customer.Orders) + "." + nameof(Order.OrderItems) + "." + nameof(OrderItem.Product)
    ];

    private readonly string[] _customerIncludesWithOrders =
    [
        nameof(Customer.Orders)
    ];

    private DataSearchableDbContext<AppDbContext> _searchableDbContext;

    private List<Product> _foundBySearchTermProducts = [];
    private List<Customer> _customersWithMultipleOrders = [];
    private const string ProductCaseInsensitiveSearchTerm = "aweSOMe";
    private const int MaxPageSize = 200;

    [GlobalSetup]
    public void Init()
    {
        var services = new ServiceCollection();
        services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(DataConst.CoreDbConnectionString),
            ServiceLifetime.Transient);

        services.Configure<SieveOptions>(options =>
        {
            options.CaseSensitive = false;
            options.DefaultPageSize = DataConst.DefaultPageSize;
        });
        services.AddScoped<SieveProcessor>();
        services.AddScoped<ISieveCustomFilterMethods, SieveCustomFilterMethods>();
        var mapper = new MapperConfiguration(cfg => cfg.AddProfile<CustomerMappingProfile>()).CreateMapper();

        GridifyGlobalConfiguration.EnableEntityFrameworkCompatibilityLayer();

        var serviceProvider = services.BuildServiceProvider();
        var dbContext = serviceProvider.GetRequiredService<AppDbContext>();
        var sieveProcessor = serviceProvider.GetRequiredService<SieveProcessor>();

        _searchableDbContext = new DataSearchableDbContext<AppDbContext>(dbContext, mapper, sieveProcessor);

        _foundBySearchTermProducts = dbContext.Products
            .Where(p => EF.Functions.Like(p.Name, $"%{ProductCaseInsensitiveSearchTerm}%"))
            .ToList();

        _customersWithMultipleOrders = _searchableDbContext.List(
                false,
                _customerIncludesWithOrders,
                CustomerFilter.HasMultipleOrders(true))
            .ToList();
    }

    [Benchmark]
    public async Task<SievePagedResult<CustomerDetailsDto>> CustomerSievePaginationWithAllSelectedBenchmark()
    {
        var sieveModel = new SieveModel()
        {
            PageSize = MaxPageSize,
            Page = 1,
            Filters = $"{nameof(Customer.Name)}@=a"
        };

        return await _searchableDbContext.ListAsync<Customer, CustomerDetailsDto>(sieveModel, _customerIncludesAll)
            .ConfigureAwait(false);
    }

    [Benchmark]
    public async Task<GridifyPagedResult<CustomerDetailsDto>> CustomerGridlifyPaginationWithAllSelectedBenchmark()
    {
        var gridlifyQuery = new GridifyQuery(pageSize: MaxPageSize, page: 1, filter: $"{nameof(Customer.Name)}=*a");

        return await _searchableDbContext.ListAsync<Customer, CustomerDetailsDto>(gridlifyQuery, _customerIncludesAll)
            .ConfigureAwait(false);
    }

    [Benchmark]
    public async Task<SievePagedResult<CustomerDetailsDto>> CustomerSievePaginationWithOrdersSelectedBenchmark()
    {
        var sieveModel = new SieveModel()
        {
            PageSize = MaxPageSize,
            Page = 1,
            Filters = $"{nameof(Customer.Name)}@=a"
        };

        return await _searchableDbContext.ListAsync<Customer, CustomerDetailsDto>(sieveModel, _customerIncludesWithOrders)
            .ConfigureAwait(false);
    }

    [Benchmark]
    public async Task<GridifyPagedResult<CustomerDetailsDto>> CustomerGridlifyPaginationWithOrdersSelectedBenchmark()
    {
        var gridlifyQuery = new GridifyQuery(pageSize: MaxPageSize, page: 1, filter: $"{nameof(Customer.Name)}=*a");

        return await _searchableDbContext.ListAsync<Customer, CustomerDetailsDto>(gridlifyQuery, _customerIncludesWithOrders)
            .ConfigureAwait(false);
    }

    [Benchmark]
    public async Task<SievePagedResult<CustomerDetailsDto>> CustomerSievePaginationBenchmark()
    {
        var sieveModel = new SieveModel()
        {
            PageSize = MaxPageSize,
            Page = 1,
            Filters = $"{nameof(Customer.Name)}@=a"
        };

        return await _searchableDbContext.ListAsync<Customer, CustomerDetailsDto>(sieveModel)
            .ConfigureAwait(false);
    }

    [Benchmark]
    public async Task<GridifyPagedResult<CustomerDetailsDto>> CustomerGridlifyPaginationBenchmark()
    {
        var gridlifyQuery = new GridifyQuery(pageSize: MaxPageSize, page: 1, filter: $"{nameof(Customer.Name)}=*a");

        return await _searchableDbContext.ListAsync<Customer, CustomerDetailsDto>(gridlifyQuery)
            .ConfigureAwait(false);
    }

    [Benchmark]
    public async Task<SievePagedResult<ProductDto>> SieveCaseInsensitiveSearchBenchmark()
    {
        var sieveModel = new SieveModel()
        {
            PageSize = MaxPageSize,
            Page = 1,
            Filters = $"{nameof(Product.Name)}@=*{ProductCaseInsensitiveSearchTerm}"
        };

        var result = await _searchableDbContext.ListAsync<Product, ProductDto>(sieveModel)
            .ConfigureAwait(false);

        if (result.TotalCount != _foundBySearchTermProducts.Count)
        {
            throw new Exception($"Sieve result of products must be in count of {_foundBySearchTermProducts.Count}");
        }

        return result;
    }

    [Benchmark]
    public async Task<GridifyPagedResult<ProductDto>> GridlifyCaseInsensitiveSearchBenchmark()
    {
        var gridlifyQuery = new GridifyQuery(pageSize: MaxPageSize, page: 1, filter: $"{nameof(Product.Name)}=*{ProductCaseInsensitiveSearchTerm}/i");

        var result = await _searchableDbContext.ListAsync<Product, ProductDto>(gridlifyQuery)
            .ConfigureAwait(false);

        if (result.TotalCount != _foundBySearchTermProducts.Count)
        {
            throw new Exception($"Sieve result of products must be in count of {_foundBySearchTermProducts.Count}");
        }

        return result;
    }

    [Benchmark]
    public async Task<SievePagedResult<CustomerDetailsDto>> SieveHasMultipleOrdersBenchmark()
    {
        var sieveModel = new SieveModel()
        {
            PageSize = MaxPageSize,
            Page = 1,
            Filters = $"{nameof(CustomerFilter.HasMultipleOrders)}==true"
        };

        var result = await _searchableDbContext.ListAsync<Customer, CustomerDetailsDto>(sieveModel, _customerIncludesWithOrders)
            .ConfigureAwait(false);

        if (result.TotalCount != _customersWithMultipleOrders.Count)
        {
            throw new Exception($"Sieve result of customers must be in count of {_customersWithMultipleOrders.Count}");
        }

        return result;
    }
}