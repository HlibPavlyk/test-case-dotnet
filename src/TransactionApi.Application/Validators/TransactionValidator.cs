using TransactionApi.Domain.Entities;

namespace TransactionApi.Application.Validators;

public static class TransactionValidator
{
    public static bool IsValidTransaction(Transaction transaction)
    {
        return !string.IsNullOrEmpty(transaction.Name) &&
               !string.IsNullOrEmpty(transaction.Email) &&
               transaction.Amount > 0 &&
               transaction.TransactionDate != default;
    }
}