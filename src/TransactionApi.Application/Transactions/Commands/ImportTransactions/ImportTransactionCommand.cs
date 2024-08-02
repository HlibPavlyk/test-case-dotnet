using MediatR;
using Microsoft.AspNetCore.Http;

namespace TransactionApi.Application.Transactions.Commands.ImportTransactions;

public sealed record ImportTransactionCommand(IFormFile File) : IRequest;
