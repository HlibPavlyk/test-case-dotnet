using System.Transactions;
using MediatR;
using TransactionApi.Application.Abstractions;
using TransactionApi.Application.Validators;

namespace TransactionApi.Application.Transactions.Commands.ImportTransactions;

public sealed class ImportTransactionCommandHandler : IRequestHandler<ImportTransactionCommand>
{
    private readonly IFileDataAccess _fileDataAccess;
    private readonly IDbDataAccess _dbDataAccess;

    public ImportTransactionCommandHandler(IFileDataAccess fileDataAccess, IDbDataAccess dbDataAccess)
    {
        _fileDataAccess = fileDataAccess;
        _dbDataAccess = dbDataAccess;
    }

    public async Task<Unit> Handle(ImportTransactionCommand request, CancellationToken cancellationToken)
    {
        var transactions = (await _fileDataAccess.ReadTransactionsFromFileAsync(request.File)).ToList();

        if (transactions == null || !transactions.Any())
        {
            throw new InvalidOperationException("No transactions found in the file.");
        }

        foreach (var transaction in transactions)
        {
            if (!TransactionValidator.IsValidTransaction(transaction))
            {
                throw new ArgumentException($"Invalid transaction data: {transaction}");
            }
        }

        using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            foreach (var transaction in transactions)
            {
                await _dbDataAccess.ImportTransactionsAsync(transaction);
            }
            transactionScope.Complete();
        }

        return Unit.Value;
    }
}