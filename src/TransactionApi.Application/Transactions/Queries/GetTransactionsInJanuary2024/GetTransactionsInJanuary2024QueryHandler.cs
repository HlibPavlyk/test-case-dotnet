using MediatR;
using TransactionApi.Application.Abstractions;
using TransactionApi.Application.Dtos;
using TransactionApi.Domain.Entities;
using TransactionApi.Domain.Exceptions;

namespace TransactionApi.Application.Transactions.Queries.GetTransactionsInJanuary2024;

public class GetTransactionsInJanuary2024QueryHandler : IRequestHandler<GetTransactionsInJanuary2024Query, PagedResponse<Transaction>>
{
    private readonly IDbDataAccess _dbDataAccess;

    public GetTransactionsInJanuary2024QueryHandler(IDbDataAccess dbDataAccess)
    {
        _dbDataAccess = dbDataAccess;
    }
    
    public async Task<PagedResponse<Transaction>> Handle(GetTransactionsInJanuary2024Query request, CancellationToken cancellationToken)
    {
        var startDate = new DateTime(2024, 1, 1);
        var endDate = new DateTime(2024, 1, 31);
        
        var transactions = (await _dbDataAccess.GetPagedTransactionsByDatesAsync(startDate, endDate,
            request.Page, request.PageSize));

        if (transactions == null || !transactions.Items.Any() || transactions.TotalPages == 0)
        {
            throw new NotFoundException("No transactions found for the specified date range.");
        }
        
        return transactions;
    }
}