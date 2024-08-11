using TransactionApi.Application.Dtos;
using TransactionApi.Domain.Entities;

namespace TransactionApi.Application.Abstractions;

public interface IDbDataAccess
{
    Task ImportTransactionsAsync(Transaction transaction);
    Task<IEnumerable<TransactionExcelGetDto>> GetExcelTransactionsAsync();

    Task<PagedResponse<Transaction>> GetPagedTransactionsByDatesAsync(DateTime startDate, DateTime endDate,
        int page, int pageSize);
}