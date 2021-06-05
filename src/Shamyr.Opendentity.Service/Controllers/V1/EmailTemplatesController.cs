using System.Collections.Generic;
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
    [Route("/api/v1/email-templates")]
    public class EmailTemplatesController: ControllerBase
    {
        private const string _GetEmailTemplateAction = "getEmailTemplate";

        private readonly ISender sender;

        public EmailTemplatesController(ISender sender)
        {
            this.sender = sender;
        }

        /// <summary>
        /// Creates new email template
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <response code="201">Email template created</response>
        /// <response code="409">Email template with provided key already exists</response>
        [HttpPost]
        [Authorize(Roles = Constants.Auth._AdminRole)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(HttpErrorResponseModel), StatusCodes.Status409Conflict)]
        public async Task<CreatedAtRouteResult> CreateAsync(CreateEmailTemplateModel model, CancellationToken cancellationToken)
        {
            var result = await sender.Send(new CreateEmailTemplateCommand(model), cancellationToken);
            return CreatedAtRoute(_GetEmailTemplateAction, new { id = result.Id }, null);
        }

        /// <summary>
        /// Gets all email template previews
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <response code="200">Returns list of email template previews</response>
        [HttpGet]
        [Authorize(Roles = Constants.Auth._AdminRole)]
        [ProducesResponseType(typeof(ICollection<EmailTemplatePreviewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(HttpErrorResponseModel), StatusCodes.Status401Unauthorized)]
        public async Task<ICollection<EmailTemplatePreviewModel>> GetAsync(CancellationToken cancellationToken)
        {
            return await sender.Send(new GetEmailTemplatesQuery(), cancellationToken);
        }

        /// <summary>
        /// Gets email template by key
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <response code="200">Returns email template</response>
        /// <response code="404">Email template with given id not found</response>
        [HttpGet("{id}", Name = _GetEmailTemplateAction)]
        [Authorize(Roles = Constants.Auth._AdminRole)]
        [ProducesResponseType(typeof(EmailTemplateModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(HttpErrorResponseModel), StatusCodes.Status404NotFound)]
        public async Task<EmailTemplateModel> GetAsync(string id, CancellationToken cancellationToken)
        {
            return await sender.Send(new GetEmailTemplateQuery(id), cancellationToken);
        }

        /// <summary>
        /// Updates email template by key
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <response code="204">Returns email template</response>
        /// <response code="404">Email template with given id not found</response>
        /// <response code="409">Email template with provided key already exists</response>
        [HttpPut("{id}")]
        [Authorize(Roles = Constants.Auth._AdminRole)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(HttpErrorResponseModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(HttpErrorResponseModel), StatusCodes.Status409Conflict)]
        public async Task<NoContentResult> UpdateAsync(string id, UpdateEmailTemplateModel model, CancellationToken cancellationToken)
        {
            await sender.Send(new UpdateEmailTemplateCommand(id, model), cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Deletes email template by key
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <response code="204">Returns email template</response>
        /// <response code="404">Email template with given id not found</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = Constants.Auth._AdminRole)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(HttpErrorResponseModel), StatusCodes.Status404NotFound)]
        public async Task<NoContentResult> DeleteAsync(string id, CancellationToken cancellationToken)
        {
            await sender.Send(new DeleteEmailTemplateCommand(id), cancellationToken);
            return NoContent();
        }
    }
}
