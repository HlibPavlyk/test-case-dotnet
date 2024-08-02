using Microsoft.AspNetCore.Http;
using TransactionApi.Domain.Entities;

namespace TransactionApi.Application.Abstractions;

public interface IFileDataAccess
{
    Task<IEnumerable<Transaction>> ReadTransactionsFromFileAsync(IFormFile file);
}