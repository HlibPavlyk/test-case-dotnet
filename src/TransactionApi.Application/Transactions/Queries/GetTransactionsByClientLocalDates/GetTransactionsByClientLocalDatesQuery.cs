using MediatR;
using TransactionApi.Application.Dtos;
using TransactionApi.Domain.Entities;

namespace TransactionApi.Application.Transactions.Queries.GetTransactionsByClientLocalDates;

/// <summary>
/// Query to get transactions by client local dates.
/// </summary>
/// <param name="StartDate">The start date of the transaction range.</param>
/// <param name="EndDate">The end date of the transaction range.</param>
/// <param name="Page">The page number for pagination.</param>
/// <param name="PageSize">The number of transactions per page.</param>
public record GetTransactionsByClientLocalDatesQuery(DateTime StartDate, DateTime EndDate, int Page, int PageSize)
    : IRequest<PagedResponse<Transaction>>;