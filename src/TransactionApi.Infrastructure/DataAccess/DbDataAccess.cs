using Dapper;
using Microsoft.Data.SqlClient;
using TransactionApi.Application.Abstractions;
using TransactionApi.Application.Dtos;
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

    public async Task<IEnumerable<TransactionExcelGetDto>> GetExcelTransactionsAsync()
    {

        var query = @"
            SELECT TransactionId, Email, Amount, TransactionDate
            FROM Transactions;";

        return await _connection.QueryAsync<TransactionExcelGetDto>(query);
    }

    public async Task<IEnumerable<Transaction>> GetTransactionsByDatesAsync(DateTime startDate, DateTime endDate,
        int page, int pageSize)
    {
        var query = @"
        SELECT TransactionId, Name, Email, Amount, TransactionDate, ClientLocation
        FROM Transactions
        WHERE TransactionDate BETWEEN @startDate AND @endDate;";

        var parameters = new { startDate, endDate };

        return await _connection.QueryAsync<Transaction>(query, parameters);
    }

    public async Task<PagedResponse<Transaction>> GetPagedTransactionsByDatesAsync(DateTime startDate, DateTime endDate,
        int page, int pageSize)
    {
        
        
        var query = @"
        SELECT TransactionId, Name, Email, Amount, TransactionDate, ClientLocation
        FROM Transactions
        WHERE TransactionDate BETWEEN @startDate AND @endDate
        ORDER BY TransactionDate
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

        var parameters = new
        {
            startDate,
            endDate,
            Offset = (page - 1) * pageSize,
            PageSize = pageSize
        };

        var items = await _connection.QueryAsync<Transaction>(query, parameters);

        var totalCount = await GetTransactionsTotalAmountByDates(startDate, endDate);
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return new PagedResponse<Transaction>
        {
            TotalPages = totalPages,
            Items = items
        };
    }

    private async Task<int> GetTransactionsTotalAmountByDates(DateTime startDate, DateTime endDate)
    {
        var countQuery = @"
        SELECT COUNT(*)
        FROM Transactions
        WHERE TransactionDate BETWEEN @startDate AND @endDate;";

        return await _connection.QuerySingleAsync<int>(countQuery, new { startDate, endDate });
    }

}
