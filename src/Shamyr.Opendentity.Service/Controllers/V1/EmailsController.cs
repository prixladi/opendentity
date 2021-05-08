using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shamyr.AspNetCore.HttpErrors;
using Shamyr.Opendentity.Service.CQRS.Commands;
using Shamyr.Opendentity.Service.Models;

namespace Shamyr.Opendentity.Service.Controllers.V1
{
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
        /// Sends password reset to specified email if it exists
        /// </summary>
        /// <param name="email"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <response code="204">Password reset sent / or email does not exist</response>
        [HttpPatch("{email}/sendPasswordReset")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<NoContentResult> SetPasswordResetAsync(string email, CancellationToken cancellationToken)
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
        /// <response code="404">Email does not exist</response>
        [HttpPut("{email}/passwordReset")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(HttpErrorResponseModel), StatusCodes.Status404NotFound)]
        public async Task<NoContentResult> PasswordResetAsync(string email, PasswordResetModel model, CancellationToken cancellationToken)
        {
            await sender.Send(new PasswordResetCommand(email, model), cancellationToken);
            return NoContent();
        }
    }
}
