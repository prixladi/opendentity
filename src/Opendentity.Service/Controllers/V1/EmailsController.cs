using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shamyr.AspNetCore.HttpErrors;
using Opendentity.Domain.CQRS;
using Opendentity.Domain.Models;

namespace Opendentity.Service.Controllers.V1;

[ApiController]
[Route("/api/v1/emails")]
public class EmailsController: ControllerBase
{
    private readonly ISender sender;

    public EmailsController(ISender sender)
    {
        this.sender = sender;
    }

    /// <summary>
    /// Sends email confirmation on given email.
    /// </summary>
    /// <param name="email"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <response code="204">Email confirmation sent</response>
    [HttpPatch("{email}/send-confirmation")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<NoContentResult> SendConfirmationAsync(string email, CancellationToken cancellationToken)
    {
        await sender.Send(new SendEmailConfirmationCommand(email), cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Sends password reset to specified email if it exists
    /// </summary>
    /// <param name="email"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <response code="204">Password reset sent / or account with provided email does not exist</response>
    [HttpPatch("{email}/send-password-reset")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<NoContentResult> PasswordResetAsync(string email, CancellationToken cancellationToken)
    {
        await sender.Send(new SendPasswordResetCommand(email), cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Resets password
    /// </summary>
    /// <param name="email"></param>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <response code="204">Password reset was successful</response>
    /// <response code="404">Account with provided email does not exist</response>
    [HttpPatch("{email}/password-reset")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(HttpErrorResponseModel), StatusCodes.Status404NotFound)]
    public async Task<NoContentResult> PasswordResetAsync(string email, PasswordResetModel model, CancellationToken cancellationToken)
    {
        await sender.Send(new PasswordResetCommand(email, model), cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Confirms account email
    /// </summary>
    /// <param name="email"></param>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <response code="204">Confirmation was successfull</response>
    /// <response code="404">Account with provided email does not exist</response>
    /// <response code="404">Account with provided email is already confirmed</response>
    [HttpPatch("{email}/confirmation")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(HttpErrorResponseModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(HttpErrorResponseModel), StatusCodes.Status409Conflict)]
    public async Task<NoContentResult> ConfirmationAsync(string email, TokenModel model, CancellationToken cancellationToken)
    {
        await sender.Send(new ConfirmEmailCommand(email, model), cancellationToken);
        return NoContent();
    }
}
