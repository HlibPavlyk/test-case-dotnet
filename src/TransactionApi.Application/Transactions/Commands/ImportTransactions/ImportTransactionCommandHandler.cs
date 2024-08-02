using MediatR;
using TransactionApi.Application.Interfaces;

namespace TransactionApi.Application.Transactions.Commands.ImportTransactions;

public sealed class ImportTransactionCommandHandler : IRequestHandler<ImportTransactionCommand>
{
    private readonly IFileDataAccess _fileDataAccess;

    public ImportTransactionCommandHandler(IFileDataAccess fileDataAccess)
    {
        _fileDataAccess = fileDataAccess;
    }

    public async Task<Unit> Handle(ImportTransactionCommand request, CancellationToken cancellationToken)
    {
        var transactions = await _fileDataAccess.ReadTransactionsFromFileAsync(request.File);
        
        return Unit.Value;

    }
}