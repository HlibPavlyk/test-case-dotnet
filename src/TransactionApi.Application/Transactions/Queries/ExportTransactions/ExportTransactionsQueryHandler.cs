using MediatR;
using TransactionApi.Application.Abstractions;

namespace TransactionApi.Application.Transactions.Queries.ExportTransactions;

public class ExportTransactionsQueryHandler : IRequestHandler<ExportTransactionsQuery, byte[]>
{
    private readonly IFileDataAccess _fileDataAccess;
    private readonly IDbDataAccess _dbDataAccess;

    public ExportTransactionsQueryHandler(IFileDataAccess fileDataAccess, IDbDataAccess dbDataAccess)
    {
        _fileDataAccess = fileDataAccess;
        _dbDataAccess = dbDataAccess;
    }

    public async Task<byte[]> Handle(ExportTransactionsQuery request, CancellationToken cancellationToken)
    {
        var transactions = (await _dbDataAccess.GetExcelTransactionsAsync()).ToList();

        if (transactions == null || !transactions.Any())
        {
            throw new InvalidOperationException("No transactions found in the database.");
        }

        return await _fileDataAccess.WriteTransactionsToExcelFileAsync(transactions);
    }
}