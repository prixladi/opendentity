using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Opendentity.Domain.CQRS;
using Opendentity.Domain.Models;
using Shamyr.AspNetCore.HttpErrors;

namespace Opendentity.Service.Controllers.V1;

[ApiController]
[Route("/api/v1/users/current")]
public class CurrentUserController: ControllerBase
{
    private readonly ISender sender;

    public CurrentUserController(ISender sender)
    {
        this.sender = sender;
    }

    /// <summary>
    /// Gets currently logged user
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <response code="200">Returns user</response>
    /// <response code="401">User is unauthorized</response>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(HttpErrorResponseModel), StatusCodes.Status401Unauthorized)]
    public async Task<UserModel> UpdatAsync(CancellationToken cancellationToken)
    {
        return await sender.Send(new GetCurrentUserQuery(), cancellationToken);
    }

    /// <summary>
    /// Updates currently logged user
    /// </summary>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <response code="204">User updated</response>
    /// <response code="401">User is unauthorized</response>
    /// <response code="409">User with provided email or username already exists</response>
    [HttpPut]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(HttpErrorResponseModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(HttpErrorResponseModel), StatusCodes.Status409Conflict)]
    public async Task<NoContentResult> UpdatAsync(UpdateUserModel model, CancellationToken cancellationToken)
    {
        await sender.Send(new UpdateCurrentUserCommand(model), cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Changes password of current user
    /// </summary>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <response code="204">User password changed</response>
    /// <response code="401">User is unauthorized</response>
    /// <response code="409">User does not have password set</response>
    [HttpPatch("change-password")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(HttpErrorResponseModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(HttpErrorResponseModel), StatusCodes.Status409Conflict)]
    public async Task<NoContentResult> ChangePasswordAsync(ChangePasswordModel model, CancellationToken cancellationToken)
    {
        await sender.Send(new ChangeCurrentUserPasswordCommand(model), cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Sets password of current user
    /// </summary>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <response code="204">User password set</response>
    /// <response code="401">User is unauthorized</response>
    /// <response code="409">User has already password set</response>
    [HttpPatch("set-password")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(HttpErrorResponseModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(HttpErrorResponseModel), StatusCodes.Status409Conflict)]
    public async Task<NoContentResult> SetPasswordAsync(SetPasswordModel model, CancellationToken cancellationToken)
    {
        await sender.Send(new SetCurrentUserPasswordCommand(model), cancellationToken);
        return NoContent();
    }
}
