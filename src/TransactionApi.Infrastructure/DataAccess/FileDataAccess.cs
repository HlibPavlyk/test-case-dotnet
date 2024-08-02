using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using TransactionApi.Application.Abstractions;
using TransactionApi.Application.Dtos;
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

    public async Task<byte[]> WriteTransactionsToExcelFileAsync(IEnumerable<TransactionExcelGetDto> transactions)
    {
        return await Task.Run(() =>
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Transactions");

            worksheet.Cells[1, 1].Value = "Transaction Id";
            worksheet.Cells[1, 2].Value = "Email";
            worksheet.Cells[1, 3].Value = "Amount";
            worksheet.Cells[1, 4].Value = "Transaction Local Date";

            using (var headerRange = worksheet.Cells[1, 1, 1, 4])
            {
                headerRange.Style.Font.Bold = true;
                headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }

            var row = 2;

            foreach (var transaction in transactions)
            {
                worksheet.Cells[row, 1].Value = transaction.TransactionId;
                worksheet.Cells[row, 2].Value = transaction.Email;

                worksheet.Cells[row, 3].Value = transaction.Amount;
                worksheet.Cells[row, 3].Style.Numberformat.Format = "$#,##0.00";

                worksheet.Cells[row, 4].Value = transaction.TransactionDate;
                worksheet.Cells[row, 4].Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";

                row++;
            }

            return package.GetAsByteArray();
        });
    }
}