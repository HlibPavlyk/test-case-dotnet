using MediatR;
using Microsoft.OpenApi.Models;
using TransactionApi.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Transaction API", Version = "v1" });

    var xmlFile = "TransactionWeb.Presentation.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});


builder
    .Services
    .AddControllers()
    .AddApplicationPart(TransactionApi.Presentation.AssemblyReference.Assembly);

builder.Services.AddMediatR(TransactionApi.Application.AssemblyReference.Assembly);

builder.Services.AddDependencies(builder.Configuration);

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Transaction API V1");
        c.RoutePrefix = string.Empty;
    });
    app.MigrateDatabase();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
