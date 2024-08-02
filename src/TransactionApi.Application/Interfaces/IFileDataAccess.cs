using Microsoft.AspNetCore.Http;
using TransactionApi.Domain.Entities;

namespace TransactionApi.Application.Interfaces;

public interface IFileDataAccess
{
    Task<IEnumerable<Transaction>> ReadTransactionsFromFileAsync(IFormFile file);
}