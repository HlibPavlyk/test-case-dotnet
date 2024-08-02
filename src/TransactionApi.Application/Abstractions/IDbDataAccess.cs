using TransactionApi.Domain.Entities;

namespace TransactionApi.Application.Abstractions;

public interface IDbDataAccess
{
    Task ImportTransactionsAsync(Transaction transaction);
}