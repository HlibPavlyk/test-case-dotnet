using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TransactionApi.Application.Abstractions;
using TransactionApi.Application.Dtos;
using TransactionApi.Infrastructure.DataAccess;
using TransactionApi.Infrastructure.DataServices;

namespace TransactionApi.Infrastructure.Extensions;

public static class DependencyContainerExtension
{
    public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbEfConnection(configuration);
        services.AddDbSqlConnection(configuration);
        
        services.AddTransient<IDbDataAccess, DbDataAccess>();
        services.AddTransient<IFileDataAccess, FileDataAccess>();

        services.AddHttpClient<ITimeZoneService, TimeZoneService>();

    }
}