using MediatR;
using TransactionApi.Application.Abstractions;
using TransactionApi.Application.Dtos;
using TransactionApi.Domain.Entities;
using TransactionApi.Domain.Exceptions;

namespace TransactionApi.Application.Transactions.Queries.GetTransactionsByClientLocalDates;

public class GetTransactionsByClientLocalDatesQueryHandler :
    IRequestHandler<GetTransactionsByClientLocalDatesQuery, PagedResponse<Transaction>>
{
    private readonly IDbDataAccess _dbDataAccess;

    public GetTransactionsByClientLocalDatesQueryHandler(IDbDataAccess dbDataAccess)
    {
        _dbDataAccess = dbDataAccess;
    }


    public async Task<PagedResponse<Transaction>> Handle(GetTransactionsByClientLocalDatesQuery request,
        CancellationToken cancellationToken)
    {
        var transactions = (await _dbDataAccess.GetPagedTransactionsByDatesAsync(request.StartDate, request.EndDate,
            request.Page, request.PageSize));

        if (transactions == null || !transactions.Items.Any() || transactions.TotalPages == 0)
        {
            throw new NotFoundException("No transactions found for the specified date range.");
        }
        
        return transactions;
    }
}