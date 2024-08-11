using MediatR;
using TransactionApi.Application.Dtos;
using TransactionApi.Domain.Entities;

namespace TransactionApi.Application.Transactions.Queries.GetTransactionsByDatesForCurrentClientTimeZone;

public record GetTransactionsByDatesForCurrentClientTimeZoneQuery(DateTime StartDate, DateTime EndDate,
    string CurrentClientTimeZone, int Page, int PageSize) : IRequest<PagedResponse<Transaction>>;
