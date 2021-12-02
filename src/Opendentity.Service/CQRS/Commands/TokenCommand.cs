using System.Security.Claims;
using MediatR;
using OpenIddict.Abstractions;

namespace Opendentity.Service.CQRS.Commands;

public record TokenCommand(OpenIddictRequest OpenIddictRequest): IRequest<ClaimsPrincipal>;
