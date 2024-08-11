using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TransactionApi.Application.Dtos;
using TransactionApi.Application.Transactions.Commands.ImportTransactions;
using TransactionApi.Application.Transactions.Queries.ExportTransactions;
using TransactionApi.Application.Transactions.Queries.GetTransactionsByClientLocalDates;
using TransactionApi.Application.Transactions.Queries.GetTransactionsByDatesForCurrentClientTimeZone;
using TransactionApi.Application.Transactions.Queries.GetTransactionsInJanuary2024;
using TransactionApi.Domain.Entities;
using TransactionApi.Domain.Exceptions;

namespace TransactionApi.Presentation.Controllers;

[Route("api/transactions")]
public class TransactionController : ApiController
{
    public TransactionController(ISender sender) : base(sender) { }

    [HttpPost("import-csv")]
    public async Task<IActionResult> ImportTransactionsAsync([FromForm] ImportTransactionCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await Sender.Send(command, cancellationToken);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("export-excel")]
    public async Task<IActionResult> ExportTransactionsAsync(CancellationToken cancellationToken)
    {
        try
        {
            var result = await Sender.Send(new ExportTransactionsQuery(), cancellationToken);
            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "transactions.xlsx");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("get-by-local-dates")]
    public async Task<IActionResult> GetAllTransactionsByLocalDatesAsync(
        [FromQuery] GetTransactionsByClientLocalDatesQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var result = await Sender.Send(query, cancellationToken);
            return Ok(result);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("get-by-dates-for-current-time-zone")]
    public async Task<IActionResult> GetTransactionsByDatesForCurrentClientTimeZoneQuery (
        [FromQuery] DateTime startDate, 
        [FromQuery] DateTime endDate, 
        [FromQuery] int page, 
        [FromQuery] int pageSize,
        [FromHeader(Name = "X-Timezone")] string timezone, 
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await Sender.Send(new GetTransactionsByDatesForCurrentClientTimeZoneQuery(startDate, endDate,
                timezone, page, pageSize), cancellationToken);
            return Ok(result);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("get-in-january-2024")]
    public async Task<IActionResult> GetTransactionsInJanuary2024Query(
        [FromQuery] int page, [FromQuery] int pageSize, CancellationToken cancellationToken)
    {
        try
        {
            var result = await Sender.Send(new GetTransactionsInJanuary2024Query(page, pageSize), cancellationToken);
            return Ok(result);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}