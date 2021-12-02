using System;
using System.Threading;
using System.Threading.Tasks;
using AspNetCoreRateLimit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Opendentity.Domain.CQRS;
using Opendentity.Domain.CQRS.Users;
using Opendentity.Domain.Models;
using Shamyr.AspNetCore.HttpErrors;

namespace Opendentity.Service.Controllers.V1;

[ApiController]
[Route("/api/v1/users")]
public class UsersController: ControllerBase
{
    private const string _GetUserAction = "getUser";

    private readonly ISender sender;

    public UsersController(ISender sender, IOptions<IpRateLimitOptions> opt)
    {
        if (opt is null)
            throw new ArgumentNullException(nameof(opt));
        this.sender = sender;
    }

    /// <summary>
    /// Creates new user
    /// </summary>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <response code="201">User created</response>
    /// <response code="409">User with provided email or username already exists</response>
    [HttpPost]
    [ProducesResponseType(typeof(CreatedModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(HttpErrorResponseModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(HttpErrorResponseModel), StatusCodes.Status409Conflict)]
    public async Task<CreatedAtRouteResult> CreateAsync(CreateUserModel model, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new CreateUserCommand(model), cancellationToken);
        return CreatedAtRoute(_GetUserAction, new { id = result.Id }, null);
    }

    /// <summary>
    /// Gets users by provided filter
    /// </summary>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <response code="200">Returns users</response>
    /// <response code="401">User is unauthorized</response>
    /// <response code="403">User needs to be admin to perform this action</response>
    [HttpGet]
    [Authorize(Roles = Constants.Auth._AdminRole)]
    [ProducesResponseType(typeof(UsersModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(HttpErrorResponseModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(HttpErrorResponseModel), StatusCodes.Status403Forbidden)]
    public async Task<UsersModel> GetAsync([FromQuery] UsersFilterModel model, CancellationToken cancellationToken)
    {
        return await sender.Send(new GetUsersQuery(model), cancellationToken);
    }

    /// <summary>
    /// Gets total user count
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <response code="200">Returns user count</response>
    /// <response code="401">User is unauthorized</response>
    /// <response code="403">User needs to be admin to perform this action</response>
    [HttpGet("count")]
    [Authorize(Roles = Constants.Auth._AdminRole)]
    [ProducesResponseType(typeof(CountModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(HttpErrorResponseModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(HttpErrorResponseModel), StatusCodes.Status403Forbidden)]
    public async Task<CountModel> GetCountAsync(CancellationToken cancellationToken)
    {
        return await sender.Send(new GetUserCountQuery(), cancellationToken);
    }

    /// <summary>
    /// Gets user by provided id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <response code="200">Returns user with provided id</response>
    /// <response code="401">User is unauthorized</response>
    /// <response code="403">User needs to be admin to perform this action</response>
    /// <response code="404">User with provided id not found</response>
    [HttpGet("{id}", Name = _GetUserAction)]
    [Authorize(Roles = Constants.Auth._AdminRole)]
    [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(HttpErrorResponseModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(HttpErrorResponseModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(HttpErrorResponseModel), StatusCodes.Status404NotFound)]
    public async Task<UserModel> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        return await sender.Send(new GetUserQuery(id), cancellationToken);
    }

    /// <summary>
    /// Changes user's disabled status
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <response code="204">Disabled changed</response>
    /// <response code="401">User is unauthorized</response>
    /// <response code="403">User needs to be admin to perform this action</response>
    /// <response code="404">User with provided id not found</response>
    [HttpPut("{id}/disabled")]
    [Authorize(Roles = Constants.Auth._AdminRole)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(HttpErrorResponseModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(HttpErrorResponseModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(HttpErrorResponseModel), StatusCodes.Status404NotFound)]
    public async Task<NoContentResult> UpdateDisabledAsync(string id, UpdateFlagModel model, CancellationToken cancellationToken)
    {
        await sender.Send(new UpdateUserDisabledCommand(id, model), cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Changes user's email confirmed status
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <response code="204">Email confirmed changed</response>
    /// <response code="401">User is unauthorized</response>
    /// <response code="403">User needs to be admin to perform this action</response>
    /// <response code="404">User with provided id not found</response>
    [HttpPut("{id}/emailConfirmed")]
    [Authorize(Roles = Constants.Auth._AdminRole)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(HttpErrorResponseModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(HttpErrorResponseModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(HttpErrorResponseModel), StatusCodes.Status404NotFound)]
    public async Task<NoContentResult> UpdateEmailConfirmedAsync(string id, UpdateFlagModel model, CancellationToken cancellationToken)
    {
        await sender.Send(new UpdateUserEmailConfirmedCommand(id, model), cancellationToken);
        return NoContent();
    }
}
