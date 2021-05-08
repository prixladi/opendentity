using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shamyr.AspNetCore.HttpErrors;
using Shamyr.Opendentity.Service.CQRS.Commands;
using Shamyr.Opendentity.Service.CQRS.Queries;
using Shamyr.Opendentity.Service.Models;

namespace Shamyr.Opendentity.Service.Controllers.V1
{
    [ApiController]
    [Route("/api/v1/users")]
    public class UsersController: ControllerBase
    {
        private const string _GetUserAction = "getUser";

        private readonly ISender sender;

        public UsersController(ISender sender)
        {
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
        /// <response code="429">Too many requests</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(HttpErrorResponseModel), StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<CreatedAtRouteResult> CreateAsync(CreateUserModel model, CancellationToken cancellationToken)
        {
            var result = await sender.Send(new CreateUserCommand(model), cancellationToken);
            return CreatedAtRoute(_GetUserAction, new { result.Id }, result);
        }

        /// <summary>
        /// Get's user by provided id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <response code="200">Returns user with provided id</response>
        /// <response code="401">User is unauthorized</response>
        /// <response code="403">User needs to be admin to perform this action</response>
        /// <response code="404">User with provided id not found</response>
        [HttpGet("{id}", Name = _GetUserAction)]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
        /// <response code="204">Returns user with provided id</response>
        /// <response code="401">User is unauthorized</response>
        /// <response code="403">User needs to be admin to perform this action</response>
        /// <response code="404">User with provided id not found</response>
        [HttpPut("{id}/disabled", Name = _GetUserAction)]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(HttpErrorResponseModel), StatusCodes.Status404NotFound)]
        public async Task<NoContentResult> UpdateDisabledAsync(string id, UpdateDisabledModel model, CancellationToken cancellationToken)
        {
            await sender.Send(new UpdateUserDisabledCommand(id, model), cancellationToken);
            return NoContent();
        }
    }
}
