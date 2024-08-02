using MediatR;

namespace TransactionApi.Application.Transactions.Queries.ExportTransactions;

public sealed record ExportTransactionsQuery : IRequest<byte[]>;
