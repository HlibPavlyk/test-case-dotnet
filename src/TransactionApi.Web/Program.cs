using System.Data;
using MediatR;
using Microsoft.Data.SqlClient;
using TransactionApi.Application.Abstractions;
using TransactionApi.Infrastructure.DataAccess;
using TransactionApi.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
