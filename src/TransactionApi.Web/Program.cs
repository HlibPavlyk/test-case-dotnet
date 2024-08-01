using TransactionApi.Application.Interfaces;
using TransactionApi.Infrastructure.DataAccess;
using TransactionApi.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbConnection(builder.Configuration);

builder.Services.AddTransient<IFileDataAccess, FileDataAccess>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
