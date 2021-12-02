using MediatR;
using Opendentity.Service.Models;

namespace Opendentity.Service.CQRS.Queries;

public record GetUserQuery(string Id): IRequest<UserModel>;
