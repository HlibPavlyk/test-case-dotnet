using System.Transactions;
using Microsoft.AspNetCore.Http;

namespace TransactionApi.Application.Interfaces;

public interface IFileDataAccess
{
    Task<IEnumerable<Transaction>> ReadTransactionsFromFileAsync(IFormFile file);
}