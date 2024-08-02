using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TransactionApi.Application.Abstractions;
using TransactionApi.Infrastructure.DataAccess;

namespace TransactionApi.Infrastructure.Extensions;

public static class DependencyContainerExtension
{
    public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbEfConnection(configuration);
        services.AddDbSqlConnection(configuration);
        services.AddTransient<IDbDataAccess, DbDataAccess>();
        services.AddTransient<IFileDataAccess, FileDataAccess>();
    }
}