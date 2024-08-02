using MediatR;
using Microsoft.AspNetCore.Mvc;
using TransactionApi.Application.Transactions.Commands.ImportTransactions;

namespace TransactionApi.Presentation.Controllers;

[Route("api/transactions")]
public class TransactionController : ApiController
{
    public TransactionController(ISender sender) : base(sender) { }
    
    [HttpPost("import")]
    public async Task<IActionResult> ImportTransactionsAsync([FromForm] ImportTransactionCommand command)
    {
        try
        {
            var result = await Sender.Send(command);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}