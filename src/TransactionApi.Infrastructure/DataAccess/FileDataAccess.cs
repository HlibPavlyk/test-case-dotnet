using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using TransactionApi.Application.Interfaces;
using TransactionApi.Domain.Entities;
using TransactionApi.Infrastructure.DataMapper;

namespace TransactionApi.Infrastructure.DataAccess;

public class FileDataAccess : IFileDataAccess
{
    public async Task<IEnumerable<Transaction>> ReadTransactionsFromFileAsync(IFormFile file)
    {
        using var reader = new StreamReader(file.OpenReadStream());
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            PrepareHeaderForMatch = args => args.Header.ToLower(),
            MissingFieldFound = null
        };
        using var csv = new CsvReader(reader, config);
        csv.Context.RegisterClassMap<TransactionMap>();
        
        var records = new List<Transaction>();
        await foreach (var record in csv.GetRecordsAsync<Transaction>())
        {
            records.Add(record);
        }
        
        return records;
    }
}