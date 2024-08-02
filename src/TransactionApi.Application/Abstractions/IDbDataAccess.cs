using TransactionApi.Application.Dtos;
using TransactionApi.Domain.Entities;

namespace TransactionApi.Application.Abstractions;

public interface IDbDataAccess
{
    Task ImportTransactionsAsync(Transaction transaction);
    Task<IEnumerable<TransactionExcelGetDto>> GetExcelTransactionsAsync();
}