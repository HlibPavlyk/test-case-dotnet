using MediatR;
using TransactionApi.Application.Dtos;
using TransactionApi.Domain.Entities;

namespace TransactionApi.Application.Transactions.Queries.GetTransactionsInJanuary2024;

public record GetTransactionsInJanuary2024Query(int Page, int PageSize) : IRequest<PagedResponse<Transaction>>;