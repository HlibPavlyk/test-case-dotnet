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

/// <summary>
/// Controller for handling transaction-related API requests.
/// </summary>
[Route("api/transactions")]
public class TransactionController : ApiController
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TransactionController"/> class.
    /// </summary>
    /// <param name="sender">The sender for sending MediatR requests.</param>
    public TransactionController(ISender sender) : base(sender) { }

    /// <summary>
    /// Imports transactions from a CSV file.
    /// </summary>
    /// <remarks>
    /// This endpoint accepts a CSV file uploaded via a form and processes it to import transactions into the system.
    /// The CSV file should be included in the form data with the appropriate field name.
    /// </remarks>
    /// <param name="command">The command containing the CSV file to import. The CSV file should be included in the form data.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
    /// <response code="200">If the transactions are successfully imported.</response>
    /// <response code="400">If there is an error during the import process.</response>
    [HttpPost("import-csv")]
    [ProducesResponseType(typeof(Unit), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
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

    /// <summary>
    /// Exports transactions to an Excel file.
    /// </summary>
    /// <remarks>
    /// This endpoint generates an Excel file containing all transactions and returns it to the client.
    /// The file is formatted as an `application/vnd.openxmlformats-officedocument.spreadsheetml.sheet` type.
    /// </remarks>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An <see cref="IActionResult"/> containing the exported file.</returns>
    /// <response code="200">If the transactions are successfully exported.</response>
    /// <response code="400">If there is an error during the export process.</response>
    [HttpGet("export-excel")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
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

    /// <summary>
    /// Gets all transactions by local dates.
    /// </summary>
    /// <remarks>
    /// This endpoint retrieves transactions that fall within the specified local date range.
    /// The dates should be provided in `yyyy-MM-ddTHH:mm:ss` format. Pagination is supported using `page` and `pageSize` parameters.
    /// </remarks>
    /// <param name="startDate">The start date of the transaction range. Format: `yyyy-MM-ddTHH:mm:ss`. Example: `2024-01-01T00:00:00`.</param>
    /// <param name="endDate">The end date of the transaction range. Format: `yyyy-MM-ddTHH:mm:ss`. Example: `2024-01-31T23:59:59`.</param>
    /// <param name="page">The page number for pagination. Must be a positive integer. Example: `1`.</param>
    /// <param name="pageSize">The number of transactions per page. Must be a positive integer. Example: `10`.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An <see cref="IActionResult"/> containing the transactions.</returns>
    /// <response code="200">If the transactions are successfully retrieved.</response>
    /// <response code="400">If there is an error during the retrieval process.</response>
    /// <response code="404">If no transactions are found for the specified date range.</response>
    [HttpGet("get-by-local-dates")]
    [ProducesResponseType(typeof(PagedResponse<Transaction>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllTransactionsByLocalDatesAsync (
        [FromQuery] DateTime startDate, 
        [FromQuery] DateTime endDate, 
        [FromQuery] int page, 
        [FromQuery] int pageSize, 
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await Sender.Send(new GetTransactionsByClientLocalDatesQuery(
                startDate, endDate, page, pageSize), cancellationToken);
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

    /// <summary>
    /// Retrieves transactions within a specified date range, adjusted for the client's time zone.
    /// </summary>
    /// <remarks>
    /// This endpoint retrieves transactions based on the provided start and end dates, taking into account the client's time zone.
    /// The `timezone` header should be a valid IANA time zone string. Pagination is supported using `page` and `pageSize` parameters.
    /// </remarks>
    /// <param name="startDate">The start date of the transaction range. Format: `yyyy-MM-ddTHH:mm:ss`. Example: `2024-01-01T00:00:00`.</param>
    /// <param name="endDate">The end date of the transaction range. Format: `yyyy-MM-ddTHH:mm:ss`. Example: `2024-01-31T23:59:59`.</param>
    /// <param name="page">The page number for pagination. Must be a positive integer. Example: `1`.</param>
    /// <param name="pageSize">The number of transactions per page. Must be a positive integer. Example: `10`.</param>
    /// <param name="timezone">The client's time zone. This should be a valid IANA time zone string. Example: `America/New_York`.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests. Used to cancel the operation if needed.</param>
    /// <returns>An <see cref="IActionResult"/> that contains a list of transactions matching the criteria.</returns>
    /// <response code="200">Returns a JSON object with the list of transactions and pagination details.</response>
    /// <response code="400">If the request parameters are invalid or an error occurs while processing the request.</response>
    /// <response code="404">If no transactions are found for the specified date range.</response>
    [HttpGet("get-by-dates-for-current-time-zone")]
    [ProducesResponseType(typeof(PagedResponse<Transaction>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
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

    /// <summary>
    /// Gets transactions in January 2024.
    /// </summary>
    /// <remarks>
    /// This endpoint retrieves all transactions that occurred in January 2024.
    /// Pagination is supported using `page` and `pageSize` parameters.
    /// </remarks>
    /// <param name="page">The page number for pagination. Must be a positive integer. Example: `1`.</param>
    /// <param name="pageSize">The number of transactions per page. Must be a positive integer. Example: `10`.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An <see cref="IActionResult"/> containing the transactions.</returns>
    /// <response code="200">If the transactions are successfully retrieved.</response>
    /// <response code="400">If there is an error during the retrieval process.</response>
    /// <response code="404">If no transactions are found for January 2024.</response>
    [HttpGet("get-in-january-2024")]
    [ProducesResponseType(typeof(PagedResponse<Transaction>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
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
