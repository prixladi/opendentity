using MediatR;
using Opendentity.Service.Models;

namespace Opendentity.Service.CQRS.Queries;

public record GetUsersQuery(UsersFilterModel Model): IRequest<UsersModel>;
