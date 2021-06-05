﻿using MediatR;
using Shamyr.Opendentity.Service.Models;

namespace Shamyr.Opendentity.Service.CQRS.Queries
{
    public record GetUserQuery(string Id): IRequest<UserModel>;
}
