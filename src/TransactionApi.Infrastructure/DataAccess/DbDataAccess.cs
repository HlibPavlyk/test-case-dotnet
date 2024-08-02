using Dapper;
using Microsoft.Data.SqlClient;
using TransactionApi.Application.Abstractions;
using TransactionApi.Domain.Entities;

namespace TransactionApi.Infrastructure.DataAccess;

public class DbDataAccess : IDbDataAccess
{
    private readonly SqlConnection _connection;

    public DbDataAccess(SqlConnection connection)
    {
        _connection = connection;
    }

    public async Task ImportTransactionsAsync(Transaction transaction)
    {
        var query = @"
            MERGE INTO Transactions AS target
            USING (SELECT @TransactionId AS TransactionId) AS source
            ON target.TransactionId = source.TransactionId
            WHEN MATCHED THEN 
                UPDATE SET Name = @Name, Email = @Email, Amount = @Amount, TransactionDate = @TransactionDate, ClientLocation = @ClientLocation
            WHEN NOT MATCHED THEN
                INSERT (TransactionId, Name, Email, Amount, TransactionDate, ClientLocation)
                VALUES (@TransactionId, @Name, @Email, @Amount, @TransactionDate, @ClientLocation);";

        await _connection.ExecuteAsync(query, transaction);
    }
}