using MediatR;
using TransactionApi.Application.Abstractions;
using TransactionApi.Application.Dtos;
using TransactionApi.Domain.Entities;
using TransactionApi.Domain.Exceptions;

namespace TransactionApi.Application.Transactions.Queries.GetTransactionsByDatesForCurrentClientTimeZone;

public class GetTransactionsByDatesForCurrentClientTimeZoneQueryHandler : IRequestHandler<GetTransactionsByDatesForCurrentClientTimeZoneQuery, PagedResponse<Transaction>>
{
    private readonly ITimeZoneService _timeZoneService;
    private readonly IDbDataAccess _dbDataAccess;

    public GetTransactionsByDatesForCurrentClientTimeZoneQueryHandler(ITimeZoneService timeZoneService, IDbDataAccess dbDataAccess)
    {
        _timeZoneService = timeZoneService;
        _dbDataAccess = dbDataAccess;
    }

    public async Task<PagedResponse<Transaction>> Handle(GetTransactionsByDatesForCurrentClientTimeZoneQuery request, CancellationToken cancellationToken)
    {
        var startDateWithOffset = request.StartDate.AddDays(1);
        var endDateWithOffset = request.EndDate.AddDays(-1);
    
        var transactions = await _dbDataAccess.GetPagedTransactionsByDatesAsync(startDateWithOffset, endDateWithOffset,
            request.Page, request.PageSize);

        if (transactions == null || !transactions.Items.Any() || transactions.TotalPages == 0)
        {
            throw new NotFoundException("No transactions found for the specified date range.");
        }

        var tasks = transactions.Items.Select(async x =>
        {
            var timeZoneId = await _timeZoneService.GetTimeZoneIdFromCoordinatesAsync(x.ClientLocation);
            var convertedTransactionDate = _timeZoneService.ConvertLocalTimeToOtherByTimeZoneIdAsync(x.TransactionDate, timeZoneId, request.CurrentClientTimeZone);

            x.TransactionDate = convertedTransactionDate;
            return x;
        });

        var processedTransactions = await Task.WhenAll(tasks);

        var filteredTransactions = processedTransactions
            .Where(t => t.TransactionDate >= request.StartDate && t.TransactionDate <= request.EndDate)
            .ToList();

        return new PagedResponse<Transaction>
        {
            TotalPages = transactions.TotalPages,
            Items = filteredTransactions,
        };
    }
}