﻿using MediatR;
using Shamyr.Opendentity.Service.Models;

namespace Shamyr.Opendentity.Service.CQRS.Queries
{
    public record GetCurrentUserQuery: IRequest<UserModel>;
}