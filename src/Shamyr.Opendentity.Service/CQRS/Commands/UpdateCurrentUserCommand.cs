﻿using MediatR;
using Shamyr.Opendentity.Service.Models;

namespace Shamyr.Opendentity.Service.CQRS.Commands
{
    public record UpdateCurrentUserCommand(UpdateUserModel Model): IRequest;
}
