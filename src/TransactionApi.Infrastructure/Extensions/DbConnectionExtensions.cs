using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TransactionApi.Infrastructure.Context;

namespace TransactionApi.Infrastructure.Extensions;

public static class DbConnectionExtensions
{
    public static void AddDbConnection(this IServiceCollection service, IConfiguration configuration)
    {
        IServiceCollection serviceCollection = service.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    }
}