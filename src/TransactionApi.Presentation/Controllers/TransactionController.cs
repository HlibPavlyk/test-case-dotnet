using MediatR;
using Microsoft.AspNetCore.Mvc;
using TransactionApi.Application.Transactions.Commands.ImportTransactions;
using TransactionApi.Application.Transactions.Queries.ExportTransactions;

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
}