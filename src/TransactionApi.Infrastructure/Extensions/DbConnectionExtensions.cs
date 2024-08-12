using System.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TransactionApi.Infrastructure.Context;

namespace TransactionApi.Infrastructure.Extensions;

public static class DbConnectionExtensions
{
    public static void AddDbEfConnection(this IServiceCollection service, IConfiguration configuration)
    { 
        service.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    }
    
    public static void AddDbSqlConnection(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddTransient<SqlConnection>(sp =>
            new SqlConnection(configuration.GetConnectionString("DefaultConnection")));
    }
    
    public static void MigrateDatabase(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        db.Database.Migrate();
    }
}