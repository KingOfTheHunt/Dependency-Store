using DependencyStore.Repositories;
using DependencyStore.Repositories.Contracts;
using DependencyStore.Services;
using DependencyStore.Services.Contracts;
using Microsoft.Data.SqlClient;

namespace DependencyStore.Extensions;

public static class BuilderExtension
{
    public static void AddRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<SqlConnection>(x => new SqlConnection("ConnectionString"));
        builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();
        builder.Services.AddTransient<IProductRepository, ProductRepository>();
        builder.Services.AddTransient<IPromoCodeRepository, PromoCodeRepository>();
    }

    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpClient<DeliveryFeeService>();
        builder.Services.AddTransient<IDeliveryFeeService, DeliveryFeeService>();
    }
}