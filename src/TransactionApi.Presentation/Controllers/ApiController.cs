using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TransactionApi.Presentation.Controllers;

/// <summary>
/// Base API controller providing common functionality for all controllers.
/// </summary>
[ApiController]
public abstract class ApiController : ControllerBase
{
    /// <summary>
    /// The sender for sending MediatR requests.
    /// </summary>
    protected readonly ISender Sender;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiController"/> class.
    /// </summary>
    /// <param name="sender">The sender for sending MediatR requests.</param>
    protected ApiController(ISender sender)
    {
        Sender = sender;
    }
}